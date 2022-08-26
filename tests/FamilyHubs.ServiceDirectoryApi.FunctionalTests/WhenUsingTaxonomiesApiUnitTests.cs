using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralTaxonomys;
using FamilyHubs.SharedKernel;
using FluentAssertions;
using System.Text.Json;

namespace FunctionalTests;


[Collection("Sequential")]
public class WhenUsingTaxonomiesApiUnitTests : BaseWhenUsingOpenReferralApiUnitTests
{
    [Fact]
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
}
