using System.Net;
using System.Text;
using System.Text.Json;
using FamilyHubs.ServiceDirectory.Shared.Dto;
using FamilyHubs.SharedKernel.Identity;
using FluentAssertions;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace FamilyHubs.ServiceDirectory.Api.FunctionalTests;

[Collection("Sequential")]
public class WhenUsingLocationApiUnitTests : BaseWhenUsingApiUnitTests
{
    [Fact]
    public async Task ThenTheLocationIsCreated()
    {
        var location = TestDataProvider.GetTestCountyCouncilRecord()
            .Services.ElementAt(0).Locations.ElementAt(0);

        var request = CreatePostRequest("api/locations", location, RoleTypes.DfeAdmin);

        using var response = await Client.SendAsync(request);

        var responseContent = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            Assert.Fail(!string.IsNullOrWhiteSpace(responseContent) ? responseContent : response.ToString());

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        long.Parse(responseContent).Should().Be(10);
    }

    [Fact]
    public async Task ThenTheLocationIsUpdated()
    {
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri(Client.BaseAddress + "api/locations/1"),

        };

        using var response = await Client.SendAsync(request);

        var responseContent = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            Assert.Fail(!string.IsNullOrWhiteSpace(responseContent) ? responseContent : response.ToString());

        var retVal = JsonSerializer.Deserialize<LocationDto>(responseContent, new JsonSerializerOptions(JsonSerializerDefaults.Web));
        ArgumentNullException.ThrowIfNull(retVal);

        retVal.Name = "Updated Location Name";

        var updateRequest = CreatePutRequest("api/locations/1", retVal, RoleTypes.DfeAdmin);

        using var updateResponse = await Client.SendAsync(updateRequest);

        var updateResponseContent = await updateResponse.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            Assert.Fail(!string.IsNullOrWhiteSpace(updateResponseContent) ? updateResponseContent : response.ToString());

        updateResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        long.Parse(updateResponseContent).Should().Be(1);
    }

    [Fact]
    public async Task ThenTheLocationIsRetrieved()
    {
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri(Client.BaseAddress + "api/locations/1"),

        };

        using var response = await Client.SendAsync(request);

        var responseContent = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            Assert.Fail(!string.IsNullOrWhiteSpace(responseContent) ? responseContent : response.ToString());

        var retVal = JsonSerializer.Deserialize<LocationDto>(responseContent, new JsonSerializerOptions(JsonSerializerDefaults.Web));

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        retVal.Should().NotBeNull();
        ArgumentNullException.ThrowIfNull(retVal);
        retVal.Id.Should().Be(1);
    }

    [Fact]
    public async Task ThenTheOrganisationLocationsAreRetrievedByOrganisationId()
    {
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri(Client.BaseAddress + "api/organisationlocations/1"),

        };

        using var response = await Client.SendAsync(request);

        var responseContent = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            Assert.Fail(!string.IsNullOrWhiteSpace(responseContent) ? responseContent : response.ToString());

        var retVal = JsonSerializer.Deserialize<IList<LocationDto>>(responseContent, new JsonSerializerOptions(JsonSerializerDefaults.Web));

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        retVal.Should().NotBeNull();
        ArgumentNullException.ThrowIfNull(retVal);
        retVal.Count.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task ThenTheServiceLocationsAreRetrievedByServiceId()
    {
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri(Client.BaseAddress + "api/servicelocations/1"),

        };

        using var response = await Client.SendAsync(request);

        var responseContent = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            Assert.Fail(!string.IsNullOrWhiteSpace(responseContent) ? responseContent : response.ToString());

        var retVal = JsonSerializer.Deserialize<IList<LocationDto>>(responseContent, new JsonSerializerOptions(JsonSerializerDefaults.Web));

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        retVal.Should().NotBeNull();
        ArgumentNullException.ThrowIfNull(retVal);
        retVal.Count.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task ThenListLocationsIsRetrieved()
    {
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri(Client.BaseAddress + "api/locations"),

        };

        using var response = await Client.SendAsync(request);

        var responseContent = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            Assert.Fail(responseContent);

        var retVal = JsonSerializer.Deserialize<List<LocationDto>>(responseContent, new JsonSerializerOptions(JsonSerializerDefaults.Web));

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        retVal.Should().NotBeNull();
        ArgumentNullException.ThrowIfNull(retVal);
        retVal.Count.Should().BeGreaterThan(0);
    }
}
