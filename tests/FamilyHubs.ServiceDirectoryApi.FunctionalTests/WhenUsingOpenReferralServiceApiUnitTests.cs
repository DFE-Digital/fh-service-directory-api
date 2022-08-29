//using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralServices;
//using FamilyHubs.SharedKernel;
//using FluentAssertions;
//using System.Text.Json;

//namespace FamilyHubs.ServiceDirectoryApi.FunctionalTests;

//[Collection("Sequential")]
//public class WhenUsingOpenReferralServiceApiUnitTests : BaseWhenUsingOpenReferralApiUnitTests
//{
//    [Fact]
//    public async Task ThenTheOpenReferralServicesAreRetrieved()
//    {
//        var request = new HttpRequestMessage
//        {
//            Method = HttpMethod.Get,
//            RequestUri = new Uri(_client.BaseAddress + "api/services?status=active&minimum_age=0&maximum_age=99&latitude=52.6312&longtitude=-1.66526&proximity=1609.34&pageNumber=1&pageSize=10&text="),
//        };

//        using var response = await _client.SendAsync(request);

//        response.EnsureSuccessStatusCode();


//        var retVal = await JsonSerializer.DeserializeAsync<PaginatedList<OpenReferralServiceDto>>(await response.Content.ReadAsStreamAsync(), options: new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
//        var item = retVal?.Items.FirstOrDefault(x => x.Id == "4591d551-0d6a-4c0d-b109-002e67318231");

//        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
//        retVal.Should().NotBeNull();
//        item.Should().NotBeNull();
//        ArgumentNullException.ThrowIfNull(item, nameof(item));
//        item.Id.Should().Be("4591d551-0d6a-4c0d-b109-002e67318231");
//    }

//    [Fact]
//    public async Task ThenTheOpenReferralServiceIsRetrieved()
//    {
//        var request = new HttpRequestMessage
//        {
//            Method = HttpMethod.Get,
//            RequestUri = new Uri(_client.BaseAddress + "api/services/4591d551-0d6a-4c0d-b109-002e67318231"),

//        };

//        using var response = await _client.SendAsync(request);

//        response.EnsureSuccessStatusCode();

//        var retVal = await JsonSerializer.DeserializeAsync<OpenReferralServiceDto>(await response.Content.ReadAsStreamAsync(), options: new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

//        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
//        retVal.Should().NotBeNull();
//        ArgumentNullException.ThrowIfNull(retVal, nameof(retVal));
//        retVal.Id.Should().Be("4591d551-0d6a-4c0d-b109-002e67318231");
//    }

//    [Fact]
//    public async Task ThenTheOpenReferralServicesWithinTheOrganisationAreRetrieved()
//    {
//        var request = new HttpRequestMessage
//        {
//            Method = HttpMethod.Get,
//            RequestUri = new Uri(_client.BaseAddress + "api/organisationservices/72e653e8-1d05-4821-84e9-9177571a6013"),
//        };

//        using var response = await _client.SendAsync(request);

//        response.EnsureSuccessStatusCode();


//        var retVal = await JsonSerializer.DeserializeAsync<List<OpenReferralServiceDto>>(await response.Content.ReadAsStreamAsync(), options: new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
//        var firstService = retVal?.FirstOrDefault();

//        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
//        retVal.Should().NotBeNull();
//        firstService.Should().NotBeNull();
//        ArgumentNullException.ThrowIfNull(firstService, nameof(firstService));
//        firstService.Id.Should().Be("4591d551-0d6a-4c0d-b109-002e67318231");
//    }
//}
