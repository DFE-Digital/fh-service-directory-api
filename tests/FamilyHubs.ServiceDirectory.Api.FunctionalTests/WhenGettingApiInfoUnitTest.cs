namespace FamilyHubs.ServiceDirectory.Api.FunctionalTests;

[Collection("Sequential")]
public class WhenGettingApiInfoUnitTest
{
    private readonly HttpClient _client;

    public WhenGettingApiInfoUnitTest()
    {
        var webAppFactory = new CustomWebApplicationFactory();

        _client = webAppFactory.CreateDefaultClient();
        _client.BaseAddress = new Uri("https://localhost:7128/");
    }


    [Fact]
    public async Task ThenReturnsVersionAndLastUpdateDate()
    {
        var response = await _client.GetAsync("api/info");
        response.EnsureSuccessStatusCode();
        var stringResponse = await response.Content.ReadAsStringAsync();

        Assert.Contains("Version", stringResponse);
        Assert.Contains("Last Updated", stringResponse);
    }
}

