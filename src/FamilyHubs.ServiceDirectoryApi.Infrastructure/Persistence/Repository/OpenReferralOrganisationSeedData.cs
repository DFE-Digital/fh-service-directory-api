using FamilyHubs.ServiceDirectory.Shared.Enums;
using fh_service_directory_api.core.Entities;

namespace fh_service_directory_api.infrastructure.Persistence.Repository;

public class OpenReferralOrganisationSeedData
{
    public IReadOnlyCollection<OpenReferralLocation> SeedFamilyHubs()
    {
        List<OpenReferralLocation> locations = new()
        {
            new OpenReferralLocation("964ea451-6146-4add-913e-dff23a1bd7b6", "Central Family Hub", "Broughton Hub", -2.259764D, 53.507025D, new List<OpenReferralPhysical_Address>() { new OpenReferralPhysical_Address("84c0a47a-08a1-4e54-814b-46ce26765c08", "50 Rigby Street", "Manchester", "M7 4BQ", "United Kingdom", "Salford") }, new List<Accessibility_For_Disabilities>()),
            new OpenReferralLocation("74c37f53-dbc0-4958-8c97-baee41a022bf", "North Family Hub", "Swinton Gateway", -2.342044D, 53.5124278D, new List<OpenReferralPhysical_Address>() { new OpenReferralPhysical_Address("83bf4f64-fdcb-46bc-a32e-8b117bf5c19a", "100 Chorley Road", "Manchester", "M27 6BP", "United Kingdom", "Salford") }, new List<Accessibility_For_Disabilities>()),
            new OpenReferralLocation("1b4a625b-54bb-407d-a508-f90cade1e96f", "South Family Hub", "Winton Children’s Centre", -2.368140748303118D, 53.48801070060149D,  new List<OpenReferralPhysical_Address>() { new OpenReferralPhysical_Address("45603703-5d3f-45f4-84f1-b294ac4d3290", "Brindley Street", "Manchester", "M30 8AB", "United Kingdom", "Salford") }, new List<Accessibility_For_Disabilities>()),
        };

        return locations;
    }
    public IReadOnlyCollection<OpenReferralTaxonomy> SeedTaxonomies()
    {
        List<OpenReferralTaxonomy> taxonomies = new()
        {
            new OpenReferralTaxonomy("d242700a-b2ad-42fe-8848-61534002156c", "FamilyHub", "FX Family Hub", null)
        };

        return taxonomies;
    }

    public IReadOnlyCollection<ModelLink> SeedModelLinks()
    {
        List<ModelLink> modelLinks = new()
        {
            new ModelLink("ebe8647d-e06d-478d-a6af-948dd7a289c7", fh_service_directory_api.core.StaticContants.Location_Taxonomy, "964ea451-6146-4add-913e-dff23a1bd7b6", "d242700a-b2ad-42fe-8848-61534002156c"),
            new ModelLink("760f05de-07df-4e58-a429-20922416b221", fh_service_directory_api.core.StaticContants.Location_Taxonomy, "74c37f53-dbc0-4958-8c97-baee41a022bf", "d242700a-b2ad-42fe-8848-61534002156c"),
            new ModelLink("9c8df56e-bebf-486f-8544-39f8deccbe94", fh_service_directory_api.core.StaticContants.Location_Taxonomy, "1b4a625b-54bb-407d-a508-f90cade1e96f", "d242700a-b2ad-42fe-8848-61534002156c"),

            new ModelLink("dc5e2bdb-ed5a-4278-b3a3-f0622502b5e4", fh_service_directory_api.core.StaticContants.Location_Organisation, "964ea451-6146-4add-913e-dff23a1bd7b6", "ca8ddaeb-b5e5-46c4-b94d-43a8e2ccc066"),
            new ModelLink("4cb330f6-f68f-4529-8a55-dffa8b9d48af", fh_service_directory_api.core.StaticContants.Location_Organisation, "74c37f53-dbc0-4958-8c97-baee41a022bf", "ca8ddaeb-b5e5-46c4-b94d-43a8e2ccc066"),
            new ModelLink("06bf080c-b161-4fc3-b015-81535ced46d8", fh_service_directory_api.core.StaticContants.Location_Organisation, "1b4a625b-54bb-407d-a508-f90cade1e96f", "ca8ddaeb-b5e5-46c4-b94d-43a8e2ccc066"),

        };

        return modelLinks;
    }

