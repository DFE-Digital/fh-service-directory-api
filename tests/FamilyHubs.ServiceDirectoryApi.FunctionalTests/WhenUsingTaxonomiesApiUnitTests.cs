using System.Net;
using System.Text;
using System.Text.Json;
using FamilyHubs.ServiceDirectory.Shared.Dto;
using FamilyHubs.ServiceDirectory.Shared.Enums;
using FamilyHubs.SharedKernel;
using FluentAssertions;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace FamilyHubs.ServiceDirectoryApi.FunctionalTests;


[Collection("Sequential")]
public class WhenUsingTaxonomiesApiUnitTests : BaseWhenUsingApiUnitTests
{
#if DEBUG
    [Fact]
#else
    [Fact(Skip = "This test should be run locally")]
#endif
    public async Task ThenTheTaxonomiesAreRetrieved()
    {
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri(_client.BaseAddress + "api/taxonomies?pageNumber=1&pageSize=10&taxonomyType=ServiceCategory"),
        };

        using var response = await _client.SendAsync(request);

        response.EnsureSuccessStatusCode();

        var retVal = await JsonSerializer.DeserializeAsync<PaginatedList<TaxonomyDto>>(await response.Content.ReadAsStreamAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        ArgumentNullException.ThrowIfNull(retVal);
        retVal.Should().NotBeNull();
        retVal.Items.Count.Should().BeGreaterThan(3);
    }

#if DEBUG
    [Fact]
#else
    [Fact(Skip = "This test should be run locally")]
#endif
    public async Task ThenTheTaxonomyIsCreated()
    {
        var commandtaxonomy = new TaxonomyDto
        {
            Name = "Test-AddTaxonomy",
            TaxonomyType = TaxonomyType.ServiceCategory,
            ParentId = null,
        };

        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri(_client.BaseAddress + "api/taxonomies"),
            Content = new StringContent(JsonConvert.SerializeObject(commandtaxonomy), Encoding.UTF8, "application/json"),
        };

        //request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue($"Bearer", $"{new JwtSecurityTokenHandler().WriteToken(_token)}");

        using var response = await _client.SendAsync(request);

        response.EnsureSuccessStatusCode();

        var stringResult = await response.Content.ReadAsStringAsync();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        long.Parse(stringResult).Should().Be(commandtaxonomy.Id);
    }

#if DEBUG
    [Fact]
#else
    [Fact(Skip = "This test should be run locally")]
#endif
    public async Task ThenTheTaxonomyIsUpdated()
    {
        var commandtaxonomy = new TaxonomyDto
        {
            Name = "Test-UpdateTaxonomy",
            TaxonomyType = TaxonomyType.ServiceCategory,
            ParentId = null
        };

        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri(_client.BaseAddress + "api/taxonomies"),
            Content = new StringContent(JsonConvert.SerializeObject(commandtaxonomy), Encoding.UTF8, "application/json"),
        };

        //request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue($"Bearer", $"{new JwtSecurityTokenHandler().WriteToken(_token)}");

        using var response = await _client.SendAsync(request);

        response.EnsureSuccessStatusCode();

        await response.Content.ReadAsStringAsync();

        var updatedtaxonomy = new TaxonomyDto
        {
            Id = commandtaxonomy.Id,
            Name = "Test-IsUpdateTaxonomy",
            TaxonomyType = TaxonomyType.ServiceCategory,
            ParentId = null
        };
        var updaterequest = new HttpRequestMessage
        {
            Method = HttpMethod.Put,
            RequestUri = new Uri(_client.BaseAddress + $"api/taxonomies/{commandtaxonomy.Id}"),
            Content = new StringContent(JsonConvert.SerializeObject(updatedtaxonomy), Encoding.UTF8, "application/json"),
        };

        //updaterequest.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue($"Bearer", $"{new JwtSecurityTokenHandler().WriteToken(_token)}");

        using var updateresponse = await _client.SendAsync(updaterequest);

        updateresponse.EnsureSuccessStatusCode();

        var updateStringResult = await updateresponse.Content.ReadAsStringAsync();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        long.Parse(updateStringResult).Should().Be(updatedtaxonomy.Id);
    }
}
