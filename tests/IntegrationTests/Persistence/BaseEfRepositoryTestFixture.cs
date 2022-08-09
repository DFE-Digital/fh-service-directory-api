using AutoFixture;
using fh_service_directory_api.core.Concretions.Entities.Aggregates;
using fh_service_directory_api.infrastructure.Persistence.Repository;
using LocalAuthorityInformationServices.SharedKernel.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace IntegrationTests.Persistence;

public abstract class BaseEfRepositoryTestFixture
{
    protected ApplicationDbContext DbContext; // see https://social.msdn.microsoft.com/Forums/en-US/930f159f-dfa5-4aa8-9af6-aa6545da5cbd/what-is-the-c-protected-property-naming-convention?forum=csharpgeneral

    protected BaseEfRepositoryTestFixture()
    {
        var options = CreateNewContextOptions();
        var mockEventDispatcher = new Mock<IDomainEventDispatcher>();

        DbContext = new ApplicationDbContext(options, mockEventDispatcher.Object);
    }

    protected static DbContextOptions<ApplicationDbContext> CreateNewContextOptions()
    {
        // Create a fresh service provider, and therefore a fresh
        // InMemory database instance.
        var serviceProvider = new ServiceCollection()
            .AddEntityFrameworkInMemoryDatabase()
            .BuildServiceProvider();

        // Create a new options instance telling the context to use an
        // InMemory database and the new service provider.
        var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
        builder.UseInMemoryDatabase("OrOrganisations")
               .UseInternalServiceProvider(serviceProvider);

        return builder.Options;
    }

    //protected EfRepository<Organisation> GetRepository()
    //{
    //    return new EfRepository<Organisation>(DbContext);
    //}
    protected EfRepository<T> GetRepository<T>() where T : class, IAggregateRoot
    {
        return new EfRepository<T>(DbContext);
    }
}
