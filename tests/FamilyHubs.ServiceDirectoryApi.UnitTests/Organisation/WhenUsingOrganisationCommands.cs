using Ardalis.GuardClauses;
using AutoMapper;
using FamilyHubs.ServiceDirectory.Api.Commands.CreateOrganisation;
using FamilyHubs.ServiceDirectory.Api.Commands.UpdateOrganisation;
using FamilyHubs.ServiceDirectory.Api.Queries.GetOrganisationAdminByOrganisationId;
using FamilyHubs.ServiceDirectory.Api.Queries.GetOrganisationById;
using FamilyHubs.ServiceDirectory.Api.Queries.GetOrganisationTypes;
using FamilyHubs.ServiceDirectory.Api.Queries.ListOrganisation;
using FamilyHubs.ServiceDirectory.Core;
using FamilyHubs.ServiceDirectory.Core.Entities;
using FamilyHubs.ServiceDirectory.Infrastructure.Persistence.Repository;
using FamilyHubs.ServiceDirectory.Shared.Builders;
using FamilyHubs.ServiceDirectory.Shared.Dto;
using FamilyHubs.ServiceDirectory.Shared.Enums;
using FamilyHubs.ServiceDirectoryApi.UnitTests.Builders;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.ObjectModel;

namespace FamilyHubs.ServiceDirectoryApi.UnitTests.Organisation;

public class WhenUsingOrganisationCommands : BaseCreateDbUnitTest
{
    [Fact]
    public async Task ThenCreateOrganisation()
    {
        //Arange
        var myProfile = new AutoMappingProfiles();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
        IMapper mapper = new Mapper(configuration);
        var logger = new Mock<ILogger<CreateOrganisationCommandHandler>>();
        var mockApplicationDbContext = GetApplicationDbContext();
        var testOrganisation = GetTestCountyCouncilDto();
        CreateOrganisationCommand command = new(testOrganisation);
        CreateOrganisationCommandHandler handler = new(mockApplicationDbContext, mapper, logger.Object);

        //Act
        var result = await handler.Handle(command, new CancellationToken());

        //Assert
        result.Should().NotBeNull();
        result.Should().Be(testOrganisation.Id);
    }

    [Fact]
    public async Task ThenCreateRelatedOrganisation()
    {
        //Arange
        var myProfile = new AutoMappingProfiles();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
        IMapper mapper = new Mapper(configuration);
        var logger = new Mock<ILogger<CreateOrganisationCommandHandler>>();
        var mockApplicationDbContext = GetApplicationDbContext();
        var testCountyCouncil = GetTestCountyCouncilDto();
        CreateOrganisationCommand createOrganisationCommand = new(testCountyCouncil);
        CreateOrganisationCommandHandler handler = new(mockApplicationDbContext, mapper, logger.Object);
        var createResult = await handler.Handle(createOrganisationCommand, new CancellationToken());

        var testOrgaisation = new OrganisationWithServicesDto(
            "e0dc6a0c-2f9c-48c6-a222-1232abbf9000",
            new OrganisationTypeDto("2", "VCFS", "Voluntary, Charitable, Faith Sector"),
            "Related VCS",
            "Related VCS",
            null,
            new Uri("https://www.relatedvcs.gov.uk/").ToString(),
            "https://www.related.gov.uk/",
            new List<ServiceDto>
            {
                 
            }
            );

        testOrgaisation.AdminAreaCode = "XTEST";

        CreateOrganisationCommand command = new(testOrgaisation);
        

        //Act
        var result = await handler.Handle(command, new CancellationToken());

        //Assert
        result.Should().NotBeNull();
        result.Should().Be(testOrgaisation.Id);
    }

