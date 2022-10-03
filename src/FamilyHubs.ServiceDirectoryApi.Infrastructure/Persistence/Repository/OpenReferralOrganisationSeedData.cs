using FamilyHubs.ServiceDirectory.Shared.Enums;
using fh_service_directory_api.core.Entities;
using fh_service_directory_api.core.Interfaces.Entities;

namespace fh_service_directory_api.infrastructure.Persistence.Repository;

public class OpenReferralOrganisationSeedData
{
    public IReadOnlyCollection<OpenReferralOrganisation> SeedOpenReferralOrganistions()
    {
        List<OpenReferralOrganisation> openReferralOrganistions = new()
        {
            GetBristolCountyCouncil()
        };

        return openReferralOrganistions;
    }

    private OpenReferralOrganisation GetBristolCountyCouncil()
    {
        var bristolCountyCouncil = new OpenReferralOrganisation(
            "72e653e8-1d05-4821-84e9-9177571a6013",
            "Bristol County Council",
            "Bristol County Council",
            null,
            new Uri("https://www.bristol.gov.uk/").ToString(),
            "https://www.bristol.gov.uk/",
            new List<OpenReferralReview>(),
            GetBristolCountyCouncilServices("72e653e8-1d05-4821-84e9-9177571a6013")
            );
        return bristolCountyCouncil;
    }

    private List<OpenReferralService> GetBristolCountyCouncilServices(string parentId)
    {
        return new()
        {
            new OpenReferralService(
                "4591d551-0d6a-4c0d-b109-002e67318231",
                parentId,
                "Aid for Children with Tracheostomies",
                @"Aid for Children with Tracheostomies is a national self help group operating as a registered charity and is run by parents of children with a tracheostomy and by people who sympathise with the needs of such families. ACT as an organisation is non profit making, it links groups and individual members throughout Great Britain and Northern Ireland.",
                null,
                null,
                null,
                null,
                null,
                "active",
                "www.actfortrachykids.com",
                "support@ACTfortrachykids.com",
                null,
                new List<OpenReferralServiceDelivery>( new List<OpenReferralServiceDelivery>
                {
                    new OpenReferralServiceDelivery("9db7f878-be53-4a45-ac47-472568dfeeea",ServiceDelivery.Online)
                }),
                new List<OpenReferralEligibility>(new List<OpenReferralEligibility>
                {
                    new OpenReferralEligibility("9109Children","",null,0,13,new List<OpenReferralTaxonomy>())
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
                        "1567",
                        "Mr",
                        "John Smith",
                        new List<OpenReferralPhone>(new List<OpenReferralPhone>()
                        {
                            new OpenReferralPhone("1567", "01827 65778")
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
                            "256d0b97-d4c4-48e8-9475-bd7d42d1fc69",
                            "",
                            "",
                            52.6312,
                            -1.66526,
                            new List<OpenReferralPhysical_Address>(new List<OpenReferralPhysical_Address>()
                            {
                                new OpenReferralPhysical_Address(
                                    Guid.NewGuid().ToString(),
                                    "75 Sheepcote Lane",
                                    ", Stathe, Tamworth, Staffordshire, ",
                                    "B77 3JN",
                                    "England",
                                    null
                                    )
                            }),
                            new List<Accessibility_For_Disabilities>()
                            ),
                        new List<OpenReferralHoliday_Schedule>(),
                        new List<OpenReferralRegular_Schedule>()
                        )

                }),
                new List<OpenReferralService_Taxonomy>( new List<OpenReferralService_Taxonomy>()
                {
                    new OpenReferralService_Taxonomy
                    ("9107",
                    null,
                    new OpenReferralTaxonomy(
                        "bccsource:Organisation",
                        "Organisation",
                        "BCC Data Sources",
                        null
                        )),

                    new OpenReferralService_Taxonomy
                    ("9108",
                    null,
                    new OpenReferralTaxonomy(
                        "bccprimaryservicetype:38",
                        "Support",
                        "BCC Primary Services",
                        null
                        )),

                    new OpenReferralService_Taxonomy
                    ("9109",
                    null,
                    new OpenReferralTaxonomy(
                        "bccagegroup:37",
                        "Children",
                        "BCC Age Groups",
                        null
                        )),

                    new OpenReferralService_Taxonomy
                    ("9110",
                    null,
                    new OpenReferralTaxonomy(
                        "bccusergroup:56",
                        "Long Term Health Conditions",
                        "BCC User Groups",
                        null
                        ))
                }
                )),

            new OpenReferralService(
                "96781fd9-95a2-4196-8db6-0f083f1c38fc",
                parentId,
                "Test Service",
                @"This is a test service.",
                null,
                null,
                null,
                null,
                null,
                "active",
                "www.test1.com",
                "support@test1.com",
                null,
                new List<OpenReferralServiceDelivery>( new List<OpenReferralServiceDelivery>
                {
                    new OpenReferralServiceDelivery("b2518131-90fc-4149-bbd1-1c9992cd92ac",ServiceDelivery.Online)
                }),
                new List<OpenReferralEligibility>(new List<OpenReferralEligibility>
                {
                    new OpenReferralEligibility("9109T1Children","",null,0,13,new List<OpenReferralTaxonomy>())
                }),
                new List<OpenReferralFunding>(),
                new List<OpenReferralHoliday_Schedule>(),
                new List<OpenReferralLanguage>()
                {
                    new OpenReferralLanguage("c3b81a2c-14c7-43f3-8ed0-a8fc2087b942","English")
                },
                new List<OpenReferralRegular_Schedule>(),
                new List<OpenReferralReview>(),
                new List<OpenReferralContact>(new List<OpenReferralContact>()
                {
                    new OpenReferralContact(
                        "1561",
                        "Mr",
                        "John Smith",
                        new List<OpenReferralPhone>(new List<OpenReferralPhone>()
                        {
                            new OpenReferralPhone("1561", "01827 65711")
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
                        "1849",
                        new OpenReferralLocation(
                            "9181c5c1-b55a-41d6-aaf4-f4fc9f0bc644",
                            "",
                            "",
                            52.6312,
                            -1.66526,
                            new List<OpenReferralPhysical_Address>(new List<OpenReferralPhysical_Address>()
                            {
                                new OpenReferralPhysical_Address(
                                    Guid.NewGuid().ToString(),
                                    "71 Sheepcote Lane",
                                    ", Stathe, Tamworth, Staffordshire, ",
                                    "B77 3JN",
                                    "England",
                                    null
                                    )
                            }),
                            new List<Accessibility_For_Disabilities>()
                            ),
                        new List<OpenReferralHoliday_Schedule>(),
                        new List<OpenReferralRegular_Schedule>()
                        )

                }),
                new List<OpenReferralService_Taxonomy>( new List<OpenReferralService_Taxonomy>()
                {
                    new OpenReferralService_Taxonomy
                    ("9110TestDelete",
                    null,
                    new OpenReferralTaxonomy(
                        "bccusergroupTestDelete:56",
                        "Test Conditions",
                        "BCC User Groups",
                        null
                        ))
                }
                ))

        };
    }
}
