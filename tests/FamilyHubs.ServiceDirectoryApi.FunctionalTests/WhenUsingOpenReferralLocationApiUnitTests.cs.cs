﻿using FamilyHubs.ServiceDirectory.Shared.Models.Api.AccessibilityForDisabilities;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralHolidaySchedule;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralLocations;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralPhysicalAddresses;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralRegularSchedule;
using fh_service_directory_api.core.Entities;
using FluentAssertions;
using System.Text;

namespace FamilyHubs.ServiceDirectoryApi.FunctionalTests;


[Collection("Sequential")]
public class WhenUsingOpenReferralLocationApiUnitTests : BaseWhenUsingOpenReferralApiUnitTests
{
#if DEBUG
    [Fact]
#else
    [Fact(Skip = "This test should be run locally")]
#endif
    public async Task ThenTheOpenReferralLocationIsCreated()
    {
        var command = new OpenReferralLocationDto("25cd5229-f90c-4243-9ce6-9f0f7a718feb", "Central Test Hub", "Test Hub", -2.459764D, 53.607025D, new List<OpenReferralPhysicalAddressDto>() { new OpenReferralPhysicalAddressDto("9bdc326f-3ea4-4569-87a8-985b66eb412f", "Test Street", "Manchester", "M7 1BQ", "United Kingdom", "Salford") });

        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri(_client.BaseAddress + "api/location/d242700a-b2ad-42fe-8848-61534002156c/ca8ddaeb-b5e5-46c4-b94d-43a8e2ccc066"),
            Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(command), Encoding.UTF8, "application/json"),
        };

        using var response = await _client.SendAsync(request);

        response.EnsureSuccessStatusCode();

        var stringResult = await response.Content.ReadAsStringAsync();

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        stringResult.ToString().Should().Be("25cd5229-f90c-4243-9ce6-9f0f7a718feb");
    }

#if DEBUG
    [Fact]
#else
    [Fact(Skip = "This test should be run locally")]
#endif
    public async Task ThenTheOpenReferralLocationIsAFamilyHub()
    {
        var command = new OpenReferralLocationDto(
                            "00768BBC-E830-4F94-9126-FB44CD31204D",
                            "",
                            "",
                            52.6312,
                            -1.66526,
                            new List<OpenReferralPhysicalAddressDto>(new List<OpenReferralPhysicalAddressDto>()
                            {
                                new OpenReferralPhysicalAddressDto(
                                    "bf962211-48d3-4332-a46f-273a8b1ccdce",
                                    "1A Del's Ave",
                                    "Salford",
                                    "ABC 123",
                                    "England",
                                    null
                                    )
                            })
                            );

        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Put,
            RequestUri = new Uri(_client.BaseAddress + "api/location/d242700a-b2ad-42fe-8848-61534002156c/ca8ddaeb-b5e5-46c4-b94d-43a8e2ccc066"),
            Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(command), Encoding.UTF8, "application/json"),
        };

        using var response = await _client.SendAsync(request);

        response.EnsureSuccessStatusCode();

        var stringResult = await response.Content.ReadAsStringAsync();

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        stringResult.ToString().Should().Be("00768BBC-E830-4F94-9126-FB44CD31204D");
    }
}
