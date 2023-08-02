using System.Net;
using System.Text;
using System.Text.Json;
using Azure.Core;
using FamilyHubs.ServiceDirectory.Shared.Dto;
using FamilyHubs.SharedKernel.Identity;
using FluentAssertions;
using Newtonsoft.Json;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace FamilyHubs.ServiceDirectory.Api.FunctionalTests;

[Collection("Sequential")]
public class WhenUsingOrganisationApiUnitTests : BaseWhenUsingApiUnitTests
{
    [Fact]
    public async Task ThenTheOrganisationIsCreated()
    {
        var command = TestDataProvider.GetTestCountyCouncilRecord();

        var request = CreatePostRequest("api/organisations", command, RoleTypes.DfeAdmin);

        using var response = await Client.SendAsync(request);

        var responseContent = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            Assert.Fail(!string.IsNullOrWhiteSpace(responseContent) ? responseContent : response.ToString());

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        long.Parse(responseContent).Should().Be(7);
    }

    [Fact]
    public async Task ThenTheOrganisationIsUpdated()
    {
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri(Client.BaseAddress + "api/organisations/1")
        };

        using var response = await Client.SendAsync(request);

        var responseContent = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            Assert.Fail(!string.IsNullOrWhiteSpace(responseContent) ? responseContent : response.ToString());

        var retVal = JsonSerializer.Deserialize<OrganisationWithServicesDto>(responseContent, new JsonSerializerOptions(JsonSerializerDefaults.Web));
        ArgumentNullException.ThrowIfNull(retVal);

        var update = new OrganisationWithServicesDto
        {
            OrganisationType = retVal.OrganisationType,
            Id = retVal.Id,
            Name = retVal.Name,
            Description = retVal.Description + " Update Test",
            AdminAreaCode = retVal.AdminAreaCode,
            Logo = retVal.Logo,
            Uri = retVal.Uri,
            Url = retVal.Url,
        };

        var updateRequest = CreatePutRequest("api/organisations/1", update, RoleTypes.DfeAdmin);

        using var updateResponse = await Client.SendAsync(updateRequest);

        var updateResponseContent = await updateResponse.Content.ReadAsStringAsync();

        if (!updateResponse.IsSuccessStatusCode)
            Assert.Fail(!string.IsNullOrWhiteSpace(updateResponseContent) ? updateResponseContent : updateResponse.ToString());

        updateResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        long.Parse(updateResponseContent).Should().Be(1);
    }

    [Fact]
    public async Task ThenTheOrganisationIsRetrieved()
    {
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri(Client.BaseAddress + "api/organisations/1"),

        };

        using var response = await Client.SendAsync(request);

        var responseContent = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            Assert.Fail(!string.IsNullOrWhiteSpace(responseContent) ? responseContent : response.ToString());

        var retVal = JsonSerializer.Deserialize<OrganisationWithServicesDto>(responseContent, new JsonSerializerOptions(JsonSerializerDefaults.Web));

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        retVal.Should().NotBeNull();
        ArgumentNullException.ThrowIfNull(retVal);
        retVal.Id.Should().Be(1);
    }

    [Fact]
    public async Task ThenListOrganisationsIsRetrieved()
    {
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri(Client.BaseAddress + "api/organisations"),

        };

        using var response = await Client.SendAsync(request);

        var responseContent = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            Assert.Fail(!string.IsNullOrWhiteSpace(responseContent) ? responseContent : response.ToString());

        var retVal = JsonSerializer.Deserialize<List<OrganisationDto>>(responseContent, new JsonSerializerOptions(JsonSerializerDefaults.Web));

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        retVal.Should().NotBeNull();
        ArgumentNullException.ThrowIfNull(retVal);
        retVal.Count.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task ThenOrganisationAdminCodeIsRetrieved()
    {
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri(Client.BaseAddress + "api/organisationAdminCode/1"),
        };

        using var response = await Client.SendAsync(request);

        var responseContent = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            Assert.Fail(!string.IsNullOrWhiteSpace(responseContent) ? responseContent : response.ToString());

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        responseContent.Should().NotBeNull();
        responseContent.Should().Be("E06000023");
    }

    [Fact]
    public async Task ThenTheOrganisationIsDeleted()
    {        
        var deleteRequest = CreateDeleteRequest("api/organisations/1", string.Empty, RoleTypes.DfeAdmin);

        using var deleteResponse = await Client.SendAsync(deleteRequest);

        var deleteResponseContent = await deleteResponse.Content.ReadAsStringAsync();

        if (!deleteResponse.IsSuccessStatusCode)
            Assert.Fail(!string.IsNullOrWhiteSpace(deleteResponseContent) ? deleteResponseContent : deleteResponse.ToString());

        deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        bool.Parse(deleteResponseContent).Should().BeTrue();
    }
}
