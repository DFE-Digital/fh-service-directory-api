using FamilyHubs.ServiceDirectory.Core;
using FamilyHubs.ServiceDirectory.Core.Entities;
using FamilyHubs.ServiceDirectory.Shared.Enums;

namespace FamilyHubs.ServiceDirectory.Infrastructure.Persistence.Repository;

public class OrganisationSeedData
{
    private bool IsProduction { get; set; }
    public OrganisationSeedData(bool isProduction)
    {
        IsProduction = isProduction;
    }
    public IReadOnlyCollection<AdminArea> SeedOrganisationAdminDistrict()
    {
        List<AdminArea> adminDistricts = new()
        {
            new AdminArea(Guid.NewGuid().ToString(),"E06000023", "72e653e8-1d05-4821-84e9-9177571a6013"), //Bristol
            new AdminArea(Guid.NewGuid().ToString(), "E10000017", "fc51795e-ea95-4af0-a0b2-4c06d5463678"), //Lancashire
            new AdminArea(Guid.NewGuid().ToString(), "E09000026", "1229cb45-0dc0-4f8a-81bd-2cd74c7cc9cc"), //Redbridge
            new AdminArea(Guid.NewGuid().ToString(), "E08000006", "ca8ddaeb-b5e5-46c4-b94d-43a8e2ccc066"), //Salford
            new AdminArea(Guid.NewGuid().ToString(), "E10000029", "6dc1c3ad-d077-46ff-9e0d-04fb263f0637"), //Suffolk
            new AdminArea(Guid.NewGuid().ToString(), "E09000030", "88e0bffd-ed0b-48ea-9a70-5f6ef729fc21"), //Tower Hamlets
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

    public IReadOnlyCollection<Taxonomy> SeedTaxonomies()
    {
        List<Taxonomy> taxonomies = new()
        {
            // categories and sub-categories
            new Taxonomy("16f3a451-e88d-4ad0-b53f-c8925d1cc9e4", "Activities, clubs and groups", "Activities, clubs and groups", null),
            new Taxonomy("aafa1cc3-b984-4b10-89d5-27388c5432de", "Activities", "Activities", "16f3a451-e88d-4ad0-b53f-c8925d1cc9e4"),
            new Taxonomy("3c207700-dc08-43bc-94ab-80c3d36d2e12", "Before and after school clubs", "Before and after school clubs", "16f3a451-e88d-4ad0-b53f-c8925d1cc9e4"),
            new Taxonomy("022ae22f-8be6-4b20-99a6-faf2b9e0291a", "Holiday clubs and schemes", "Holiday clubs and schemes", "16f3a451-e88d-4ad0-b53f-c8925d1cc9e4"),
            new Taxonomy("4d362474-79cc-449a-bafe-b128ab3b4f63", "Music, arts and dance", "Music, arts and dance", "16f3a451-e88d-4ad0-b53f-c8925d1cc9e4"),
            new Taxonomy("27ae8b5f-3249-40b0-b12c-e0b4b664d758", "Parent, baby and toddler groups", "Parent, baby and toddler groups", "16f3a451-e88d-4ad0-b53f-c8925d1cc9e4"),
            new Taxonomy("85cc81bd-c81a-4565-94fc-094bc605489e", "Pre-school playgroup", "Pre-school playgroup", "16f3a451-e88d-4ad0-b53f-c8925d1cc9e4"),
            new Taxonomy("e48bd335-ac3c-44ce-a0f7-57c91a823a2f", "Sports and recreation", "Sports and recreation","16f3a451-e88d-4ad0-b53f-c8925d1cc9e4"),

            new Taxonomy("94f0ba86-d5fb-4fac-a1ee-f12ba4ef3012", "Family support", "Family support", null),
            new Taxonomy("a6a8e423-7c32-493d-ad21-4732a40f2793", "Bullying and cyber bullying", "Bullying and cyber bullying", "94f0ba86-d5fb-4fac-a1ee-f12ba4ef3012"),
            new Taxonomy("9944d79d-00c8-43b7-b369-2f1baca1dcb0", "Debt and welfare advice", "Debt and welfare advice", "94f0ba86-d5fb-4fac-a1ee-f12ba4ef3012"),
            new Taxonomy("47599de2-6638-4ed7-8bc1-afe4ba47a797", "Domestic abuse", "Domestic abuse", "94f0ba86-d5fb-4fac-a1ee-f12ba4ef3012"),
            new Taxonomy("8ec7ae63-3f88-472a-92c9-27dcfca9565b", "Intensive targeted family support", "Intensive targeted family support", "94f0ba86-d5fb-4fac-a1ee-f12ba4ef3012"),
            new Taxonomy("94713493-8f49-4e96-8828-13aa12484866", "Money, benefits and housing", "Money, benefits and housing", "94f0ba86-d5fb-4fac-a1ee-f12ba4ef3012"),
            new Taxonomy("f11a9fdd-de48-499a-ac2d-2bd01dfc22f1", "Parenting support", "Parenting support", "94f0ba86-d5fb-4fac-a1ee-f12ba4ef3012"),
            new Taxonomy("a092d35f-87f1-453d-937f-00534cd339aa", "Reducing parental conflict","Reducing parental conflict", "94f0ba86-d5fb-4fac-a1ee-f12ba4ef3012"),
            new Taxonomy("110ba031-18e8-4e7c-8b91-70a5b3504d0f", "Separating and separated parent support","Separating and separated parent support", "94f0ba86-d5fb-4fac-a1ee-f12ba4ef3012"),
            new Taxonomy("6bca5e3d-ad93-4083-b721-31d89c9e357d", "Stopping smoking","Stopping smoking", "94f0ba86-d5fb-4fac-a1ee-f12ba4ef3012"),
            new Taxonomy("8fc58423-2f78-41f8-8211-947318940c50", "Substance misuse (including alcohol and drug)","Substance misuse (including alcohol and drug)", "94f0ba86-d5fb-4fac-a1ee-f12ba4ef3012"),
            new Taxonomy("8a74745f-b95e-4c57-be27-f3cc4e24ddd6", "Targeted youth support","Targeted youth support", "94f0ba86-d5fb-4fac-a1ee-f12ba4ef3012"),
            new Taxonomy("be1de9a2-a833-498b-95d3-9e525d4d9951", "Youth justice services","Youth justice services", "94f0ba86-d5fb-4fac-a1ee-f12ba4ef3012"),

            new Taxonomy("32712b43-e4f7-484f-97d7-beb3bb463133", "Health","Health", null),
            new Taxonomy("11696b1f-209a-47b1-9ef5-c588a14d43c6", "Hearing and sight","Hearing and sight", "32712b43-e4f7-484f-97d7-beb3bb463133"),
            new Taxonomy("7c39c3df-2ad4-4f94-aedd-d1cf5981e195", "Mental health, social and emotional support","Mental health, social and emotional support", "32712b43-e4f7-484f-97d7-beb3bb463133"),
            new Taxonomy("5b64e25a-929c-4a41-9c47-9faeaf2b5f29", "Nutrition and weight management","Nutrition and weight management", "32712b43-e4f7-484f-97d7-beb3bb463133"),
            new Taxonomy("2bc4dba9-14a8-4942-b52d-c0cf5622f5a7", "Oral health","Oral health", "32712b43-e4f7-484f-97d7-beb3bb463133"),
            new Taxonomy("75fe2e44-69c8-4da7-b4bd-d2d042acc657", "Public health services","Public health services", "32712b43-e4f7-484f-97d7-beb3bb463133"),

            new Taxonomy("ff704172-db6a-4e7a-b612-cd925e0aa7a0", "Pregnancy, birth and early years","Pregnancy, birth and early years", null),
            new Taxonomy("1e305952-ea60-43d6-bbc8-6cf6f1a1c7f0", "Birth registration","Birth registration", "ff704172-db6a-4e7a-b612-cd925e0aa7a0"),
            new Taxonomy("5f9acf55-be45-4847-9d45-cf94445b71ca", "Early years language and learning","Early years language and learning", "ff704172-db6a-4e7a-b612-cd925e0aa7a0"),
            new Taxonomy("e734cabd-2b69-4583-a844-9b6a4b266c71", "Health visiting","Health visiting", "ff704172-db6a-4e7a-b612-cd925e0aa7a0"),
            new Taxonomy("231a2bf5-b05a-4da7-a338-a838b83806db", "Infant feeding support (including breastfeeding)","Infant feeding support (including breastfeeding)", "ff704172-db6a-4e7a-b612-cd925e0aa7a0"),
            new Taxonomy("19c29d11-ffbc-41d0-841c-ea8f0dfdda94", "Midwife and maternity","Midwife and maternity", "ff704172-db6a-4e7a-b612-cd925e0aa7a0"),
            new Taxonomy("2b2489d5-aee8-42ef-8c97-d8fcccdb5a8b", "Perinatal mental health support (pregnancy to one year post birth)","Perinatal mental health support (pregnancy to one year post birth)", "ff704172-db6a-4e7a-b612-cd925e0aa7a0"),

            new Taxonomy("6c873b97-6978-4c0f-8e3c-0b2804dd3826", "Special educational needs and disabilities (SEND)","Special educational needs and disabilities (SEND)", null),
            new Taxonomy("2db648ae-69fb-4f06-b76f-b66a3bb64215", "Autistic Spectrum Disorder (ASD)","Autistic Spectrum Disorder (ASD)", "6c873b97-6978-4c0f-8e3c-0b2804dd3826"),
            new Taxonomy("9d5161ee-a289-47b4-a967-a5912ae143ba", "Breaks and respite","Breaks and respite", "6c873b97-6978-4c0f-8e3c-0b2804dd3826"),
            new Taxonomy("dacf095a-83df-4d23-a20a-da751d956ab9", "Early years support","Early years support", "6c873b97-6978-4c0f-8e3c-0b2804dd3826"),
            new Taxonomy("43349183-1145-4bae-bf49-d5398e33d1b2", "Groups for parents and carers of children with SEND","Groups for parents and carers of children with SEND", "6c873b97-6978-4c0f-8e3c-0b2804dd3826"),
            new Taxonomy("72a24e08-79d3-49d5-89ca-2d3d8c0e470e", "Hearing impairment","Hearing impairment", "6c873b97-6978-4c0f-8e3c-0b2804dd3826"),
            new Taxonomy("619344e1-2185-4d62-b3b3-fc95ac18cd9f", "Learning difficulties and disabilities","Learning difficulties and disabilities", "6c873b97-6978-4c0f-8e3c-0b2804dd3826"),
            new Taxonomy("0bc60c67-9fac-4b9e-aeee-38950859c700", "Multi-sensory impairment","Multi-sensory impairment", "6c873b97-6978-4c0f-8e3c-0b2804dd3826"),
            new Taxonomy("bf6db3be-b539-4a02-a212-3858126d35d2", "Other difficulties or disabilities","Other difficulties or disabilities", "6c873b97-6978-4c0f-8e3c-0b2804dd3826"),
            new Taxonomy("cbda7b61-d330-4923-a174-3cb4c5cf9c0a", "Physical disabilities","Physical disabilities", "6c873b97-6978-4c0f-8e3c-0b2804dd3826"),
            new Taxonomy("4f4053cc-9250-4109-9c30-3b53960524f7", "Social, emotional and mental health support","Social, emotional and mental health support", "6c873b97-6978-4c0f-8e3c-0b2804dd3826"),
            new Taxonomy("38bf4fc2-f6b9-4c15-bc07-b03b707659bd", "Speech, language and communication needs","Speech, language and communication needs", "6c873b97-6978-4c0f-8e3c-0b2804dd3826"),
            new Taxonomy("4c219f95-21da-4222-8286-bbe1cfaf675c", "Visual impairment","Visual impairment", "6c873b97-6978-4c0f-8e3c-0b2804dd3826"),

            new Taxonomy("be261f9e-f024-46f8-8b5b-58251f25388d", "Transport","Transport", null),
            new Taxonomy("93a29b1e-acd9-4abf-9f30-07dce3378558", "Community transport","Community transport", "be261f9e-f024-46f8-8b5b-58251f25388d"),

            // location
            new Taxonomy("4DC40D99-BA5D-45E1-886E-8D34F398B869", "FamilyHub", null, null),
        };

        return taxonomies;
    }
    public IReadOnlyCollection<Organisation> SeedOrganisations(OrganisationType organisationType)
    {
        List<Organisation> organisations = new()
        {
            GetBristolCountyCouncil(organisationType),
            new Organisation(
            "fc51795e-ea95-4af0-a0b2-4c06d5463678",
            organisationType, "Lancashire County Council", "Lancashire County Council", null, new Uri("https://www.lancashire.gov.uk/").ToString(), "https://www.lancashire.gov.uk/", new List<Review>(), new List<Service>()),
            new Organisation(
            "1229cb45-0dc0-4f8a-81bd-2cd74c7cc9cc",
            organisationType, "London Borough of Redbridge", "London Borough of Redbridge", null, new Uri("https://www.redbridge.gov.uk/").ToString(), "https://www.redbridge.gov.uk/", new List<Review>(), new List<Service>()),
            new Organisation(
            "ca8ddaeb-b5e5-46c4-b94d-43a8e2ccc066",
            organisationType, "Salford City Council", "Salford City Council", null, new Uri("https://www.salford.gov.uk/").ToString(), "https://www.salford.gov.uk/", new List<Review>(), GetSalfordHubsAndServices("ca8ddaeb-b5e5-46c4-b94d-43a8e2ccc066")),
            new Organisation(
            "6dc1c3ad-d077-46ff-9e0d-04fb263f0637",
            organisationType, "Suffolk County Council", "Suffolk County Council", null, new Uri("https://www.suffolk.gov.uk/").ToString(), "https://www.suffolk.gov.uk/", new List<Review>(), new List<Service>()),
            new Organisation(
            "88e0bffd-ed0b-48ea-9a70-5f6ef729fc21",
            organisationType, "Tower Hamlets Council", "Tower Hamlets Council", null, new Uri("https://www.towerhamlets.gov.uk/").ToString(), "https://www.towerhamlets.gov.uk/", new List<Review>(), new List<Service>())
        };

        return organisations;
    }

    private Organisation GetBristolCountyCouncil(OrganisationType organisationType)
    {
        var bristolCountyCouncil = new Organisation(
            "72e653e8-1d05-4821-84e9-9177571a6013",
            organisationType, "Bristol County Council", "Bristol County Council", null, new Uri("https://www.bristol.gov.uk/").ToString(), "https://www.bristol.gov.uk/", new List<Review>(), GetBristolCountyCouncilServices("72e653e8-1d05-4821-84e9-9177571a6013"));
        return bristolCountyCouncil;
    }

    private List<Service> GetSalfordHubsAndServices(string parentId)
    {
        List<Service> services = new();
        if (!IsProduction)
        {
            services.AddRange(GetSalfordFamilyService(parentId));
        }
        return services;
    }

    private List<Service> GetSalfordFamilyService(string parentId)
    {
        if (IsProduction)
        {
            return new List<Service>();
        }

        return new List<Service>{

            new Service(
                "1a000f89-6487-49ae-9f70-166070b02b48",
                new ServiceType("2", "Family Experience", ""),
                parentId,
                "Baby Social at Ordsall Neighbourhood Centre",
                "This session is for babies non mobile aged from birth to twelve months. Each week we will introduce you to one of our five to thrive key messages and a fun activity you can do at home with your baby. It will also give you the opportunity to connect with other parents and share your experiences.",
                null,
                null,
                null,
                null,
                "https://directory.salford.gov.uk/kb5/salford/directory/service.page?id=B3Z2uCshOk4&localofferchannel=2_8",
                "active",
                null,
                false,
                new List<ServiceDelivery>( new List<ServiceDelivery>
                {
                    new ServiceDelivery("71cdb1fc-324a-4161-9ced-13e9d504942b",ServiceDeliveryType.InPerson)
                }),
                new List<Eligibility>(new List<Eligibility>
                {
                    new Eligibility("e884bf7d-1479-4ed0-8cf8-fede12ea4ac2","Children",null,1,0,new List<Taxonomy>())
                }),
                new List<Funding>(),
                new List<CostOption>(new List<CostOption>
                {
                    new CostOption("983764", "Session", 2.5m, null, null, null, null)
                }),
                new List<Language>
                {
                    new Language("705b93e5-f70d-4237-84ab-36aef2c864e3","English")
                },
                new List<Review>(),
                new List<ServiceArea>
                {
                    new ServiceArea(Guid.NewGuid().ToString(), "Local", null, null, "http://statistics.data.gov.uk/id/statistical-geography/K02000001")
                },
                new List<ServiceAtLocation>( new List<ServiceAtLocation>
                {
                    new ServiceAtLocation(
                        "5fdbbefd-4cf4-4c4a-8178-c001fcb25570",
                        new Location("b405ae9a-5410-4136-86f3-dc1e85f1ec68", "Ordsall Neighbourhood Centre", "2, Robert Hall Street M5 3LT", 53.474103227856105D, -2.2721559641660787D,
                            new List<LinkTaxonomy>(),
                            new List<PhysicalAddress> { new PhysicalAddress("67efce6d-248b-4e71-b08d-4c21009d01f0", "2, Robert Hall Street", "Ordsall", "M5 3LT", "United Kingdom", "Salford") }, new List<AccessibilityForDisabilities>(), new List<LinkContact>()),
                        new List<RegularSchedule>
                        {
                            new RegularSchedule("16785195-413f-417c-8c77-8f3e3fe08e6f",
                                string.Empty,
                                null,
                                null,
                                "Friday 1.30pm - 2.30pm",
                                null,
                                "1.30pm - 2.30pm",
                                "Every Friday",
                                null,
                                null,
                                null
                            )
                        },
                        new List<HolidaySchedule>(),
                        new List<LinkContact>())

                }),
                new List<ServiceTaxonomy>( new List<ServiceTaxonomy>
                {
                    new ServiceTaxonomy
                    ("ae95b14e-2559-4ba0-8c31-3085777bfdd4",
                        null,
                        new Taxonomy("231a2bf5-b05a-4da7-a338-a838b83806db", "Infant feeding support (including breastfeeding)","Infant feeding support (including breastfeeding)", "ff704172-db6a-4e7a-b612-cd925e0aa7a0"))
                }),
                new List<HolidaySchedule>(),
                new List<RegularSchedule>(),
                new List<LinkContact>(new List<LinkContact>
                {
                    new LinkContact(
                        "6f4251a7-ae32-44cd-84fc-a632e944fccf",
                        "6f4251a7-ae32-44cd-84fc-a632e944fccc",
                        "Service",
                    new Contact(
                        "7a488a62-6c5b-4fa7-88bc-f60f83627e38",
                        "",
                        "Broughton Hub",
                        "0161 778 0601",
                        "0161 778 0601",
                        "www.gov.uk",
                        "help@gov.uk"))
                })),

            new Service(
                "2dde869a-914a-4acc-915b-10d368808022",
                new ServiceType("2", "Family Experience", ""),
                parentId,
                "Oakwood Academy",
                "Oakwood Academy is a special school for pupils aged 9-18 years who have a range of moderate and/or complex learning difficulties. The school has Visual Arts, Technology and Sports Specialist status. \r\n\r\nAdmissions to Oakwood Academy are controlled by Salford Local Authority. We are unable to accept direct requests for placement from parents or carers or other local authorities. Pupils who attend Oakwood Academy have an Educational, Health and Care Plan which outlines the area of need and what provision and resources are needed to support the pupil. \r\n\r\nIn rare cases, a child may be admitted on an assessment placement to determine what the pupil's needs are and whether their needs can be met at Oakwood Academy. ",
                null,
                null,
                null,
                null,
                null,
                "active",
                null,
                false,
                new List<ServiceDelivery>( new List<ServiceDelivery>
                {
                    new ServiceDelivery("31ac8379-1b1b-416f-a1e2-6c617c40cf32",ServiceDeliveryType.InPerson)
                }),
                new List<Eligibility>(new List<Eligibility>
                {
                    new Eligibility("c3398bf9-c10e-4309-94dd-234ac43e4abc","Children",null,10,4,new List<Taxonomy>())
                }),
                new List<Funding>(),
                new List<CostOption>(),
                new List<Language>
                {
                    new Language("fa4183e5-006d-45f4-89a0-f285e1d33575","English")
                },
                new List<Review>(),
                new List<ServiceArea>
                {
                    new ServiceArea(Guid.NewGuid().ToString(), "Local", null, null, "http://statistics.data.gov.uk/id/statistical-geography/K02000001")
                },
                new List<ServiceAtLocation>( new List<ServiceAtLocation>
                {
                    new ServiceAtLocation(
                        "6e0bb978-fe9e-4c13-b8e7-50c97bf06ed7",
                        new Location("f7301eeb-3293-4d7b-9869-d1e8a6368a13", "Oakwood Academy", "", 53.493505779578605D, -2.336084327089324D,
                            new List<LinkTaxonomy>(),
                            new List<PhysicalAddress> { new PhysicalAddress("387b5dff-a26a-42fe-84d6-1a744097cf4f", "Chatsworth Road", "Eccles", "M30 9DY", "United Kingdom", "Manchester") }, new List<AccessibilityForDisabilities>(), new List<LinkContact>()),
                        new List<RegularSchedule>(),
                        new List<HolidaySchedule>(),
                        new List<LinkContact>())

                }),
                new List<ServiceTaxonomy>( new List<ServiceTaxonomy>
                {
                    new ServiceTaxonomy
                    ("5b9eb3b7-aace-4161-aee1-e5ae5c6fd767",
                        null,
                        new Taxonomy("dacf095a-83df-4d23-a20a-da751d956ab9", "Early years support","Early years support", "6c873b97-6978-4c0f-8e3c-0b2804dd3826"))
                }),
                new List<HolidaySchedule>(),
                new List<RegularSchedule>(),
                new List<LinkContact>(new List<LinkContact>
                {
                    new LinkContact(
                        "6f4251a7-ae32-44cd-84fc-a632e944fccf",
                        "6f4251a7-ae32-44cd-84fc-a632e944fccc",
                        "Service",
                    new Contact(
                        "aaef3e12-849d-4a09-8606-17252cee6572",
                        "Ms",
                        "Kate Berry",
                        "01619212880",
                        "01619212880",
                        "www.gov.uk",
                        "help@gov.uk"))
                })),

            new Service(
                "6f4251a7-ae32-44cd-84fc-a632e944fccf",
                new ServiceType("2", "Family Experience", ""),
                "b2446860-cff0-4fb7-a703-bc4a919b3417",
                "Central Family Hub",
                "Family Hub",
                null,
                null,
                null,
                null,
                null,
                "active",
                null,
                false,
                new List<ServiceDelivery>( new List<ServiceDelivery>
                {
                    new ServiceDelivery("c820ea55-14e9-4061-9c89-6906ec3064d1",ServiceDeliveryType.InPerson)
                }),
                new List<Eligibility>(new List<Eligibility>
                {
                    new Eligibility("4468ed19-0b26-414b-8d71-b09ca3802b75","Children",null,25,0,new List<Taxonomy>())
                }),
                new List<Funding>(),
                new List<CostOption>(),
                new List<Language>
                {
                    new Language("41814ec8-410a-4f37-aa9f-7b3a83b7f33b","English")
                },
                new List<Review>(),
                new List<ServiceArea>
                {
                    new ServiceArea(Guid.NewGuid().ToString(), "Local", null, null, "http://statistics.data.gov.uk/id/statistical-geography/K02000001")
                },
                new List<ServiceAtLocation>( new List<ServiceAtLocation>
                {
                    new ServiceAtLocation(
                        "e234f5b5-fb74-4e68-bd29-88e736bfc317",
                        new Location("964ea451-6146-4add-913e-dff23a1bd7b6", "Central Family Hub", "Broughton Hub", 53.507025D, -2.259764D,
                            new List<LinkTaxonomy> {new
                            ("CD2324A3-AB3F-4707-9D38-34DFB7722B62",
                                "964ea451-6146-4add-913e-dff23a1bd7b6",
                                LinkType.Location,
                                new Taxonomy("4DC40D99-BA5D-45E1-886E-8D34F398B869", "FamilyHub", null, null))},
                            new List<PhysicalAddress> { new PhysicalAddress("84c0a47a-08a1-4e54-814b-46ce26765c08", "50 Rigby Street", "Manchester", "M7 4BQ", "United Kingdom", "Salford") }, new List<AccessibilityForDisabilities>(), new List<LinkContact>()),
                        new List<RegularSchedule>(),
                        new List<HolidaySchedule>(),
                        new List<LinkContact>())
                }),
                new List<ServiceTaxonomy>(),
                new List<HolidaySchedule>(),
                new List<RegularSchedule>(),
                new List<LinkContact>(new List<LinkContact>
                {
                    new LinkContact(
                        "6f4251a7-ae32-44cd-84fc-a632e944fccf",
                        "6f4251a7-ae32-44cd-84fc-a632e944fccc",
                        "Service",
                    new Contact(
                        "6a84d5af-2e5c-40fa-9689-6b11181db320",
                        "Ms",
                        "Kate Berry",
                        "0161 778 0601"
                        , "0161 778 0601",
                        "www.gov.uk",
                        "help@gov.uk"))
                })),

            new Service(
                "57bde76b-885d-484d-869f-7e60faf4b1b2",
                new ServiceType("2", "Family Experience", ""),
                "18e95341-8464-4375-84fd-195af5fe7d9c",
                "North Family Hub",
                "Family Hub",
                null,
                null,
                null,
                null,
                null,
                "active",
                null,
                false,
                new List<ServiceDelivery>( new List<ServiceDelivery>
                {
                    new ServiceDelivery("5432f1f5-b822-4501-a018-7581add71dbc",ServiceDeliveryType.InPerson)
                }),
                new List<Eligibility>(new List<Eligibility>
                {
                    new Eligibility("758aedf1-a177-42ed-9ea9-3ef6e95b9b6b","Children",null,25,0,new List<Taxonomy>())
                }),
                new List<Funding>(),
                new List<CostOption>(),
                new List<Language>
                {
                    new Language("c3ee6046-6caf-4204-8eaf-db9bc192c7da","English")
                },
                new List<Review>(),
                new List<ServiceArea>
                {
                    new ServiceArea(Guid.NewGuid().ToString(), "Local", null, null, "http://statistics.data.gov.uk/id/statistical-geography/K02000001")
                },
                new List<ServiceAtLocation>(new List<ServiceAtLocation>
                {
                    new ServiceAtLocation(
                        "2995a7a0-6552-4c71-9e91-7f860d5a993e",
                        new Location("74c37f53-dbc0-4958-8c97-baee41a022bf", "North Family Hub", "Swinton Gateway", 53.5124278D, -2.342044D,
                            new List<LinkTaxonomy> {new
                            ("3A724AE8-8E9E-4AC5-95BC-E5B07795A8DD",
                                "74c37f53-dbc0-4958-8c97-baee41a022bf",
                                LinkType.Location,
                                new Taxonomy("4DC40D99-BA5D-45E1-886E-8D34F398B869", "FamilyHub", null, null))},
                            new List<PhysicalAddress> { new PhysicalAddress("83bf4f64-fdcb-46bc-a32e-8b117bf5c19a", "100 Chorley Road", "Manchester", "M27 6BP", "United Kingdom", "Salford") }, new List<AccessibilityForDisabilities>(), new List<LinkContact>()),
                        new List<RegularSchedule>(),
                        new List<HolidaySchedule>(),
                        new List<LinkContact>())

                }),
                new List<ServiceTaxonomy>(),
                new List<HolidaySchedule>(),
                new List<RegularSchedule>(),
                new List<LinkContact>(new List<LinkContact>
                {
                    new LinkContact(
                        "06c16312-fa5f-4e82-b672-3a9ab099649a",
                        "06c16312-fa5f-4e82-b672-3a9ab099649b",
                        "Service",
                    new Contact(
                        "560e159d-c54c-4925-bd55-8bbfd5071520",
                        "Ms",
                        "Kate Berry",
                        "0161 778 0495",
                        "0161 778 0495",
                        "www.gov.uk",
                        "help@gov.uk"))
                })),

            new Service(
                "06c16312-fa5f-4e82-b672-3a9ab099649a",
                new ServiceType("2", "Family Experience", ""),
                "99d11261-5551-40c1-988e-59d13a377e75",
                "South Family Hub",
                "Family Hub",
                null,
                null,
                null,
                null,
                null,
                "active",
                null,
                false,
                new List<ServiceDelivery>( new List<ServiceDelivery>
                {
                    new ServiceDelivery("3af77d95-1e1a-412f-9daa-98dc74af8e54",ServiceDeliveryType.InPerson)
                }),
                new List<Eligibility>(new List<Eligibility>
                {
                    new Eligibility("94589b64-2ab8-4748-8a70-13f3ba1e35e8","Children",null,25,0,new List<Taxonomy>())
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
                    new ServiceArea(Guid.NewGuid().ToString(), "Local", null, null, "http://statistics.data.gov.uk/id/statistical-geography/K02000001")
                },
                new List<ServiceAtLocation>(new List<ServiceAtLocation>
                {
                    new ServiceAtLocation(
                        "4e969589-e2b3-47d7-b079-1af1e3de5554",
                        new Location("1b4a625b-54bb-407d-a508-f90cade1e96f", "South Family Hub", "Winton Children’s Centre", 53.48801070060149D, -2.368140748303118D,
                            new List<LinkTaxonomy> {new LinkTaxonomy(
                                "4594A80C-42EC-4F80-AABE-E216E7732DC8",
                                "1b4a625b-54bb-407d-a508-f90cade1e96f",
                                LinkType.Location,
                                new Taxonomy("4DC40D99-BA5D-45E1-886E-8D34F398B869", "FamilyHub", null, null))},
                            new List<PhysicalAddress> { new PhysicalAddress("45603703-5d3f-45f4-84f1-b294ac4d3290", "Brindley Street", "Manchester", "M30 8AB", "United Kingdom", "Salford") }, new List<AccessibilityForDisabilities>(), new List<LinkContact>()),
                        new List<RegularSchedule>(),
                        new List<HolidaySchedule>(),
                        new List<LinkContact>())

                }),
                new List<ServiceTaxonomy>(),
                new List<HolidaySchedule>(),
                new List<RegularSchedule>(),
                new List<LinkContact>(new List<LinkContact>
                {
                    new LinkContact(
                        "06c16312-fa5f-4e82-b672-3a9ab099649a",
                        "06c16312-fa5f-4e82-b672-3a9ab099649b",
                        "Service",
                    new Contact(
                        "6596d609-8ba1-4188-a357-3d92ae9dec8f",
                        "Ms",
                        "Kate Berry",
                        "0161 686 5260",
                        "0161 686 5260",
                        "www.gov.uk",
                        "help@gov.uk"))
                }))
        };
    }

    private List<Service> GetBristolCountyCouncilServices(string parentId)
    {
        if (IsProduction)
        {
            return new List<Service>();
        }

        return new()
        {
            new Service(
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
                null,
                false,
                new List<ServiceDelivery>( new List<ServiceDelivery>
                {
                    new ServiceDelivery("9db7f878-be53-4a45-ac47-472568dfeeea",ServiceDeliveryType.Online)
                }),
                new List<Eligibility>(new List<Eligibility>
                {
                    new Eligibility("9109Children","Children",null,13,0,new List<Taxonomy>())
                }),
                new List<Funding>(),
                new List<CostOption>(),
                new List<Language>
                {
                    new Language("29794770-11bc-400f-82b7-f03d256ca5dc","English")
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
                            "256d0b97-d4c4-48e8-9475-bd7d42d1fc69",
                            "",
                            "",
                            52.6312,
                            -1.66526,
                            new List<LinkTaxonomy>(),
                            new List<PhysicalAddress>(new List<PhysicalAddress>
                            {
                                new PhysicalAddress(
                                    Guid.NewGuid().ToString(),
                                    "7A Boyce's Ave, Clifton",
                                    "Bristol",
                                    "BS8 4AA",
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
                        new ServiceTaxonomy
                        ("9109",
                            null,
                            new Taxonomy(
                                "11696b1f-209a-47b1-9ef5-c588a14d43c6",
                                "Hearing and sight","Hearing and sight",
                                "32712b43-e4f7-484f-97d7-beb3bb463133"
                            ))
                    }),
                new List<HolidaySchedule>(),
                new List<RegularSchedule>(),
                new List<LinkContact>(new List<LinkContact> {
                    new LinkContact(
                        "4591d551-0d6a-4c0d-b109-002e67318231",
                        "4591d551-0d6a-4c0d-b109-002e67318232",
                        "Service",
                        new Contact(
                        "1567",
                        "Mr",
                        "John Smith",
                        "01827 65778",
                        "01827 65778",
                        "www.gov.uk",
                        "help@gov.uk"))
                })),

            new Service(
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
                null,
                false,
                new List<ServiceDelivery>( new List<ServiceDelivery>
                {
                    new ServiceDelivery("b2518131-90fc-4149-bbd1-1c9992cd92ac",ServiceDeliveryType.InPerson)
                }),
                new List<Eligibility>(new List<Eligibility>
                {
                    new Eligibility("9109T1Children","Children",null,15,10,new List<Taxonomy>())
                }),
                new List<Funding>(),
                new List<CostOption>(),
                new List<Language>
                {
                    new Language("c3b81a2c-14c7-43f3-8ed0-a8fc2087b942","English")
                },
                new List<Review>(),
                new List<ServiceArea>
                {
                    new ServiceArea(Guid.NewGuid().ToString(), "National", null, null, "http://statistics.data.gov.uk/id/statistical-geography/K02000001")
                },
                new List<ServiceAtLocation>(new List<ServiceAtLocation>
                {
                    new ServiceAtLocation(
                        "1849",
                        new Location(
                            "9181c5c1-b55a-41d6-aaf4-f4fc9f0bc644",
                            "",
                            "",
                            52.6312,
                            -1.66526,
                            new List<LinkTaxonomy>(),
                            new List<PhysicalAddress>(new List<PhysicalAddress>
                            {
                                new PhysicalAddress(
                                    Guid.NewGuid().ToString(),
                                    "7A Boyce's Ave, Clifton",
                                    "Bristol",
                                    "BS8 4AA",
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
                new List<ServiceTaxonomy>(new List<ServiceTaxonomy>
                    {
                        new ServiceTaxonomy
                        ("9110TestDelete",
                            null,
                            new Taxonomy("aafa1cc3-b984-4b10-89d5-27388c5432de", "Activities", "Activities", "16f3a451-e88d-4ad0-b53f-c8925d1cc9e4")
                        )
                    }),
                new List<HolidaySchedule>(),
                new List<RegularSchedule>(),
                new List<LinkContact>(new List<LinkContact>
                {
                    new LinkContact(
                        "207613c5-4cdf-46b2-a110-cf120a9412f9",
                        "207613c5-4cdf-46b2-a110-cf120a9412f1",
                        "Service",
                        new Contact(
                        "1561",
                        "Mr",
                        "John Smith",
                        "01827 65711",
                        "01827 65711",
                        "www.gov.uk",
                        "help@gov.uk"))
                })),

            new Service(
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
                null,
                false,
                new List<ServiceDelivery>( new List<ServiceDelivery>
                {
                    new ServiceDelivery("46d1d3df-c7f7-47f0-9672-e18b67594d4e",ServiceDeliveryType.Telephone)
                }),
                new List<Eligibility>(new List<Eligibility>
                {
                    new Eligibility("9109T2Children","Children",null,13,0,new List<Taxonomy>())
                }),
                new List<Funding>(),
                new List<CostOption>(new List<CostOption>
                {
                    new CostOption("1345", "Session", 45.0m, null, null, null, null)
                }),
                new List<Language>
                {
                    new Language("51e75b71-56b9-4cde-9a14-f2a520deff5e","English")
                },
                new List<Review>(),
                new List<ServiceArea>
                {
                    new ServiceArea(Guid.NewGuid().ToString(), "National", null, null, "http://statistics.data.gov.uk/id/statistical-geography/K02000001")
                },
                new List<ServiceAtLocation>( new List<ServiceAtLocation>
                {
                    new ServiceAtLocation(
                        "1867",
                        new Location(
                            "fb82fbea-78bb-40c7-bbca-a29f95589483",
                            "",
                            "",
                            52.63123,
                            -1.66519,
                            new List<LinkTaxonomy>(),
                            new List<PhysicalAddress>(new List<PhysicalAddress>
                            {
                                new PhysicalAddress(
                                    Guid.NewGuid().ToString(),
                                    "7A Boyce's Ave, Clifton",
                                    "Bristol",
                                    "BS8 4AA",
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
                        new ServiceTaxonomy
                        ("9120TestDelete",
                            null,
                            new Taxonomy("aafa1cc3-b984-4b10-89d5-27388c5432de", "Activities", "Activities", "16f3a451-e88d-4ad0-b53f-c8925d1cc9e4")
                        )
                    }),
                new List<HolidaySchedule>(),
                new List<RegularSchedule>(),
                new List<LinkContact>(new List<LinkContact> {
                    new LinkContact(
                        "207613c5-4cdf-46b2-a110-cf120a9412f9",
                        "207613c5-4cdf-46b2-a110-cf120a9412f1",
                        "Service",
                        new Contact(
                        "1562",
                        "Mr",
                        "John Smith",
                        "01827 64328",
                        "01827 64328",
                        "www.gov.uk",
                        "help@gov.uk"))
                })),

            new Service(
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
                null,
                true,
                new List<ServiceDelivery>( new List<ServiceDelivery>
                {
                    new ServiceDelivery("d4e1fbfb-dd48-4950-b259-7547260ef838",ServiceDeliveryType.InPerson)
                }),
                new List<Eligibility>(new List<Eligibility>
                {
                    new Eligibility("9109T3Children","Children",null,20,15,new List<Taxonomy>())
                }),
                new List<Funding>(),
                new List<CostOption>(new List<CostOption>
                {
                    new CostOption("1346", "Hour", 25.0m, null, null, null, null)
                }),
                new List<Language>
                {
                    new Language("73a688a2-0dad-4674-ac63-5115c990ea1f","Afrikaans")
                },
                new List<Review>(),
                new List<ServiceArea>
                {
                    new ServiceArea(Guid.NewGuid().ToString(), "National", null, null, "http://statistics.data.gov.uk/id/statistical-geography/K02000001")
                },
                new List<ServiceAtLocation>(new List<ServiceAtLocation>
                {
                    new ServiceAtLocation(
                        "1868",
                        new Location(
                            "68615e6e-c9e2-4a1f-8824-a5c1da36a953",
                            "",
                            "",
                            52.63123,
                            -1.66519,
                            new List<LinkTaxonomy>(),
                            new List<PhysicalAddress>(new List<PhysicalAddress>
                            {
                                new PhysicalAddress(
                                    Guid.NewGuid().ToString(),
                                    "7A Boyce's Ave, Clifton",
                                    "Bristol",
                                    "BS8 4AA",
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
                new List<ServiceTaxonomy>(new List<ServiceTaxonomy>
                    {
                        new ServiceTaxonomy
                        ("9121TestDelete",
                            null,
                            new Taxonomy("aafa1cc3-b984-4b10-89d5-27388c5432de", "Activities", "Activities", "16f3a451-e88d-4ad0-b53f-c8925d1cc9e4")
                        )
                    }),
                new List<HolidaySchedule>(),
                new List<RegularSchedule>(),
                new List<LinkContact>(new List<LinkContact> {
                    new LinkContact(
                        "39f211fc-73d4-4ea1-a1c9-d8fc063d6b52",
                        "39f211fc-73d4-4ea1-a1c9-d8fc063d6b51",
                        "Service",
                        new Contact(
                            "1563",
                            "Mr",
                            "John Smith",
                            "01827 64328",
                            "01827 64328",
                            "www.gov.uk",
                            "help@gov.uk")
                        )
                })),
        };
    }
}
