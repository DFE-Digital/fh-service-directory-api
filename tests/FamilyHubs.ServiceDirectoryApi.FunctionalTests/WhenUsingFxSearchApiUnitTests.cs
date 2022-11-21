using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralLocations;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralServices;
using FamilyHubs.ServiceDirectory.Shared.Models;
using fh_service_directory_api.core.Entities;
using FluentAssertions;
using Newtonsoft.Json;

namespace FamilyHubs.ServiceDirectoryApi.FunctionalTests;


[Collection("Sequential")]
public class WhenUsingFxSearchApiUnitTests : BaseWhenUsingOpenReferralApiUnitTests
{
#if DEBUG
    [Fact]
#else
    [Fact(Skip = "This test should be run locally")]
#endif
    public async Task ThenResultsAreReturned()
    {
        var command = new OpenReferralLocation("25cd5229-f90c-4243-9ce6-9f0f7a718feb", "Central Test Hub", "Test Hub", -2.459764D, 53.607025D, new List<OpenReferralPhysical_Address>() { new OpenReferralPhysical_Address("9bdc326f-3ea4-4569-87a8-985b66eb412f", "Test Street", "Manchester", "M7 1BQ", "United Kingdom", "Salford") }, new List<Accessibility_For_Disabilities>());

        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri(_client.BaseAddress + "api/search?districtCode=E08000006&longitude=-2.340202&latitude=53.510849")
        };

        using var response = await _client.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        var results = JsonConvert.DeserializeObject<List<Either<OpenReferralServiceDto, OpenReferralLocationDto, double>>>(content);

        results.Should().NotBeNull();
    }
}
