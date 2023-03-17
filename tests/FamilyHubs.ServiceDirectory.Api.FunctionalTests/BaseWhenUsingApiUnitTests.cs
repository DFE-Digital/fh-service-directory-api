namespace FamilyHubs.ServiceDirectory.Api.FunctionalTests;

public abstract class BaseWhenUsingApiUnitTests
{
    protected readonly HttpClient Client;

    protected BaseWhenUsingApiUnitTests()
    {
        var webAppFactory = new CustomWebApplicationFactory();
        webAppFactory.SetupTestDatabaseAndSeedData();

        Client = webAppFactory.CreateDefaultClient();
        Client.BaseAddress = new Uri("https://localhost:7128/");
    }
}
