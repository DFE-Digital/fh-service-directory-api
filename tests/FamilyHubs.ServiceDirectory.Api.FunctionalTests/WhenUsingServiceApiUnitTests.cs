using System.Net;
using System.Text;
using System.Text.Json;
using FamilyHubs.ServiceDirectory.Shared.Dto;
using FamilyHubs.ServiceDirectory.Shared.Models;
using FluentAssertions;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace FamilyHubs.ServiceDirectory.Api.FunctionalTests;

[Collection("Sequential")]
public class WhenUsingServiceApiUnitTests : BaseWhenUsingApiUnitTests
{
    [Fact]
    public async Task ThenTheServiceIsCreated()
    {
        var service = TestDataProvider.GetTestCountyCouncilServicesCreateRecord(1);

        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri(Client.BaseAddress + "api/services"),
            Content = new StringContent(JsonConvert.SerializeObject(service), Encoding.UTF8, "application/json"),
        };

        using var response = await Client.SendAsync(request);

        var responseContent = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            ArgumentException.ThrowIfNullOrEmpty(responseContent);

        response.StatusCode.Should().Be(HttpStatusCode.OK, responseContent);
        long.Parse(responseContent).Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task ThenTheServiceIsUpdated()
    {
        var getServicesUrlBuilder = new GetServicesUrlBuilder();
        var url = getServicesUrlBuilder
                    .WithServiceType("InformationSharing")
                    .WithStatus("Active")
                    .WithEligibility(0, 99)
                    .WithProximity(52.6312, -1.66526, 1609.34)
                    .WithPage(1, 10)
                    .Build();

        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri(Client.BaseAddress + $"api/services{url}")
        };

        using var response = await Client.SendAsync(request);

        response.EnsureSuccessStatusCode();

        var retVal = await JsonSerializer.DeserializeAsync<PaginatedList<ServiceDto>>(await response.Content.ReadAsStreamAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        var item = retVal?.Items.FirstOrDefault(x => x.ServiceOwnerReferenceId == "Bristol-Service-2");
        ArgumentNullException.ThrowIfNull(item);

        item.Name += " Changed";
        item.Description += " Changed";

        var updateRequest = new HttpRequestMessage
        {
            Method = HttpMethod.Put,
            RequestUri = new Uri(Client.BaseAddress + $"api/services/{item.Id}"),
            Content = new StringContent(JsonConvert.SerializeObject(item), Encoding.UTF8, "application/json"),
        };

        using var updateResponse = await Client.SendAsync(updateRequest);

        updateResponse.EnsureSuccessStatusCode();

        var stringResult = await updateResponse.Content.ReadAsStringAsync();

        updateResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        stringResult.Should().Be("2");

    }

    [Fact]
    public async Task ThenTheServicesIsDeleted()
    {
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Delete,
            RequestUri = new Uri(Client.BaseAddress + "api/services/1")
        };

        using var response = await Client.SendAsync(request);

        response.EnsureSuccessStatusCode();

        var retVal = await JsonSerializer.DeserializeAsync<bool>(await response.Content.ReadAsStreamAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        retVal.Should().Be(true);
    }

    [Fact]
    public async Task ThenTheServicesAreRetrieved()
    {
        var getServicesUrlBuilder = new GetServicesUrlBuilder();
        var url = getServicesUrlBuilder
                    .WithServiceType("InformationSharing")
                    .WithStatus("Active")
                    .WithEligibility(0,99)
                    .WithProximity(52.6312, -1.66526, 1609.34)
                    .WithPage(1, 10)
                    .Build();

        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri(Client.BaseAddress + $"api/services{url}")
        };

        using var response = await Client.SendAsync(request);

        response.EnsureSuccessStatusCode();


        var retVal = await JsonSerializer.DeserializeAsync<PaginatedList<ServiceDto>>(await response.Content.ReadAsStreamAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        var item = retVal?.Items.FirstOrDefault(x => x.ServiceOwnerReferenceId == "Bristol-Service-2");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        retVal.Should().NotBeNull();
        item.Should().NotBeNull();
        ArgumentNullException.ThrowIfNull(item);
        item.ServiceOwnerReferenceId.Should().Be("Bristol-Service-2");
    }

