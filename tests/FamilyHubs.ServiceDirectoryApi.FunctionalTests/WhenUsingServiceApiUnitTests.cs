using System.Net;
using System.Text;
using System.Text.Json;
using FamilyHubs.ServiceDirectory.Shared.Dto;
using FamilyHubs.SharedKernel;
using FluentAssertions;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace FamilyHubs.ServiceDirectoryApi.FunctionalTests;

[Collection("Sequential")]
public class WhenUsingServiceApiUnitTests : BaseWhenUsingApiUnitTests
{
#if DEBUG
    [Fact]
#else
    [Fact(Skip = "This test should be run locally")]
#endif
    public async Task ThenTheServicesIsDeleted()
    {
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Delete,
            RequestUri = new Uri(_client.BaseAddress + "api/services/96781fd9-95a2-4196-8db6-0f083f1c38fc")
        };

        using var response = await _client.SendAsync(request);

        response.EnsureSuccessStatusCode();

        var retVal = await JsonSerializer.DeserializeAsync<bool>(await response.Content.ReadAsStreamAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        retVal.Should().Be(true);
    }

#if DEBUG
    [Fact]
#else
    [Fact(Skip = "This test should be run locally")]
#endif
    public async Task ThenTheServicesAreRetrieved()
    {
        var getServicesUrlBuilder = new GetServicesUrlBuilder();
        var url = getServicesUrlBuilder
                    .WithServiceType("Information Sharing")
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


        var retVal = await JsonSerializer.DeserializeAsync<PaginatedList<ServiceDto>>(await response.Content.ReadAsStreamAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        var item = retVal?.Items.FirstOrDefault(x => x.Id == "4591d551-0d6a-4c0d-b109-002e67318231");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        retVal.Should().NotBeNull();
        item.Should().NotBeNull();
        ArgumentNullException.ThrowIfNull(item);
        item.Id.Should().Be("4591d551-0d6a-4c0d-b109-002e67318231");
    }

#if DEBUG
    [Fact]
#else
    [Fact(Skip = "This test should be run locally")]
#endif
    public async Task ThenTheServicesWithEligabiltyAreRetrieved()
    {
        var getServicesUrlBuilder = new GetServicesUrlBuilder();
        var url = getServicesUrlBuilder
                    .WithServiceType("Information Sharing")
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


        var retVal = await JsonSerializer.DeserializeAsync<PaginatedList<ServiceDto>>(await response.Content.ReadAsStreamAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        var item = retVal?.Items.FirstOrDefault(x => x.Id == "4591d551-0d6a-4c0d-b109-002e67318231");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        retVal.Should().NotBeNull();
        item.Should().NotBeNull();
        ArgumentNullException.ThrowIfNull(item);
        item.Id.Should().Be("4591d551-0d6a-4c0d-b109-002e67318231");
    }

#if DEBUG
    [Fact]
#else
    [Fact(Skip = "This test should be run locally")]
#endif
    public async Task ThenTheServicesWithProximityAreRetrieved()
    {
        var getServicesUrlBuilder = new GetServicesUrlBuilder();
        var url = getServicesUrlBuilder
                    .WithServiceType("Information Sharing")
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


        var retVal = await JsonSerializer.DeserializeAsync<PaginatedList<ServiceDto>>(await response.Content.ReadAsStreamAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        var item = retVal?.Items.FirstOrDefault(x => x.Id == "4591d551-0d6a-4c0d-b109-002e67318231");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        retVal.Should().NotBeNull();
        item.Should().NotBeNull();
        ArgumentNullException.ThrowIfNull(item);
        item.Id.Should().Be("4591d551-0d6a-4c0d-b109-002e67318231");
    }

#if DEBUG
    [Fact]
#else
    [Fact(Skip = "This test should be run locally")]
#endif
    public async Task ThenTheServicesWithServiceDeliveryAreRetrieved()
    {
        var getServicesUrlBuilder = new GetServicesUrlBuilder();
        var url = getServicesUrlBuilder
                    .WithServiceType("Information Sharing")
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

        var retVal = await JsonSerializer.DeserializeAsync<PaginatedList<ServiceDto>>(await response.Content.ReadAsStreamAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        var item = retVal?.Items.FirstOrDefault(x => x.Id == "4591d551-0d6a-4c0d-b109-002e67318231");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        retVal.Should().NotBeNull();
        item.Should().NotBeNull();
        ArgumentNullException.ThrowIfNull(item);
        item.Id.Should().Be("4591d551-0d6a-4c0d-b109-002e67318231");
    }

#if DEBUG
    [Fact]
#else
    [Fact(Skip = "This test should be run locally")]
#endif
    public async Task ThenTheServicesWithTaxonomiesAreRetrieved()
    {
        var getServicesUrlBuilder = new GetServicesUrlBuilder();
        var url = getServicesUrlBuilder
                    .WithServiceType("Information Sharing")
                    .WithStatus("active")
                    .WithDelimitedTaxonomies("11696b1f-209a-47b1-9ef5-c588a14d43c6")
                    .WithPage(1, 10)
                    .Build();

        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri(_client.BaseAddress + $"api/services{url}")
        };

        using var response = await _client.SendAsync(request);

        response.EnsureSuccessStatusCode();

        var retVal = await JsonSerializer.DeserializeAsync<PaginatedList<ServiceDto>>(await response.Content.ReadAsStreamAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        var item = retVal?.Items.FirstOrDefault(x => x.Id == "4591d551-0d6a-4c0d-b109-002e67318231");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        retVal.Should().NotBeNull();
        item.Should().NotBeNull();
        ArgumentNullException.ThrowIfNull(item);
        item.Id.Should().Be("4591d551-0d6a-4c0d-b109-002e67318231");
    }

#if DEBUG
    [Fact]
#else
    [Fact(Skip = "This test should be run locally")]
#endif
    public async Task ThenTheServiceByIdIsRetrieved()
    {
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri(_client.BaseAddress + "api/services/4591d551-0d6a-4c0d-b109-002e67318231"),

        };

        using var response = await _client.SendAsync(request);

        response.EnsureSuccessStatusCode();

        var retVal = await JsonSerializer.DeserializeAsync<ServiceDto>(await response.Content.ReadAsStreamAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        retVal.Should().NotBeNull();
        ArgumentNullException.ThrowIfNull(retVal);
        retVal.Id.Should().Be("4591d551-0d6a-4c0d-b109-002e67318231");
    }

#if DEBUG
    [Fact]
#else
    [Fact(Skip = "This test should be run locally")]
#endif
    public async Task ThenTheServicesWithinTheOrganisationAreRetrieved()
    {
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri(_client.BaseAddress + "api/organisationservices/72e653e8-1d05-4821-84e9-9177571a6013"),
        };

        using var response = await _client.SendAsync(request);

        response.EnsureSuccessStatusCode();


        var retVal = await JsonSerializer.DeserializeAsync<List<ServiceDto>>(await response.Content.ReadAsStreamAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        var firstService = retVal?.FirstOrDefault();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        retVal.Should().NotBeNull();
        firstService.Should().NotBeNull();
        ArgumentNullException.ThrowIfNull(firstService);
        firstService.Id.Should().Be("39f211fc-73d4-4ea1-a1c9-d8fc063d6b51");
    }

#if DEBUG
    [Fact]
#else
    [Fact(Skip = "This test should be run locally")]
#endif
    public async Task ThenTheServiceIsUpdated()
    {
        var getServicesUrlBuilder = new GetServicesUrlBuilder();
        var url = getServicesUrlBuilder
                    .WithServiceType("Information Sharing")
                    .WithStatus("active")
                    .WithEligibility(0, 99)
                    .WithProximity(52.6312, -1.66526, 1609.34)
                    .WithPage(1, 10)
                    .Build();

        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri(_client.BaseAddress + $"api/services{url}")
        };

        //request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue($"Bearer", $"{new JwtSecurityTokenHandler().WriteToken(_token)}");

        using var response = await _client.SendAsync(request);

        response.EnsureSuccessStatusCode();


        var retVal = await JsonSerializer.DeserializeAsync<PaginatedList<ServiceDto>>(await response.Content.ReadAsStreamAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        var item = retVal?.Items.FirstOrDefault(x => x.Id == "4591d551-0d6a-4c0d-b109-002e67318231");
        ArgumentNullException.ThrowIfNull(item);

        item.Name += " Changed";
        item.Description += " Changed";

        var updaterequest = new HttpRequestMessage
        {
            Method = HttpMethod.Put,
            RequestUri = new Uri(_client.BaseAddress + "api/services/4591d551-0d6a-4c0d-b109-002e67318231"),
            Content = new StringContent(JsonConvert.SerializeObject(item), Encoding.UTF8, "application/json"),
        };

        //updaterequest.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue($"Bearer", $"{new JwtSecurityTokenHandler().WriteToken(_token)}");

        using var updateresponse = await _client.SendAsync(updaterequest);

        updateresponse.EnsureSuccessStatusCode();

        var stringResult = await updateresponse.Content.ReadAsStringAsync();

        updateresponse.StatusCode.Should().Be(HttpStatusCode.OK);
        stringResult.Should().Be("4591d551-0d6a-4c0d-b109-002e67318231");

    }

#if DEBUG
    [Fact]
#else
    [Fact(Skip = "This test should be run locally")]
#endif
    public async Task ThenTheServiceIsCreated()
    {
        var service = WhenUsingOrganisationApiUnitTests.GetTestCountyCouncilServicesCreateRecord("72e653e8-1d05-4821-84e9-9177571a6013");

        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri(_client.BaseAddress + "api/services"),
            Content = new StringContent(JsonConvert.SerializeObject(service), Encoding.UTF8, "application/json"),
        };

        //request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue($"Bearer", $"{new JwtSecurityTokenHandler().WriteToken(_token)}");

        using var response = await _client.SendAsync(request);

        response.EnsureSuccessStatusCode();

        var stringResult = await response.Content.ReadAsStringAsync();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        stringResult.Should().Be(service.Id);

    }

#if DEBUG
    [Fact]
#else
    [Fact(Skip = "This test should be run locally")]
#endif
    public async Task ThenTheServicesWithFamilyHubsAreRetrieved()
    {
        var getServicesUrlBuilder = new GetServicesUrlBuilder();
        var url = getServicesUrlBuilder
                    .WithStatus("active")
                    .WithServiceType("Family Experience")
                    .WithFamilyHub(true)
                    .WithEligibility(0, 99)
                    .WithProximity(53.507025D, -2.259764D, 32186.9)
                    .WithPage(1, 10)
                    .Build();

        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri(_client.BaseAddress + $"api/services{url}")
        };

        using var response = await _client.SendAsync(request);

        response.EnsureSuccessStatusCode();

        var retVal = await JsonSerializer.DeserializeAsync<PaginatedList<ServiceDto>>(await response.Content.ReadAsStreamAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        retVal.Should().NotBeNull();
        retVal?.Items.Count.Should().BeGreaterThan(2);
    }

#if DEBUG
    [Fact]
#else
    [Fact(Skip = "This test should be run locally")]
#endif
    public async Task ThenTheServicesWithOutFamilyHubsAreRetrieved()
    {
        var getServicesUrlBuilder = new GetServicesUrlBuilder();
        var url = getServicesUrlBuilder
                    .WithStatus("active")
                    .WithServiceType("Family Experience")
                    .WithFamilyHub(false)
                    .WithEligibility(0, 99)
                    .WithProximity(53.507025D, -2.259764D, 32186.9)
                    .WithPage(1, 10)
                    .Build();

        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri(_client.BaseAddress + $"api/services{url}")
        };

        using var response = await _client.SendAsync(request);

        response.EnsureSuccessStatusCode();

        var retVal = await JsonSerializer.DeserializeAsync<PaginatedList<ServiceDto>>(await response.Content.ReadAsStreamAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        retVal.Should().NotBeNull();
        retVal?.Items.Count.Should().Be(2);
    }

#if DEBUG
    [Fact]
#else
    [Fact(Skip = "This test should be run locally")]
#endif
    public async Task ThenTheServicesLimitedByMaxFamilyHubsAreRetrieved()
    {
        var getServicesUrlBuilder = new GetServicesUrlBuilder();
        var url = getServicesUrlBuilder
            .WithServiceType("Family Experience")
            .WithStatus("active")
            .WithMaxFamilyHubs(1)
            .WithPage(1, 10)
            .Build();

        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri(_client.BaseAddress + $"api/services{url}")
        };

        using var response = await _client.SendAsync(request);

        response.EnsureSuccessStatusCode();

        var retVal = await JsonSerializer.DeserializeAsync<PaginatedList<ServiceDto>>(await response.Content.ReadAsStreamAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        var items = retVal?.Items;
   
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        retVal.Should().NotBeNull();
        items.Should().NotBeNull();

        items!.Where(i => i.Description == "Family Hub").Should().HaveCount(1);
    }
}
