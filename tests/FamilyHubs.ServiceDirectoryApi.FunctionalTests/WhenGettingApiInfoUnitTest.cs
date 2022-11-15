using Microsoft.AspNetCore.Mvc.Testing;

namespace FamilyHubs.ServiceDirectoryApi.FunctionalTests;

[Collection("Sequential")]
public class WhenGettingApiInfoUnitTest
{
    private readonly HttpClient _client;

    public WhenGettingApiInfoUnitTest()
    {
        var webAppFactory = new MyWebApplicationFactory();

        _client = webAppFactory.CreateDefaultClient();
        _client.BaseAddress = new Uri("https://localhost:7128/");
    }


#if DEBUG
    [Fact]
#else
    [Fact(Skip = "This test should be run locally")]
#endif
    public async Task ThenReturnsVersionAndLastUpdateDate()
    {
        var response = await _client.GetAsync("api/info");
        response.EnsureSuccessStatusCode();
        var stringResponse = await response.Content.ReadAsStringAsync();

        Assert.Contains("Version", stringResponse);
        Assert.Contains("Last Updated", stringResponse);
    }
}

