namespace FamilyHubs.ServiceDirectory.Api.FunctionalTests;

[Collection("Sequential")]
public class WhenGettingApiInfoUnitTest : BaseWhenUsingApiUnitTests
{
    [Fact]
    public async Task ThenReturnsVersionAndLastUpdateDate()
    {
        if (!IsRunningLocally() || Client == null)
        {
            // Skip the test if not running locally
            Assert.True(true, "Test skipped because it is not running locally.");
            return;
        }

        var response = await Client.GetAsync("api/info");
        response.EnsureSuccessStatusCode();
        var stringResponse = await response.Content.ReadAsStringAsync();

        Assert.Contains("Version", stringResponse);
        Assert.Contains("Last Updated", stringResponse);
    }
}

