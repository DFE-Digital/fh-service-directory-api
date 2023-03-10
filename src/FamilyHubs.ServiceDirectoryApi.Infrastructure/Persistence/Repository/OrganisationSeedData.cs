using FamilyHubs.ServiceDirectory.Core.Entities;
using FamilyHubs.ServiceDirectory.Shared.Enums;

namespace FamilyHubs.ServiceDirectory.Infrastructure.Persistence.Repository;

public class OrganisationSeedData
{
    private readonly ApplicationDbContext _dbContext;
    private bool IsProduction { get; set; }
    public OrganisationSeedData(bool isProduction, ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
        IsProduction = isProduction;
    }

    public async Task SeedTaxonomies()
    {
        var activity = new Taxonomy { Name = "Activities, clubs and groups", TaxonomyType = TaxonomyType.ServiceCategory };
        var support = new Taxonomy { Name = "Family support", TaxonomyType = TaxonomyType.ServiceCategory };
        var health = new Taxonomy { Name = "Health", TaxonomyType = TaxonomyType.ServiceCategory };
        var earlyYear = new Taxonomy { Name = "Pregnancy, birth and early years", TaxonomyType = TaxonomyType.ServiceCategory };
        var send = new Taxonomy { Name = "Special educational needs and disabilities (SEND)", TaxonomyType = TaxonomyType.ServiceCategory };
        var transport = new Taxonomy { Name = "Transport", TaxonomyType = TaxonomyType.ServiceCategory };

        var parentTaxonomies = new List<Taxonomy>
        {
            activity,
            support,
            health,
            earlyYear,
            send,
            transport,
        };

        _dbContext.Taxonomies.AddRange(parentTaxonomies);
        await _dbContext.SaveChangesAsync();

        var taxonomies = new List<Taxonomy>
        {
            new Taxonomy { Name = "Activities", TaxonomyType = TaxonomyType.ServiceCategory, ParentId = activity.Id },
            new Taxonomy { Name = "Before and after school clubs", TaxonomyType = TaxonomyType.ServiceCategory, ParentId = activity.Id },
            new Taxonomy { Name = "Holiday clubs and schemes", TaxonomyType = TaxonomyType.ServiceCategory, ParentId = activity.Id },
            new Taxonomy { Name = "Music, arts and dance", TaxonomyType = TaxonomyType.ServiceCategory, ParentId = activity.Id },
            new Taxonomy { Name = "Parent, baby and toddler groups", TaxonomyType = TaxonomyType.ServiceCategory, ParentId = activity.Id },
            new Taxonomy { Name = "Pre-school playgroup", TaxonomyType = TaxonomyType.ServiceCategory, ParentId = activity.Id },
            new Taxonomy { Name = "Sports and recreation", TaxonomyType = TaxonomyType.ServiceCategory, ParentId = activity.Id },

            new Taxonomy { Name = "Bullying and cyber bullying", TaxonomyType = TaxonomyType.ServiceCategory, ParentId = support.Id },
            new Taxonomy { Name = "Debt and welfare advice", TaxonomyType = TaxonomyType.ServiceCategory, ParentId = support.Id },
            new Taxonomy { Name = "Domestic abuse", TaxonomyType = TaxonomyType.ServiceCategory, ParentId = support.Id },
            new Taxonomy { Name = "Intensive targeted family support", TaxonomyType = TaxonomyType.ServiceCategory, ParentId = support.Id },
            new Taxonomy { Name = "Money, benefits and housing", TaxonomyType = TaxonomyType.ServiceCategory, ParentId = support.Id },
            new Taxonomy { Name = "Parenting support", TaxonomyType = TaxonomyType.ServiceCategory, ParentId = support.Id },
            new Taxonomy { Name = "Reducing parental conflict", TaxonomyType = TaxonomyType.ServiceCategory, ParentId = support.Id },
            new Taxonomy { Name = "Separating and separated parent support", TaxonomyType = TaxonomyType.ServiceCategory, ParentId = support.Id },
            new Taxonomy { Name = "Stopping smoking", TaxonomyType = TaxonomyType.ServiceCategory, ParentId = support.Id },
            new Taxonomy { Name = "Substance misuse (including alcohol and drug)", TaxonomyType = TaxonomyType.ServiceCategory, ParentId = support.Id },
            new Taxonomy { Name = "Targeted youth support", TaxonomyType = TaxonomyType.ServiceCategory, ParentId = support.Id },
            new Taxonomy { Name = "Youth justice services", TaxonomyType = TaxonomyType.ServiceCategory, ParentId = support.Id },

            new Taxonomy { Name = "Hearing and sight", TaxonomyType = TaxonomyType.ServiceCategory, ParentId = health.Id },
            new Taxonomy { Name = "Mental health, social and emotional support", TaxonomyType = TaxonomyType.ServiceCategory, ParentId = health.Id },
            new Taxonomy { Name = "Nutrition and weight management", TaxonomyType = TaxonomyType.ServiceCategory, ParentId = health.Id },
            new Taxonomy { Name = "Oral health", TaxonomyType = TaxonomyType.ServiceCategory, ParentId = health.Id },
            new Taxonomy { Name = "Public health services", TaxonomyType = TaxonomyType.ServiceCategory, ParentId = health.Id },

            new Taxonomy { Name = "Birth registration", TaxonomyType = TaxonomyType.ServiceCategory, ParentId = earlyYear.Id },
            new Taxonomy { Name = "Early years language and learning", TaxonomyType = TaxonomyType.ServiceCategory, ParentId = earlyYear.Id },
            new Taxonomy { Name = "Health visiting", TaxonomyType = TaxonomyType.ServiceCategory, ParentId = earlyYear.Id },
            new Taxonomy { Name = "Infant feeding support (including breastfeeding)", TaxonomyType = TaxonomyType.ServiceCategory, ParentId = earlyYear.Id },
            new Taxonomy { Name = "Midwife and maternity", TaxonomyType = TaxonomyType.ServiceCategory, ParentId = earlyYear.Id },
            new Taxonomy { Name = "Perinatal mental health support (pregnancy to one year post birth)", TaxonomyType = TaxonomyType.ServiceCategory, ParentId = earlyYear.Id },

            new Taxonomy { Name = "Autistic Spectrum Disorder (ASD)", TaxonomyType = TaxonomyType.ServiceCategory, ParentId = send.Id },
            new Taxonomy { Name = "Breaks and respite", TaxonomyType = TaxonomyType.ServiceCategory, ParentId = send.Id },
            new Taxonomy { Name = "Early years support", TaxonomyType = TaxonomyType.ServiceCategory, ParentId = send.Id },
            new Taxonomy { Name = "Groups for parents and carers of children with SEND", TaxonomyType = TaxonomyType.ServiceCategory, ParentId = send.Id },
            new Taxonomy { Name = "Hearing impairment", TaxonomyType = TaxonomyType.ServiceCategory, ParentId = send.Id },
            new Taxonomy { Name = "Learning difficulties and disabilities", TaxonomyType = TaxonomyType.ServiceCategory, ParentId = send.Id },
            new Taxonomy { Name = "Multi-sensory impairment", TaxonomyType = TaxonomyType.ServiceCategory, ParentId = send.Id },
            new Taxonomy { Name = "Other difficulties or disabilities", TaxonomyType = TaxonomyType.ServiceCategory, ParentId = send.Id },
            new Taxonomy { Name = "Physical disabilities", TaxonomyType = TaxonomyType.ServiceCategory, ParentId = send.Id },
            new Taxonomy { Name = "Social, emotional and mental health support", TaxonomyType = TaxonomyType.ServiceCategory, ParentId = send.Id },
            new Taxonomy { Name = "Speech, language and communication needs", TaxonomyType = TaxonomyType.ServiceCategory, ParentId = send.Id },
            new Taxonomy { Name = "Visual impairment", TaxonomyType = TaxonomyType.ServiceCategory, ParentId = send.Id },

            new Taxonomy { Name = "Community transport", TaxonomyType = TaxonomyType.ServiceCategory, ParentId = transport.Id },

        };

        _dbContext.Taxonomies.AddRange(taxonomies);
        await _dbContext.SaveChangesAsync();
    }