    [Fact]
    public async Task ThenCreateAnotherOrganisation()
    {
        //Arange
        var myProfile = new AutoMappingProfiles();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
        IMapper mapper = new Mapper(configuration);
        var logger = new Mock<ILogger<CreateOrganisationCommandHandler>>();
        var mockApplicationDbContext = GetApplicationDbContext();
        CreateOrganisationCommand initialcommand = new(GetTestCountyCouncilDto());
        CreateOrganisationCommandHandler initialhandler = new(mockApplicationDbContext, mapper, logger.Object);
        await initialhandler.Handle(initialcommand, new CancellationToken());

        var testOrganisation = GetTestCountyCouncilDto2();
        CreateOrganisationCommand command = new(GetTestCountyCouncilDto2());
        CreateOrganisationCommandHandler handler = new(mockApplicationDbContext, mapper, logger.Object);

        //Act
        var result = await handler.Handle(command, new CancellationToken());

        //Assert
        result.Should().NotBeNull();
        result.Should().Be(testOrganisation.Id);
    }

    [Fact]
    public async Task ThenCreateDuplicateOrganisation_ShouldThrowException()
    {
        //Arange
        var myProfile = new AutoMappingProfiles();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
        IMapper mapper = new Mapper(configuration);
        var logger = new Mock<ILogger<CreateOrganisationCommandHandler>>();
        var mockApplicationDbContext = GetApplicationDbContext();
        var testOrganisation = GetTestCountyCouncilDto();
        CreateOrganisationCommand initialcommand = new(testOrganisation);
        CreateOrganisationCommandHandler initialhandler = new(mockApplicationDbContext, mapper, logger.Object);
        await initialhandler.Handle(initialcommand, new CancellationToken());

        
        CreateOrganisationCommand command = new(testOrganisation);
        CreateOrganisationCommandHandler handler = new(mockApplicationDbContext, mapper, logger.Object);

        // Act 
        Func<Task> act = () => handler.Handle(command, new CancellationToken());


        // Assert
        var exception = await Assert.ThrowsAsync<Exception>(act);
        await act.Should().ThrowAsync<System.Exception>().WithMessage("Duplicate Id");

    }

    [Fact]
    public async Task ThenUpdateOrganisation()
    {
        //Arange
        var myProfile = new AutoMappingProfiles();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
        IMapper mapper = new Mapper(configuration);
        var logger = new Mock<ILogger<CreateOrganisationCommandHandler>>();
        var mockApplicationDbContext = GetApplicationDbContext();
        var testOrganisation = GetTestCountyCouncilDto();
        var updatelogger = new Mock<ILogger<UpdateOrganisationCommandHandler>>();
        var mockMediator = new Mock<ISender>();
        CreateOrganisationCommand command = new(testOrganisation);
        CreateOrganisationCommandHandler handler = new(mockApplicationDbContext, mapper, logger.Object);
        var id = await handler.Handle(command, new CancellationToken());


        var updateTestOrganisation = GetTestCountyCouncilDto(true);
        

        UpdateOrganisationCommand updatecommand = new(updateTestOrganisation.Id, updateTestOrganisation);
        UpdateOrganisationCommandHandler updatehandler = new(mockApplicationDbContext, updatelogger.Object, mockMediator.Object, mapper);

        //Act
        var result = await updatehandler.Handle(updatecommand, new CancellationToken());

        //Assert
        result.Should().NotBeNull();
        result.Should().Be(testOrganisation.Id);
    }

