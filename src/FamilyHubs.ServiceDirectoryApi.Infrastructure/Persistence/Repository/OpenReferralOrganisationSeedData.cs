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

    public IReadOnlyCollection<OpenReferralTaxonomy> SeedOpenReferralTaxonomies()
    {
        List<OpenReferralTaxonomy> openReferralTaxonomies = new()
        {
            new OpenReferralTaxonomy("16f3a451-e88d-4ad0-b53f-c8925d1cc9e4", "Activities, clubs and groups", "Activities, clubs and groups", null),
            new OpenReferralTaxonomy("aafa1cc3-b984-4b10-89d5-27388c5432de", "Activities", "Activities", "16f3a451-e88d-4ad0-b53f-c8925d1cc9e4"),
            new OpenReferralTaxonomy("3c207700-dc08-43bc-94ab-80c3d36d2e12", "Before and after school clubs", "Before and after school clubs", "16f3a451-e88d-4ad0-b53f-c8925d1cc9e4"),
            new OpenReferralTaxonomy("022ae22f-8be6-4b20-99a6-faf2b9e0291a", "Holiday clubs and schemes", "Holiday clubs and schemes", "16f3a451-e88d-4ad0-b53f-c8925d1cc9e4"),
            new OpenReferralTaxonomy("4d362474-79cc-449a-bafe-b128ab3b4f63", "Music, arts and dance", "Music, arts and dance", "16f3a451-e88d-4ad0-b53f-c8925d1cc9e4"),
            new OpenReferralTaxonomy("27ae8b5f-3249-40b0-b12c-e0b4b664d758", "Parent, baby and toddler groups", "Parent, baby and toddler groups", "16f3a451-e88d-4ad0-b53f-c8925d1cc9e4"),
            new OpenReferralTaxonomy("85cc81bd-c81a-4565-94fc-094bc605489e", "Pre-school playgroup", "Pre-school playgroup", "16f3a451-e88d-4ad0-b53f-c8925d1cc9e4"),
            new OpenReferralTaxonomy("e48bd335-ac3c-44ce-a0f7-57c91a823a2f", "Sports and recreation", "Sports and recreation","16f3a451-e88d-4ad0-b53f-c8925d1cc9e4"),

            new OpenReferralTaxonomy("94f0ba86-d5fb-4fac-a1ee-f12ba4ef3012", "Family support", "Family support", null),
            new OpenReferralTaxonomy("a6a8e423-7c32-493d-ad21-4732a40f2793", "Bullying and cyber bullying", "Bullying and cyber bullying", "94f0ba86-d5fb-4fac-a1ee-f12ba4ef3012"),
            new OpenReferralTaxonomy("9944d79d-00c8-43b7-b369-2f1baca1dcb0", "Debt and welfare advice", "Debt and welfare advice", "94f0ba86-d5fb-4fac-a1ee-f12ba4ef3012"),
            new OpenReferralTaxonomy("47599de2-6638-4ed7-8bc1-afe4ba47a797", "Domestic abuse", "Domestic abuse", "94f0ba86-d5fb-4fac-a1ee-f12ba4ef3012"),
            new OpenReferralTaxonomy("8ec7ae63-3f88-472a-92c9-27dcfca9565b", "Intensive targeted family support", "Intensive targeted family support", "94f0ba86-d5fb-4fac-a1ee-f12ba4ef3012"),
            new OpenReferralTaxonomy("94713493-8f49-4e96-8828-13aa12484866", "Money, benefits and housing", "Money, benefits and housing", "94f0ba86-d5fb-4fac-a1ee-f12ba4ef3012"),
            new OpenReferralTaxonomy("f11a9fdd-de48-499a-ac2d-2bd01dfc22f1", "Parenting support", "Parenting support", "94f0ba86-d5fb-4fac-a1ee-f12ba4ef3012"),
            new OpenReferralTaxonomy("a092d35f-87f1-453d-937f-00534cd339aa", "Reducing parental conflict","Reducing parental conflict", "94f0ba86-d5fb-4fac-a1ee-f12ba4ef3012"),
            new OpenReferralTaxonomy("110ba031-18e8-4e7c-8b91-70a5b3504d0f", "Separating and separated parent support","Separating and separated parent support", "94f0ba86-d5fb-4fac-a1ee-f12ba4ef3012"),
            new OpenReferralTaxonomy("6bca5e3d-ad93-4083-b721-31d89c9e357d", "Stopping smoking","Stopping smoking", "94f0ba86-d5fb-4fac-a1ee-f12ba4ef3012"),
            new OpenReferralTaxonomy("8fc58423-2f78-41f8-8211-947318940c50", "Substance misuse (including alcohol and drug)","Substance misuse (including alcohol and drug)", "94f0ba86-d5fb-4fac-a1ee-f12ba4ef3012"),
            new OpenReferralTaxonomy("005b3184-6ffb-414a-a1e3-6d5674dc0e63", "Support with parenting","Support with parenting", "94f0ba86-d5fb-4fac-a1ee-f12ba4ef3012"),
            new OpenReferralTaxonomy("8a74745f-b95e-4c57-be27-f3cc4e24ddd6", "Targeted youth support","Targeted youth support", "94f0ba86-d5fb-4fac-a1ee-f12ba4ef3012"),
            new OpenReferralTaxonomy("be1de9a2-a833-498b-95d3-9e525d4d9951", "Youth justice services","Youth justice services", "94f0ba86-d5fb-4fac-a1ee-f12ba4ef3012"),

            new OpenReferralTaxonomy("32712b43-e4f7-484f-97d7-beb3bb463133", "Health","Health", null),
            new OpenReferralTaxonomy("11696b1f-209a-47b1-9ef5-c588a14d43c6", "Hearing and sight","Hearing and sight", "32712b43-e4f7-484f-97d7-beb3bb463133"),
            new OpenReferralTaxonomy("7c39c3df-2ad4-4f94-aedd-d1cf5981e195", "Mental health, social and emotional support","Mental health, social and emotional support", "32712b43-e4f7-484f-97d7-beb3bb463133"),
            new OpenReferralTaxonomy("5b64e25a-929c-4a41-9c47-9faeaf2b5f29", "Nutrition and weight management","Nutrition and weight management", "32712b43-e4f7-484f-97d7-beb3bb463133"),
            new OpenReferralTaxonomy("2bc4dba9-14a8-4942-b52d-c0cf5622f5a7", "Oral health","Oral health", "32712b43-e4f7-484f-97d7-beb3bb463133"),
            new OpenReferralTaxonomy("75fe2e44-69c8-4da7-b4bd-d2d042acc657", "Public health services","Public health services", "32712b43-e4f7-484f-97d7-beb3bb463133"),

            new OpenReferralTaxonomy("ff704172-db6a-4e7a-b612-cd925e0aa7a0", "Pregnancy, birth and early years","Pregnancy, birth and early years", null),
            new OpenReferralTaxonomy("1e305952-ea60-43d6-bbc8-6cf6f1a1c7f0", "Birth registration","Birth registration", "ff704172-db6a-4e7a-b612-cd925e0aa7a0"),
            new OpenReferralTaxonomy("5f9acf55-be45-4847-9d45-cf94445b71ca", "Early years language and learning","Early years language and learning", "ff704172-db6a-4e7a-b612-cd925e0aa7a0"),
            new OpenReferralTaxonomy("e734cabd-2b69-4583-a844-9b6a4b266c71", "Health visiting","Health visiting", "ff704172-db6a-4e7a-b612-cd925e0aa7a0"),
            new OpenReferralTaxonomy("231a2bf5-b05a-4da7-a338-a838b83806db", "Infant feeding support (including breastfeeding)","Infant feeding support (including breastfeeding)", "ff704172-db6a-4e7a-b612-cd925e0aa7a0"),
            new OpenReferralTaxonomy("19c29d11-ffbc-41d0-841c-ea8f0dfdda94", "Midwife and maternity","Midwife and maternity", "ff704172-db6a-4e7a-b612-cd925e0aa7a0"),
            new OpenReferralTaxonomy("2b2489d5-aee8-42ef-8c97-d8fcccdb5a8b", "Perinatal mental health support (pregnancy to one year post birth)","Perinatal mental health support (pregnancy to one year post birth)", "ff704172-db6a-4e7a-b612-cd925e0aa7a0"),

            new OpenReferralTaxonomy("6c873b97-6978-4c0f-8e3c-0b2804dd3826", "Special educational needs and disabilities (SEND)","Special educational needs and disabilities (SEND)", null),
            new OpenReferralTaxonomy("2db648ae-69fb-4f06-b76f-b66a3bb64215", "Autistic Spectrum Disorder (ASD)","Autistic Spectrum Disorder (ASD)", "6c873b97-6978-4c0f-8e3c-0b2804dd3826"),
            new OpenReferralTaxonomy("9d5161ee-a289-47b4-a967-a5912ae143ba", "Breaks and respite","Breaks and respite", "6c873b97-6978-4c0f-8e3c-0b2804dd3826"),
            new OpenReferralTaxonomy("dacf095a-83df-4d23-a20a-da751d956ab9", "Early years support","Early years support", "6c873b97-6978-4c0f-8e3c-0b2804dd3826"),
            new OpenReferralTaxonomy("43349183-1145-4bae-bf49-d5398e33d1b2", "Groups for parents and carers of children with SEND","Groups for parents and carers of children with SEND", "6c873b97-6978-4c0f-8e3c-0b2804dd3826"),
            new OpenReferralTaxonomy("72a24e08-79d3-49d5-89ca-2d3d8c0e470e", "Hearing impairment","Hearing impairment", "6c873b97-6978-4c0f-8e3c-0b2804dd3826"),
            new OpenReferralTaxonomy("619344e1-2185-4d62-b3b3-fc95ac18cd9f", "Learning difficulties and disabilities","Learning difficulties and disabilities", "6c873b97-6978-4c0f-8e3c-0b2804dd3826"),
            new OpenReferralTaxonomy("0bc60c67-9fac-4b9e-aeee-38950859c700", "Multi-sensory impairment","Multi-sensory impairment", "6c873b97-6978-4c0f-8e3c-0b2804dd3826"),
            new OpenReferralTaxonomy("bf6db3be-b539-4a02-a212-3858126d35d2", "Other difficulties or disabilities","Other difficulties or disabilities", "6c873b97-6978-4c0f-8e3c-0b2804dd3826"),
            new OpenReferralTaxonomy("cbda7b61-d330-4923-a174-3cb4c5cf9c0a", "Physical disabilities","Physical disabilities", "6c873b97-6978-4c0f-8e3c-0b2804dd3826"),
            new OpenReferralTaxonomy("4f4053cc-9250-4109-9c30-3b53960524f7", "Social, emotional and mental health support","Social, emotional and mental health support", "6c873b97-6978-4c0f-8e3c-0b2804dd3826"),
            new OpenReferralTaxonomy("38bf4fc2-f6b9-4c15-bc07-b03b707659bd", "Speech, language and communication needs","Speech, language and communication needs", "6c873b97-6978-4c0f-8e3c-0b2804dd3826"),
            new OpenReferralTaxonomy("4c219f95-21da-4222-8286-bbe1cfaf675c", "Visual impairment","Visual impairment", "6c873b97-6978-4c0f-8e3c-0b2804dd3826"),

            new OpenReferralTaxonomy("be261f9e-f024-46f8-8b5b-58251f25388d", "Transport","Transport", null),
            new OpenReferralTaxonomy("93a29b1e-acd9-4abf-9f30-07dce3378558", "Community transport","Community transport", "be261f9e-f024-46f8-8b5b-58251f25388d"),

            new OpenReferralTaxonomy("d242700a-b2ad-42fe-8848-61534002156c", "FamilyHub", "FX Family Hub", null)
        };
        
        return openReferralTaxonomies;
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
            {
                new OpenReferralService(
                "1144EB57-32D2-4915-8807-8884EC27B48D",
                SeedServiceTypes().ElementAt(0),
                "ca8ddaeb-b5e5-46c4-b94d-43a8e2ccc066",
                "Salford Service 1",
                @"Salford Service 1.",
                null,
                null,
                null,
                null,
                null,
                "active",
                "www.Salford.gov.uk",
                "supporttestdev@SalfordTest.Gov.UK",
                null,
                false,
                new List<OpenReferralServiceDelivery>(),
                new List<OpenReferralEligibility>(),
                new List<OpenReferralFunding>(),
                new List<OpenReferralHoliday_Schedule>(),
                new List<OpenReferralLanguage>(),
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
                            new OpenReferralPhone("1568", "01234 567890")
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
                        "1750",
                        new OpenReferralLocation(
                            "00768BBC-E830-4F94-9126-FB44CD31204D",
                            "",
                            "",
                            52.6312,
                            -1.66526,
                            new List<OpenReferralPhysical_Address>(new List<OpenReferralPhysical_Address>()
                            {
                                new OpenReferralPhysical_Address(
                                    Guid.NewGuid().ToString(),
                                    "1A Del's Ave",
                                    "Salford",
                                    "ABC 123",
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
                    ("9999",
                    null,
                    new OpenReferralTaxonomy(
                        "3f6221f3-ce5f-4500-962e-3a5960d20474",
                        "Test Category 1","Test Category 1",
                        "32712b43-e4f7-484f-97d7-beb3bb463133"
                        ))
                }
                ))
            }
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
                false,
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
                    ("9109",
                    null,
                    new OpenReferralTaxonomy(
                        "11696b1f-209a-47b1-9ef5-c588a14d43c6",
                        "Hearing and sight","Hearing and sight",
                        "32712b43-e4f7-484f-97d7-beb3bb463133"
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
                false,
                new List<OpenReferralServiceDelivery>( new List<OpenReferralServiceDelivery>
                {
                    new OpenReferralServiceDelivery("b2518131-90fc-4149-bbd1-1c9992cd92ac",ServiceDelivery.InPerson)
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
                false,
                new List<OpenReferralServiceDelivery>( new List<OpenReferralServiceDelivery>
                {
                    new OpenReferralServiceDelivery("46d1d3df-c7f7-47f0-9672-e18b67594d4e",ServiceDelivery.Telephone)
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

            new OpenReferralService(
                "39f211fc-73d4-4ea1-a1c9-d8fc063d6b51",
                SeedServiceTypes().ElementAt(0),
                parentId,
                "Test Service - Paid - 15 to 20yrs - Afrikaans",
                @"This is an Afrikaans test service.",
                null,
                null,
                null,
                null,
                null,
                "active",
                "www.test1.com",
                "support@test1.com",
                null,
                true,
                new List<OpenReferralServiceDelivery>( new List<OpenReferralServiceDelivery>
                {
                    new OpenReferralServiceDelivery("d4e1fbfb-dd48-4950-b259-7547260ef838",ServiceDelivery.InPerson)
                }),
                new List<OpenReferralEligibility>(new List<OpenReferralEligibility>
                {
                    new OpenReferralEligibility("9109T3Children","Children",null,20,15,new List<OpenReferralTaxonomy>())
                }),
                new List<OpenReferralFunding>(),
                new List<OpenReferralHoliday_Schedule>(),
                new List<OpenReferralLanguage>()
                {
                    new OpenReferralLanguage("73a688a2-0dad-4674-ac63-5115c990ea1f","Afrikaans")
                },
                new List<OpenReferralRegular_Schedule>(),
                new List<OpenReferralReview>(),
                new List<OpenReferralContact>(new List<OpenReferralContact>()
                {
                    new OpenReferralContact(
                        "1563",
                        "Mr",
                        "John Smith",
                        new List<OpenReferralPhone>(new List<OpenReferralPhone>()
                        {
                            new OpenReferralPhone("1563", "01827 64328")
                        }
                        ))
                }),
                new List<OpenReferralCost_Option>(new List<OpenReferralCost_Option>()
                {
                    new OpenReferralCost_Option("1346", "Hour", 25.0m, null, null, null, null)
                }),
                new List<OpenReferralService_Area>()
                {
                    new OpenReferralService_Area(Guid.NewGuid().ToString(), "National", null, null, "http://statistics.data.gov.uk/id/statistical-geography/K02000001")
                },
                new List<OpenReferralServiceAtLocation>( new List<OpenReferralServiceAtLocation>()
                {
                    new OpenReferralServiceAtLocation(
                        "1868",
                        new OpenReferralLocation(
                            "68615e6e-c9e2-4a1f-8824-a5c1da36a953",
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
                    ("9121TestDelete",
                    null,
                    new OpenReferralTaxonomy(
                        "bccusergroupTestDelete3:56",
                        "Test Conditions",
                        "BCC User Groups",
                        null
                        ))
                }
                )),

        };
    }
}
