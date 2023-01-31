using FamilyHubs.ServiceDirectory.Core.Entities;
using FamilyHubs.ServiceDirectory.Shared.Enums;

namespace FamilyHubs.ServiceDirectoryApi.IntegrationTests;

public class TestData
{
    public IReadOnlyCollection<Organisation> SeedOrganisations()
    {
        List<Organisation> organisations = new()
        {
            GetTestCountyCouncil()
        };

        return organisations;
    }

    public Organisation GetTestCountyCouncil()
    {
        var bristolCountyCouncil = new Organisation(
            "dcf1d9a2-004f-40e8-82aa-8a2660765d6e",
            new OrganisationType("1", "LA", "Local Authority"),
            "Test County Council",
            "Test County Council",
            null,
            new Uri("https://www.testcouncil.gov.uk/").ToString(),
            "https://www.testcouncil.gov.uk/",
            new List<Review>(),
            GetBristolCountyCouncilServices("dcf1d9a2-004f-40e8-82aa-8a2660765d6e"),
            new List<LinkContact>());

        bristolCountyCouncil.CreatedBy = "TestSystem";
        bristolCountyCouncil.Created = DateTime.UtcNow;
        if (bristolCountyCouncil.Services != null)
        {
            bristolCountyCouncil.Services.ElementAt(0).CreatedBy = "TestSystem";
            bristolCountyCouncil.Services.ElementAt(0).Created = DateTime.UtcNow;
            bristolCountyCouncil.Services.ElementAt(0).ServiceDeliveries.ElementAt(0).CreatedBy = "TestSystem";
            bristolCountyCouncil.Services.ElementAt(0).ServiceDeliveries.ElementAt(0).Created = DateTime.UtcNow;
            bristolCountyCouncil.Services.ElementAt(0).Eligibilities.ElementAt(0).CreatedBy = "TestSystem";
            bristolCountyCouncil.Services.ElementAt(0).Eligibilities.ElementAt(0).Created = DateTime.UtcNow;
            bristolCountyCouncil.Services.ElementAt(0).Languages.ElementAt(0).CreatedBy = "TestSystem";
            bristolCountyCouncil.Services.ElementAt(0).Languages.ElementAt(0).Created = DateTime.UtcNow;
            bristolCountyCouncil.Services.ElementAt(0).LinkContacts.ElementAt(0).CreatedBy = "TestSystem";
            bristolCountyCouncil.Services.ElementAt(0).LinkContacts.ElementAt(0).Created = DateTime.UtcNow;
            bristolCountyCouncil.Services.ElementAt(0).ServiceAtLocations.ElementAt(0).CreatedBy = "TestSystem";
            bristolCountyCouncil.Services.ElementAt(0).ServiceAtLocations.ElementAt(0).Created = DateTime.UtcNow;
        }

        return bristolCountyCouncil;
    }

    private List<Service> GetBristolCountyCouncilServices(string parentId)
    {
        return new()
        {
            new Service(
                "9f01190b-429c-41fd-ba38-936d2995398b",
                new ServiceType("1", "Information Sharing", ""),
                parentId,
                "Test Service",
                @"Test Service Description",
                null,
                null,
                null,
                "active",
                null,
                null,
                null,
                false,
                new List<ServiceDelivery>( new List<ServiceDelivery>
                {
                    new ServiceDelivery("4c10bb58-eee2-4508-842d-a903949304cc",ServiceDeliveryType.Online)
                }),
                new List<Eligibility>(new List<Eligibility>
                {
                    new Eligibility("9109TestChildren","",null,0,13,new List<Taxonomy>())
                }),
                new List<Funding>(),
                new List<CostOption>(),
                new List<Language>
                {
                    new Language("724630f7-4c8b-4864-96be-bc74891f2b4a","English")
                },
                new List<Review>(),
                new List<ServiceArea>
                {
                    new ServiceArea(Guid.NewGuid().ToString(), "National", null, null, "http://statistics.data.gov.uk/id/statistical-geography/K02000001")
                },
                new List<ServiceAtLocation>( new List<ServiceAtLocation>
                {
                    new ServiceAtLocation(
                        "1749",
                        new Location(
                            "2e78451d-ceb9-4664-bd5c-303e3a11245f",
                            "",
                            "",
                            52.6312,
                            -1.66526,
                            new List<LinkTaxonomy>(),
                            new List<PhysicalAddress>(new List<PhysicalAddress>
                            {
                                new PhysicalAddress(
                                    Guid.NewGuid().ToString(),
                                    "76 Sheepcote Lane",
                                    ", Stathe, Tamworth, Staffordshire, ",
                                    "B77 3JN",
                                    "England",
                                    null
                                )
                            }),
                            new List<AccessibilityForDisabilities>(), 
                            new List<LinkContact>()),
                        new List<RegularSchedule>(),
                        new List<HolidaySchedule>(), 
                        new List<LinkContact>())

                }),
                new List<ServiceTaxonomy>( new List<ServiceTaxonomy>
                {
                    new ServiceTaxonomy("9111",
                        null,
                        new Taxonomy(
                            "bccsource:Organisation",
                            "Organisation",
                            "BCC Data Sources",
                            null
                        )),
                    new ServiceTaxonomy("9112",
                        null,
                        new Taxonomy(
                            "bccprimaryservicetype:38",
                            "Support",
                            "BCC Primary Services",
                            null
                        )),
                    new ServiceTaxonomy("9113",
                        null,
                        new Taxonomy(
                            "bccagegroup:37",
                            "Children",
                            "BCC Age Groups",
                            null
                        )),
                    new ServiceTaxonomy("9114",
                        null,
                        new Taxonomy(
                            "bccusergroup:56",
                            "Long Term Health Conditions",
                            "BCC User Groups",
                            null
                        ))
                    }
                ),
                new List<HolidaySchedule>(),
                new List<RegularSchedule>(),
                new List<LinkContact>(new List<LinkContact>
                {
                    new LinkContact(
                        "3010521b-6e0a-41b0-b610-200edbbeeb14",
                        "3010521b-6e0a-41b0-b610-200edbbeeb11",
                        "Service",
                    new Contact(
                        "1568",
                        "Mr",
                        "John Smith",
                        "01827 65779",
                        "01827 65779",
                        "www.testservice.com",
                        "support@testservice.com"))
                }))

        };
    }
}
