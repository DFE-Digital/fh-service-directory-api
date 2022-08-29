//using FamilyHubs.ServiceDirectory.Shared.Builders;
//using FamilyHubs.ServiceDirectory.Shared.Enums;
//using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralContacts;
//using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralCostOptions;
//using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralEligibilitys;
//using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralLanguages;
//using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralLocations;
//using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralOrganisations;
//using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralPhones;
//using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralPhysicalAddresses;
//using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralServiceAreas;
//using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralServiceAtLocations;
//using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralServiceDeliverysEx;
//using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralServices;
//using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralServiceTaxonomys;
//using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralTaxonomys;
//using fh_service_directory_api.core.Entities;
//using fh_service_directory_api.core.Interfaces.Entities;
//using FluentAssertions;
//using System.Text;
//using System.Text.Json;

//namespace FamilyHubs.ServiceDirectoryApi.FunctionalTests;

//[Collection("Sequential")]
//public class WhenUsingOpenReferralOrganisationApiUnitTests : BaseWhenUsingOpenReferralApiUnitTests
//{
//    [Fact]
//    public async Task ThenTheOpenReferralOrganisationIsCreated()
//    {
//        var command = GetTestCountyCouncilRecord();

//        var request = new HttpRequestMessage
//        {
//            Method = HttpMethod.Post,
//            //RequestUri = new Uri(_client.BaseAddress + "organizations"),
//            RequestUri = new Uri(_client.BaseAddress + "api/organizations"),
//            Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(command), Encoding.UTF8, "application/json"),
//        };

//        using var response = await _client.SendAsync(request);

//        response.EnsureSuccessStatusCode();

//        var stringResult = await response.Content.ReadAsStringAsync();

//        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
//        stringResult.ToString().Should().Be("ba1cca90-b02a-4a0b-afa0-d8aed1083c0d");
//    }

//    [Fact]
//    public async Task ThenTheOpenReferralOrganisationIsRetrieved()
//    {
//        var request = new HttpRequestMessage
//        {
//            Method = HttpMethod.Get,
//            RequestUri = new Uri(_client.BaseAddress + "api/organizations/72e653e8-1d05-4821-84e9-9177571a6013"),

//        };

//        using var response = await _client.SendAsync(request);

//        response.EnsureSuccessStatusCode();


//        var retVal = await JsonSerializer.DeserializeAsync<OpenReferralOrganisationWithServicesDto>(await response.Content.ReadAsStreamAsync(), options: new JsonSerializerOptions { PropertyNameCaseInsensitive = true });


//        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
//        retVal.Should().NotBeNull();
//        ArgumentNullException.ThrowIfNull(retVal, nameof(retVal));
//        retVal.Id.Should().Be("72e653e8-1d05-4821-84e9-9177571a6013");
//    }

//    [Fact]
//    public async Task ThenListOpenReferralOrganisationsIsRetrieved()
//    {
//        var request = new HttpRequestMessage
//        {
//            Method = HttpMethod.Get,
//            RequestUri = new Uri(_client.BaseAddress + "api/organizations"),

//        };

//        using var response = await _client.SendAsync(request);

//        response.EnsureSuccessStatusCode();

//        var retVal = await JsonSerializer.DeserializeAsync<List<OpenReferralOrganisationDto>>(await response.Content.ReadAsStreamAsync(), options: new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

//        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
//        retVal.Should().NotBeNull();
//        ArgumentNullException.ThrowIfNull(retVal, nameof(retVal));
//        retVal.Count.Should().BeGreaterThan(0);
//    }

//    [Fact]
//    public async Task ThenTheOpenReferralOrganisationIsUpdated()
//    {
//        var request = new HttpRequestMessage
//        {
//            Method = HttpMethod.Get,
//            RequestUri = new Uri(_client.BaseAddress + "api/organizations/72e653e8-1d05-4821-84e9-9177571a6013"),

//        };

//        using var response = await _client.SendAsync(request);

//        response.EnsureSuccessStatusCode();


//        var retVal = await JsonSerializer.DeserializeAsync<OpenReferralOrganisationWithServicesDto>(await response.Content.ReadAsStreamAsync(), options: new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
//        ArgumentNullException.ThrowIfNull(retVal, nameof(retVal));

//        var update = new OpenReferralOrganisationWithServicesDto(
//            retVal.Id,
//            retVal.Name,
//            retVal.Description + " Update Test",
//            retVal.Logo,
//            retVal.Uri,
//            retVal.Url,
//            retVal.Services
//            );

//        var updaterequest = new HttpRequestMessage
//        {
//            Method = HttpMethod.Put,
//            RequestUri = new Uri(_client.BaseAddress + "api/organizations/72e653e8-1d05-4821-84e9-9177571a6013"),
//            Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(update), Encoding.UTF8, "application/json"),
//        };

//        using var updateresponse = await _client.SendAsync(updaterequest);

//        updateresponse.EnsureSuccessStatusCode();

//        var stringResult = await updateresponse.Content.ReadAsStringAsync();

