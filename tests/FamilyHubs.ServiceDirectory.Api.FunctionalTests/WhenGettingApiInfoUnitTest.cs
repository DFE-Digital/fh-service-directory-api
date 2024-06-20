namespace FamilyHubs.ServiceDirectory.Api.FunctionalTests;

[Collection("Sequential")]
public class WhenGettingApiInfoUnitTest : BaseWhenUsingApiUnitTests
{
    [Fact]
    public async Task ThenReturnsVersionAndLastUpdateDate()
    {
        var response = await Client.GetAsync("info");
        response.EnsureSuccessStatusCode();
        var stringResponse = await response.Content.ReadAsStringAsync();

        Assert.Contains("Version", stringResponse);
    }
}

