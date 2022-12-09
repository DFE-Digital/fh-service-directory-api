using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralTaxonomys;
using FamilyHubs.SharedKernel;
using FluentAssertions;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Text.Json;

namespace FamilyHubs.ServiceDirectoryApi.FunctionalTests;


[Collection("Sequential")]
public class WhenUsingTaxonomiesApiUnitTests : BaseWhenUsingOpenReferralApiUnitTests
{
#if DEBUG
    [Fact]
#else
    [Fact(Skip = "This test should be run locally")]
#endif
    public async Task ThenTheOpenReferralTaxonomiesAreRetrieved()
    {
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri(_client.BaseAddress + "api/taxonomies?pageNumber=1&pageSize=10"),
        };

        using var response = await _client.SendAsync(request);

        response.EnsureSuccessStatusCode();

        var retVal = await JsonSerializer.DeserializeAsync<PaginatedList<OpenReferralTaxonomyDto>>(await response.Content.ReadAsStreamAsync(), options: new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        ArgumentNullException.ThrowIfNull(retVal, nameof(retVal));
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
        var commandtaxonomy = new OpenReferralTaxonomyDto(Guid.NewGuid().ToString(), "Test-AddTaxonomy", "Test-AddVocab", null);
        
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri(_client.BaseAddress + "api/taxonomies"),
            Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(commandtaxonomy), Encoding.UTF8, "application/json"),
        };

        //request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue($"Bearer", $"{new JwtSecurityTokenHandler().WriteToken(_token)}");

        using var response = await _client.SendAsync(request);

        response.EnsureSuccessStatusCode();

        var stringResult = await response.Content.ReadAsStringAsync();

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        stringResult.ToString().Should().Be(commandtaxonomy.Id);
    }

#if DEBUG
    [Fact]
#else
    [Fact(Skip = "This test should be run locally")]
#endif
    public async Task ThenTheTaxonomyIsUpdated()
    {
        var commandtaxonomy = new OpenReferralTaxonomyDto(Guid.NewGuid().ToString(), "Test-UpdateTaxonomy", "Test-UpDateVocab", null);

        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri(_client.BaseAddress + "api/taxonomies"),
            Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(commandtaxonomy), Encoding.UTF8, "application/json"),
        };

        //request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue($"Bearer", $"{new JwtSecurityTokenHandler().WriteToken(_token)}");

        using var response = await _client.SendAsync(request);

        response.EnsureSuccessStatusCode();

        var stringResult = await response.Content.ReadAsStringAsync();

        var updatedtaxonomy = new OpenReferralTaxonomyDto(commandtaxonomy.Id, "Test-IsUpdateTaxonomy", "Test-IsUpDateVocab", null);

        var updaterequest = new HttpRequestMessage
        {
            Method = HttpMethod.Put,
            RequestUri = new Uri(_client.BaseAddress + $"api/taxonomies/{commandtaxonomy.Id}"),
            Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(updatedtaxonomy), Encoding.UTF8, "application/json"),
        };

        //updaterequest.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue($"Bearer", $"{new JwtSecurityTokenHandler().WriteToken(_token)}");

        using var updateresponse = await _client.SendAsync(updaterequest);

        updateresponse.EnsureSuccessStatusCode();

        var updateStringResult = await updateresponse.Content.ReadAsStringAsync();

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        updateStringResult.ToString().Should().Be(updatedtaxonomy.Id);
    }
}