//        updateresponse.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
//        stringResult.ToString().Should().Be("72e653e8-1d05-4821-84e9-9177571a6013");
//    }

//    public IReadOnlyCollection<OpenReferralOrganisation> GetTestOpenReferralOrganistions()
//    {
//        List<OpenReferralOrganisation> openReferralOrganistions = new()
//        {
//            GetTestCountyCouncil()
//        };

//        return openReferralOrganistions;
//    }

//    private OpenReferralOrganisationWithServicesDto GetTestCountyCouncilRecord()
//    {
//        var bristolCountyCouncil = new OpenReferralOrganisationWithServicesDto(
//            "ba1cca90-b02a-4a0b-afa0-d8aed1083c0d",
//            "Test County Council",
//            "Test County Council",
//            null,
//            new Uri("https://www.test.gov.uk/").ToString(),
//            "https://www.test.gov.uk/",
//            new List<IOpenReferralServiceDto>
//            {
//                 GetTestCountyCouncilServicesRecord()
//            }
//            );

//        return bristolCountyCouncil;
//    }

//    private IOpenReferralServiceDto GetTestCountyCouncilServicesRecord()
//    {
//        var contactId = Guid.NewGuid().ToString();

//        ServicesDtoBuilder builder = new ServicesDtoBuilder();
//        IOpenReferralServiceDto service = builder.WithMainProperties("c1b5dd80-7506-4424-9711-fe175fa13eb8",
//                "Test Organisation for Children with Tracheostomies",
//                @"Test Organisation for for Children with Tracheostomies is a national self help group operating as a registered charity and is run by parents of children with a tracheostomy and by people who sympathise with the needs of such families. ACT as an organisation is non profit making, it links groups and individual members throughout Great Britain and Northern Ireland.",
//                null,
//                null,
//                null,
//                null,
//                null,
//                "active",
//                "www.testservice.com",
//                "support@testservice.com",
//                null)
//            .WithServiceDelivery(new List<IOpenReferralServiceDeliveryExDto>
//                {
//                    new OpenReferralServiceDeliveryExDto(Guid.NewGuid().ToString(),ServiceDelivery.Online)
//                })
//            .WithEligibility(new List<IOpenReferralEligibilityDto>
//                {
//                    new OpenReferralEligibilityDto("Test9109Children","",0,13)
//                })
//            .WithContact(new List<IOpenReferralContactDto>()
//            {
//                new OpenReferralContactDto(
//                    contactId,
//                    "Service",
//                    string.Empty,
//                    new List<IOpenReferralPhoneDto>()
//                    {
//                        new OpenReferralPhoneDto("1568", "01827 65779")
//                    }
//                    )
//            })
//            .WithCostOption(new List<IOpenReferralCostOptionDto>())
//            .WithLanguages(new List<IOpenReferralLanguageDto>()
//                {
//                    new OpenReferralLanguageDto("442a06cd-aa14-4ea3-9f11-b45c1bc4861f", "English")
//                })
//            .WithServiceAreas(new List<IOpenReferralServiceAreaDto>()
//                {
//                    new OpenReferralServiceAreaDto(Guid.NewGuid().ToString(), "National", null,"http://statistics.data.gov.uk/id/statistical-geography/K02000001")
//                })
//            .WithServiceAtLocations(new List<IOpenReferralServiceAtLocationDto>()
//                {
//                    new OpenReferralServiceAtLocationDto(
//                        "Test1749",
//                        new OpenReferralLocationDto(
//                            "a878aadc-6097-4a0f-b3e1-77fd4511175d",
//                            "",
//                            "",
//                            52.6312,
//                            -1.66526,
//                            new List<IOpenReferralPhysicalAddressDto>()
//                            {
//                                new OpenReferralPhysicalAddressDto(
//                                    Guid.NewGuid().ToString(),
//                                    "75 Sheepcote Lane",
//                                    ", Stathe, Tamworth, Staffordshire, ",
//                                    "B77 3JN",
//                                    "England",
//                                    null
//                                    )
//                            }
//                            //new List<Accessibility_For_Disabilities>()
//                            )
//                        //new List<OpenReferralHoliday_Schedule>(),
//                        //new List<OpenReferralRegular_Schedule>()
//                        )

//                })
//            .WithServiceTaxonomies(new List<IOpenReferralServiceTaxonomyDto>()
//                {
//                    new OpenReferralServiceTaxonomyDto
//                    ("Test9107",
//                    new OpenReferralTaxonomyDto(
//                        "Test bccsource:Organisation",
//                        "Organisation",
//                        "Test BCC Data Sources",
//                        null
//                        )),

//                    new OpenReferralServiceTaxonomyDto
//                    ("Test9108",
//                    new OpenReferralTaxonomyDto(
//                        "Test bccprimaryservicetype:38",
//                        "Support",
//                        "Test BCC Primary Services",
//                        null
//                        )),

//                    new OpenReferralServiceTaxonomyDto
//                    ("Test9109",
//                    new OpenReferralTaxonomyDto(
//                        "Test bccagegroup:37",
//                        "Children",
//                        "Test BCC Age Groups",
//                        null
//                        )),

