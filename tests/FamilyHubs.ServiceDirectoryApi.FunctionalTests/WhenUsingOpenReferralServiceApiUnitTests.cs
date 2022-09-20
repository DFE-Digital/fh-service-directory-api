using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralServices;
using FamilyHubs.SharedKernel;
using FluentAssertions;
using System.Text.Json;

namespace FamilyHubs.ServiceDirectoryApi.FunctionalTests;

[Collection("Sequential")]
public class WhenUsingOpenReferralServiceApiUnitTests : BaseWhenUsingOpenReferralApiUnitTests
{
    [Fact]
    public async Task ThenTheOpenReferralServicesIsDeleted()
    {
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Delete,
            RequestUri = new Uri(_client.BaseAddress + $"api/services/96781fd9-95a2-4196-8db6-0f083f1c38fc")
        };

        using var response = await _client.SendAsync(request);

        response.EnsureSuccessStatusCode();

        var retVal = await JsonSerializer.DeserializeAsync<bool>(await response.Content.ReadAsStreamAsync(), options: new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        retVal.Should().Be(true);
    }

    [Fact]
    public async Task ThenTheOpenReferralServicesAreRetrieved()
    {
        GetServicesUrlBuilder getServicesUrlBuilder = new GetServicesUrlBuilder();
        string url = getServicesUrlBuilder
                    .WithStatus("active")
                    .WithEligibility(0,99)
                    .WithProximity(52.6312, -1.66526, 1609.34)
                    .WithPage(1, 10)
                    .Build();

        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri(_client.BaseAddress + $"api/services{url}")
        };

        using var response = await _client.SendAsync(request);

        response.EnsureSuccessStatusCode();


        var retVal = await JsonSerializer.DeserializeAsync<PaginatedList<OpenReferralServiceDto>>(await response.Content.ReadAsStreamAsync(), options: new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        var item = retVal?.Items.FirstOrDefault(x => x.Id == "4591d551-0d6a-4c0d-b109-002e67318231");

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        retVal.Should().NotBeNull();
        item.Should().NotBeNull();
        ArgumentNullException.ThrowIfNull(item, nameof(item));
        item.Id.Should().Be("4591d551-0d6a-4c0d-b109-002e67318231");
    }

    [Fact]
    public async Task ThenTheOpenReferralServicesWithEligabiltyAreRetrieved()
    {
        GetServicesUrlBuilder getServicesUrlBuilder = new GetServicesUrlBuilder();
        string url = getServicesUrlBuilder
                    .WithStatus("active")
                    .WithEligibility(0, 99)
                    .WithPage(1, 10)
                    .Build();

        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri(_client.BaseAddress + $"api/services{url}")
        };

        using var response = await _client.SendAsync(request);

        response.EnsureSuccessStatusCode();


        var retVal = await JsonSerializer.DeserializeAsync<PaginatedList<OpenReferralServiceDto>>(await response.Content.ReadAsStreamAsync(), options: new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        var item = retVal?.Items.FirstOrDefault(x => x.Id == "4591d551-0d6a-4c0d-b109-002e67318231");

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        retVal.Should().NotBeNull();
        item.Should().NotBeNull();
        ArgumentNullException.ThrowIfNull(item, nameof(item));
        item.Id.Should().Be("4591d551-0d6a-4c0d-b109-002e67318231");
    }

    [Fact]
    public async Task ThenTheOpenReferralServicesWithProximityAreRetrieved()
    {
        GetServicesUrlBuilder getServicesUrlBuilder = new GetServicesUrlBuilder();
        string url = getServicesUrlBuilder
                    .WithStatus("active")
                    .WithProximity(52.6312, -1.66526, 1609.34)
                    .WithPage(1, 10)
                    .Build();

        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri(_client.BaseAddress + $"api/services{url}")
        };

        using var response = await _client.SendAsync(request);

        response.EnsureSuccessStatusCode();


