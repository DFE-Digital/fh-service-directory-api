using FamilyHubs.SharedKernel;
using FamilyHubs.SharedKernel.Interfaces;
using fh_service_directory_api.infrastructure.Persistence.Interceptors;
using fh_service_directory_api.infrastructure.Persistence.Repository;
using fh_service_directory_api.infrastructure.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace fh_service_directory_api.api.Data;

public class DatabaseContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        var builder = new DbContextOptionsBuilder<ApplicationDbContext>();

        string useDbType = configuration.GetValue<string>("UseDbType");

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
                        builder.UseNpgsql(connectionString, b => b.MigrationsAssembly("FamilyHubs.ServiceDirectoryApi.Api"));

                }
                break;
        }

        AuditableEntitySaveChangesInterceptor auditableEntitySaveChangesInterceptor = new(new CurrentUserService(new HttpContextAccessor()), new DateTimeService());

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        return new ApplicationDbContext(builder.Options, null, auditableEntitySaveChangesInterceptor);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
    }
}
