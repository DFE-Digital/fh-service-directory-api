using System.Net;
using System.Text;
using System.Text.Json;
using FamilyHubs.ServiceDirectory.Core.Entities;
using FamilyHubs.ServiceDirectory.Shared.Builders;
using FamilyHubs.ServiceDirectory.Shared.Dto;
using FamilyHubs.ServiceDirectory.Shared.Enums;
using FluentAssertions;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace FamilyHubs.ServiceDirectoryApi.FunctionalTests;

[Collection("Sequential")]
public class WhenUsingOrganisationApiUnitTests : BaseWhenUsingApiUnitTests
{
#if DEBUG
    [Fact]
#else
    [Fact(Skip = "This test should be run locally")]
#endif
    public async Task ThenTheOrganisationIsCreated()
    {
        var command = GetTestCountyCouncilRecord();

        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri(_client.BaseAddress + "api/organizations"),
            Content = new StringContent(JsonConvert.SerializeObject(command), Encoding.UTF8, "application/json"),
        };

        //request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue($"Bearer", $"{new JwtSecurityTokenHandler().WriteToken(_token)}");

        using var response = await _client.SendAsync(request);

        response.EnsureSuccessStatusCode();

        var stringResult = await response.Content.ReadAsStringAsync();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        stringResult.Should().Be("ba1cca90-b02a-4a0b-afa0-d8aed1083c0d");
    }

#if DEBUG
    [Fact]
#else
    [Fact(Skip = "This test should be run locally")]
#endif
    public async Task ThenTheOrganisationIsRetrieved()
    {
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri(_client.BaseAddress + "api/organizations/72e653e8-1d05-4821-84e9-9177571a6013"),

        };

        using var response = await _client.SendAsync(request);

        response.EnsureSuccessStatusCode();


        var retVal = await JsonSerializer.DeserializeAsync<OrganisationWithServicesDto>(await response.Content.ReadAsStreamAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });


        response.StatusCode.Should().Be(HttpStatusCode.OK);
        retVal.Should().NotBeNull();
        ArgumentNullException.ThrowIfNull(retVal);
        retVal.Id.Should().Be("72e653e8-1d05-4821-84e9-9177571a6013");
    }

#if DEBUG
    [Fact]
#else
    [Fact(Skip = "This test should be run locally")]
#endif
    public async Task ThenListOrganisationsIsRetrieved()
    {
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri(_client.BaseAddress + "api/organizations"),

        };

        using var response = await _client.SendAsync(request);

        response.EnsureSuccessStatusCode();

        var retVal = await JsonSerializer.DeserializeAsync<List<OrganisationDto>>(await response.Content.ReadAsStreamAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        retVal.Should().NotBeNull();
        ArgumentNullException.ThrowIfNull(retVal);
        retVal.Count.Should().BeGreaterThan(0);
    }

#if DEBUG
    [Fact]
#else
    [Fact(Skip = "This test should be run locally")]
#endif
    public async Task ThenListOrganisationTypesIsRetrieved()
    {
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri(_client.BaseAddress + "api/organizationtypes"),

        };

        using var response = await _client.SendAsync(request);

        response.EnsureSuccessStatusCode();

        var retVal = await JsonSerializer.DeserializeAsync<List<OrganisationTypeDto>>(await response.Content.ReadAsStreamAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        retVal.Should().NotBeNull();
        ArgumentNullException.ThrowIfNull(retVal);
        retVal.Count.Should().BeGreaterThan(2);
    }

    //todo: if data seeding fails, the tests should fail fast
    //todo: if not found, server returns a 500, but should return a 404

#if DEBUG
    [Fact]
#else
    [Fact(Skip = "This test should be run locally")]
#endif
    public async Task ThenOrganisationAdminCodeIsRetrieved()
    {
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri(_client.BaseAddress + "api/organizationAdminCode/72e653e8-1d05-4821-84e9-9177571a6013"),
        };

        using var response = await _client.SendAsync(request);

        response.EnsureSuccessStatusCode();

        var stringResult = await response.Content.ReadAsStringAsync();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        stringResult.Should().NotBeNull();
        stringResult.Should().Be("E06000023");
    }


#if DEBUG
    [Fact]
#else
    [Fact(Skip = "This test should be run locally")]
