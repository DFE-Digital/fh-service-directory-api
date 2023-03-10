using System.Net;
using System.Text;
using System.Text.Json;
using FamilyHubs.ServiceDirectory.Core.Entities;
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
            RequestUri = new Uri(_client.BaseAddress + "api/organizations/1"),

        };

        using var response = await _client.SendAsync(request);

        response.EnsureSuccessStatusCode();


        var retVal = await JsonSerializer.DeserializeAsync<OrganisationWithServicesDto>(await response.Content.ReadAsStreamAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });


        response.StatusCode.Should().Be(HttpStatusCode.OK);
        retVal.Should().NotBeNull();
        ArgumentNullException.ThrowIfNull(retVal);
        retVal.Id.Should().Be(1);
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

        var retVal = await JsonSerializer.DeserializeAsync<List<string>>(await response.Content.ReadAsStreamAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

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
            RequestUri = new Uri(_client.BaseAddress + "api/organizationAdminCode/1"),
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
            RequestUri = new Uri(_client.BaseAddress + "api/organizations/1"),

        };

        //request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue($"Bearer", $"{new JwtSecurityTokenHandler().WriteToken(_token)}");

        using var response = await _client.SendAsync(request);

        response.EnsureSuccessStatusCode();


        var retVal = await JsonSerializer.DeserializeAsync<OrganisationWithServicesDto>(await response.Content.ReadAsStreamAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        ArgumentNullException.ThrowIfNull(retVal);

        var update = new OrganisationWithServicesDto
        {
            OrganisationType = retVal.OrganisationType,
            Id = retVal.Id,
            Name = retVal.Name,
            Description = retVal.Description + " Update Test",
            AdminAreaCode = retVal.AdminAreaCode,
            Logo = retVal.Logo,
            Uri = retVal.Uri,
            Url = retVal.Url,
        };

        var updaterequest = new HttpRequestMessage
        {
            Method = HttpMethod.Put,
            RequestUri = new Uri(_client.BaseAddress + "api/organizations/1"),
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
        var bristolCountyCouncil = new OrganisationWithServicesDto
        {
            AdminAreaCode = "E06000023",
            OrganisationType = OrganisationType.LA,
            Name = "Test County Council",
            Description = "Test County Council",
            Url = new Uri("https://www.test.gov.uk/").ToString(),
            Uri = "https://www.test.gov.uk/",
            Services = new List<ServiceDto>
            {
                GetTestCountyCouncilServicesRecord(0)
            }
        };

        return bristolCountyCouncil;
    }

    public static ServiceDto GetTestCountyCouncilServicesRecord(long parentId)
    {
        var serviceId = "c1b5dd80-7506-4424-9711-fe175fa13eb8";
        
        var service = new ServiceDto
        {
            ServiceOwnerReferenceId = serviceId,
            ServiceType = ServiceType.InformationSharing,
            Name = "Test Organisation for Children with Tracheostomies",
            Description = @"Test Organisation for for Children with Tracheostomies is a national self help group operating as a registered charity and is run by parents of children with a tracheostomy and by people who sympathise with the needs of such families. ACT as an organisation is non profit making, it links groups and individual members throughout Great Britain and Northern Ireland.",
            ServiceDeliveries = new List<ServiceDeliveryDto>
            {
                new ServiceDeliveryDto
                {
                    Name = ServiceDeliveryType.Online,
                }
            },
            Eligibilities = new List<EligibilityDto>
            {
                new EligibilityDto
                {
                    MaximumAge = 13,
                    MinimumAge = 0
                }
            },
            Contacts = new List<ContactDto>
            {
                new ContactDto
                {
                    Name = "Service",
                    Title = string.Empty,
                    Telephone = "01827 65780",
                    TextPhone = "01827 65780",
                    Url = "www.testservice.com",
                    Email = "support@testservice.com"
                }
            },
            Languages = new List<LanguageDto>
            {
                new LanguageDto
                {
                    Name = "English"
                }
            },
            ServiceAreas = new List<ServiceAreaDto>
            {
                new ServiceAreaDto
                {
                    Uri = "http://statistics.data.gov.uk/id/statistical-geography/K02000001",
                    ServiceAreaName = "National",
                    Extent = null
                }
            },
            Locations = new List<LocationDto>
            {
                new LocationDto
                {
                    LocationType = LocationType.FamilyHub,
                    Name = "",
                    Latitude = 52.6312,
                    Longitude = -1.66526,
                    Address1 = "75 Sheepcote Lane",
                    City = ", Stathe, Tamworth, Staffordshire, ",
                    PostCode = "B77 3JN",
                    StateProvince = "",
                    Country = "England",
                }
            },
            Taxonomies = new List<TaxonomyDto>
            {
                new TaxonomyDto
                {
                    Name = "Organisation",
                    TaxonomyType = TaxonomyType.ServiceCategory
                },
                new TaxonomyDto
                {
                    Name = "Support",
                    TaxonomyType = TaxonomyType.ServiceCategory
                },
                new TaxonomyDto
                {
                    Name = "Children",
                    TaxonomyType = TaxonomyType.ServiceCategory
                },
                new TaxonomyDto
                {
                    Name = "Long Term Health Conditions",
                    TaxonomyType = TaxonomyType.ServiceCategory
                }
            }
        };

        return service;
    }

    public static ServiceDto GetTestCountyCouncilServicesCreateRecord(string parentId)
    {
        var serviceId = "9066bccb-79cb-401f-818f-86ad23b022cf";

        var service = new ServiceDto
        {
            ServiceOwnerReferenceId = serviceId,
            ServiceType = ServiceType.InformationSharing,
            Name = "Test1 Organisation for Children with Tracheostomies",
            Description = @"Test1 Organisation for for Children with Tracheostomies is a national self help group operating as a registered charity and is run by parents of children with a tracheostomy and by people who sympathise with the needs of such families. ACT as an organisation is non profit making, it links groups and individual members throughout Great Britain and Northern Ireland.",
            ServiceDeliveries = new List<ServiceDeliveryDto>
            {
                new ServiceDeliveryDto
                {
                    Name = ServiceDeliveryType.Online,
                }
            },
            Eligibilities = new List<EligibilityDto>
            {
                new EligibilityDto
                {
                    MaximumAge = 13,
                    MinimumAge = 0
                }
            },
            Contacts = new List<ContactDto>
            {
                new ContactDto
                {
                    Name =  "Service",
                    Title = string.Empty,
                    Telephone = "01827 65770",
                    TextPhone = "01827 65770",
                    Url = "www.testservice1.com",
                    Email = "support@testservice1.com"
                }
            },
            Languages = new List<LanguageDto>
            {
                new LanguageDto
                {
                    Name = "English"
                }
            },
            ServiceAreas = new List<ServiceAreaDto>
            {
                new ServiceAreaDto
                {
                    ServiceAreaName = "National",
                    Extent = null,
                    Uri = "http://statistics.data.gov.uk/id/statistical-geography/K02000001",
                }
            },
            Locations = new List<LocationDto>
            {
                new LocationDto
                {
                    Name = "Test",
                    Description = "",
                    Latitude = 52.6312,
                    Longitude = -1.66526,
                    Address1 = "76 Sheepcote Lane",
                    City = ", Stathe, Tamworth, Staffordshire, ",
                    PostCode = "B77 3JN",
                    Country = "England",
                    StateProvince = "null",
                    LocationType = LocationType.FamilyHub
                }
            }
        };

        return service;
    }

    private Organisation GetTestCountyCouncil()
    {
        var bristolCountyCouncil = new Organisation
        {
            OrganisationType = OrganisationType.LA,
            Name = "Test County Council",
            Description = "Test County Council",
            AdminAreaCode = "",
            Url = new Uri("https://www.test.gov.uk/").ToString(),
            Uri = "https://www.test.gov.uk/",
            Logo = null,
            Services = GetTestCountyCouncilServices()
        };

        return bristolCountyCouncil;
    }

    private List<Service> GetTestCountyCouncilServices()
    {
        return new List<Service>
        {
            new Service
            {
                ServiceOwnerReferenceId = "c1b5dd80-7506-4424-9711-fe175fa13eb8",
                OrganisationId = 1,
                ServiceType = ServiceType.InformationSharing,
                Name = "Test Organisation for Children with Tracheostomies",
                Description = @"Test Organisation for for Children with Tracheostomies is a national self help group operating as a registered charity and is run by parents of children with a tracheostomy and by people who sympathise with the needs of such families. ACT as an organisation is non profit making, it links groups and individual members throughout Great Britain and Northern Ireland.",
                AttendingAccess = AttendingAccessType.NotSet,
                AttendingType = AttendingType.NotSet,
                DeliverableType = DeliverableType.NotSet,
                Status = ServiceStatusType.Active,
                CanFamilyChooseDeliveryLocation = false,
                ServiceDeliveries = new List<ServiceDelivery>
                {
                    new ServiceDelivery
                    {
                        Name = ServiceDeliveryType.Online,
                    }
                },
                Eligibilities = new List<Eligibility>
                {
                    new Eligibility
                    {
                        MaximumAge = 13,
                        MinimumAge = 0,
                    }
                },
                ServiceAreas = new List<ServiceArea>
                {
                    new ServiceArea
                    {
                        ServiceAreaName = "National",
                        Extent = null,
                        Uri = "http://statistics.data.gov.uk/id/statistical-geography/K02000001",
                    }
                },
                Locations = new List<Location>
                {
                    new Location
                    {
                        Name = "",
                        Description = "",
                        Latitude = 52.6312,
                        Longitude = -1.66526,
                        Address1 = "75 Sheepcote Lane",
                        City = ", Stathe, Tamworth, Staffordshire, ",
                        PostCode = "B77 3JN",
                        Country = "England",
                        LocationType = LocationType.NotSet,
                        StateProvince = "",
                    }
                },
                Taxonomies = new List<Taxonomy>
                {
                    new Taxonomy
                    {
                        Name = "Organisation",
                        TaxonomyType = TaxonomyType.ServiceCategory,
                        ParentId = null
                    },
                    new Taxonomy
                    {
                        Name = "Support",
                        TaxonomyType = TaxonomyType.ServiceCategory,
                        ParentId = null
                    },
                    new Taxonomy
                    {
                        Name = "Children",
                        TaxonomyType = TaxonomyType.ServiceCategory,
                        ParentId = null
                    },
                    new Taxonomy
                    {
                        Name = "Long Term Health Conditions",
                        TaxonomyType = TaxonomyType.ServiceCategory,
                        ParentId = null
                    }
                },
                Contacts = new List<Contact>
                {
                    new Contact
                    {
                        Name = "",
                        Title = "",
                        Telephone = "01827 65779",
                        TextPhone = "01827 65779",
                        Url = "www.gov.uk",
                        Email = "help@gov.uk"
                    }
                },
            }
        };
    }
}
