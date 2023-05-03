using FamilyHubs.ServiceDirectory.Data.Entities;
using FamilyHubs.ServiceDirectory.Shared.Dto;
using FamilyHubs.ServiceDirectory.Shared.Enums;

namespace FamilyHubs.ServiceDirectory.Core.IntegrationTests;

public static class TestDataProvider
{
    public static OrganisationWithServicesDto GetTestCountyCouncilDto(bool updated = false)
    {
        var testCountyCouncil = new OrganisationWithServicesDto
        {
            OrganisationType = OrganisationType.LA,
            Name = updated == false ? "Unit Test County Council" : "Unit Test County Council Updated",
            Description = updated == false ? "Unit Test County Council" : "Unit Test County Council Updated",
            Uri = new Uri("https://www.unittest.gov.uk/").ToString(),
            Url = "https://www.unittest.gov.uk/",
            AdminAreaCode = "XTEST",
            Services = new List<ServiceDto>
            {
                GetTestCountyCouncilServicesDto(0, updated)
            },
        };

        return testCountyCouncil;
    }

    public static OrganisationWithServicesDto GetTestCountyCouncilDto2()
    {
        var testCountyCouncil = new OrganisationWithServicesDto
        {
            OrganisationType = OrganisationType.LA,
            Name = "Unit Test County Council 2",
            Description = "Unit Test County Council 2",
            Uri = new Uri("https://www.unittest2.gov.uk/").ToString(),
            Url = "https://www.unittest2.gov.uk/",
            Services = new List<ServiceDto>
            {
                GetTestCountyCouncilServicesDto2(0)
            },
            AdminAreaCode = "XTEST"
        };

        return testCountyCouncil;
    }