    [Fact]
    public async Task ThenUpdateOrganisationWithNewService()
    {
        //Arange
        var myProfile = new AutoMappingProfiles();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
        IMapper mapper = new Mapper(configuration);
        var logger = new Mock<ILogger<CreateOrganisationCommandHandler>>();
        var mockApplicationDbContext = GetApplicationDbContext();
        var testOrganisation = GetTestCountyCouncilDto();
        testOrganisation.Services = default!;
        var updatelogger = new Mock<ILogger<UpdateOrganisationCommandHandler>>();
        var mockMediator = new Mock<ISender>();
        CreateOrganisationCommand command = new(testOrganisation);
        CreateOrganisationCommandHandler handler = new(mockApplicationDbContext, mapper, logger.Object);
        var id = await handler.Handle(command, new CancellationToken());

        var updateTestOrganisation = GetTestCountyCouncilDto();
        updateTestOrganisation.Services = new List<ServiceDto>
        {
             GetTestCountyCouncilServicesDto2("56e62852-1b0b-40e5-ac97-54a67ea957dc")
        };

        UpdateOrganisationCommand updatecommand = new(updateTestOrganisation.Id, updateTestOrganisation);
        UpdateOrganisationCommandHandler updatehandler = new(mockApplicationDbContext, updatelogger.Object, mockMediator.Object, mapper);

        //Act
        var result = await updatehandler.Handle(updatecommand, new CancellationToken());

        //Assert
        result.Should().NotBeNull();
        result.Should().Be(testOrganisation.Id);
    }

    [Fact]
    public async Task ThenGetOrganisationById()
    {
        //Arange
        var myProfile = new AutoMappingProfiles();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
        IMapper mapper = new Mapper(configuration);
        var logger = new Mock<ILogger<CreateOrganisationCommandHandler>>();
        var mockApplicationDbContext = GetApplicationDbContext();
        var testOrganisation = GetTestCountyCouncilDto();
        CreateOrganisationCommand command = new(testOrganisation);
        CreateOrganisationCommandHandler handler = new(mockApplicationDbContext, mapper, logger.Object);
        var id = await handler.Handle(command, new CancellationToken());

        GetOrganisationByIdCommand getcommand = new() { Id = testOrganisation.Id };
        GetOrganisationByIdHandler gethandler = new(mockApplicationDbContext);
        testOrganisation.Logo = "";

        //Act
        var result = await gethandler.Handle(getcommand, new CancellationToken());

        //Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(testOrganisation, opts => opts.Excluding(si => si.AdminAreaCode));
    }

    [Fact]
    public async Task ThenGetOrganisationById_ShouldThrowExceptionWhenIdDoesNotExist()
    {
        //Arange
        var myProfile = new AutoMappingProfiles();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
        IMapper mapper = new Mapper(configuration);
        var logger = new Mock<ILogger<CreateOrganisationCommandHandler>>();
        var mockApplicationDbContext = GetApplicationDbContext();
        GetOrganisationByIdCommand getcommand = new() { Id = Guid.NewGuid().ToString() };
        GetOrganisationByIdHandler gethandler = new(mockApplicationDbContext);
        

        // Act 
        Func<Task> act = () => gethandler.Handle(getcommand, new CancellationToken());


        // Assert
        var exception = await Assert.ThrowsAsync<NotFoundException>(act);

    }

    [Fact]
    public async Task ThenListOrganisations()
    {
        //Arange
        var myProfile = new AutoMappingProfiles();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
        IMapper mapper = new Mapper(configuration);
        var logger = new Mock<ILogger<CreateOrganisationCommandHandler>>();
        var mockApplicationDbContext = GetApplicationDbContext();
        var testOrganisation = GetTestCountyCouncilDto();
        CreateOrganisationCommand command = new(testOrganisation);
        CreateOrganisationCommandHandler handler = new(mockApplicationDbContext, mapper, logger.Object);
        var id = await handler.Handle(command, new CancellationToken());

        ListOrganisationCommand getcommand = new();
        ListOrganisationCommandHandler gethandler = new(mockApplicationDbContext);
        testOrganisation.Logo = "";

        //Act
        var result = await gethandler.Handle(getcommand, new CancellationToken());

        //Assert
        result.Should().NotBeNull();
        result[0].Should().BeEquivalentTo(testOrganisation, opts => opts.Excluding(si => si.Services).Excluding(si => si.AdminAreaCode));
    }

