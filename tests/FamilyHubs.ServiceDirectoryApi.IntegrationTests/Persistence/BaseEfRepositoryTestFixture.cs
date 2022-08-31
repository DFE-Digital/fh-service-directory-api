using FamilyHubs.SharedKernel.Interfaces;
using fh_service_directory_api.core.Interfaces;
using fh_service_directory_api.infrastructure.Persistence.Interceptors;
using fh_service_directory_api.infrastructure.Persistence.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace FamilyHubs.ServiceDirectoryApi.IntegrationTests.Persistence;

public abstract class BaseEfRepositoryTestFixture
{
    protected ApplicationDbContext DbContext; // see https://social.msdn.microsoft.com/Forums/en-US/930f159f-dfa5-4aa8-9af6-aa6545da5cbd/what-is-the-c-protected-property-naming-convention?forum=csharpgeneral

    protected BaseEfRepositoryTestFixture()
    {
        var options = CreateNewContextOptions();
        var mockEventDispatcher = new Mock<IDomainEventDispatcher>();
        var mockDateTime = new Mock<IDateTime>();
        var mockCurrentUserService = new Mock<ICurrentUserService>();
        var auditableEntitySaveChangesInterceptor = new AuditableEntitySaveChangesInterceptor(mockCurrentUserService.Object, mockDateTime.Object);
        

        DbContext = new ApplicationDbContext(options, mockEventDispatcher.Object, auditableEntitySaveChangesInterceptor);
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
        builder.UseInMemoryDatabase("OrOpenReferralOrganisations")
               .UseInternalServiceProvider(serviceProvider);

        return builder.Options;
    }

    protected EfRepository<T> GetRepository<T>() where T : class, IAggregateRoot
    {
        return new EfRepository<T>(DbContext);
    }
}