        var retVal = await JsonSerializer.DeserializeAsync<PaginatedList<OpenReferralServiceDto>>(await response.Content.ReadAsStreamAsync(), options: new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        var item = retVal?.Items.FirstOrDefault(x => x.Id == "4591d551-0d6a-4c0d-b109-002e67318231");

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        retVal.Should().NotBeNull();
        item.Should().NotBeNull();
        ArgumentNullException.ThrowIfNull(item, nameof(item));
        item.Id.Should().Be("4591d551-0d6a-4c0d-b109-002e67318231");
    }

    [Fact]
    public async Task ThenTheOpenReferralServicesWithServiceDeliveryAreRetrieved()
    {
        GetServicesUrlBuilder getServicesUrlBuilder = new GetServicesUrlBuilder();
        string url = getServicesUrlBuilder
                    .WithStatus("active")
                    .WithDelimitedSearchDeliveries("online")
                    .WithPage(1, 10)
                    .Build();

        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri(_client.BaseAddress + $"api/services{url}")
        };

        using var response = await _client.SendAsync(request);

        response.EnsureSuccessStatusCode();

        var retVal = await JsonSerializer.DeserializeAsync<PaginatedList<OpenReferralServiceDto>>(await response.Content.ReadAsStreamAsync(), options: new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        var item = retVal?.Items.FirstOrDefault(x => x.Id == "4591d551-0d6a-4c0d-b109-002e67318231");

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        retVal.Should().NotBeNull();
        item.Should().NotBeNull();
        ArgumentNullException.ThrowIfNull(item, nameof(item));
        item.Id.Should().Be("4591d551-0d6a-4c0d-b109-002e67318231");
    }

    [Fact]
    public async Task ThenTheOpenReferralServicesWithTaxonomiesAreRetrieved()
    {
        GetServicesUrlBuilder getServicesUrlBuilder = new GetServicesUrlBuilder();
        string url = getServicesUrlBuilder
                    .WithStatus("active")
                    .WithDelimitedTaxonomies("bccprimaryservicetype:38,bccagegroup:37")
                    .WithPage(1, 10)
                    .Build();

        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri(_client.BaseAddress + $"api/services{url}")
        };

        using var response = await _client.SendAsync(request);

        response.EnsureSuccessStatusCode();

        var retVal = await JsonSerializer.DeserializeAsync<PaginatedList<OpenReferralServiceDto>>(await response.Content.ReadAsStreamAsync(), options: new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        var item = retVal?.Items.FirstOrDefault(x => x.Id == "4591d551-0d6a-4c0d-b109-002e67318231");

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        retVal.Should().NotBeNull();
        item.Should().NotBeNull();
        ArgumentNullException.ThrowIfNull(item, nameof(item));
        item.Id.Should().Be("4591d551-0d6a-4c0d-b109-002e67318231");
    }

    [Fact]
    public async Task ThenTheOpenReferralServiceByIdIsRetrieved()
    {
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri(_client.BaseAddress + "api/services/4591d551-0d6a-4c0d-b109-002e67318231"),

        };

        using var response = await _client.SendAsync(request);

        response.EnsureSuccessStatusCode();

        var retVal = await JsonSerializer.DeserializeAsync<OpenReferralServiceDto>(await response.Content.ReadAsStreamAsync(), options: new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        retVal.Should().NotBeNull();
        ArgumentNullException.ThrowIfNull(retVal, nameof(retVal));
        retVal.Id.Should().Be("4591d551-0d6a-4c0d-b109-002e67318231");
    }

    [Fact]
    public async Task ThenTheOpenReferralServicesWithinTheOrganisationAreRetrieved()
    {
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri(_client.BaseAddress + "api/organisationservices/72e653e8-1d05-4821-84e9-9177571a6013"),
        };

        using var response = await _client.SendAsync(request);

        response.EnsureSuccessStatusCode();


        var retVal = await JsonSerializer.DeserializeAsync<List<OpenReferralServiceDto>>(await response.Content.ReadAsStreamAsync(), options: new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        var firstService = retVal?.FirstOrDefault();

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        retVal.Should().NotBeNull();
        firstService.Should().NotBeNull();
        ArgumentNullException.ThrowIfNull(firstService, nameof(firstService));
        firstService.Id.Should().Be("4591d551-0d6a-4c0d-b109-002e67318231");
    }
}