    [Fact]
    public async Task ThenListOrganisationTypes()
    {
        //Arange
        var myProfile = new AutoMappingProfiles();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
        IMapper mapper = new Mapper(configuration);
        var logger = new Mock<ILogger<CreateOrganisationCommandHandler>>();
        var mockApplicationDbContext = GetApplicationDbContext();
        var seedData = new OrganisationSeedData(false);
        if (!mockApplicationDbContext.AdminAreas.Any())
        {
            mockApplicationDbContext.OrganisationTypes.AddRange(seedData.SeedOrganisationTypes());
            await mockApplicationDbContext.SaveChangesAsync();
        }

        GetOrganisationTypesCommand getcommand = new();
        GetOrganisationTypesCommandHandler gethandler = new(mockApplicationDbContext);
        

        //Act
        var result = await gethandler.Handle(getcommand, new CancellationToken());

        //Assert
        result.Should().NotBeNull();
        result.Count.Should().Be(3);
    }

    [Fact]
    public async Task ThenGetAdminByOrganisationId()
    {
        //Arange
        var myProfile = new AutoMappingProfiles();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
        IMapper mapper = new Mapper(configuration);
        var logger = new Mock<ILogger<CreateOrganisationCommandHandler>>();
        var mockApplicationDbContext = GetApplicationDbContext();
        var testOrganisation = GetTestCountyCouncilDto();
        CreateOrganisationCommand command = new(testOrganisation);
        CreateOrganisationCommandHandler handler = new(mockApplicationDbContext, mapper, logger.Object);
        var id = await handler.Handle(command, new CancellationToken());

        GetOrganisationAdminByOrganisationIdCommand getcommand = new(testOrganisation.Id);
        GetOrganisationAdminByOrganisationIdCommandHandler gethandler = new(mockApplicationDbContext);
        testOrganisation.Logo = "";

        //Act
        var result = await gethandler.Handle(getcommand, new CancellationToken());

        //Assert
        result.Should().NotBeNull();
        result.Should().Be("XTEST");
    }

    public static OrganisationWithServicesDto GetTestCountyCouncilDto(bool updated = false, bool newGuid = false)
    {
        var testCountyCouncil = new OrganisationWithServicesDto(
            "56e62852-1b0b-40e5-ac97-54a67ea957dc",
            new OrganisationTypeDto("1", "LA", "Local Authority"),
            (updated == false) ? "Unit Test County Council" : "Unit Test County Council Updated",
            (updated == false) ? "Unit Test County Council" : "Unit Test County Council Updated",
            null,
            new Uri("https://www.unittest.gov.uk/").ToString(),
            "https://www.unittest.gov.uk/",
            new List<ServiceDto>
            {
                 GetTestCountyCouncilServicesDto("56e62852-1b0b-40e5-ac97-54a67ea957dc", updated, newGuid)
            }
            );

        testCountyCouncil.AdminAreaCode = "XTEST";

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
            );

        testCountyCouncil.AdminAreaCode = "XTEST";