    [Fact]
    public async Task ThenTheServicesWithEligibilityAreRetrieved()
    {
        var getServicesUrlBuilder = new GetServicesUrlBuilder();
        var url = getServicesUrlBuilder
                    .WithServiceType("InformationSharing")
                    .WithStatus("Active")
                    .WithEligibility(0, 99)
                    .WithPage(1, 10)
                    .Build();

        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri(Client.BaseAddress + $"api/services{url}")
        };

        using var response = await Client.SendAsync(request);

        response.EnsureSuccessStatusCode();


        var retVal = await JsonSerializer.DeserializeAsync<PaginatedList<ServiceDto>>(await response.Content.ReadAsStreamAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        var item = retVal?.Items.FirstOrDefault(x => x.ServiceOwnerReferenceId == "Bristol-Service-2");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        retVal.Should().NotBeNull();
        item.Should().NotBeNull();
        ArgumentNullException.ThrowIfNull(item);
        item.ServiceOwnerReferenceId.Should().Be("Bristol-Service-2");
    }

    [Fact]
    public async Task ThenTheServicesWithProximityAreRetrieved()
    {
        var getServicesUrlBuilder = new GetServicesUrlBuilder();
        var url = getServicesUrlBuilder
                    .WithServiceType("InformationSharing")
                    .WithStatus("Active")
                    .WithProximity(52.6312, -1.66526, 1609.34)
                    .WithPage(1, 10)
                    .Build();

        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri(Client.BaseAddress + $"api/services{url}")
        };

        using var response = await Client.SendAsync(request);

        response.EnsureSuccessStatusCode();


        var retVal = await JsonSerializer.DeserializeAsync<PaginatedList<ServiceDto>>(await response.Content.ReadAsStreamAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        var item = retVal?.Items.FirstOrDefault(x => x.ServiceOwnerReferenceId == "Bristol-Service-2");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        retVal.Should().NotBeNull();
        item.Should().NotBeNull();
        ArgumentNullException.ThrowIfNull(item);
        item.ServiceOwnerReferenceId.Should().Be("Bristol-Service-2");
    }

    [Fact]
    public async Task ThenTheServicesWithServiceDeliveryAreRetrieved()
    {
        var getServicesUrlBuilder = new GetServicesUrlBuilder();
        var url = getServicesUrlBuilder
                    .WithServiceType("InformationSharing")
                    .WithStatus("Active")
                    .WithDelimitedSearchDeliveries("online")
                    .WithPage(1, 10)
                    .Build();

        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri(Client.BaseAddress + $"api/services{url}")
        };

        using var response = await Client.SendAsync(request);

        response.EnsureSuccessStatusCode();

        var retVal = await JsonSerializer.DeserializeAsync<PaginatedList<ServiceDto>>(await response.Content.ReadAsStreamAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        var item = retVal?.Items.FirstOrDefault(x => x.ServiceOwnerReferenceId == "Bristol-Service-1");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        retVal.Should().NotBeNull();
        item.Should().NotBeNull();
        ArgumentNullException.ThrowIfNull(item);
        item.ServiceOwnerReferenceId.Should().Be("Bristol-Service-1");
    }

    [Fact]
    public async Task ThenTheServicesWithTaxonomiesAreRetrieved()
    {
        var getServicesUrlBuilder = new GetServicesUrlBuilder();
        var url = getServicesUrlBuilder
                    .WithServiceType("InformationSharing")
                    .WithStatus("Active")
                    .WithDelimitedTaxonomies("1")
                    .WithPage(1, 10)
                    .Build();

        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri(Client.BaseAddress + $"api/services{url}")
        };

        using var response = await Client.SendAsync(request);

        response.EnsureSuccessStatusCode();

        var retVal = await JsonSerializer.DeserializeAsync<PaginatedList<ServiceDto>>(await response.Content.ReadAsStreamAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        var item = retVal?.Items.FirstOrDefault(x => x.ServiceOwnerReferenceId == "Bristol-Service-2");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        retVal.Should().NotBeNull();
        item.Should().NotBeNull();
        ArgumentNullException.ThrowIfNull(item);
        item.ServiceOwnerReferenceId.Should().Be("Bristol-Service-2");
    }

    [Fact]
    public async Task ThenTheServiceByIdIsRetrieved()
    {
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri(Client.BaseAddress + "api/services/1"),
        };

        using var response = await Client.SendAsync(request);

        response.EnsureSuccessStatusCode();

        var retVal = await JsonSerializer.DeserializeAsync<ServiceDto>(await response.Content.ReadAsStreamAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        retVal.Should().NotBeNull();
        ArgumentNullException.ThrowIfNull(retVal);
        retVal.ServiceOwnerReferenceId.Should().Be("Bristol-Service-1");
    }

