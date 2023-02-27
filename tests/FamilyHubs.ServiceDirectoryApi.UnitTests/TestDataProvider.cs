using FamilyHubs.ServiceDirectory.Shared.Builders;
using FamilyHubs.ServiceDirectory.Shared.Dto;
using FamilyHubs.ServiceDirectory.Shared.Enums;

namespace FamilyHubs.ServiceDirectoryApi.UnitTests;

public static class TestDataProvider
{
    public static OrganisationWithServicesDto GetTestCountyCouncilDto(
        bool updated = false, bool newGuid = false)
    {
        var testCountyCouncil = new OrganisationWithServicesDto(
            "56e62852-1b0b-40e5-ac97-54a67ea957dc",
            new OrganisationTypeDto("1", "LA", "Local Authority"),
            updated == false ? "Unit Test County Council" : "Unit Test County Council Updated",
            updated == false ? "Unit Test County Council" : "Unit Test County Council Updated",
            null,
            new Uri("https://www.unittest.gov.uk/").ToString(),
            "https://www.unittest.gov.uk/",
            new List<ServiceDto>
            {
                GetTestCountyCouncilServicesDto("56e62852-1b0b-40e5-ac97-54a67ea957dc", updated, newGuid)
            }
        )
        {
            AdminAreaCode = "XTEST"
        };

        return testCountyCouncil;
    }

    public static OrganisationWithServicesDto GetTestCountyCouncilDto2()
    {
        var testCountyCouncil = new OrganisationWithServicesDto(
            "26f10023-7570-4bcd-b9ed-70f51ad43f62",
            new OrganisationTypeDto("1", "LA", "Local Authority"),
            "Unit Test County Council 2",
            "Unit Test County Council 2",
            null,
            new Uri("https://www.unittest2.gov.uk/").ToString(),
            "https://www.unittest2.gov.uk/",
            new List<ServiceDto>
            {
                GetTestCountyCouncilServicesDto2("26f10023-7570-4bcd-b9ed-70f51ad43f62")
            }
        )
        {
            AdminAreaCode = "XTEST"
        };

        return testCountyCouncil;
    }