        return testCountyCouncil;
    }

    public static ServiceDto GetTestCountyCouncilServicesDto(string parentId, bool updated = false, bool newGuid = false)
    {
        var contactId = Guid.NewGuid().ToString();

        var builder = new ServicesDtoBuilder();
        var service = builder.WithMainProperties("3010521b-6e0a-41b0-b610-200edbbeeb14",
                new ServiceTypeDto("1", "Information Sharing", ""),
                parentId,
                (updated == false) ? "Unit Test Service" : "Unit Test Service Updated",
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
                    new ServiceDeliveryDto((newGuid == false) ? "77cc3815-6b95-4618-ab27-ac9f35c44614" : Guid.NewGuid().ToString(),(updated == false) ? ServiceDeliveryType.Online : ServiceDeliveryType.Telephone)
                })
            .WithEligibility(new List<EligibilityDto>
                {
                    new EligibilityDto((newGuid == false) ? "Test9111Children" : Guid.NewGuid().ToString(),"",(updated == false) ? 0 : 1,(updated == false) ? 13 : 14)
                })
            .WithLinkContact(new List<LinkContactDto>
            {
                new LinkContactDto(
                    "3010521b-6e0a-41b0-b610-200edbbeeb11",
                    "Service",
                    "3010521b-6e0a-41b0-b610-200edbbeeb14",
                new ContactDto(
                    (newGuid == false) ? contactId : Guid.NewGuid().ToString(),
                    (updated == false) ? "Contact" : "Updated Contact",
                    string.Empty,
                    "01827 65777",
                    "01827 65777",
                    "www.unittestservice.com",
                    "support@unittestservice.com"
                ))
            })
            .WithCostOption(new List<CostOptionDto> {  new(
                    (newGuid == false) ? "22001144-26d5-4dcc-a6a5-62ce2ce98cc0" : Guid.NewGuid().ToString(),
                    (updated == false) ? "amount_description1" : "amount_description2",
                    decimal.Zero,
                    default!,
                    "free",
                    DateTime.UtcNow,
                    DateTime.UtcNow.AddHours(8))
            })
            .WithLanguages(new List<LanguageDto>
            {
                    new LanguageDto((newGuid == false) ? "1bb6c313-648d-4226-9e96-b7d37eaeb3dd" : Guid.NewGuid().ToString(), (updated == false) ? "English"  : "French")
            })
            .WithServiceAreas(new List<ServiceAreaDto>
            {
                    new ServiceAreaDto((newGuid == false) ? "a302aea4-fe0c-4ccc-9178-bea39f3edc30" : Guid.NewGuid().ToString(), (updated == false) ? "National" : "Local", null,"http://statistics.data.gov.uk/id/statistical-geography/K02000001")
                })
            .WithServiceAtLocations(new List<ServiceAtLocationDto>
            {
                    new ServiceAtLocationDto(
                        "Test1749",
                        new LocationDto(
                            (newGuid == false) ? "6ea31a4f-7dcc-4350-9fba-20525efe092f" : Guid.NewGuid().ToString(),
                            (updated == false) ? "Test Location" : "Test New Location",
                            "",
                            52.6312,
                            -1.66526,
                            new List<PhysicalAddressDto>
                            {
                                new PhysicalAddressDto(
                                    (newGuid == false) ? "c576191d-9f14-4963-885e-2889a7a2b48f" : Guid.NewGuid().ToString(),
                                    (updated == false) ? "77 Sheepcote Lane" : "78 Sheepcote Lane",
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
                                        (newGuid == false) ? "b60b7f3e-9ff4-48b2-bded-b00272ed3aba" : Guid.NewGuid().ToString(),
                                        (updated == false) ? "Family_hub 1" : "Family_hub 2",
                                        null,
                                        null
                                    )
                                )
                            },
                            new List<LinkContactDto>()
                            ),
                        new List<RegularScheduleDto>()
                        {
                            new RegularScheduleDto(
                                (newGuid == false) ? "5e5ba093-a5f9-49ce-826c-52851e626288" : Guid.NewGuid().ToString(), 
                                "Description",
                                DateTime.UtcNow,
                                DateTime.UtcNow.AddHours(8),
                                (updated == false) ?  "byDay1" : "byDay2", 
                                "byMonth",
                                "dtStart",
                                "freq",
                                "interval",
                                DateTime.UtcNow,
                                DateTime.UtcNow.AddMonths(6)
                                )
                        },
                        new List<HolidayScheduleDto>()
                        {
                            new HolidayScheduleDto(
                                (newGuid == false) ?  "bc946512-7f8c-4c54-b7ed-ad8fefde7b48" : Guid.NewGuid().ToString(),
                                (updated == false) ? false : true,
                                DateTime.UtcNow,
                                DateTime.UtcNow,
                                DateTime.UtcNow.AddDays(5) ,
                                DateTime.UtcNow 
                                )
                        },
                        new List<LinkContactDto>()
                        )

                })
            .WithServiceTaxonomies(new List<ServiceTaxonomyDto>
            {
                    new ServiceTaxonomyDto
                    ("UnitTest9107",
                    new TaxonomyDto(
                        "UnitTest bccsource:Organisation",
                        (updated == false) ? "Organisation" : "Organisation Updated",
                        "Test BCC Data Sources",
                        null
                        )),

                    new ServiceTaxonomyDto
                    ("UnitTest9108",
                    new TaxonomyDto(
                        "UnitTest bccprimaryservicetype:38",
                        (updated == false) ? "Support" : "Support Updated",
                        "Test BCC Primary Services",
                        null
                        )),

                    new ServiceTaxonomyDto
                    ("UnitTest9109",
                    new TaxonomyDto(
                        "UnitTest bccagegroup:37",
                        "Children",
                        "Test BCC Age Groups",
                        null
                        )),

                    new ServiceTaxonomyDto
                    ("UnitTest9110",
                    new TaxonomyDto(
                        "UnitTestbccusergroup:56",
                        "Long Term Health Conditions",
                        "Test BCC User Groups",
                        null
                        ))
                })
            .Build();

        service.RegularSchedules = new List<RegularScheduleDto>();
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
                    new EligibilityDto("Test9111Children","Test9111Children",0,13)
                })
            .WithLinkContact(new List<LinkContactDto>
            {
                new LinkContactDto(
                    "3010521b-6e0a-41b0-b610-200edbbeeb14",
                    "3010521b-6e0a-41b0-b610-200edbbeeb11",
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
            .WithCostOption(new List<CostOptionDto> {new() {Id = Guid.NewGuid().ToString(), Amount = decimal.Zero, Option = "free", AmountDescription = ""}})
            .WithLanguages(new List<LanguageDto>
            {
                    new LanguageDto("1bb6c313-648d-4226-9e96-b7d37eaeb3dd", "English")
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
                            "6ea31a4f-7dcc-4350-9fba-20525efe092f",
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
                                        null,
                                        null
                                    )
                                )
                            },
                            new List<LinkContactDto>()),
                        new List<RegularScheduleDto>()
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
                        new List<HolidayScheduleDto>()
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
                        new List<LinkContactDto>()
                        )

                })
            .WithServiceTaxonomies(new List<ServiceTaxonomyDto>
            {
                    new ServiceTaxonomyDto
                    ("UnitTest9107B",
                    new TaxonomyDto(
                        "UnitTest bccsource:Organisation",
                        "Organisation",
                        "Test BCC Data Sources",
                        null
                        )),

                    new ServiceTaxonomyDto
                    ("UnitTest9108B",
                    new TaxonomyDto(
                        "UnitTest bccprimaryservicetype:38",
                        "Support",
                        "Test BCC Primary Services",
                        null
                        )),

                    new ServiceTaxonomyDto
                    ("UnitTest9109B",
                    new TaxonomyDto(
                        "UnitTest bccagegroup:37",
                        "Children",
                        "Test BCC Age Groups",
                        null
                        )),

                    new ServiceTaxonomyDto
                    ("UnitTest9110B",
                    new TaxonomyDto(
                        "UnitTestbccusergroup:56",
                        "Long Term Health Conditions",
                        "Test BCC User Groups",
                        null
                        ))
                })
            .Build();

        service.RegularSchedules = new List<RegularScheduleDto>();
        service.HolidaySchedules = new List<HolidayScheduleDto>();

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
            //new List<ReviewDto>(),
            new List<ServiceDto>
            {
                 //GetTestCountyCouncilServicesRecord()
            }
            );

        testCountyCouncil.AdminAreaCode = "XTEST";

        return testCountyCouncil;
    }

    private static Service GetTestCountyCouncilServicesRecord()
    {
        var contactId = Guid.NewGuid().ToString();

        var builder = new ServicesBuilder();
        var service = builder.WithMainProperties("3010521b-6e0a-41b0-b610-200edbbeeb14",
                "Unit Test Organisation for Children with Tracheostomies",
                @"Unit Test Organisation for for Children with Tracheostomies is a national self help group operating as a registered charity and is run by parents of children with a tracheostomy and by people who sympathise with the needs of such families. ACT as an organisation is non profit making, it links groups and individual members throughout Great Britain and Northern Ireland.",
                null,
                null,
                null,
                null,
                null,
                "active",
                null)
            .WithServiceDelivery(new List<ServiceDelivery>
                {
                    new ServiceDelivery(Guid.NewGuid().ToString(),ServiceDeliveryType.Online)
                })
            .WithEligibility(new List<Eligibility>
                {
                    new Eligibility("Test9120Children","eligability","",0,13, new Collection<Taxonomy>())
                })
            .WithContact(new List<LinkContact>
            {
                new LinkContact(
                    "3010521b-6e0a-41b0-b610-200edbbeeb14",
                    "3010521b-6e0a-41b0-b610-200edbbeeb11",
                    "Service",
                new Contact(
                    contactId,
                    "New Contact",
                    string.Empty,
                    "01827 65776", 
                    "01827 65776",
                    "www.gov.uk",
                    "help@gov.uk"))
            })
            .WithCostOption(new List<CostOption>())
            .WithLanguages(new List<Language>
            {
                    new Language("386b9b56-4bb0-4ff9-817f-99e4d94f77f5", "English")
                })
            .WithServiceAreas(new List<ServiceArea>
            {
                    new ServiceArea(Guid.NewGuid().ToString(), "National", null,"extent","http://statistics.data.gov.uk/id/statistical-geography/K02000001")
                })
            .WithServiceAtLocations(new List<ServiceAtLocation>
            {
                    new ServiceAtLocation(
                        "Test1750",
                        new Location(
                            "b4f8f02b-34e0-4366-ad8a-7ef75271a3bc",
                            "",
                            "",
                            52.6312,
                            -1.66526,
                            new List<LinkTaxonomy>(),
                            new List<PhysicalAddress>
                            {
                                new PhysicalAddress(
                                    Guid.NewGuid().ToString(),
                                    "79 Sheepcote Lane",
                                    ", Stathe, Tamworth, Staffordshire, ",
                                    "B77 3JN",
                                    "England",
                                    null
                                )
                            },
                            new List<AccessibilityForDisabilities>()
                            {
                                new AccessibilityForDisabilities("f9025e1f-3c16-48f3-a1bb-bfb36c216b45", "accessibility")
                            },
                            new List<LinkContact>()),
                        new List<RegularSchedule>(),
                        new List<HolidaySchedule>(), 
                        new List<LinkContact>())

                })
            .WithServiceTaxonomies(new List<ServiceTaxonomy>
            {
                    new ServiceTaxonomy
                    ("UnitTestA9107",
                    null,
                    new Taxonomy(
                        "UnitTestA bccsource:Organisation",
                        "Organisation",
                        "Test BCC Data Sources",
                        null
                        )),

                    new ServiceTaxonomy
                    ("UnitTestA9108",
                    null,
                    new Taxonomy(
                        "UnitTestA bccprimaryservicetype:38",
                        "Support",
                        "Test BCC Primary Services",
                        null
                        )),

                    new ServiceTaxonomy
                    ("UnitTestA9109",
                    null,
                    new Taxonomy(
                        "UnitTestA bccagegroup:37",
                        "Children",
                        "Test BCC Age Groups",
                        null
                        )),

                    new ServiceTaxonomy
                    ("UnitTestA9110",
                    null,
                    new Taxonomy(
                        "UnitTestAbccusergroup:56",
                        "Long Term Health Conditions",
                        "Test BCC User Groups",
                        null
                        ))
                })
            .Build();

        service.ServiceType = new ServiceType("1", "Information Sharing", "");

        return service;
    }
}