#endif
    public async Task ThenTheOrganisationIsUpdated()
    {
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri(_client.BaseAddress + "api/organizations/72e653e8-1d05-4821-84e9-9177571a6013"),

        };

        //request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue($"Bearer", $"{new JwtSecurityTokenHandler().WriteToken(_token)}");

        using var response = await _client.SendAsync(request);

        response.EnsureSuccessStatusCode();


        var retVal = await JsonSerializer.DeserializeAsync<OrganisationWithServicesDto>(await response.Content.ReadAsStreamAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        ArgumentNullException.ThrowIfNull(retVal);

        var update = new OrganisationWithServicesDto(
            retVal.Id,
            new OrganisationTypeDto(retVal.OrganisationType.Id, retVal.OrganisationType.Name, retVal.OrganisationType.Description),
            retVal.Name,
            retVal.Description + " Update Test",
            retVal.Logo,
            retVal.Uri,
            retVal.Url,
            retVal.Services
            );

        var updaterequest = new HttpRequestMessage
        {
            Method = HttpMethod.Put,
            RequestUri = new Uri(_client.BaseAddress + "api/organizations/72e653e8-1d05-4821-84e9-9177571a6013"),
            Content = new StringContent(JsonConvert.SerializeObject(update), Encoding.UTF8, "application/json"),
        };

        //updaterequest.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue($"Bearer", $"{new JwtSecurityTokenHandler().WriteToken(_token)}");

        using var updateresponse = await _client.SendAsync(updaterequest);

        updateresponse.EnsureSuccessStatusCode();

        var stringResult = await updateresponse.Content.ReadAsStringAsync();

        updateresponse.StatusCode.Should().Be(HttpStatusCode.OK);
        stringResult.Should().Be("72e653e8-1d05-4821-84e9-9177571a6013");
    }

    public IReadOnlyCollection<Organisation> GetTestOrganisations()
    {
        var organisations = new List<Organisation>
        {
            GetTestCountyCouncil()
        };

        return organisations;
    }

    private OrganisationWithServicesDto GetTestCountyCouncilRecord()
    {
        var bristolCountyCouncil = new OrganisationWithServicesDto(
            "ba1cca90-b02a-4a0b-afa0-d8aed1083c0d",
            new OrganisationTypeDto("1", "LA", "Local Authority"),
            "Test County Council",
            "Test County Council",
            null,
            new Uri("https://www.test.gov.uk/").ToString(),
            "https://www.test.gov.uk/",
            new List<ServiceDto>
            {
                 GetTestCountyCouncilServicesRecord("ba1cca90-b02a-4a0b-afa0-d8aed1083c0d")
            }
            )
        {
            AdminAreaCode = "E06000023"
        };

        return bristolCountyCouncil;
    }

    public static ServiceDto GetTestCountyCouncilServicesRecord(string parentId)
    {
        var contactId = Guid.NewGuid().ToString();

        var builder = new ServicesDtoBuilder();
        var service = builder.WithMainProperties("c1b5dd80-7506-4424-9711-fe175fa13eb8",
                new ServiceTypeDto("1", "Information Sharing", ""),
                parentId,
                "Test Organisation for Children with Tracheostomies",
                @"Test Organisation for for Children with Tracheostomies is a national self help group operating as a registered charity and is run by parents of children with a tracheostomy and by people who sympathise with the needs of such families. ACT as an organisation is non profit making, it links groups and individual members throughout Great Britain and Northern Ireland.",
                null,
                null,
                null,
                null,
                null,
                "active",
                null, false)
            .WithServiceDelivery(new List<ServiceDeliveryDto>
                {
                    new ServiceDeliveryDto(Guid.NewGuid().ToString(),ServiceDeliveryType.Online)
                })
            .WithEligibility(new List<EligibilityDto>
                {
                    new EligibilityDto("Test9109Children","Test9109Children",0,13)
                })
            .WithLinkContact(new List<LinkContactDto>
            {
                new LinkContactDto(
                    "c1b5dd80-7506-4424-9711-fe175fa13eb1",
                    "c1b5dd80-7506-4424-9711-fe175fa13eb8",
                    "Service",
                new ContactDto(
                    contactId,
                    "Service",
                    string.Empty,
                    "01827 65780",
                    "01827 65780",
                    "www.testservice.com",
                    "support@testservice.com"
                    ))
            })
            .WithCostOption(new List<CostOptionDto>())
            .WithLanguages(new List<LanguageDto>
            {
                    new LanguageDto("442a06cd-aa14-4ea3-9f11-b45c1bc4861f", "English")
                })
            .WithServiceAreas(new List<ServiceAreaDto>
            {
                    new ServiceAreaDto(Guid.NewGuid().ToString(), "National", null,"http://statistics.data.gov.uk/id/statistical-geography/K02000001")
                })
            .WithServiceAtLocations(new List<ServiceAtLocationDto>
            {
                    new ServiceAtLocationDto(
                        "Test17499",
                        new LocationDto(
                            "a878aadc-6097-4a0f-b3e1-77fd4511175d",
                            "",
                            "",
                            52.6312,
                            -1.66526,
                            new List<PhysicalAddressDto>
                            {
                                new PhysicalAddressDto(
                                    Guid.NewGuid().ToString(),
                                    "75 Sheepcote Lane",
                                    ", Stathe, Tamworth, Staffordshire, ",
                                    "B77 3JN",
                                    "England",
                                    null
                                    )
                            },
                            new List<LinkTaxonomyDto>
                            {
                                new LinkTaxonomyDto(
                                    Guid.NewGuid().ToString(),
                                    "Location",
                                    "a878aadc-6097-4a0f-b3e1-77fd4511175d",
                                    new TaxonomyDto(
                                        //todo: real guid
                                        Guid.NewGuid().ToString(),
                                        "Family_hub",
                                        null,
                                        null
                                        )
                                    )
                            },
                            new List<LinkContactDto>()
                            ),
                        new List<RegularScheduleDto>(),
                        new List<HolidayScheduleDto>(),
                        new List<LinkContactDto>()
                        )

                })
            .WithServiceTaxonomies(new List<ServiceTaxonomyDto>
            {
                    new ServiceTaxonomyDto("Test9107",
                    new TaxonomyDto(
                        "Test bccsource:Organisation",
                        "Organisation",
                        "Test BCC Data Sources",
                        null
                        )),

                    new ServiceTaxonomyDto("Test9108",
                    new TaxonomyDto(
                        "Test bccprimaryservicetype:38",
                        "Support",
                        "Test BCC Primary Services",
                        null
                        )),

                    new ServiceTaxonomyDto("Test9109",
                    new TaxonomyDto(
                        "Test bccagegroup:37",
                        "Children",
                        "Test BCC Age Groups",
                        null
                        )),

                    new ServiceTaxonomyDto("Test9110",
                    new TaxonomyDto(
                        "Testbccusergroup:56",
                        "Long Term Health Conditions",
                        "Test BCC User Groups",
                        null
                        ))
                })
            .Build();

        return service;
    }

    public static ServiceDto GetTestCountyCouncilServicesCreateRecord(string parentId)
    {
        var contactId = Guid.NewGuid().ToString();

        var builder = new ServicesDtoBuilder();
        var service = builder.WithMainProperties("9066bccb-79cb-401f-818f-86ad23b022cf",
                new ServiceTypeDto("1", "Information Sharing", ""),
                parentId,
                "Test1 Organisation for Children with Tracheostomies",
                @"Test1 Organisation for for Children with Tracheostomies is a national self help group operating as a registered charity and is run by parents of children with a tracheostomy and by people who sympathise with the needs of such families. ACT as an organisation is non profit making, it links groups and individual members throughout Great Britain and Northern Ireland.",
                null,
                null,
                null,
                null,
                null,
                "active",
                null,
                false)
            .WithServiceDelivery(new List<ServiceDeliveryDto>
                {
                    new ServiceDeliveryDto(Guid.NewGuid().ToString(),ServiceDeliveryType.Online)
                })
            .WithEligibility(new List<EligibilityDto>
                {
                    new EligibilityDto("Test91091Children","Test91091Children",0,13)
                })
            .WithLinkContact(new List<LinkContactDto>
            {
                new LinkContactDto(
                    "9066bccb-79cb-401f-818f-86ad23b022ca",
                    "9066bccb-79cb-401f-818f-86ad23b022cf",
                    "Service",
                new ContactDto(
                    contactId,
                    "Service",
                    string.Empty,
                    "01827 65770",
                    "01827 65770",
                    "www.testservice1.com",
                    "support@testservice1.com"
                    ))
            })
            .WithCostOption(new List<CostOptionDto>())
            .WithLanguages(new List<LanguageDto>
            {
                    new LanguageDto("943bc803-39f4-4805-8805-bc7d3eeae3ff", "English")
                })
            .WithServiceAreas(new List<ServiceAreaDto>
            {
                    new ServiceAreaDto(Guid.NewGuid().ToString(), "National", null,"http://statistics.data.gov.uk/id/statistical-geography/K02000001")
                })
            .WithServiceAtLocations(new List<ServiceAtLocationDto>
            {
                    new ServiceAtLocationDto(
                        "Test1749",
                        new LocationDto(
                            "86119575-017f-4eeb-b92e-cb3f62d54840",
                            "",
                            "",
                            52.6312,
                            -1.66526,
                            new List<PhysicalAddressDto>
                            {
                                new PhysicalAddressDto(
                                    Guid.NewGuid().ToString(),
                                    "76 Sheepcote Lane",
                                    ", Stathe, Tamworth, Staffordshire, ",
                                    "B77 3JN",
                                    "England",
                                    null
                                    )
                            },
                            new List<LinkTaxonomyDto>
                            {
                                new LinkTaxonomyDto(
                                    Guid.NewGuid().ToString(),
                                    "Location",
                                    "86119575-017f-4eeb-b92e-cb3f62d54840",
                                    new TaxonomyDto(
                                        //todo: real guid

                                        Guid.NewGuid().ToString(),
                                        "Family_hub",
                                        null,
                                        null
                                    )
                                )
                            },
                            new List<LinkContactDto>()
                            ),
                        new List<RegularScheduleDto>(),
                        new List<HolidayScheduleDto>(),
                        new List<LinkContactDto>()
                        )
            })
            .Build();

        return service;
    }

    private Organisation GetTestCountyCouncil()
    {
        var bristolCountyCouncil = new Organisation(
            "ba1cca90-b02a-4a0b-afa0-d8aed1083c0d",
            new OrganisationType("1", "LA", "Local Authority"),
            "Test County Council",
            "Test County Council",
            null,
            new Uri("https://www.test.gov.uk/").ToString(),
            "https://www.test.gov.uk/",
            null,
            GetTestCountyCouncilServices(),
            new List<LinkContact>());

        return bristolCountyCouncil;
    }

    private List<Service> GetTestCountyCouncilServices()
    {
        return new List<Service>
        {
            new Service(
                "c1b5dd80-7506-4424-9711-fe175fa13eb8",
                new ServiceType("1", "Information Sharing", ""),
                "ba1cca90-b02a-4a0b-afa0-d8aed1083c0d",
                "Test Organisation for Children with Tracheostomies",
                @"Test Organisation for for Children with Tracheostomies is a national self help group operating as a registered charity and is run by parents of children with a tracheostomy and by people who sympathise with the needs of such families. ACT as an organisation is non profit making, it links groups and individual members throughout Great Britain and Northern Ireland.",
                null,
                null,
                null,
                "active",
                "www.testservice.com",
                "support@testservice.com",
                null,
                false,
                new List<ServiceDelivery>
                {
                    new ServiceDelivery(Guid.NewGuid().ToString(),ServiceDeliveryType.Online)
                },
                new List<Eligibility>
                {
                    new Eligibility("Test9109Children","",null,0,13,new List<Taxonomy>())
                },
                new List<Funding>(),
                new List<CostOption>(),
                new List<Language>(),
                new List<Review>(),
                new List<ServiceArea>
                {
                    new ServiceArea(Guid.NewGuid().ToString(), "National", null, null, "http://statistics.data.gov.uk/id/statistical-geography/K02000001")
                },
                new List<ServiceAtLocation>
                {
                    new ServiceAtLocation(
                        "Test1749",
                        new Location(
                            "a878aadc-6097-4a0f-b3e1-77fd4511175d",
                            "",
                            "",
                            52.6312,
                            -1.66526,
                            new List<LinkTaxonomy>(),
                            new List<PhysicalAddress>
                            {
                                new PhysicalAddress(
                                    Guid.NewGuid().ToString(),
                                    "75 Sheepcote Lane",
                                    ", Stathe, Tamworth, Staffordshire, ",
                                    "B77 3JN",
                                    "England",
                                    null
                                )
                            },
                            new List<AccessibilityForDisabilities>(),
                            new List<LinkContact>()),
                        new List<RegularSchedule>(),
                        new List<HolidaySchedule>(),
                        new List<LinkContact>())

                },
                new List<ServiceTaxonomy>
                {
                    new ServiceTaxonomy
                    ("Test9107",
                        null,
                        new Taxonomy(
                            "Test bccsource:Organisation",
                            "Organisation",
                            "Test BCC Data Sources",
                            null
                        )),

                    new ServiceTaxonomy
                    ("Test9108",
                        null,
                        new Taxonomy(
                            "Test bccprimaryservicetype:38",
                            "Support",
                            "Test BCC Primary Services",
                            null
                        )),

                    new ServiceTaxonomy
                    ("Test9109",
                        null,
                        new Taxonomy(
                            "Test bccagegroup:37",
                            "Children",
                            "Test BCC Age Groups",
                            null
                        )),

                    new ServiceTaxonomy
                    ("Test9110",
                        null,
                        new Taxonomy(
                            "Testbccusergroup:56",
                            "Long Term Health Conditions",
                            "Test BCC User Groups",
                            null
                        ))
                },
                new List<HolidaySchedule>(),
                new List<RegularSchedule>(),
                new List<LinkContact>
                {
                    new LinkContact(
                        "c1b5dd80-7506-4424-9711-fe175fa13eb8",
                        "c1b5dd80-7506-4424-9711-fe175fa13eb1",
                        "Service",
                    new Contact(
                        "Test1567",
                        "",
                        "",
                        "01827 65779",
                        "01827 65779",
                        "www.gov.uk",
                        "help@gov.uk"))
                })

        };
    }
}