//                    new OpenReferralServiceTaxonomyDto
//                    ("Test9110",
//                    new OpenReferralTaxonomyDto(
//                        "Testbccusergroup:56",
//                        "Long Term Health Conditions",
//                        "Test BCC User Groups",
//                        null
//                        ))
//                })
//            .Build();

//        return service;
//    }

//    private OpenReferralOrganisation GetTestCountyCouncil()
//    {
//        var bristolCountyCouncil = new OpenReferralOrganisation(
//            "ba1cca90-b02a-4a0b-afa0-d8aed1083c0d",
//            "Test County Council",
//            "Test County Council",
//            null,
//            new Uri("https://www.test.gov.uk/").ToString(),
//            "https://www.test.gov.uk/",
//            null,
//            GetTestCountyCouncilServices()
//            );
//        return bristolCountyCouncil;
//    }

//    private List<OpenReferralService> GetTestCountyCouncilServices()
//    {
//        return new()
//        {
//            new OpenReferralService(
//                "c1b5dd80-7506-4424-9711-fe175fa13eb8",
//                "ba1cca90-b02a-4a0b-afa0-d8aed1083c0d",
//                "Test Organisation for Children with Tracheostomies",
//                @"Test Organisation for for Children with Tracheostomies is a national self help group operating as a registered charity and is run by parents of children with a tracheostomy and by people who sympathise with the needs of such families. ACT as an organisation is non profit making, it links groups and individual members throughout Great Britain and Northern Ireland.",
//                null,
//                null,
//                null,
//                null,
//                null,
//                "active",
//                "www.testservice.com",
//                "support@testservice.com",
//                null,
//                new List<IOpenReferralServiceDelivery>
//                {
//                    new OpenReferralServiceDelivery(Guid.NewGuid().ToString(),ServiceDelivery.Online)
//                },
//                new List<IOpenReferralEligibility>
//                {
//                    new OpenReferralEligibility("Test9109Children","",null,0,13,new List<IOpenReferralTaxonomy>())
//                },
//                new List<IOpenReferralFunding>(),
//                new List<IOpenReferralHoliday_Schedule>(),
//                new List<IOpenReferralLanguage>(),
//                new List<IOpenReferralRegular_Schedule>(),
//                new List<IOpenReferralReview>(),
//                new List<IOpenReferralContact>()
//                {
//                    new OpenReferralContact(
//                        "Test1567",
//                        "",
//                        "",
//                        new List<IOpenReferralPhone>()
//                        {
//                            new OpenReferralPhone("1568", "01827 65779")
//                        }
//                        )
//                },
//                new List<IOpenReferralCost_Option>(),
//                new List<IOpenReferralService_Area>()
//                {
//                    new OpenReferralService_Area(Guid.NewGuid().ToString(), "National", null, null, "http://statistics.data.gov.uk/id/statistical-geography/K02000001")
//                },
//                new List<IOpenReferralServiceAtLocation>()
//                {
//                    new OpenReferralServiceAtLocation(
//                        "Test1749",
//                        new OpenReferralLocation(
//                            "a878aadc-6097-4a0f-b3e1-77fd4511175d",
//                            "",
//                            "",
//                            52.6312,
//                            -1.66526,
//                            new List<IOpenReferralPhysical_Address>()
//                            {
//                                new OpenReferralPhysical_Address(
//                                    Guid.NewGuid().ToString(),
//                                    "75 Sheepcote Lane",
//                                    ", Stathe, Tamworth, Staffordshire, ",
//                                    "B77 3JN",
//                                    "England",
//                                    null
//                                    )
//                            },
//                            new List<IAccessibility_For_Disabilities>()
//                            ),
//                        new List<IOpenReferralHoliday_Schedule>(),
//                        new List<IOpenReferralRegular_Schedule>()
//                        )

//                },
//                new List<IOpenReferralService_Taxonomy>()
//                {
//                    new OpenReferralService_Taxonomy
//                    ("Test9107",
//                    null,
//                    new OpenReferralTaxonomy(
//                        "Test bccsource:Organisation",
//                        "Organisation",
//                        "Test BCC Data Sources",
//                        null
//                        )),

//                    new OpenReferralService_Taxonomy
//                    ("Test9108",
//                    null,
//                    new OpenReferralTaxonomy(
//                        "Test bccprimaryservicetype:38",
//                        "Support",
//                        "Test BCC Primary Services",
//                        null
//                        )),

//                    new OpenReferralService_Taxonomy
//                    ("Test9109",
//                    null,
//                    new OpenReferralTaxonomy(
//                        "Test bccagegroup:37",
//                        "Children",
//                        "Test BCC Age Groups",
//                        null
//                        )),

//                    new OpenReferralService_Taxonomy
//                    ("Test9110",
//                    null,
//                    new OpenReferralTaxonomy(
//                        "Testbccusergroup:56",
//                        "Long Term Health Conditions",
//                        "Test BCC User Groups",
//                        null
//                        ))
//                }
//                )

//        };
//    }
//}