    public static ServiceDto GetTestCountyCouncilServicesDto(string parentId, bool updated = false, bool newGuid = false)
    {
        var contactId = Guid.NewGuid().ToString();

        var builder = new ServicesDtoBuilder();
        var service = builder.WithMainProperties("3010521b-6e0a-41b0-b610-200edbbeeb14",
                new ServiceTypeDto("1", "Information Sharing", ""),
                parentId,
                updated == false ? "Unit Test Service" : "Unit Test Service Updated",
                @"Unit Test Service Description",
                "accreditations",
                DateTime.UtcNow,
                "attending access",
                "attending type",
                "delivery type",
                "active",
                null,
                false)
            .WithServiceDelivery(new List<ServiceDeliveryDto>
            {
                new ServiceDeliveryDto(newGuid == false ? "77cc3815-6b95-4618-ab27-ac9f35c44614" : Guid.NewGuid().ToString(),updated == false ? ServiceDeliveryType.Online : ServiceDeliveryType.Telephone)
            })
            .WithEligibility(new List<EligibilityDto>
            {
                new EligibilityDto(newGuid == false ? "Test9111Children" : Guid.NewGuid().ToString(),"",updated == false ? 0 : 1,updated == false ? 13 : 14)
            })
            .WithLinkContact(new List<LinkContactDto>
            {
                new LinkContactDto(
                    "3010521b-6e0a-41b0-b610-200edbbeeb11",
                    "3010521b-6e0a-41b0-b610-200edbbeeb14",
                    "Service",
                    new ContactDto(
                        newGuid == false ? contactId : Guid.NewGuid().ToString(),
                        updated == false ? "Contact" : "Updated Contact",
                        string.Empty,
                        "01827 65777",
                        "01827 65777",
                        "www.unittestservice.com",
                        "support@unittestservice.com"
                    ))
            })
            .WithCostOption(new List<CostOptionDto> {  new CostOptionDto(
                newGuid == false ? "22001144-26d5-4dcc-a6a5-62ce2ce98cc0" : Guid.NewGuid().ToString(),
                updated == false ? "amount_description1" : "amount_description2",
                decimal.Zero,
                default!,
                "free",
                DateTime.UtcNow,
                DateTime.UtcNow.AddHours(8))
            })
            .WithLanguages(new List<LanguageDto>
            {
                new LanguageDto(newGuid == false ? "1bb6c313-648d-4226-9e96-b7d37eaeb312" : Guid.NewGuid().ToString(), updated == false ? "English"  : "French")
            })
            .WithServiceAreas(new List<ServiceAreaDto>
            {
                new ServiceAreaDto(newGuid == false ? "a302aea4-fe0c-4ccc-9178-bea39f3edc30" : Guid.NewGuid().ToString(), updated == false ? "National" : "Local", null,"http://statistics.data.gov.uk/id/statistical-geography/K02000001")
            })
            .WithServiceAtLocations(new List<ServiceAtLocationDto>
            {
                new ServiceAtLocationDto(
                    "Test1749",
                    new LocationDto(
                        newGuid == false ? "6ea31a4f-7dcc-4350-9fba-20525efe092f" : Guid.NewGuid().ToString(),
                        updated == false ? "Test Location" : "Test New Location",
                        "",
                        52.6312,
                        -1.66526,
                        new List<PhysicalAddressDto>
                        {
                            new PhysicalAddressDto(
                                newGuid == false ? "c576191d-9f14-4963-885e-2889a7a2b48f" : Guid.NewGuid().ToString(),
                                updated == false ? "77 Sheepcote Lane" : "78 Sheepcote Lane",
                                ", Stathe, Tamworth, Staffordshire, ",
                                "B77 3JN",
                                "England",
                                null
                            )
                        },
                        new List<LinkTaxonomyDto>
                        {
                            new LinkTaxonomyDto(
                                "d53b3524-bd3e-443c-ae14-69482afc7d2a",
                                "Location",
                                "6ea31a4f-7dcc-4350-9fba-20525efe092f",
                                new TaxonomyDto(
                                    newGuid == false ? "b60b7f3e-9ff4-48b2-bded-b00272ed3aba" : Guid.NewGuid().ToString(),
                                    updated == false ? "Family_hub 1" : "Family_hub 2",
                                    TaxonomyType.LocationType,
                                    null
                                )
                            )
                        },
                        new List<LinkContactDto>
                        {
                            new LinkContactDto(
                                "3010521b-6e0a-41b0-b610-200edbbeeb33",
                                newGuid == false ? "6ea31a4f-7dcc-4350-9fba-20525efe092f" : Guid.NewGuid().ToString(),
                                "Service",
                                new ContactDto(
                                    Guid.NewGuid().ToString(),
                                    updated == false ? "Contact" : "Updated Contact",
                                    string.Empty,
                                    "01827 65777",
                                    "01827 65777",
                                    "www.unittestservice.com",
                                    "support@unittestservice.com"
                                ))
                        }
                    ),
                    new List<RegularScheduleDto>
                    {
                        new RegularScheduleDto(
                            newGuid == false ? "5e5ba093-a5f9-49ce-826c-52851e626288" : Guid.NewGuid().ToString(),
                            "Description",
                            DateTime.UtcNow,
                            DateTime.UtcNow.AddHours(8),
                            updated == false ?  "byDay1" : "byDay2",
                            "byMonth",
                            "dtStart",
                            "freq",
                            "interval",
                            DateTime.UtcNow,
                            DateTime.UtcNow.AddMonths(6)
                        )
                    },
                    new List<HolidayScheduleDto>
                    {
                        new HolidayScheduleDto(
                            newGuid == false ?  "bc946512-7f8c-4c54-b7ed-ad8fefde7b48" : Guid.NewGuid().ToString(),
                            updated == false ? false : true,
                            DateTime.UtcNow,
                            DateTime.UtcNow,
                            DateTime.UtcNow.AddDays(5) ,
                            DateTime.UtcNow
                        )
                    },
                    new List<LinkContactDto>
                    {
                        new LinkContactDto(
                            "Test17491234",
                            "Test1749",
                            "ServiceAtLocation",
                            new ContactDto(
                                Guid.NewGuid().ToString(),
                                updated == false ? "Contact" : "Updated Contact",
                                string.Empty,
                                "01827 65777",
                                "01827 65777",
                                "www.unittestservice.com",
                                "support@unittestservice.com"
                            ))
                    }
                )

            })
            .WithServiceTaxonomies(new List<ServiceTaxonomyDto>
            {
                new ServiceTaxonomyDto(
                    "UnitTest9107",
                    new TaxonomyDto(
                        "UnitTest bccsource:Organisation",
                        updated == false ? "Organisation" : "Organisation Updated",
                        TaxonomyType.ServiceCategory,
                        null
                    )),

                new ServiceTaxonomyDto(
                    "UnitTest9108",
                    new TaxonomyDto(
                        "UnitTest bccprimaryservicetype:38",
                        updated == false ? "Support" : "Support Updated",
                        TaxonomyType.ServiceCategory,
                        null
                    )),

                new ServiceTaxonomyDto(
                    "UnitTest9109",
                    new TaxonomyDto(
                        "UnitTest bccagegroup:37",
                        "Children",
                        TaxonomyType.ServiceCategory,
                        null
                    )),

                new ServiceTaxonomyDto(
                    "UnitTest9110",
                    new TaxonomyDto(
                        "UnitTestbccusergroup:56",
                        "Long Term Health Conditions",
                        TaxonomyType.ServiceCategory,
                        null
                    ))
            })
            .WithFundings(new List<FundingDto>())
            .WithRegularSchedules(new List<RegularScheduleDto>
            {
                new RegularScheduleDto(
                    Guid.NewGuid().ToString(),
                    "Description",
                    DateTime.UtcNow,
                    DateTime.UtcNow.AddHours(8),
                    "byDay1",
                    "byMonth",
                    "dtStart",
                    "freq",
                    "interval",
                    DateTime.UtcNow,
                    DateTime.UtcNow.AddMonths(6)
                )
            })
            .Build();

        service.HolidaySchedules = new List<HolidayScheduleDto>();

        return service;
    }

