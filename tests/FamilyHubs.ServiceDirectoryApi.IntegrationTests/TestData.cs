using FamilyHubs.ServiceDirectory.Core.Entities;
using FamilyHubs.ServiceDirectory.Shared.Enums;

namespace FamilyHubs.ServiceDirectoryApi.IntegrationTests;

public class TestData
{
    public IReadOnlyCollection<Organisation> SeedOrganisations()
    {
        var organisations = new List<Organisation>
        {
            GetTestCountyCouncil()
        };

        return organisations;
    }

    public Organisation GetTestCountyCouncil()
    {
        var bristolCountyCouncil = new Organisation
        {
            OrganisationType = OrganisationType.LA,
            Name = "Test County Council",
            Description = "Test County Council",
            Logo = null,
            Uri = new Uri("https://www.testcouncil.gov.uk/").ToString(),
            Url = "https://www.testcouncil.gov.uk/",
            Reviews = new List<Review>(),
            Services = GetBristolCountyCouncilServices(0),
            CreatedBy = "TestSystem",
            Created = DateTime.UtcNow,
            AdminAreaCode = "TestCode"
        };

        bristolCountyCouncil.Services.ElementAt(0).CreatedBy = "TestSystem";
        bristolCountyCouncil.Services.ElementAt(0).Created = DateTime.UtcNow;
        bristolCountyCouncil.Services.ElementAt(0).ServiceDeliveries.ElementAt(0).CreatedBy = "TestSystem";
        bristolCountyCouncil.Services.ElementAt(0).ServiceDeliveries.ElementAt(0).Created = DateTime.UtcNow;
        bristolCountyCouncil.Services.ElementAt(0).Eligibilities.ElementAt(0).CreatedBy = "TestSystem";
        bristolCountyCouncil.Services.ElementAt(0).Eligibilities.ElementAt(0).Created = DateTime.UtcNow;
        bristolCountyCouncil.Services.ElementAt(0).Languages.ElementAt(0).CreatedBy = "TestSystem";
        bristolCountyCouncil.Services.ElementAt(0).Languages.ElementAt(0).Created = DateTime.UtcNow;
        bristolCountyCouncil.Services.ElementAt(0).Contacts.ElementAt(0).CreatedBy = "TestSystem";
        bristolCountyCouncil.Services.ElementAt(0).Contacts.ElementAt(0).Created = DateTime.UtcNow;
        bristolCountyCouncil.Services.ElementAt(0).Locations.ElementAt(0).CreatedBy = "TestSystem";
        bristolCountyCouncil.Services.ElementAt(0).Locations.ElementAt(0).Created = DateTime.UtcNow;

        return bristolCountyCouncil;
    }

    private List<Service> GetBristolCountyCouncilServices(long parentId)
    {
        var serviceId = "9f01190b-429c-41fd-ba38-936d2995398b";
        return new List<Service>
        {
            new Service
            {
                ServiceOwnerReferenceId = serviceId,
                ServiceType = ServiceType.InformationSharing,
                OrganisationId = parentId,
                Name = "Test Service",
                Description = @"Test Service Description",
                DeliverableType = DeliverableType.NotSet,
                AttendingAccess = AttendingAccessType.NotSet,
                AttendingType = AttendingType.NotSet,
                Status = ServiceStatusType.Active,
                CanFamilyChooseDeliveryLocation = false,
                ServiceDeliveries = new List<ServiceDelivery>( new List<ServiceDelivery>
                {
                    new ServiceDelivery
                    {
                        Name = ServiceDeliveryType.Online,
                    }
                }),
                Eligibilities = new List<Eligibility>(new List<Eligibility>
                {
                    new Eligibility
                    {
                        EligibilityType = EligibilityType.NotSet,
                        MinimumAge = 0,
                        MaximumAge = 13,
                    }
                }),
                Languages = new List<Language>
                {
                    new Language
                    {
                        Name = "English",
                    }
                },
                ServiceAreas = new List<ServiceArea>
                {
                    new ServiceArea
                    {
                       ServiceAreaName = "National",
                       Extent = null,
                       Uri = "http://statistics.data.gov.uk/id/statistical-geography/K02000001"
                    },
                },
                Locations = new List<Location>
                {
                    new Location
                    {
                        Name = "Test",
                        Description = "",
                        Latitude = 52.6312,
                        Longitude = -1.66526,
                        Address1 = "76 Sheepcote Lane",
                        City = ", Stathe, Tamworth, Staffordshire, ",
                        PostCode = "B77 3JN",
                        Country = "England",
                        StateProvince = "",
                        LocationType = LocationType.NotSet,
                        Contacts = new List<Contact>
                        {
                            new Contact
                            {
                                Title = "Mr",
                                Name = "John Smith",
                                Telephone = "01827 65779",
                                TextPhone = "01827 65779",
                                Url = "www.testservice.com",
                                Email = "support@testservice.com"
                            }
                        }
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
                        Title = "Mr",
                        Name = "John Smith",
                        Telephone = "01827 65779",
                        TextPhone = "01827 65779",
                        Url = "www.testservice.com",
                        Email = "support@testservice.com"
                    }
                }
            }
        };
    }
}