    public static ServiceDto GetTestCountyCouncilServicesDto(long organisationId, bool updated = false)
    {
        var serviceId = "3010521b-6e0a-41b0-b610-200edbbeeb14";

        var service = new ServiceDto
        {
            ServiceOwnerReferenceId = serviceId,
            OrganisationId = organisationId,
            ServiceType = ServiceType.InformationSharing,
            Status = ServiceStatusType.Active,
            Name = updated == false ? "Unit Test Service" : "Unit Test Service Updated",
            ServiceDeliveries = new List<ServiceDeliveryDto>
            {
                new ServiceDeliveryDto
                {
                    Name = updated == false ? ServiceDeliveryType.Online : ServiceDeliveryType.Telephone,
                }
            },
            Eligibilities = new List<EligibilityDto>
            {
                new EligibilityDto
                {
                    EligibilityType = EligibilityType.NotSet,
                    MinimumAge = updated == false ? 0 : 1,
                    MaximumAge = updated == false ? 13 : 14,
                }
            },
            Contacts = new List<ContactDto>
            {
                new ContactDto
                {
                    Name = updated == false ? "Service Contact" : "Updated Service Contact",
                    Title = string.Empty,
                    Telephone = "01827 65777",
                    TextPhone = "01827 65777",
                    Url = "https://www.unittestservice.com",
                    Email = "support@unittestservice.com"
                }
            },
            CostOptions = new List<CostOptionDto>
            {
                new CostOptionDto
                {
                    AmountDescription = updated == false ? "amount_description1" : "amount_description2",
                    Amount = decimal.Zero,
                    Option = "free",
                    ValidFrom = new DateTime(2023, 1, 1).ToUniversalTime(),
                    ValidTo = new DateTime(2023, 1, 1).ToUniversalTime().AddHours(8),
                }
            },
            Languages = new List<LanguageDto>
            {
                new LanguageDto
                {
                    Name = updated == false ? "English"  : "French",
                }
            },
            ServiceAreas = new List<ServiceAreaDto>
            {
                new ServiceAreaDto
                {
                    ServiceAreaName = updated == false ? "National" : "Local",
                    Extent = null,
                    Uri = "http://statistics.data.gov.uk/id/statistical-geography/K02000001",
                }
            },
            Locations = new List<LocationDto>
            {
                new LocationDto
                {
                    Name = updated == false ? "Test Location" : "Test New Location",
                    Description = "",
                    Latitude = 52.6312,
                    Longitude = -1.66526,
                    Address1 = updated == false ? "77 Sheepcote Lane" : "78 Sheepcote Lane",
                    City = ", Stathe, Tamworth, Staffordshire, ",
                    PostCode = "B77 3JN",
                    Country = "England",
                    StateProvince = "null",
                    LocationType = updated == false ? LocationType.FamilyHub : LocationType.NotSet,
                    Contacts = new List<ContactDto>
                    {
                        new ContactDto
                        {
                            Name = updated == false ? "Location Contact" : "Updated Location Contact",
                            Title = string.Empty,
                            TextPhone = "01827 65777",
                            Telephone = "01827 65777",
                            Url = "https://www.unittestservice.com",
                            Email = "support@unittestservice.com"
                        }
                    },
                    RegularSchedules = new List<RegularScheduleDto>
                    {
                        new RegularScheduleDto
                        {
                            Description = "Location Level Description",
                            ValidFrom = new DateTime(2023, 1, 1).ToUniversalTime(),
                            ValidTo = new DateTime(2023, 1, 1).ToUniversalTime().AddHours(8),
                            ByDay = updated == false ?  "byDay1" : "byDay2",
                            ByMonthDay = "byMonth",
                            DtStart = "dtStart",
                            Freq = FrequencyType.NotSet,
                            Interval = "interval",
                            OpensAt = new DateTime(2023, 1, 1).ToUniversalTime(),
                            ClosesAt = new DateTime(2023, 1, 1).ToUniversalTime().AddMonths(6)
                        }
                    },
                    HolidaySchedules = new List<HolidayScheduleDto>
                    {
                        new HolidayScheduleDto
                        {
                            Closed = updated,
                            ClosesAt = new DateTime(2023, 1, 1).ToUniversalTime().AddDays(1),
                            OpensAt = new DateTime(2023, 1, 1).ToUniversalTime().AddDays(1),
                            StartDate = new DateTime(2023, 1, 1).ToUniversalTime().AddDays(1),
                            EndDate = new DateTime(2023, 1, 1).ToUniversalTime().AddDays(1)
                        }
                    }
                }
            },
            Taxonomies = new List<TaxonomyDto>
            {
                new TaxonomyDto
                {
                    Name = updated == false ? "Organisation" : "Organisation Updated",
                    TaxonomyType = TaxonomyType.ServiceCategory,
                    ParentId = null
                },
                new TaxonomyDto
                {
                    Name = updated == false ? "Support" : "Support Updated",
                    TaxonomyType = TaxonomyType.ServiceCategory,
                    ParentId = null
                },
                new TaxonomyDto
                {
                    Name = "Children",
                    TaxonomyType = TaxonomyType.ServiceCategory,
                    ParentId = null
                },
                new TaxonomyDto
                {
                    Name = "Long Term Health Conditions",
                    TaxonomyType = TaxonomyType.ServiceCategory,
                    ParentId = null
                }
            },
            RegularSchedules = new List<RegularScheduleDto>
            {
                new RegularScheduleDto
                {
                    Description = "Service Level Description",
                    OpensAt = new DateTime(2023, 1, 1).ToUniversalTime(),
                    ClosesAt = new DateTime(2023, 1, 1).ToUniversalTime().AddHours(8),
                    ByDay = "byDay1",
                    ByMonthDay = "byMonth",
                    DtStart = "dtStart",
                    Freq = FrequencyType.NotSet,
                    Interval = "interval",
                    ValidFrom = new DateTime(2023, 1, 1).ToUniversalTime(),
                    ValidTo = new DateTime(2023, 1, 1).ToUniversalTime().AddMonths(6)
                }
            },
            HolidaySchedules = new List<HolidayScheduleDto>
            {
                new HolidayScheduleDto
                {
                    Closed = !updated,
                    ClosesAt = new DateTime(2023, 1, 1).ToUniversalTime(),
                    OpensAt = new DateTime(2023, 1, 1).ToUniversalTime(),
                    StartDate = new DateTime(2023, 1, 1).ToUniversalTime(),
                    EndDate = new DateTime(2023, 1, 1).ToUniversalTime()
                }
            }
        };

        return service;
    }