    public async Task SeedOrganisations()
    {
        const OrganisationType organisationType = OrganisationType.LA;
        var organisations = new List<Organisation>
        {
            new Organisation
            {
                OrganisationType =      organisationType,
                Name =                  "Bristol County Council",
                Description =           "Bristol County Council",
                Uri =                   new Uri("https://www.bristol.gov.uk/").ToString(),
                Url =                   "https://www.bristol.gov.uk/",
                AdminAreaCode =         "E06000023",
                Services =              SeedBristolServices(0)
            },
            new Organisation
            {
                OrganisationType =      organisationType,
                Name =                  "Lancashire County Council",
                Description =           "Lancashire County Council",
                Uri =                   new Uri("https://www.lancashire.gov.uk/").ToString(),
                Url =                   "https://www.lancashire.gov.uk/",
                AdminAreaCode =         "E10000017",
            },
            new Organisation
            {
                OrganisationType =      organisationType,
                Name =                  "London Borough of Redbridge",
                Description =           "London Borough of Redbridge",
                Uri =                   new Uri("https://www.redbridge.gov.uk/").ToString(),
                Url =                   "https://www.redbridge.gov.uk/",
                AdminAreaCode =         "E09000026",
            },
            new Organisation
            {
                OrganisationType =      organisationType,
                Name =                  "Salford City Council",
                Description =           "Salford City Council",
                Uri =                   new Uri("https://www.salford.gov.uk/").ToString(),
                Url =                   "https://www.salford.gov.uk/",
                AdminAreaCode =         "E08000006",
                Services =              SeedSalfordService(0)
            },
            new Organisation
            {
                OrganisationType =      organisationType,
                Name =                  "Suffolk County Council",
                Description =           "Suffolk County Council",
                Uri =                   new Uri("https://www.suffolk.gov.uk/").ToString(),
                Url =                   "https://www.suffolk.gov.uk/",
                AdminAreaCode =         "E10000029",
            },
            new Organisation
            {
                OrganisationType =      organisationType,
                Name =                  "Tower Hamlets Council",
                Description =           "Tower Hamlets Council",
                Uri =                   new Uri("https://www.towerhamlets.gov.uk/").ToString(),
                Url =                   "https://www.towerhamlets.gov.uk/",
                AdminAreaCode =         "E09000030",
            }
        };

        _dbContext.Organisations.AddRange(organisations);
        await _dbContext.SaveChangesAsync();
    }