    public IReadOnlyCollection<OrganisationAdminDistrict> SeedOrganisationAdminDistrict()
    {
        List<OrganisationAdminDistrict> adminDistricts = new()
        {
            new OrganisationAdminDistrict(Guid.NewGuid().ToString(),"E06000023", "72e653e8-1d05-4821-84e9-9177571a6013"), //Bristol
            new OrganisationAdminDistrict(Guid.NewGuid().ToString(), "E07000127", "fc51795e-ea95-4af0-a0b2-4c06d5463678"), //Lancashire
            new OrganisationAdminDistrict(Guid.NewGuid().ToString(), "E09000026", "1229cb45-0dc0-4f8a-81bd-2cd74c7cc9cc"), //Redbridge
            new OrganisationAdminDistrict(Guid.NewGuid().ToString(), "E08000006", "ca8ddaeb-b5e5-46c4-b94d-43a8e2ccc066"), //Salford
            new OrganisationAdminDistrict(Guid.NewGuid().ToString(), "E07000203", "6dc1c3ad-d077-46ff-9e0d-04fb263f0637"), //Suffolk
            new OrganisationAdminDistrict(Guid.NewGuid().ToString(), "E09000030", "88e0bffd-ed0b-48ea-9a70-5f6ef729fc21"), //Tower Hamlets
        };

        return adminDistricts;
    }

    public IReadOnlyCollection<OrganisationType> SeedOrganisationTypes()
    {
        List<OrganisationType> serviceTypes = new()
        {
            new OrganisationType("1", "LA", "Local Authority"),
            new OrganisationType("2", "VCFS", "Voluntary, Charitable, Faith Sector"),
            new OrganisationType("3", "Company", "Public / Private Company eg: Child Care Centre")
        };

        return serviceTypes;
    }

    public IReadOnlyCollection<ServiceType> SeedServiceTypes()
    {
        List<ServiceType> serviceTypes = new()
        {
            new ServiceType("1", "Information Sharing", ""),
            new ServiceType("2", "Family Experience", "")
        };

        return serviceTypes;
    }
    public IReadOnlyCollection<OpenReferralOrganisation> SeedOpenReferralOrganistions(OrganisationType organisationType)
    {
        List<OpenReferralOrganisation> openReferralOrganistions = new()
        {
            GetBristolCountyCouncil(organisationType),
            new OpenReferralOrganisation(
            "fc51795e-ea95-4af0-a0b2-4c06d5463678",
            organisationType,
            "Lancashire County Council",
            "Lancashire County Council",
            null,
            new Uri("https://www.lancashire.gov.uk/").ToString(),
            "https://www.lancashire.gov.uk/",
            new List<OpenReferralReview>(),
            new List<OpenReferralService>()
            ),
            new OpenReferralOrganisation(
            "1229cb45-0dc0-4f8a-81bd-2cd74c7cc9cc",
            organisationType,
            "London Borough of Redbridge",
            "London Borough of Redbridge",
            null,
            new Uri("https://www.redbridge.gov.uk/").ToString(),
            "https://www.redbridge.gov.uk/",
            new List<OpenReferralReview>(),
            new List<OpenReferralService>()
            ),
            new OpenReferralOrganisation(
            "ca8ddaeb-b5e5-46c4-b94d-43a8e2ccc066",
            organisationType,
            "Salford City Council",
            "Salford City Council",
            null,
            new Uri("https://www.salford.gov.uk/").ToString(),
            "https://www.salford.gov.uk/",
            new List<OpenReferralReview>(),
            new List<OpenReferralService>()
            ),
            new OpenReferralOrganisation(
            "6dc1c3ad-d077-46ff-9e0d-04fb263f0637",
            organisationType,
            "Suffolk County Council",
            "Suffolk County Council",
            null,
            new Uri("https://www.suffolk.gov.uk/").ToString(),
            "https://www.suffolk.gov.uk/",
            new List<OpenReferralReview>(),
            new List<OpenReferralService>()
            ),
            new OpenReferralOrganisation(
            "88e0bffd-ed0b-48ea-9a70-5f6ef729fc21",
            organisationType,
            "Tower Hamlets Council",
            "Tower Hamlets Council",
            null,
            new Uri("https://www.towerhamlets.gov.uk/").ToString(),
            "https://www.towerhamlets.gov.uk/",
            new List<OpenReferralReview>(),
            new List<OpenReferralService>()
            )
        };

        return openReferralOrganistions;
    }

