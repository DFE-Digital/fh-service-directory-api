using FamilyHubs.ServiceDirectory.Api;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Runtime.InteropServices.JavaScript;
using IdGen.DependencyInjection;

namespace FamilyHubs.ServiceDirectoryApi.FunctionalTests;

public class MyWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureHostConfiguration(config =>
        {
            IEnumerable<KeyValuePair<string, string?>>? initialData = new List<KeyValuePair<string, string?>>
            { 
                new KeyValuePair<string, string?>("UseDbType", "UseInMemoryDatabase"),
                new KeyValuePair<string, string?>("UseVault", "false")
            };
            config.AddInMemoryCollection(initialData);
        });

        return base.CreateHost(builder);
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services.AddIdGen(1);
        });
        base.ConfigureWebHost(builder);
    }
}