    private ICollection<Service> SeedSalfordService(long organisationId)
    {
        if (IsProduction) return new List<Service>();

        return new List<Service>
        {
            new Service
            {
                OrganisationId = organisationId,
                ServiceOwnerReferenceId = Guid.NewGuid().ToString(),
                ServiceType = ServiceType.FamilyExperience,
                Name = "Baby Social at Ordsall Neighbourhood Centre",
                Description = "This session is for babies non mobile aged from birth to twelve months. Each week we will introduce you to one of our five to thrive key messages and a fun activity you can do at home with your baby. It will also give you the opportunity to connect with other parents and share your experiences.",
                AttendingAccess = AttendingAccessType.NotSet,
                AttendingType = AttendingType.NotSet,
                DeliverableType = DeliverableType.NotSet,
                Status = ServiceStatusType.Active,
                CanFamilyChooseDeliveryLocation = false,
                ServiceDeliveries = new List<ServiceDelivery>
                {
                    new ServiceDelivery
                    {
                        Name = ServiceDeliveryType.InPerson,
                        ServiceId = 0
                    }
                },
                Eligibilities = new List<Eligibility>
                {
                    new Eligibility
                    {
                        EligibilityType = EligibilityType.Child,
                        MaximumAge = 1,
                        MinimumAge = 0,
                        ServiceId = 0
                    }
                },
                CostOptions = new List<CostOption>
                {
                    new CostOption
                    {
                        Option = "Session",
                        Amount = 2.5m,
                        AmountDescription = "AmountDescription",
                        ServiceId = 0
                    }
                },
                Languages = new List<Language>
                {
                    new Language
                    {
                        Name = "English",
                        ServiceId = 0
                    }
                },
                ServiceAreas = new List<ServiceArea>
                {
                    new ServiceArea
                    {
                        ServiceAreaName = "Local",
                        Uri = "http://statistics.data.gov.uk/id/statistical-geography/K02000001",
                        ServiceId = 0
                    }
                },
                Locations = new List<Location>
                {
                    new Location
                    {
                        LocationType = LocationType.NotSet,
                        Name = "Ordsall Neighbourhood Centre",
                        Description = "2, Robert Hall Street M5 3LT",
                        Longitude = 53.474103227856105D,
                        Latitude = -2.2721559641660787D,
                        Address1 = "2, Robert Hall Street",
                        City = "Ordsall",
                        PostCode = "M5 3LT",
                        Country = "United Kingdom",
                        StateProvince = "Salford",
                        RegularSchedules = new List<RegularSchedule>
                        {
                            new RegularSchedule
                            {
                                Description = "Friday 1.30pm - 2.30pm",
                                ByDay = "1.30pm - 2.30pm",
                                Interval = "Every Friday",
                            }
                        },
                        Contacts = new List<Contact>
                        {
                            new Contact
                            {
                                Title = "",
                                Name = "Broughton Hub",
                                Telephone = "0161 778 0601",
                                TextPhone = "0161 778 0601",
                                Url = "www.gov.uk",
                                Email = "help@gov.uk"
                            }
                        }
                    }
                },
                Taxonomies = new List<Taxonomy>
                {
                    new Taxonomy
                    {
                        Name = "Infant feeding support",
                        TaxonomyType = TaxonomyType.ServiceCategory,
                        ParentId = 1
                    }
                }
            },
            new Service
            {
                OrganisationId = organisationId,
                ServiceOwnerReferenceId = Guid.NewGuid().ToString(),
                ServiceType = ServiceType.FamilyExperience,
                Name = "Oakwood Academy",
                Description = "Oakwood Academy is a special school for pupils aged 9-18 years who have a range of moderate and/or complex learning difficulties. The school has Visual Arts, Technology and Sports Specialist status. \r\n\r\nAdmissions to Oakwood Academy are controlled by Salford Local Authority. We are unable to accept direct requests for placement from parents or carers or other local authorities. Pupils who attend Oakwood Academy have an Educational, Health and Care Plan which outlines the area of need and what provision and resources are needed to support the pupil. \r\n\r\nIn rare cases, a child may be admitted on an assessment placement to determine what the pupil's needs are and whether their needs can be met at Oakwood Academy. ",
                AttendingAccess = AttendingAccessType.NotSet,
                AttendingType = AttendingType.NotSet,
                DeliverableType = DeliverableType.NotSet,
                Status = ServiceStatusType.NotSet,
                CanFamilyChooseDeliveryLocation = false,
                ServiceDeliveries = new List<ServiceDelivery>
                {
                    new ServiceDelivery
                    {
                        Name = ServiceDeliveryType.InPerson,
                        ServiceId = 0
                    }
                },
                Eligibilities = new List<Eligibility>
                {
                    new Eligibility
                    {
                        EligibilityType = EligibilityType.Child,
                        MaximumAge = 10,
                        MinimumAge = 4,
                        ServiceId = 0
                    }
                },
                Languages = new List<Language>
                {
                    new Language
                    {
                        Name = "English",
                        ServiceId = 0
                    }
                },
                ServiceAreas = new List<ServiceArea>
                {
                    new ServiceArea
                    {
                        ServiceAreaName = "Local",
                        Uri = "http://statistics.data.gov.uk/id/statistical-geography/K02000001",
                        ServiceId = 0
                    }
                },
                Locations = new List<Location>
                {
                    new Location
                    {
                        LocationType = LocationType.NotSet,
                        Name = "Oakwood Academy",
                        Description = "",
                        Longitude = 53.493505779578605D,
                        Latitude = -2.336084327089324D,
                        Address1 = "Chatsworth Road",
                        City = "Eccles",
                        PostCode = "M30 9DY",
                        Country = "United Kingdom",
                        StateProvince = "Manchester",
                        Contacts = new List<Contact>
                        {
                            new Contact
                            {
                                Title = "Ms",
                                Name = "Kate Berry",
                                Telephone = "01619212880",
                                TextPhone = "01619212880",
                                Url = "www.gov.uk",
                                Email = "help@gov.uk"
                            }
                        }
                    }
                },
                Taxonomies = new List<Taxonomy>
                {
                    new Taxonomy
                    {
                        Name = "Early years support",
                        TaxonomyType = TaxonomyType.ServiceCategory,
                        ParentId = 2
                    }
                }
            },
            new Service
            {
                OrganisationId = organisationId,
                ServiceOwnerReferenceId = Guid.NewGuid().ToString(),
                ServiceType = ServiceType.FamilyExperience,
                Name = "Central Family Hub",
                Description = "Family Hub",
                AttendingAccess = AttendingAccessType.NotSet,
                AttendingType = AttendingType.NotSet,
                DeliverableType = DeliverableType.NotSet,
                Status = ServiceStatusType.Active,
                CanFamilyChooseDeliveryLocation = false,
                ServiceDeliveries = new List<ServiceDelivery>
                {
                    new ServiceDelivery
                    {
                        Name = ServiceDeliveryType.InPerson,
                        ServiceId = 0,
                    }
                },
                Eligibilities = new List<Eligibility>
                {
                    new Eligibility
                    {
                        EligibilityType = EligibilityType.Child,
                        MaximumAge = 25,
                        MinimumAge = 0,
                        ServiceId = 0,
                    }
                },
                Languages = new List<Language>
                {
                    new Language
                    {
                        Name = "English",
                        ServiceId = 0,
                    }
                },
                ServiceAreas = new List<ServiceArea>
                {
                    new ServiceArea
                    {
                        ServiceAreaName = "Local",
                        Uri = "http://statistics.data.gov.uk/id/statistical-geography/K02000001",
                        ServiceId = 0,
                    }
                },
                Locations = new List<Location>
                {
                    new Location
                    {
                        LocationType = LocationType.FamilyHub,
                        Name = "Central Family Hub",
                        Description = "Broughton Hub",
                        Longitude = .507025D,
                        Latitude = -2.259764D,
                        Address1 = "50 Rigby Street",
                        City = "Manchester",
                        PostCode = "M7 4BQ",
                        Country = "United Kingdom",
                        StateProvince = "Salford",
                        Contacts = new List<Contact>
                        {
                            new Contact
                            {
                                Title = "Ms",
                                Name = "Kate Berry",
                                Telephone = "0161 778 0601",
                                TextPhone = "0161 778 0601",
                                Url = "www.gov.uk",
                                Email = "help@gov.uk"
                            }
                        }
                    }
                }
            },
            new Service
            {
                OrganisationId = organisationId,
                ServiceOwnerReferenceId = Guid.NewGuid().ToString(),
                ServiceType = ServiceType.FamilyExperience,
                Name = "North Family Hub",
                Description = "Family Hub",
                AttendingAccess = AttendingAccessType.NotSet,
                AttendingType = AttendingType.NotSet,
                DeliverableType = DeliverableType.NotSet,
                Status = ServiceStatusType.Active,
                CanFamilyChooseDeliveryLocation = false,
                ServiceDeliveries = new List<ServiceDelivery>
                {
                    new ServiceDelivery
                    {
                        Name = ServiceDeliveryType.InPerson,
                        ServiceId = 0
                    }
                },
                Eligibilities = new List<Eligibility>
                {
                    new Eligibility
                    {
                        EligibilityType = EligibilityType.Child,
                        MaximumAge = 25,
                        MinimumAge = 0,
                        ServiceId = 0
                    }
                },
                Languages = new List<Language>
                {
                    new Language
                    {
                        Name = "English",
                        ServiceId = 0
                    }
                },
                ServiceAreas = new List<ServiceArea>
                {
                    new ServiceArea
                    {
                        ServiceAreaName = "Local",
                        Uri = "http://statistics.data.gov.uk/id/statistical-geography/K02000001",
                        ServiceId = 0
                    }
                },
                Locations = new List<Location>
                {
                    new Location
                    {
                        LocationType = LocationType.FamilyHub,
                        Name = "North Family Hub",
                        Description = "Swinton Gateway",
                        Longitude = .5124278D,
                        Latitude = -2.342044D,
                        Address1 = "100 Chorley Road",
                        City = "Manchester",
                        PostCode = "M27 6BP",
                        Country = "United Kingdom",
                        StateProvince = "Salford",
                        Contacts = new List<Contact>
                        {
                            new Contact
                            {
                                Title = "Ms",
                                Name = "Kate Berry",
                                Telephone = "0161 778 0495",
                                TextPhone = "0161 778 0495",
                                Url = "www.gov.uk",
                                Email = "help@gov.uk"
                            }
                        }
                    }
                }
            },
            new Service
            {
                OrganisationId = organisationId,
                ServiceOwnerReferenceId = Guid.NewGuid().ToString(),
                ServiceType = ServiceType.FamilyExperience,
                Name = "South Family Hub",
                Description = "Family Hub",
                AttendingAccess = AttendingAccessType.NotSet,
                AttendingType = AttendingType.NotSet,
                DeliverableType = DeliverableType.NotSet,
                Status = ServiceStatusType.Active,
                CanFamilyChooseDeliveryLocation = false,
                ServiceDeliveries = new List<ServiceDelivery>
                {
                    new ServiceDelivery
                    {
                        Name = ServiceDeliveryType.InPerson,
                        ServiceId = 0,
                    }
                },
                Eligibilities = new List<Eligibility>
                {
                    new Eligibility
                    {
                        EligibilityType = EligibilityType.Child,
                        MaximumAge = 25,
                        MinimumAge = 0,
                        ServiceId = 0,
                    }
                },
                Fundings = new List<Funding>
                {
                    new Funding
                    {
                        Source = "funding source",
                        ServiceId = 0,
                    }
                },
                CostOptions = new List<CostOption>
                {
                    new CostOption
                    {
                        AmountDescription = "Amount Description",
                        Amount = 100000000,
                        Option = "options",
                        ValidFrom = new DateTime(2023, 01, 01).ToUniversalTime(),
                        ValidTo = new DateTime(2023, 01, 01).ToUniversalTime(),
                        ServiceId = 0
                    }
                },
                Languages = new List<Language>
                {
                    new Language
                    {
                        Name = "English",
                        ServiceId = 0
                    }
                },
                Reviews = new List<Review>
                {
                    new Review
                    {
                        ServiceId = 0,
                        Title = "Test Review",
                        Description = "Review description",
                        Date = new DateTime(2023, 01, 01).ToUniversalTime(),
                        Score = "5 start",
                        Url = "gov.uk",
                        Widget = "widget"
                    }
                },
                ServiceAreas = new List<ServiceArea>
                {
                    new ServiceArea
                    {
                        ServiceAreaName = "Local",
                        Uri = "http://statistics.data.gov.uk/id/statistical-geography/K02000001",
                        ServiceId = 0
                    }
                },
                HolidaySchedules = new List<HolidaySchedule>
                {
                    new HolidaySchedule
                    {
                        Closed = true,
                        OpensAt = new DateTime(2023, 01, 01).ToUniversalTime(),
                        ClosesAt = new DateTime(2023, 01, 01).ToUniversalTime(),
                        StartDate = new DateTime(2023, 01, 01).ToUniversalTime(),
                        EndDate = new DateTime(2023, 01, 01).ToUniversalTime(),
                        ServiceId = 0
                    }
                },
                RegularSchedules = new List<RegularSchedule>
                {
                    new RegularSchedule
                    {
                        Description = "RegularSchedule",
                        OpensAt = new DateTime(2023, 01, 01).ToUniversalTime(),
                        ClosesAt = new DateTime(2023, 01, 01).ToUniversalTime(),
                        ByDay = "byDay",
                        ByMonthDay = "byMonth",
                        DtStart = "dtStart",
                        Freq = FrequencyType.NotSet,
                        Interval = "Interval",
                        ValidFrom = new DateTime(2023, 01, 01).ToUniversalTime(),
                        ValidTo = new DateTime(2023, 01, 01).ToUniversalTime(),
                        ServiceId = 0
                    }
                },
                Locations = new List<Location>
                {
                    new Location
                    {
                        LocationType = LocationType.FamilyHub,
                        Name = "South Family Hub",
                        Description = "Winton Children’s Centre",
                        Longitude = .48801070060149D,
                        Latitude = -2.368140748303118D,
                        Address1 = "Brindley Street",
                        City = "Manchester",
                        PostCode = "M30 8AB",
                        Country = "United Kingdom",
                        StateProvince = "Salford",
                        AccessibilityForDisabilities = new List<AccessibilityForDisabilities>
                        {
                            new AccessibilityForDisabilities
                            {
                                Accessibility = "AccessibilityForDisabilities",
                                LocationId = 0
                            }
                        },
                        RegularSchedules = new List<RegularSchedule>
                        {
                            new RegularSchedule
                            {
                                Description = "RegularSchedule",
                                OpensAt = new DateTime(2023, 01, 01).ToUniversalTime(),
                                ClosesAt = new DateTime(2023, 01, 01).ToUniversalTime(),
                                ByDay = "byDay",
                                ByMonthDay = "byMonth",
                                DtStart = "dtStart",
                                Freq = FrequencyType.NotSet,
                                Interval = "Interval",
                                ValidFrom = new DateTime(2023, 01, 01).ToUniversalTime(),
                                ValidTo = new DateTime(2023, 01, 01).ToUniversalTime(),
                            }
                        },
                        HolidaySchedules = new List<HolidaySchedule>
                        {
                            new HolidaySchedule
                            {
                                Closed = true,
                                OpensAt = new DateTime(2023, 01, 01).ToUniversalTime(),
                                ClosesAt = new DateTime(2023, 01, 01).ToUniversalTime(),
                                StartDate = new DateTime(2023, 01, 01).ToUniversalTime(),
                                EndDate = new DateTime(2023, 01, 01).ToUniversalTime(),
                            }
                        },
                        Contacts = new List<Contact>
                        {
                            new Contact
                            {
                                Title = "Ms",
                                Name = "Kate Berry",
                                Telephone = "0161 686 5260",
                                TextPhone = "0161 686 5260",
                                Url = "www.gov.uk",
                                Email = "help@gov.uk"
                            }
                        }
                    }
                },
                Taxonomies = new List<Taxonomy>
                {
                    new Taxonomy
                    {
                        Name = "breastfeeding support",
                        TaxonomyType = TaxonomyType.ServiceCategory,
                        ParentId = 1
                    }
                },
                Contacts = new List<Contact>
                {
                    new Contact
                    {
                        Title = "Ms",
                        Name = "Kate Berry",
                        Telephone = "0161 686 5260",
                        TextPhone = "0161 686 5260",
                        Url = "www.gov.uk",
                        Email = "help@gov.uk"
                    }
                }
            },
        };
    }

