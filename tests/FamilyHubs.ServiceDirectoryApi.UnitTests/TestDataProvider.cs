using FamilyHubs.ServiceDirectory.Shared.Dto;
using FamilyHubs.ServiceDirectory.Shared.Enums;

namespace FamilyHubs.ServiceDirectoryApi.UnitTests;

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
            Services = new List<ServiceDto>
            {
                GetTestCountyCouncilServicesDto(0, updated)
            },
            AdminAreaCode = "XTEST"
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
                    Name = updated == false ? "Contact" : "Updated Contact",
                    Title = string.Empty,
                    Telephone = "01827 65777",
                    TextPhone = "01827 65777",
                    Url = "www.unittestservice.com",
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
                    ValidFrom = DateTime.UtcNow,
                    ValidTo = DateTime.UtcNow.AddHours(8),
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
                            Name = updated == false ? "Contact" : "Updated Contact",
                            Title = string.Empty,
                            TextPhone = "01827 65777",
                            Telephone = "01827 65777",
                            Url = "www.unittestservice.com",
                            Email = "support@unittestservice.com"
                        }
                    },
                    RegularSchedules = new List<RegularScheduleDto>
                    {
                        new RegularScheduleDto
                        {
                            Description = "Description",
                            ValidFrom = DateTime.UtcNow,
                            ValidTo = DateTime.UtcNow.AddHours(8),
                            ByDay = updated == false ?  "byDay1" : "byDay2",
                            ByMonthDay = "byMonth",
                            DtStart = "dtStart",
                            Freq = FrequencyType.NotSet,
                            Interval = "interval",
                            OpensAt = DateTime.UtcNow,
                            ClosesAt = DateTime.UtcNow.AddMonths(6)
                        }
                    },
                    HolidaySchedules = new List<HolidayScheduleDto>
                    {
                        new HolidayScheduleDto
                        {
                            Closed = updated,
                            ClosesAt = DateTime.UtcNow,
                            OpensAt = DateTime.UtcNow,
                            StartDate = DateTime.UtcNow.AddDays(5) ,
                            EndDate = DateTime.UtcNow
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
                    Description = "Description",
                    OpensAt = DateTime.UtcNow,
                    ClosesAt = DateTime.UtcNow.AddHours(8),
                    ByDay = "byDay1",
                    ByMonthDay = "byMonth",
                    DtStart = "dtStart",
                    Freq = FrequencyType.NotSet,
                    Interval = "interval",
                    ValidFrom = DateTime.UtcNow,
                    ValidTo = DateTime.UtcNow.AddMonths(6)
                }
            }
        };

        return service;
    }

    public static ServiceDto GetTestCountyCouncilServicesDto2(long organisationId)
    {
        var serviceId = "5059a0b2-ad5d-4288-b7c1-e30d35345b0e";

        var service = new ServiceDto
        {
            ServiceOwnerReferenceId = serviceId,
            OrganisationId = organisationId,
            ServiceType = ServiceType.InformationSharing,
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
                    Url = "www.unittestservice.com",
                    Email = "support@unittestservice.com"
                }
            },
            CostOptions = new List<CostOptionDto>
            {
                new CostOptionDto
                {
                    Amount = decimal.Zero,
                    Option = "free",
                    AmountDescription = "",
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
                            Url = "www.unittestservice.com",
                            Email = "support@unittestservice.com"
                        }
                    },
                    RegularSchedules = new List<RegularScheduleDto>
                    {
                        new RegularScheduleDto
                        {
                            Description = "Description",
                            ValidFrom = DateTime.UtcNow,
                            ValidTo = DateTime.UtcNow.AddHours(8),
                            ByDay = "byDay",
                            ByMonthDay = "byMonth",
                            DtStart = "dtStart",
                            Freq = FrequencyType.NotSet,
                            Interval = "interval",
                            OpensAt = DateTime.UtcNow,
                            ClosesAt = DateTime.UtcNow.AddMonths(6)
                        }
                    },
                    HolidaySchedules = new List<HolidayScheduleDto>
                    {
                        new HolidayScheduleDto
                        {
                            Closed = false,
                            ClosesAt = DateTime.UtcNow,
                            OpensAt = DateTime.UtcNow,
                            StartDate = DateTime.UtcNow.AddDays(5) ,
                            EndDate = DateTime.UtcNow
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
                    OpensAt = DateTime.UtcNow,
                    ClosesAt = DateTime.UtcNow.AddHours(8),
                    ByDay = "byDay1",
                    ByMonthDay = "byMonth",
                    DtStart = "dtStart",
                    Freq = FrequencyType.NotSet,
                    Interval = "interval",
                    ValidTo = DateTime.UtcNow,
                    ValidFrom = DateTime.UtcNow.AddMonths(6)
                }
            },
            HolidaySchedules = new List<HolidayScheduleDto>
            {
                new HolidayScheduleDto
                {
                    Closed = true,
                    ClosesAt = DateTime.UtcNow,
                    OpensAt = DateTime.UtcNow,
                    StartDate = DateTime.UtcNow.AddDays(5),
                    EndDate = DateTime.UtcNow
                }
            }
        };

        return service;
    }

    public static OrganisationWithServicesDto GetTestCountyCouncilRecord()
    {
        var testCountyCouncil = new OrganisationWithServicesDto
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
}