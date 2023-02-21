using FamilyHubs.ServiceDirectory.Infrastructure.Persistence.Interceptors;
using FamilyHubs.ServiceDirectory.Infrastructure.Persistence.Repository;
using FamilyHubs.ServiceDirectory.Infrastructure.Services;
using FamilyHubs.SharedKernel.Interfaces;
using IdGen;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace FamilyHubs.ServiceDirectory.Api.Data;

public class DatabaseContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    private readonly IIdGenerator<long> _idGenerator;

    public DatabaseContextFactory(IIdGenerator<long> idGenerator)
    {
        _idGenerator = idGenerator;
    }

    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        var builder = new DbContextOptionsBuilder<ApplicationDbContext>();

        var useDbType = configuration.GetValue<string>("UseDbType");

        switch (useDbType)
        {
            default:
                builder.UseInMemoryDatabase("FH-LAHubDb");
                break;

            case "UseSqlServerDatabase":
                {
                    var connectionString = configuration.GetConnectionString("ServiceDirectoryConnection");
                    if (connectionString != null)
                        builder.UseSqlServer(connectionString, b => b.MigrationsAssembly("FamilyHubs.ServiceDirectoryApi.Api"));

                }
                break;

            case "UsePostgresDatabase":
                {
                    var connectionString = configuration.GetConnectionString("ServiceDirectoryConnection");
                    if (connectionString != null)
                        builder
                            .UseNpgsql(connectionString, b => b.MigrationsAssembly("FamilyHubs.ServiceDirectoryApi.Api"))
                            .UseLowerCaseNamingConvention()
                            ;

                }
                break;
        }

        var auditableEntitySaveChangesInterceptor =
            new AuditableEntitySaveChangesInterceptor(new CurrentUserService(new HttpContextAccessor()),
                new DateTimeService());

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        return new ApplicationDbContext(builder.Options, null, auditableEntitySaveChangesInterceptor, _idGenerator);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
    }
}
