using Microsoft.Extensions.Configuration;
using System.IO;

namespace FamilyHubs.ServiceDirectory.Api.AcceptanceTests.Configuration;

public class ConfigAccessor
{
    static IConfigurationRoot root;

    private static IConfigurationRoot GetIConfigurationRoot()
    {
        if (root == null)
        {
            root = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false)
                .AddEnvironmentVariables()
                .Build();
        }

        return root;

    }

    public static ConfigModel GetApplicationConfiguration()
    {
        var configuration = new ConfigModel();

        var iConfig = GetIConfigurationRoot();

        iConfig.Bind(configuration);

        return configuration;
    }
}