    public static ServiceDto GetTestCountyCouncilServicesDto2(long organisationId, string serviceId = "5059a0b2-ad5d-4288-b7c1-e30d35345b0e")
    {
        var service = new ServiceDto
        {
            ServiceOwnerReferenceId = serviceId,
            OrganisationId = organisationId,
            ServiceType = ServiceType.InformationSharing,
            Status = ServiceStatusType.Active,
            Name = "Unit Test Service",
            Description = @"Unit Test Service Description",
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
                    EligibilityType = EligibilityType.NotSet,
                    MinimumAge = 0,
                    MaximumAge = 13,
                }
            },
            Contacts = new List<ContactDto>
            {
                new ContactDto
                {
                    Name = "Contact",
                    Title = string.Empty,
                    Telephone = "01827 65777",
                    TextPhone = "01827 65777",
                    Url = "https://www.unittestservice.com",
                    Email = "support@unittestservice.com"
                }
            },
            CostOptions = new List<CostOptionDto>
            {
                new CostOptionDto
                {
                    Amount = 1,
                    Option = "paid",
                    AmountDescription = "£1 a session",
                }
            },
            Languages = new List<LanguageDto>
            {
                new LanguageDto
                {
                    Name = "English",
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
                    Name = "Test Location",
                    Description = "",
                    Latitude = 52.6312,
                    Longitude = -1.66526,
                    Address1 = "Some Lane",
                    City = ", Stathe, Tamworth, Staffordshire, ",
                    PostCode = "B77 3JN",
                    Country = "England",
                    StateProvince = "null",
                    LocationType = LocationType.FamilyHub,
                    Contacts = new List<ContactDto>
                    {
                        new ContactDto
                        {
                            Name = "Contact",
                            Title = string.Empty,
                            TextPhone = "01827 65777",
                            Telephone = "01827 65777",
                            Url = "https://www.unittestservice.com",
                            Email = "support@unittestservice.com"
                        }
                    },
                    RegularSchedules = new List<RegularScheduleDto>
                    {
                        new RegularScheduleDto
                        {
                            Description = "Description",
                            ValidFrom = new DateTime(2023, 1, 1).ToUniversalTime(),
                            ValidTo = new DateTime(2023, 1, 1).ToUniversalTime().AddHours(8),
                            ByDay = "byDay",
                            ByMonthDay = "byMonth",
                            DtStart = "dtStart",
                            Freq = FrequencyType.NotSet,
                            Interval = "interval",
                            OpensAt = new DateTime(2023, 1, 1).ToUniversalTime(),
                            ClosesAt = new DateTime(2023, 1, 1).ToUniversalTime().AddMonths(6)
                        }
                    },
                    HolidaySchedules = new List<HolidayScheduleDto>
                    {
                        new HolidayScheduleDto
                        {
                            Closed = false,
                            ClosesAt = new DateTime(2023, 1, 1).ToUniversalTime(),
                            OpensAt = new DateTime(2023, 1, 1).ToUniversalTime(),
                            StartDate = new DateTime(2023, 1, 1).ToUniversalTime().AddDays(5) ,
                            EndDate = new DateTime(2023, 1, 1).ToUniversalTime()
                        }
                    }
                }
            },
            Taxonomies = new List<TaxonomyDto>
            {
                new TaxonomyDto
                {
                    Name = "Organisation",
                    TaxonomyType = TaxonomyType.ServiceCategory,
                    ParentId = null
                },
                new TaxonomyDto
                {
                    Name = "Support",
                    TaxonomyType = TaxonomyType.ServiceCategory,
                    ParentId = null
                },
                new TaxonomyDto
                {
                    Name = "Children",
                    TaxonomyType = TaxonomyType.ServiceCategory,
                    ParentId = null
                },
                new TaxonomyDto
                {
                    Name = "Long Term Health Conditions",
                    TaxonomyType = TaxonomyType.ServiceCategory,
                    ParentId = null
                }
            },
            RegularSchedules = new List<RegularScheduleDto>
            {
                new RegularScheduleDto
                {
                    Description = "Description",
                    OpensAt = new DateTime(2023, 1, 1).ToUniversalTime(),
                    ClosesAt = new DateTime(2023, 1, 1).ToUniversalTime().AddHours(8),
                    ByDay = "byDay1",
                    ByMonthDay = "byMonth",
                    DtStart = "dtStart",
                    Freq = FrequencyType.NotSet,
                    Interval = "interval",
                    ValidTo = new DateTime(2023, 1, 1).ToUniversalTime(),
                    ValidFrom = new DateTime(2023, 1, 1).ToUniversalTime().AddMonths(6)
                }
            },
            HolidaySchedules = new List<HolidayScheduleDto>
            {
                new HolidayScheduleDto
                {
                    Closed = true,
                    ClosesAt = new DateTime(2023, 1, 1).ToUniversalTime(),
                    OpensAt = new DateTime(2023, 1, 1).ToUniversalTime(),
                    StartDate = new DateTime(2023, 1, 1).ToUniversalTime().AddDays(5),
                    EndDate = new DateTime(2023, 1, 1).ToUniversalTime()
                }
            }
        };

        return service;
    }

    public static OrganisationDto GetTestCountyCouncilWithoutAnyServices()
    {
        var testCountyCouncil = new OrganisationDto
        {
            OrganisationType = OrganisationType.LA,
            Name = "Unit Test A County Council",
            Description = "Unit Test A County Council",
            AdminAreaCode = "XTEST",
            Uri = new Uri("https://www.unittesta.gov.uk/").ToString(),
            Url = "https://www.unittesta.gov.uk/"
        };

        return testCountyCouncil;
    }

    public static IList<Service> SeedSalfordService(long organisationId)
    {
        return new List<Service>
        {
            new Service
            {
                OrganisationId = organisationId,
                ServiceOwnerReferenceId = "Salford-Service-1",
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
                                Url = "https://www.gov.uk",
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
                ServiceOwnerReferenceId = "Salford-Service-2",
                ServiceType = ServiceType.FamilyExperience,
                Name = "Oakwood Academy",
                Description = "Oakwood Academy is a special school for pupils aged 9-18 years who have a range of moderate and/or complex learning difficulties. The school has Visual Arts, Technology and Sports Specialist status. \r\n\r\nAdmissions to Oakwood Academy are controlled by Salford Local Authority. We are unable to accept direct requests for placement from parents or carers or other local authorities. Pupils who attend Oakwood Academy have an Educational, Health and Care Plan which outlines the area of need and what provision and resources are needed to support the pupil. \r\n\r\nIn rare cases, a child may be admitted on an assessment placement to determine what the pupil's needs are and whether their needs can be met at Oakwood Academy. ",
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
                                Url = "https://www.gov.uk",
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
                ServiceOwnerReferenceId = "Salford-Service-3",
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
                                Url = "https://www.gov.uk",
                                Email = "help@gov.uk"
                            }
                        }
                    }
                }
            },
            new Service
            {
                OrganisationId = organisationId,
                ServiceOwnerReferenceId = "Salford-Service-4",
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
                                Url = "https://www.gov.uk",
                                Email = "help@gov.uk"
                            }
                        }
                    }
                }
            },
            new Service
            {
                OrganisationId = organisationId,
                ServiceOwnerReferenceId = "Salford-Service-5",
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
                        ValidFrom = new DateTime(2023, 1, 1).ToUniversalTime(),
                        ValidTo = new DateTime(2023, 1, 1).ToUniversalTime(),
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
                        Date = new DateTime(2023, 1, 1).ToUniversalTime(),
                        Score = "5 start",
                        Url = "https://gov.uk",
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
                        OpensAt = new DateTime(2023, 1, 1).ToUniversalTime(),
                        ClosesAt = new DateTime(2023, 1, 1).ToUniversalTime(),
                        StartDate = new DateTime(2023, 1, 1).ToUniversalTime(),
                        EndDate = new DateTime(2023, 1, 1).ToUniversalTime(),
                        ServiceId = 0
                    }
                },
                RegularSchedules = new List<RegularSchedule>
                {
                    new RegularSchedule
                    {
                        Description = "RegularSchedule",
                        OpensAt = new DateTime(2023, 1, 1).ToUniversalTime(),
                        ClosesAt = new DateTime(2023, 1, 1).ToUniversalTime(),
                        ByDay = "byDay",
                        ByMonthDay = "byMonth",
                        DtStart = "dtStart",
                        Freq = FrequencyType.NotSet,
                        Interval = "Interval",
                        ValidFrom = new DateTime(2023, 1, 1).ToUniversalTime(),
                        ValidTo = new DateTime(2023, 1, 1).ToUniversalTime(),
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
                                OpensAt = new DateTime(2023, 1, 1).ToUniversalTime(),
                                ClosesAt = new DateTime(2023, 1, 1).ToUniversalTime(),
                                ByDay = "byDay",
                                ByMonthDay = "byMonth",
                                DtStart = "dtStart",
                                Freq = FrequencyType.NotSet,
                                Interval = "Interval",
                                ValidFrom = new DateTime(2023, 1, 1).ToUniversalTime(),
                                ValidTo = new DateTime(2023, 1, 1).ToUniversalTime(),
                            }
                        },
                        HolidaySchedules = new List<HolidaySchedule>
                        {
                            new HolidaySchedule
                            {
                                Closed = true,
                                OpensAt = new DateTime(2023, 1, 1).ToUniversalTime(),
                                ClosesAt = new DateTime(2023, 1, 1).ToUniversalTime(),
                                StartDate = new DateTime(2023, 1, 1).ToUniversalTime(),
                                EndDate = new DateTime(2023, 1, 1).ToUniversalTime(),
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
                                Url = "https://www.gov.uk",
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
                        Url = "https://www.gov.uk",
                        Email = "help@gov.uk"
                    }
                }
            },
        };
    }

    public static IList<Service> SeedBristolServices(long organisationId)
    {
        return new List<Service>
        {
            new Service
            {
                OrganisationId = organisationId,
                ServiceOwnerReferenceId = "Bristol-Service-1",
                ServiceType = ServiceType.InformationSharing,
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
                        Url = "https://www.gov.uk",
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
                ServiceOwnerReferenceId = "Bristol-Service-2",
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
                        Url = "https://www.gov.uk",
                        Email = "help@gov.uk"
                    }
                }
            },
            new Service
            {
                OrganisationId = organisationId,
                ServiceOwnerReferenceId = "Bristol-Service-3",
                ServiceType = ServiceType.InformationSharing,
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
                        Url = "https://www.gov.uk",
                        Email = "help@gov.uk"
                    }
                }
            },
            new Service
            {
                OrganisationId = organisationId,
                ServiceOwnerReferenceId = "Bristol-Service-4",
                ServiceType = ServiceType.InformationSharing,
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
                                Url = "https://www.gov.uk",
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