    public static ServiceDto GetTestCountyCouncilServicesDto2(string parentId)
    {
        var contactId = Guid.NewGuid().ToString();

        var builder = new ServicesDtoBuilder();
        var service = builder.WithMainProperties("5059a0b2-ad5d-4288-b7c1-e30d35345b0e",
                new ServiceTypeDto("1", "Information Sharing", ""),
                parentId,
                "Unit Test Service",
                @"Unit Test Service Description",
                "accreditations",
                DateTime.Now,
                "attending access",
                "attending type",
                "delivery type",
                "active",
                null,
                false)
            .WithServiceDelivery(new List<ServiceDeliveryDto>
            {
                new ServiceDeliveryDto(Guid.NewGuid().ToString(),ServiceDeliveryType.Online)
            })
            .WithEligibility(new List<EligibilityDto>
            {
                new EligibilityDto("Test9112Children","Test9112Children",0,13)
            })
            .WithLinkContact(new List<LinkContactDto>
            {
                new LinkContactDto(
                    "5059a0b2-ad5d-4288-b7c1-e30d35345bab",
                    "5059a0b2-ad5d-4288-b7c1-e30d35345b0e",
                    "Service",
                    new ContactDto(
                        contactId,
                        "Contact",
                        string.Empty,
                        "01827 65777",
                        "01827 65777",
                        "www.unittestservice.com",
                        "support@unittestservice.com"
                    ))
            })
            .WithCostOption(new List<CostOptionDto>
            {
                new CostOptionDto
                {
                    Id = Guid.NewGuid().ToString(),
                    Amount = decimal.Zero,
                    Option = "free",
                    AmountDescription = ""
                }
            })
            .WithLanguages(new List<LanguageDto>
            {
                new LanguageDto("1bb6c313-648d-4226-9e96-b7d37eaeb3ab", "English")
            })
            .WithServiceAreas(new List<ServiceAreaDto>
            {
                new ServiceAreaDto(Guid.NewGuid().ToString(), "National", null,"http://statistics.data.gov.uk/id/statistical-geography/K02000001")
            })
            .WithServiceAtLocations(new List<ServiceAtLocationDto>
            {
                new ServiceAtLocationDto(
                    "Test1750",
                    new LocationDto(
                        "6ea31a4f-7dcc-4350-9fba-20525efe091a",
                        "Test Location",
                        "",
                        52.6312,
                        -1.66526,
                        new List<PhysicalAddressDto>
                        {
                            new PhysicalAddressDto(
                                Guid.NewGuid().ToString(),
                                "Some Lane",
                                ", Stathe, Tamworth, Staffordshire, ",
                                "B77 4JN",
                                "England",
                                null
                            )
                        },
                        new List<LinkTaxonomyDto>
                        {
                            new LinkTaxonomyDto(
                                Guid.NewGuid().ToString(),
                                "Location",
                                "6ea31a4f-7dcc-4350-9fba-20525efe092f",
                                new TaxonomyDto(
                                    //todo: real guid

                                    Guid.NewGuid().ToString(),
                                    "Family_hub",
                                    TaxonomyType.LocationType,
                                    null
                                )
                            )
                        },
                        new List<LinkContactDto>()),
                    new List<RegularScheduleDto>
                    {
                        new RegularScheduleDto(
                            "67806edd-8427-4126-8ec1-06d59c2209ae",
                            "Description",
                            DateTime.UtcNow,
                            DateTime.UtcNow.AddHours(8),
                            "byDay",
                            "byMonth",
                            "dtStart",
                            "freq",
                            "interval",
                            DateTime.UtcNow,
                            DateTime.UtcNow.AddMonths(6)
                        )
                    },
                    new List<HolidayScheduleDto>
                    {
                        new HolidayScheduleDto(
                            "60ef490f-1e6f-4a1b-bf96-337768e578cf",
                            false,
                            DateTime.UtcNow,
                            DateTime.UtcNow,
                            DateTime.UtcNow.AddDays(5) ,
                            DateTime.UtcNow
                        )
                    },
                    new List<LinkContactDto>
                    {
                        new LinkContactDto(
                            "Test17500978",
                            "Test17500123",
                            "ServiceAtLocation",
                            new ContactDto(
                                Guid.NewGuid().ToString(),
                                "Contact",
                                string.Empty,
                                "01827 65777",
                                "01827 65777",
                                "www.unittestservice.com",
                                "support@unittestservice.com"
                            ))
                    }
                )

            })
            .WithServiceTaxonomies(new List<ServiceTaxonomyDto>
            {
                new ServiceTaxonomyDto(
                    "UnitTest9107B",
                    new TaxonomyDto(
                        "UnitTest1",
                        "Organisation",
                        TaxonomyType.ServiceCategory,
                        null
                    )),

                new ServiceTaxonomyDto(
                    "UnitTest9108B",
                    new TaxonomyDto(
                        "UnitTest2",
                        "Support",
                        TaxonomyType.ServiceCategory,
                        null
                    )),

                new ServiceTaxonomyDto(
                    "UnitTest9109B",
                    new TaxonomyDto(
                        "UnitTest3",
                        "Children",
                        TaxonomyType.ServiceCategory,
                        null
                    )),

                new ServiceTaxonomyDto(
                    "UnitTest9110B",
                    new TaxonomyDto(
                        "UnitTest4",
                        "Long Term Health Conditions",
                        TaxonomyType.ServiceCategory,
                        null
                    ))
            })
            .WithRegularSchedules(new List<RegularScheduleDto>
                {
                    new RegularScheduleDto(
                        Guid.NewGuid().ToString(),
                        "Description",
                        DateTime.UtcNow,
                        DateTime.UtcNow.AddHours(8),
                        "byDay1",
                        "byMonth",
                        "dtStart",
                        "freq",
                        "interval",
                        DateTime.UtcNow,
                        DateTime.UtcNow.AddMonths(6)
                    )
                })
            .WithHolidaySchedules(new List<HolidayScheduleDto>
            {
                new HolidayScheduleDto(
                    Guid.NewGuid().ToString(),
                    true,
                    DateTime.UtcNow,
                    DateTime.UtcNow,
                    DateTime.UtcNow.AddDays(5),
                    DateTime.UtcNow
                )
            })
            .Build();

        return service;
    }

    public static OrganisationWithServicesDto GetTestCountyCouncilRecord()
    {
        var testCountyCouncil = new OrganisationWithServicesDto(
            "56e62852-1b0b-40e5-ac97-54a67ea957dc",
            new OrganisationTypeDto("1", "LA", "Local Authority"),
            "Unit Test A County Council",
            "Unit Test A County Council",
            null,
            new Uri("https://www.unittesta.gov.uk/").ToString(),
            "https://www.unittesta.gov.uk/",
            new List<ServiceDto>
            {
                //GetTestCountyCouncilServicesRecord()
            }
        )
        {
            AdminAreaCode = "XTEST"
        };

        return testCountyCouncil;
    }
}