    private OpenReferralOrganisation GetBristolCountyCouncil(OrganisationType organisationType)
    {
        var bristolCountyCouncil = new OpenReferralOrganisation(
            "72e653e8-1d05-4821-84e9-9177571a6013",
            organisationType,
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
                SeedServiceTypes().ElementAt(0),
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
                    new OpenReferralEligibility("9109Children","",null,13,0,new List<OpenReferralTaxonomy>())
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
                                    "7A Boyce's Ave, Clifton", 
                                    "Bristol", 
                                    "BS8 4AA",
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
                SeedServiceTypes().ElementAt(0),
                parentId,
                "Test Service - Free - 10 to 15 yrs",
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
                    new OpenReferralEligibility("9109T1Children","Children",null,15,10,new List<OpenReferralTaxonomy>())
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
                                    "7A Boyce's Ave, Clifton",
                                    "Bristol",
                                    "BS8 4AA",
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
                    ("9110TestDelete",
                    null,
                    new OpenReferralTaxonomy(
                        "bccusergroupTestDelete:56",
                        "Test Conditions",
                        "BCC User Groups",
                        null
                        ))
                }
                )),

            new OpenReferralService(
                "207613c5-4cdf-46b2-a110-cf120a9412f9",
                SeedServiceTypes().ElementAt(0),
                parentId,
                "Test Service - Paid - 0 to 13yrs",
                @"This is a paid test service.",
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
                    new OpenReferralServiceDelivery("46d1d3df-c7f7-47f0-9672-e18b67594d4e",ServiceDelivery.Online)
                }),
                new List<OpenReferralEligibility>(new List<OpenReferralEligibility>
                {
                    new OpenReferralEligibility("9109T2Children","Children",null,13,0,new List<OpenReferralTaxonomy>())
                }),
                new List<OpenReferralFunding>(),
                new List<OpenReferralHoliday_Schedule>(),
                new List<OpenReferralLanguage>()
                {
                    new OpenReferralLanguage("51e75b71-56b9-4cde-9a14-f2a520deff5e","English")
                },
                new List<OpenReferralRegular_Schedule>(),
                new List<OpenReferralReview>(),
                new List<OpenReferralContact>(new List<OpenReferralContact>()
                {
                    new OpenReferralContact(
                        "1562",
                        "Mr",
                        "John Smith",
                        new List<OpenReferralPhone>(new List<OpenReferralPhone>()
                        {
                            new OpenReferralPhone("1562", "01827 64328")
                        }
                        ))
                }),
                new List<OpenReferralCost_Option>(new List<OpenReferralCost_Option>()
                {
                    new OpenReferralCost_Option("1345", "Session", 45.0m, null, null, null, null)
                }),
                new List<OpenReferralService_Area>()
                {
                    new OpenReferralService_Area(Guid.NewGuid().ToString(), "National", null, null, "http://statistics.data.gov.uk/id/statistical-geography/K02000001")
                },
                new List<OpenReferralServiceAtLocation>( new List<OpenReferralServiceAtLocation>()
                {
                    new OpenReferralServiceAtLocation(
                        "1867",
                        new OpenReferralLocation(
                            "fb82fbea-78bb-40c7-bbca-a29f95589483",
                            "",
                            "",
                            52.63123,
                            -1.66519,
                            new List<OpenReferralPhysical_Address>(new List<OpenReferralPhysical_Address>()
                            {
                                new OpenReferralPhysical_Address(
                                    Guid.NewGuid().ToString(),
                                    "7A Boyce's Ave, Clifton",
                                    "Bristol",
                                    "BS8 4AA",
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
                    ("9120TestDelete",
                    null,
                    new OpenReferralTaxonomy(
                        "bccusergroupTestDelete2:56",
                        "Test Conditions",
                        "BCC User Groups",
                        null
                        ))
                }
                )),

        };
    }
}
