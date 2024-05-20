using FamilyHubs.ServiceDirectory.Data.Repository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FamilyHubs.ServiceDirectory.Api.FunctionalTests;
public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly string _serviceDirectoryConnection;
    public CustomWebApplicationFactory()
    {
        _serviceDirectoryConnection = $"Data Source=sd-{Random.Shared.Next().ToString()}.db;Mode=ReadWriteCreate;Cache=Shared;Foreign Keys=True;Recursive Triggers=True;Default Timeout=30;Pooling=True";
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var efCoreServices = services.Where(s => 
                s.ServiceType.FullName?.Contains("EntityFrameworkCore") == true).ToList();

            efCoreServices.ForEach(s => services.Remove(s));

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlite(_serviceDirectoryConnection, mg =>
                    mg.UseNetTopologySuite().MigrationsAssembly(typeof(ApplicationDbContext).Assembly.ToString()));
            });
        });

        builder.ConfigureAppConfiguration((context, configurationBuilder) =>
        {
            configurationBuilder.AddInMemoryCollection(
                new Dictionary<string, string?> {
                    {"UseSqlite", "true"},
                }
            );
        });

        builder.UseEnvironment("Development");
    }
    public void SetupTestDatabaseAndSeedData()
    {
        using var scope = Services.CreateScope();

        var scopedServices = scope.ServiceProvider;
        var logger = scopedServices.GetRequiredService<ILogger<CustomWebApplicationFactory>>();

        try
        {
            var context = scopedServices.GetRequiredService<ApplicationDbContext>();

            var testOrganisations = context.Organisations.Select(o => new { o.Id, o.Name })
                .Where(o => o.Name == "Bristol County Council" || o.Name == "Salford City Council")
                .ToDictionary(arg => arg.Name, arg => arg.Id);

            context.Services.AddRange(TestDataProvider.SeedBristolServices(testOrganisations["Bristol County Council"]));
            context.Services.AddRange(TestDataProvider.SeedSalfordService(testOrganisations["Salford City Council"]));

            context.SaveChanges();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred seeding the database with test messages. Error: {exceptionMessage}", ex.Message);
        }
    }
}
