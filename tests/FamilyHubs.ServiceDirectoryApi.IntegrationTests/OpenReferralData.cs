using FamilyHubs.ServiceDirectory.Shared.Enums;
using fh_service_directory_api.core.Entities;

namespace FamilyHubs.ServiceDirectoryApi.IntegrationTests;

public class OpenReferralData
{
    public IReadOnlyCollection<OpenReferralOrganisation> SeedOpenReferralOrganistions()
    {
        List<OpenReferralOrganisation> openReferralOrganistions = new()
        {
            GetTestCountyCouncil()
        };

        return openReferralOrganistions;
    }

    public OpenReferralOrganisation GetTestCountyCouncil()
    {
        var bristolCountyCouncil = new OpenReferralOrganisation(
            "dcf1d9a2-004f-40e8-82aa-8a2660765d6e",
            GetOrganisationTypes().ElementAt(0),
            "Bristol City",
            "Test County Council",
            "Test County Council",
            null,
            new Uri("https://www.testcouncil.gov.uk/").ToString(),
            "https://www.testcouncil.gov.uk/",
            new List<OpenReferralReview>(),
            GetBristolCountyCouncilServices("dcf1d9a2-004f-40e8-82aa-8a2660765d6e")
            );

        bristolCountyCouncil.CreatedBy = "TestSystem";
        bristolCountyCouncil.Created = DateTime.Now;
        if (bristolCountyCouncil.Services != null)
        {
            bristolCountyCouncil.Services.ElementAt(0).CreatedBy = "TestSystem";
            bristolCountyCouncil.Services.ElementAt(0).Created = DateTime.Now;
            bristolCountyCouncil.Services.ElementAt(0).ServiceDelivery.ElementAt(0).CreatedBy = "TestSystem";
            bristolCountyCouncil.Services.ElementAt(0).ServiceDelivery.ElementAt(0).Created = DateTime.Now;
            bristolCountyCouncil.Services.ElementAt(0).Eligibilities.ElementAt(0).CreatedBy = "TestSystem";
            bristolCountyCouncil.Services.ElementAt(0).Eligibilities.ElementAt(0).Created = DateTime.Now;
            bristolCountyCouncil.Services.ElementAt(0).Languages.ElementAt(0).CreatedBy = "TestSystem";
            bristolCountyCouncil.Services.ElementAt(0).Languages.ElementAt(0).Created = DateTime.Now;
            bristolCountyCouncil.Services.ElementAt(0).Contacts.ElementAt(0).CreatedBy = "TestSystem";
            bristolCountyCouncil.Services.ElementAt(0).Contacts.ElementAt(0).Created = DateTime.Now;
            if (bristolCountyCouncil.Services.ElementAt(0).Contacts.ElementAt(0).Phones != null)
            {
#pragma warning disable CS8604 // Possible null reference argument.
                bristolCountyCouncil.Services.ElementAt(0).Contacts.ElementAt(0).Phones.ElementAt(0).CreatedBy = "TestSystem";
                bristolCountyCouncil.Services.ElementAt(0).Contacts.ElementAt(0).Phones.ElementAt(0).Created = DateTime.Now;
#pragma warning restore CS8604 // Possible null reference argument.
            }
            bristolCountyCouncil.Services.ElementAt(0).Service_at_locations.ElementAt(0).CreatedBy = "TestSystem";
            bristolCountyCouncil.Services.ElementAt(0).Service_at_locations.ElementAt(0).Created = DateTime.Now;


        }
        

        return bristolCountyCouncil;
    }

    public IReadOnlyCollection<OrganisationType> GetOrganisationTypes()
    {
        List<OrganisationType> serviceTypes = new()
        {
            new OrganisationType("1", "Local Authority", "Local Authority"),
            new OrganisationType("2", "Voluntary, Charitable, Faith", "Voluntary, Charitable, Faith"),
            new OrganisationType("2", "Private Company", "Private Company")
        };

        return serviceTypes;
    }

