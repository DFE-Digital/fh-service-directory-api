using System.Collections.ObjectModel;
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
        
        var updateTestOrganisation = GetTestCountyCouncilRecord();
        updateTestOrganisation.Name = "Unit Test B County Council";
        updateTestOrganisation.Description = "Unit Test B County Council Descrition";
        UpdateOrganisationCommand updatecommand = new(updateTestOrganisation.Id, updateTestOrganisation);
        UpdateOrganisationCommandHandler updatehandler = new(mockApplicationDbContext, updatelogger.Object, mockMediator.Object, mapper);

        ////Act
        var result = await updatehandler.Handle(updatecommand, new CancellationToken());

        ////Assert
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
        GetOrganisationAdminByOrganisationIdCommandHandler gethandler = new(mockApplicationDbContext, mapper);
        testOrganisation.Logo = "";

        //Act
        var result = await gethandler.Handle(getcommand, new CancellationToken());

        //Assert
        result.Should().NotBeNull();
        result.Should().Be("XTEST");
    }

    public static OrganisationWithServicesDto GetTestCountyCouncilDto()
    {
        var testCountyCouncil = new OrganisationWithServicesDto(
            "56e62852-1b0b-40e5-ac97-54a67ea957dc",
            new OrganisationTypeDto("1", "LA", "Local Authority"),
            "Unit Test County Council",
            "Unit Test County Council",
            null,
            new Uri("https://www.unittest.gov.uk/").ToString(),
            "https://www.unittest.gov.uk/",
            new List<ServiceDto>
            {
                 GetTestCountyCouncilServicesDto("56e62852-1b0b-40e5-ac97-54a67ea957dc")
            }
            );

        testCountyCouncil.AdminAreaCode = "XTEST";

        return testCountyCouncil;
    }

    public static ServiceDto GetTestCountyCouncilServicesDto(string parentId)
    {
        var contactId = Guid.NewGuid().ToString();

        var builder = new ServicesDtoBuilder();
        var service = builder.WithMainProperties("3010521b-6e0a-41b0-b610-200edbbeeb14",
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
            .WithContact(new List<ContactDto>
            {
                new ContactDto(
                    contactId,
                    "Contact",
                    string.Empty,
                    "01827 65777",
                    "01827 65777",
                    "www.unittestservice.com",
                    "support@unittestservice.com"
                    )
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
                        "Test1749",
                        new LocationDto(
                            "6ea31a4f-7dcc-4350-9fba-20525efe092f",
                            "",
                            "",
                            52.6312,
                            -1.66526,
                            new List<PhysicalAddressDto>
                            {
                                new PhysicalAddressDto(
                                    Guid.NewGuid().ToString(),
                                    "77 Sheepcote Lane",
                                    ", Stathe, Tamworth, Staffordshire, ",
                                    "B77 3JN",
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
                            }),
                        new List<RegularScheduleDto>(),
                        new List<HolidayScheduleDto>()
                        )

                })
            .WithServiceTaxonomies(new List<ServiceTaxonomyDto>
            {
                    new ServiceTaxonomyDto
                    ("UnitTest9107",
                    new TaxonomyDto(
                        "UnitTest bccsource:Organisation",
                        "Organisation",
                        "Test BCC Data Sources",
                        null
                        )),

                    new ServiceTaxonomyDto
                    ("UnitTest9108",
                    new TaxonomyDto(
                        "UnitTest bccprimaryservicetype:38",
                        "Support",
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
            .WithContact(new List<Contact>
            {
                new Contact(
                    contactId,
                    "New Contact",
                    string.Empty,
                    "01827 65776", 
                    "01827 65776",
                    "www.gov.uk",
                    "help@gov.uk")
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
                            ),
                        new List<RegularSchedule>(),
                        new List<HolidaySchedule>()
                        )

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
