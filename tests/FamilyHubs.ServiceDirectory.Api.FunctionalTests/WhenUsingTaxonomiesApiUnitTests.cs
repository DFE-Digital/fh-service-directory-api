using System.Net;
using System.Text;
using System.Text.Json;
using FamilyHubs.ServiceDirectory.Shared.Dto;
using FamilyHubs.ServiceDirectory.Shared.Enums;
using FamilyHubs.ServiceDirectory.Shared.Models;
using FamilyHubs.SharedKernel.Identity;
using FluentAssertions;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace FamilyHubs.ServiceDirectory.Api.FunctionalTests;


[Collection("Sequential")]
public class WhenUsingTaxonomiesApiUnitTests : BaseWhenUsingApiUnitTests
{
    [Fact]
    public async Task ThenTheTaxonomiesAreRetrieved()
    {
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri(Client.BaseAddress + "api/taxonomies?pageNumber=1&pageSize=10&taxonomyType=ServiceCategory"),
        };

        using var response = await Client.SendAsync(request);

        var responseContent = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            ArgumentException.ThrowIfNullOrEmpty(responseContent);

        var retVal = JsonSerializer.Deserialize<PaginatedList<TaxonomyDto>>(responseContent, new JsonSerializerOptions(JsonSerializerDefaults.Web));

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        ArgumentNullException.ThrowIfNull(retVal);
        retVal.Should().NotBeNull();
        retVal.Items.Count.Should().BeGreaterThan(3);
    }

    [Fact]
    public async Task ThenTheTaxonomyIsCreated()
    {
        var commandTaxonomy = new TaxonomyDto
        {
            Name = "Test-AddTaxonomy",
            TaxonomyType = TaxonomyType.ServiceCategory,
            ParentId = null,
        };

        var request = CreatePostRequest("api/taxonomies", commandTaxonomy, RoleTypes.DfeAdmin);

        using var response = await Client.SendAsync(request);

        var responseContent = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            ArgumentException.ThrowIfNullOrEmpty(responseContent);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        long.Parse(responseContent).Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task ThenTheTaxonomyIsUpdated()
    {
        var commandTaxonomy = new TaxonomyDto
        {
            Name = "Test-UpdateTaxonomy",
            TaxonomyType = TaxonomyType.ServiceCategory,
            ParentId = null
        };

        var request = CreatePostRequest("api/taxonomies", commandTaxonomy, RoleTypes.DfeAdmin);

        using var response = await Client.SendAsync(request);

        var responseContent = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            ArgumentException.ThrowIfNullOrEmpty(responseContent);

        var createdTaxonomyId = JsonSerializer.Deserialize<long>(responseContent);

        var updatedTaxonomy = new TaxonomyDto
        {
            Id = createdTaxonomyId,
            Name = "Test-IsUpdateTaxonomy",
            TaxonomyType = TaxonomyType.ServiceCategory,
            ParentId = null
        };

        var updateRequest = CreatePutRequest($"api/taxonomies/{createdTaxonomyId}", updatedTaxonomy, RoleTypes.DfeAdmin);

        using var updateResponse = await Client.SendAsync(updateRequest);

        var updateResponseContent = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            ArgumentException.ThrowIfNullOrEmpty(updateResponseContent);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        long.Parse(updateResponseContent).Should().Be(updatedTaxonomy.Id);
    }
}