    [Fact]
    public async Task ThenTheServicesWithinTheOrganisationAreRetrieved()
    {
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri(Client.BaseAddress + "api/organisationservices/1"),
        };

        using var response = await Client.SendAsync(request);

        response.EnsureSuccessStatusCode();


        var retVal = await JsonSerializer.DeserializeAsync<List<ServiceDto>>(await response.Content.ReadAsStreamAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        var firstService = retVal?.FirstOrDefault();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        retVal.Should().NotBeNull();
        firstService.Should().NotBeNull();
        ArgumentNullException.ThrowIfNull(firstService);
        firstService.ServiceOwnerReferenceId.Should().Be("Bristol-Service-1");
    }

    [Fact]
    public async Task ThenTheServicesWithFamilyHubsAreRetrieved()
    {
        var getServicesUrlBuilder = new GetServicesUrlBuilder();
        var url = getServicesUrlBuilder
                    .WithStatus("Active")
                    .WithServiceType("FamilyExperience")
                    .WithFamilyHub(true)
                    .WithEligibility(0, 99)
                    .WithProximity(53.507025D, -2.259764D, 32186.9)
                    .WithPage(1, 10)
                    .Build();

        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri(Client.BaseAddress + $"api/services{url}")
        };

        using var response = await Client.SendAsync(request);

        var responseContent = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            ArgumentException.ThrowIfNullOrEmpty(responseContent);

        var retVal = JsonSerializer.Deserialize<PaginatedList<ServiceDto>>(responseContent, new JsonSerializerOptions(JsonSerializerDefaults.Web));

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        retVal.Should().NotBeNull();
        retVal?.Items.Count.Should().BeGreaterThan(2);
    }

    [Fact]
    public async Task ThenTheServicesWithOutFamilyHubsAreRetrieved()
    {
        var getServicesUrlBuilder = new GetServicesUrlBuilder();
        var url = getServicesUrlBuilder
                    .WithStatus("Active")
                    .WithServiceType("FamilyExperience")
                    .WithFamilyHub(false)
                    .WithEligibility(0, 99)
                    .WithProximity(53.507025D, -2.259764D, 32186.9)
                    .WithPage(1, 10)
                    .Build();

        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri(Client.BaseAddress + $"api/services{url}")
        };

        using var response = await Client.SendAsync(request);

        var responseContent = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            ArgumentException.ThrowIfNullOrEmpty(responseContent);

        var retVal = JsonSerializer.Deserialize<PaginatedList<ServiceDto>>(responseContent, new JsonSerializerOptions(JsonSerializerDefaults.Web));

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        retVal.Should().NotBeNull();
        retVal?.Items.Count.Should().Be(2);
    }

    [Fact]
    public async Task ThenTheServicesLimitedByMaxFamilyHubsAreRetrieved()
    {
        var getServicesUrlBuilder = new GetServicesUrlBuilder();
        var url = getServicesUrlBuilder
            .WithServiceType("FamilyExperience")
            .WithStatus("Active")
            .WithMaxFamilyHubs(1)
            .WithPage(1, 10)
            .Build();

        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri(Client.BaseAddress + $"api/services{url}")
        };

        using var response = await Client.SendAsync(request);

        var responseContent = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            ArgumentException.ThrowIfNullOrEmpty(responseContent);

        var retVal = JsonSerializer.Deserialize<PaginatedList<ServiceDto>>(responseContent, new JsonSerializerOptions(JsonSerializerDefaults.Web));

        var items = retVal?.Items;
   
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        retVal.Should().NotBeNull();
        items.Should().NotBeNull();

        items!.Where(i => i.Description == "Family Hub").Should().HaveCount(1);
    }

    [Fact]
    public async Task ThenTheServicesByOwnerReferenceIdAreRetrieved()
    {
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri(Client.BaseAddress + "api/servicesByOwnerReference/Bristol-Service-1")
        };

        using var response = await Client.SendAsync(request);

        var responseContent = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            ArgumentException.ThrowIfNullOrEmpty(responseContent);

        var retVal = JsonSerializer.Deserialize<ServiceDto>(responseContent, new JsonSerializerOptions(JsonSerializerDefaults.Web));

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        retVal.Should().NotBeNull();
        retVal!.ServiceOwnerReferenceId.Should().Be("Bristol-Service-1");
    }
}