    private IReadOnlyCollection<ServiceType> GetServiceTypes()
    {
        List<ServiceType> serviceTypes = new()
        {
            new ServiceType("1", "Family Experience","Family Experience"),
            new ServiceType("2", "Family Information","Family Experience")
        };

        return serviceTypes;
    }

    private List<OpenReferralService> GetBristolCountyCouncilServices(string parentId)
    {
        return new()
        {
            new OpenReferralService(
                "9f01190b-429c-41fd-ba38-936d2995398b",
                 GetServiceTypes().ElementAt(1),
                parentId,
                "Test Service",
                @"Test Service Description",
                null,
                null,
                null,
                null,
                null,
                "active",
                "www.testservice.com",
                "support@testservice.com",
                null,
                new List<OpenReferralServiceDelivery>( new List<OpenReferralServiceDelivery>
                {
                    new OpenReferralServiceDelivery("4c10bb58-eee2-4508-842d-a903949304cc",ServiceDelivery.Online)
                }),
                new List<OpenReferralEligibility>(new List<OpenReferralEligibility>
                {
                    new OpenReferralEligibility("9109TestChildren","",null,0,13,new List<OpenReferralTaxonomy>())
                }),
                new List<OpenReferralFunding>(),
                new List<OpenReferralHoliday_Schedule>(),
                new List<OpenReferralLanguage>()
                {
                    new OpenReferralLanguage("724630f7-4c8b-4864-96be-bc74891f2b4a","English")
                },
                new List<OpenReferralRegular_Schedule>(),
                new List<OpenReferralReview>(),
                new List<OpenReferralContact>(new List<OpenReferralContact>()
                {
                    new OpenReferralContact(
                        "1568",
                        "Mr",
                        "John Smith",
                        new List<OpenReferralPhone>(new List<OpenReferralPhone>()
                        {
                            new OpenReferralPhone("1568", "01827 65779")
                        }
                        ))
                }),
                new List<OpenReferralCost_Option>(),
                new List<OpenReferralService_Area>()
                {
                    new OpenReferralService_Area(Guid.NewGuid().ToString(), "National", null, null, "http://statistics.data.gov.uk/id/statistical-geography/K02000001")
                },
                new List<OpenReferralServiceAtLocation>( new List<OpenReferralServiceAtLocation>()
                {
                    new OpenReferralServiceAtLocation(
                        "1749",
                        new OpenReferralLocation(
                            "2e78451d-ceb9-4664-bd5c-303e3a11245f",
                            "",
                            "",
                            52.6312,
                            -1.66526,
                            new List<OpenReferralPhysical_Address>(new List<OpenReferralPhysical_Address>()
                            {
                                new OpenReferralPhysical_Address(
                                    Guid.NewGuid().ToString(),
                                    "76 Sheepcote Lane",
                                    ", Stathe, Tamworth, Staffordshire, ",
                                    "B77 3JN",
                                    "England",
                                    null
                                    )
                            }),
                            new List<Accessibility_For_Disabilities>()
                            ),
                        new List<OpenReferralRegular_Schedule>(),
                        new List<OpenReferralHoliday_Schedule>()
                        )

                }),
                new List<OpenReferralService_Taxonomy>( new List<OpenReferralService_Taxonomy>()
                {
                    new OpenReferralService_Taxonomy
                    ("9111",
                    null,
                    new OpenReferralTaxonomy(
                        "bccsource:Organisation",
                        "Organisation",
                        "BCC Data Sources",
                        null
                        )),

                    new OpenReferralService_Taxonomy
                    ("9112",
                    null,
                    new OpenReferralTaxonomy(
                        "bccprimaryservicetype:38",
                        "Support",
                        "BCC Primary Services",
                        null
                        )),

                    new OpenReferralService_Taxonomy
                    ("9113",
                    null,
                    new OpenReferralTaxonomy(
                        "bccagegroup:37",
                        "Children",
                        "BCC Age Groups",
                        null
                        )),

                    new OpenReferralService_Taxonomy
                    ("9114",
                    null,
                    new OpenReferralTaxonomy(
                        "bccusergroup:56",
                        "Long Term Health Conditions",
                        "BCC User Groups",
                        null
                        ))
                }
                ))

        };
    }
}