    private ICollection<Service> SeedBristolServices(long organisationId)
    {
        if (IsProduction) return new List<Service>();

        return new List<Service>
        {
            new Service
            {
                OrganisationId = organisationId,
                ServiceOwnerReferenceId = Guid.NewGuid().ToString(),
                ServiceType = ServiceType.FamilyExperience,
                Name = "Aid for Children with Tracheostomies",
                Description = @"Aid for Children with Tracheostomies is a national self help group operating as a registered charity and is run by parents of children with a tracheostomy and by people who sympathise with the needs of such families. ACT as an organisation is non profit making, it links groups and individual members throughout Great Britain and Northern Ireland.",
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
                        EligibilityType = EligibilityType.Child,
                        MaximumAge = 13,
                        MinimumAge = 0,
                    }
                },
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
                        Uri = "http://statistics.data.gov.uk/id/statistical-geography/K02000001",
                    }
                },
                Taxonomies = new List<Taxonomy>
                {
                    new Taxonomy
                    {
                        Name = "Hearing and sight",
                        TaxonomyType = TaxonomyType.ServiceCategory,
                        ParentId = 3
                    }
                },
                Contacts = new List<Contact>
                {
                    new Contact
                    {
                        Title = "Mr",
                        Name = "John Smith",
                        Telephone = "01827 65778",
                        TextPhone = "01827 65778",
                        Url = "www.gov.uk",
                        Email = "help@gov.uk"
                    }
                },
                Locations = new List<Location>
                {
                    new Location
                    {
                        LocationType = LocationType.NotSet,
                        Name = "test",
                        Description = "",
                        Longitude = .48801070060149D,
                        Latitude = -1.66526,
                        Address1 = "7A Boyce's Ave, Clifton",
                        City = "Bristol",
                        PostCode = "BS8 4AA",
                        Country = "England",
                        StateProvince = "Bristol",
                    },
                }
            },
            new Service
            {
                OrganisationId = organisationId,
                ServiceOwnerReferenceId = Guid.NewGuid().ToString(),
                ServiceType = ServiceType.InformationSharing,
                Name = "Test Service - Free - 10 to 15 yrs",
                Description = @"This is a test service.",
                AttendingAccess = AttendingAccessType.NotSet,
                AttendingType = AttendingType.NotSet,
                DeliverableType = DeliverableType.NotSet,
                Status = ServiceStatusType.Active,
                CanFamilyChooseDeliveryLocation = false,
                ServiceDeliveries = new List<ServiceDelivery>
                {
                    new ServiceDelivery
                    {
                        Name = ServiceDeliveryType.InPerson,
                    }
                },
                Eligibilities = new List<Eligibility>
                {
                    new Eligibility
                    {
                        EligibilityType = EligibilityType.Child,
                        MaximumAge = 15,
                        MinimumAge = 10,
                    }
                },
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
                        Uri = "http://statistics.data.gov.uk/id/statistical-geography/K02000001",
                    }
                },
                Taxonomies = new List<Taxonomy>
                {
                    new Taxonomy
                    {
                        Name = "Childrens Activities",
                        TaxonomyType = TaxonomyType.ServiceCategory,
                        ParentId = 4
                    }
                },
                Locations = new List<Location>
                {
                    new Location
                    {
                        LocationType = LocationType.NotSet,
                        Name = "",
                        Description = "",
                        Longitude = .6312,
                        Latitude = -1.66526,
                        Address1 = "7A Boyce's Ave, Clifton",
                        City = "Bristol",
                        PostCode = "BS8 4AA",
                        Country = "England",
                        StateProvince = "Bristol",
                    }
                },
                Contacts = new List<Contact>
                {
                    new Contact
                    {
                        Title = "Mr",
                        Name = "John Smith",
                        Telephone = "01827 65711",
                        TextPhone = "01827 65711",
                        Url = "www.gov.uk",
                        Email = "help@gov.uk"
                    }
                }
            },
            new Service
            {
                OrganisationId = organisationId,
                ServiceOwnerReferenceId = Guid.NewGuid().ToString(),
                ServiceType = ServiceType.FamilyExperience,
                Name = "Test Service - Paid - 0 to 13yrs",
                Description = @"This is a paid test service.",
                AttendingAccess = AttendingAccessType.NotSet,
                AttendingType = AttendingType.NotSet,
                DeliverableType = DeliverableType.NotSet,
                Status = ServiceStatusType.Active,
                CanFamilyChooseDeliveryLocation = false,
                ServiceDeliveries = new List<ServiceDelivery>
                {
                    new ServiceDelivery
                    {
                        Name = ServiceDeliveryType.Telephone,
                    }
                },
                Eligibilities = new List<Eligibility>
                {
                    new Eligibility
                    {
                        EligibilityType = EligibilityType.Child,
                        MaximumAge = 13,
                        MinimumAge = 0,
                    }
                },
                CostOptions = new List<CostOption>
                {
                    new CostOption
                    {
                        Option = "Session",
                        Amount = 45.0m,
                        AmountDescription = "AmountDescription"
                    }
                },
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
                        Uri = "http://statistics.data.gov.uk/id/statistical-geography/K02000001",
                    }
                },
                Locations = new List<Location>
                {
                    new Location
                    {
                        LocationType = LocationType.NotSet,
                        Name = "",
                        Description = "",
                        Longitude = .63123,
                        Latitude = -1.66519,
                        Address1 = "7A Boyce's Ave, Clifton",
                        City = "Bristol",
                        PostCode = "BS8 4AA",
                        Country = "England",
                        StateProvince = "Bristol",
                    },
                },
                Taxonomies = new List<Taxonomy>
                {
                    new Taxonomy
                    {
                        Name = "parent child activities",
                        TaxonomyType = TaxonomyType.ServiceCategory,
                        ParentId = 4
                    }
                },
                Contacts = new List<Contact>
                {
                    new Contact
                    {
                        Title = "Mr",
                        Name = "John Smith",
                        Telephone = "01827 64328",
                        TextPhone = "01827 64328",
                        Url = "www.gov.uk",
                        Email = "help@gov.uk"
                    }
                }
            },
            new Service
            {
                OrganisationId = organisationId,
                ServiceOwnerReferenceId = Guid.NewGuid().ToString(),
                ServiceType = ServiceType.FamilyExperience,
                Name = "Test Service - Paid - 15 to 20yrs - Afrikaans",
                Description = @"This is an Afrikaans test service.",
                AttendingAccess = AttendingAccessType.NotSet,
                AttendingType = AttendingType.NotSet,
                DeliverableType = DeliverableType.NotSet,
                Status = ServiceStatusType.Active,
                CanFamilyChooseDeliveryLocation = true,
                ServiceDeliveries = new List<ServiceDelivery>
                {
                    new ServiceDelivery
                    {
                        Name = ServiceDeliveryType.InPerson,
                    }
                },
                Eligibilities = new List<Eligibility>
                {
                    new Eligibility
                    {
                        EligibilityType = EligibilityType.Child,
                        MaximumAge = 20,
                        MinimumAge = 15,
                    }
                },
                CostOptions = new List<CostOption>
                {
                    new CostOption
                    {
                        Option = "Hour",
                        Amount = 25.0m,
                        AmountDescription = "AmountDescription"
                    }
                },
                Languages = new List<Language>
                {
                    new Language
                    {
                        Name = "Afrikaans",
                    }
                },
                ServiceAreas = new List<ServiceArea>
                {
                    new ServiceArea
                    {
                        ServiceAreaName = "National",
                        Uri = "http://statistics.data.gov.uk/id/statistical-geography/K02000001",
                    }
                },
                Locations = new List<Location>
                {
                    new Location
                    {
                        LocationType = LocationType.NotSet,
                        Name = "",
                        Description = "",
                        Longitude = .63123,
                        Latitude = -1.66519,
                        Address1 = "7A Boyce's Ave, Clifton",
                        City = "Bristol",
                        PostCode = "BS8 4AA",
                        Country = "England",
                        StateProvince = "",
                        Contacts = new List<Contact>
                        {
                            new Contact
                            {
                                Title = "Mr",
                                Name = "John Smith",
                                Telephone = "01827 64328",
                                TextPhone = "01827 64328",
                                Url = "www.gov.uk",
                                Email = "help@gov.uk"
                            }
                        }
                    }
                },
                Taxonomies = new List<Taxonomy>
                {
                    new Taxonomy
                    {
                        Name = "swimming",
                        TaxonomyType = TaxonomyType.ServiceCategory,
                        ParentId = 4
                    }
                }
            }
        };
    }
}
