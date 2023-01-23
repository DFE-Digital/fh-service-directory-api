﻿using System.Collections.ObjectModel;
using AutoMapper;
using FamilyHubs.ServiceDirectory.Shared.Builders;
using FamilyHubs.ServiceDirectory.Shared.Enums;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralContacts;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralCostOptions;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralEligibilitys;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralHolidaySchedule;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralLanguages;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralLinkTaxonomies;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralLocations;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralOrganisations;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralPhysicalAddresses;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralRegularSchedule;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralServiceAreas;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralServiceAtLocations;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralServiceDeliverysEx;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralServices;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralServiceTaxonomys;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralTaxonomys;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OrganisationType;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.ServiceType;
using FamilyHubs.ServiceDirectoryApi.UnitTests.Builders;
using fh_service_directory_api.api.Commands.CreateOpenReferralOrganisation;
using fh_service_directory_api.api.Commands.UpdateOpenReferralOrganisation;
using fh_service_directory_api.api.Queries.GetOpenReferralOrganisationById;
using fh_service_directory_api.api.Queries.GetOrganisationAdminByOrganisationId;
using fh_service_directory_api.api.Queries.GetOrganisationTypes;
using fh_service_directory_api.api.Queries.ListOrganisation;
using fh_service_directory_api.core;
using fh_service_directory_api.core.Entities;
using fh_service_directory_api.infrastructure.Persistence.Repository;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;

namespace FamilyHubs.ServiceDirectoryApi.UnitTests.Organisation;

public class WhenUsingOrganisationCommands : BaseCreateDbUnitTest
{
    [Fact]
    public async Task ThenCreateOpenReferralOrganisation()
    {
        //Arange
        var myProfile = new AutoMappingProfiles();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
        IMapper mapper = new Mapper(configuration);
        var logger = new Mock<ILogger<CreateOpenReferralOrganisationCommandHandler>>();
        var mockApplicationDbContext = GetApplicationDbContext();
        var testOrganisation = GetTestCountyCouncilDto();
        CreateOpenReferralOrganisationCommand command = new(testOrganisation);
        CreateOpenReferralOrganisationCommandHandler handler = new(mockApplicationDbContext, mapper, logger.Object);

        //Act
        var result = await handler.Handle(command, new CancellationToken());

        //Assert
        result.Should().NotBeNull();
        result.Should().Be(testOrganisation.Id);
    }

    [Fact]
    public async Task ThenUpdateOpenReferralOrganisation()
    {
        //Arange
        var myProfile = new AutoMappingProfiles();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
        IMapper mapper = new Mapper(configuration);
        var logger = new Mock<ILogger<CreateOpenReferralOrganisationCommandHandler>>();
        var mockApplicationDbContext = GetApplicationDbContext();
        var testOrganisation = GetTestCountyCouncilDto();
        var updatelogger = new Mock<ILogger<UpdateOpenReferralOrganisationCommandHandler>>();
        var mockMediator = new Mock<ISender>();
        CreateOpenReferralOrganisationCommand command = new(testOrganisation);
        CreateOpenReferralOrganisationCommandHandler handler = new(mockApplicationDbContext, mapper, logger.Object);
        var id = await handler.Handle(command, new CancellationToken());
        
        var updateTestOrganisation = GetTestCountyCouncilRecord();
        updateTestOrganisation.Name = "Unit Test B County Council";
        updateTestOrganisation.Description = "Unit Test B County Council Descrition";
        UpdateOpenReferralOrganisationCommand updatecommand = new(updateTestOrganisation.Id, updateTestOrganisation);
        UpdateOpenReferralOrganisationCommandHandler updatehandler = new(mockApplicationDbContext, updatelogger.Object, mockMediator.Object, mapper);

        ////Act
        var result = await updatehandler.Handle(updatecommand, new CancellationToken());

        ////Assert
        result.Should().NotBeNull();
        result.Should().Be(testOrganisation.Id);
    }

    [Fact]
    public async Task ThenGetOpenReferralOrganisationById()
    {
        //Arange
        var myProfile = new AutoMappingProfiles();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
        IMapper mapper = new Mapper(configuration);
        var logger = new Mock<ILogger<CreateOpenReferralOrganisationCommandHandler>>();
        var mockApplicationDbContext = GetApplicationDbContext();
        var testOrganisation = GetTestCountyCouncilDto();
        CreateOpenReferralOrganisationCommand command = new(testOrganisation);
        CreateOpenReferralOrganisationCommandHandler handler = new(mockApplicationDbContext, mapper, logger.Object);
        var id = await handler.Handle(command, new CancellationToken());

        GetOpenReferralOrganisationByIdCommand getcommand = new() { Id = testOrganisation.Id };
        GetOpenReferralOrganisationByIdHandler gethandler = new(mockApplicationDbContext);
        testOrganisation.Logo = "";

        //Act
        var result = await gethandler.Handle(getcommand, new CancellationToken());

        //Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(testOrganisation, opts => opts.Excluding(si => si.AdministractiveDistrictCode));
    }

    [Fact]
    public async Task ThenListOpenReferralOrganisations()
    {
        //Arange
        var myProfile = new AutoMappingProfiles();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
        IMapper mapper = new Mapper(configuration);
        var logger = new Mock<ILogger<CreateOpenReferralOrganisationCommandHandler>>();
        var mockApplicationDbContext = GetApplicationDbContext();
        var testOrganisation = GetTestCountyCouncilDto();
        CreateOpenReferralOrganisationCommand command = new(testOrganisation);
        CreateOpenReferralOrganisationCommandHandler handler = new(mockApplicationDbContext, mapper, logger.Object);
        var id = await handler.Handle(command, new CancellationToken());

        ListOpenReferralOrganisationCommand getcommand = new();
        ListOpenReferralOrganisationCommandHandler gethandler = new(mockApplicationDbContext);
        testOrganisation.Logo = "";

        //Act
        var result = await gethandler.Handle(getcommand, new CancellationToken());

        //Assert
        result.Should().NotBeNull();
        result[0].Should().BeEquivalentTo(testOrganisation, opts => opts.Excluding(si => si.Services).Excluding(si => si.AdministractiveDistrictCode));
    }

    [Fact]
    public async Task ThenListOpenReferralOrganisationTypes()
    {
        //Arange
        var myProfile = new AutoMappingProfiles();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
        IMapper mapper = new Mapper(configuration);
        var logger = new Mock<ILogger<CreateOpenReferralOrganisationCommandHandler>>();
        var mockApplicationDbContext = GetApplicationDbContext();
        var openReferralOrganisationSeedData = new OpenReferralOrganisationSeedData(false);
        if (!mockApplicationDbContext.AdminAreas.Any())
        {
            mockApplicationDbContext.OrganisationTypes.AddRange(openReferralOrganisationSeedData.SeedOrganisationTypes());
            mockApplicationDbContext.SaveChanges();
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
    public async Task ThenGetOpenReferralAdminByOrganisationId()
    {
        //Arange
        var myProfile = new AutoMappingProfiles();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
        IMapper mapper = new Mapper(configuration);
        var logger = new Mock<ILogger<CreateOpenReferralOrganisationCommandHandler>>();
        var mockApplicationDbContext = GetApplicationDbContext();
        var testOrganisation = GetTestCountyCouncilDto();
        CreateOpenReferralOrganisationCommand command = new(testOrganisation);
        CreateOpenReferralOrganisationCommandHandler handler = new(mockApplicationDbContext, mapper, logger.Object);
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

    public static OpenReferralOrganisationWithServicesDto GetTestCountyCouncilDto()
    {
        var testCountyCouncil = new OpenReferralOrganisationWithServicesDto(
            "56e62852-1b0b-40e5-ac97-54a67ea957dc",
            new OrganisationTypeDto("1", "LA", "Local Authority"),
            "Unit Test County Council",
            "Unit Test County Council",
            null,
            new Uri("https://www.unittest.gov.uk/").ToString(),
            "https://www.unittest.gov.uk/",
            new List<OpenReferralServiceDto>
            {
                 GetTestCountyCouncilServicesDto("56e62852-1b0b-40e5-ac97-54a67ea957dc")
            }
            );

        testCountyCouncil.AdministractiveDistrictCode = "XTEST";

        return testCountyCouncil;
    }

    public static OpenReferralServiceDto GetTestCountyCouncilServicesDto(string parentId)
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
                "www.unittestservice.com",
                "support@unittestservice.com",
                null,
                false)
            .WithServiceDelivery(new List<OpenReferralServiceDeliveryExDto>
                {
                    new OpenReferralServiceDeliveryExDto(Guid.NewGuid().ToString(),ServiceDelivery.Online)
                })
            .WithEligibility(new List<OpenReferralEligibilityDto>
                {
                    new OpenReferralEligibilityDto("Test9111Children","",0,13)
                })
            .WithContact(new List<OpenReferralContactDto>
            {
                new OpenReferralContactDto(
                    contactId,
                    "Contact",
                    string.Empty,
                    "01827 65777",
                    "01827 65777"
                    )
            })
            .WithCostOption(new List<OpenReferralCostOptionDto> {new() {Id = Guid.NewGuid().ToString(), Amount = decimal.Zero, Option = "free", Amount_description = ""}})
            .WithLanguages(new List<OpenReferralLanguageDto>
            {
                    new OpenReferralLanguageDto("1bb6c313-648d-4226-9e96-b7d37eaeb3dd", "English")
                })
            .WithServiceAreas(new List<OpenReferralServiceAreaDto>
            {
                    new OpenReferralServiceAreaDto(Guid.NewGuid().ToString(), "National", null,"http://statistics.data.gov.uk/id/statistical-geography/K02000001")
                })
            .WithServiceAtLocations(new List<OpenReferralServiceAtLocationDto>
            {
                    new OpenReferralServiceAtLocationDto(
                        "Test1749",
                        new OpenReferralLocationDto(
                            "6ea31a4f-7dcc-4350-9fba-20525efe092f",
                            "",
                            "",
                            52.6312,
                            -1.66526,
                            new List<OpenReferralPhysicalAddressDto>
                            {
                                new OpenReferralPhysicalAddressDto(
                                    Guid.NewGuid().ToString(),
                                    "77 Sheepcote Lane",
                                    ", Stathe, Tamworth, Staffordshire, ",
                                    "B77 3JN",
                                    "England",
                                    null
                                    )
                            },
                            new List<OpenReferralLinkTaxonomyDto>
                            {
                                new OpenReferralLinkTaxonomyDto(
                                    Guid.NewGuid().ToString(),
                                    "Location",
                                    "6ea31a4f-7dcc-4350-9fba-20525efe092f",
                                    new OpenReferralTaxonomyDto(
                                        //todo: real guid

                                        Guid.NewGuid().ToString(),
                                        "Family_hub",
                                        null,
                                        null
                                    )
                                )
                            }),
                        new List<OpenReferralRegularScheduleDto>(),
                        new List<OpenReferralHolidayScheduleDto>()
                        )

                })
            .WithServiceTaxonomies(new List<OpenReferralServiceTaxonomyDto>
            {
                    new OpenReferralServiceTaxonomyDto
                    ("UnitTest9107",
                    new OpenReferralTaxonomyDto(
                        "UnitTest bccsource:Organisation",
                        "Organisation",
                        "Test BCC Data Sources",
                        null
                        )),

                    new OpenReferralServiceTaxonomyDto
                    ("UnitTest9108",
                    new OpenReferralTaxonomyDto(
                        "UnitTest bccprimaryservicetype:38",
                        "Support",
                        "Test BCC Primary Services",
                        null
                        )),

                    new OpenReferralServiceTaxonomyDto
                    ("UnitTest9109",
                    new OpenReferralTaxonomyDto(
                        "UnitTest bccagegroup:37",
                        "Children",
                        "Test BCC Age Groups",
                        null
                        )),

                    new OpenReferralServiceTaxonomyDto
                    ("UnitTest9110",
                    new OpenReferralTaxonomyDto(
                        "UnitTestbccusergroup:56",
                        "Long Term Health Conditions",
                        "Test BCC User Groups",
                        null
                        ))
                })
            .Build();

        service.RegularSchedules = new List<OpenReferralRegularScheduleDto>();
        service.HolidaySchedules = new List<OpenReferralHolidayScheduleDto>();

        return service;
    }

    
    public static OpenReferralOrganisationWithServicesDto GetTestCountyCouncilRecord()
    {
        var testCountyCouncil = new OpenReferralOrganisationWithServicesDto(
            "56e62852-1b0b-40e5-ac97-54a67ea957dc",
            new OrganisationTypeDto("1", "LA", "Local Authority"),
            "Unit Test A County Council",
            "Unit Test A County Council",
            null,
            new Uri("https://www.unittesta.gov.uk/").ToString(),
            "https://www.unittesta.gov.uk/",
            //new List<OpenReferralReviewDto>(),
            new List<OpenReferralServiceDto>
            {
                 //GetTestCountyCouncilServicesRecord()
            }
            );

        testCountyCouncil.AdministractiveDistrictCode = "XTEST";

        return testCountyCouncil;
    }

    private static OpenReferralService GetTestCountyCouncilServicesRecord()
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
                "www.unittestservice.com",
                "support@unittestservice.com",
                null)
            .WithServiceDelivery(new List<OpenReferralServiceDelivery>
                {
                    new OpenReferralServiceDelivery(Guid.NewGuid().ToString(),ServiceDelivery.Online)
                })
            .WithEligibility(new List<OpenReferralEligibility>
                {
                    new OpenReferralEligibility("Test9120Children","eligability","",0,13, new Collection<OpenReferralTaxonomy>())
                })
            .WithContact(new List<OpenReferralContact>
            {
                new OpenReferralContact(
                    contactId,
                    "New Contact",
                    string.Empty,
                    "01827 65776", 
                    "01827 65776"
                    )
            })
            .WithCostOption(new List<OpenReferralCost_Option>())
            .WithLanguages(new List<OpenReferralLanguage>
            {
                    new OpenReferralLanguage("386b9b56-4bb0-4ff9-817f-99e4d94f77f5", "English")
                })
            .WithServiceAreas(new List<OpenReferralService_Area>
            {
                    new OpenReferralService_Area(Guid.NewGuid().ToString(), "National", null,"extent","http://statistics.data.gov.uk/id/statistical-geography/K02000001")
                })
            .WithServiceAtLocations(new List<OpenReferralServiceAtLocation>
            {
                    new OpenReferralServiceAtLocation(
                        "Test1750",
                        new OpenReferralLocation(
                            "b4f8f02b-34e0-4366-ad8a-7ef75271a3bc",
                            "",
                            "",
                            52.6312,
                            -1.66526,
                            new List<OpenReferralLinkTaxonomy>(),
                            new List<OpenReferralPhysical_Address>
                            {
                                new OpenReferralPhysical_Address(
                                    Guid.NewGuid().ToString(),
                                    "79 Sheepcote Lane",
                                    ", Stathe, Tamworth, Staffordshire, ",
                                    "B77 3JN",
                                    "England",
                                    null
                                    )
                            },
                            new List<Accessibility_For_Disabilities>()
                            ),
                        new List<OpenReferralRegular_Schedule>(),
                        new List<OpenReferralHoliday_Schedule>()
                        )

                })
            .WithServiceTaxonomies(new List<OpenReferralService_Taxonomy>
            {
                    new OpenReferralService_Taxonomy
                    ("UnitTestA9107",
                    null,
                    new OpenReferralTaxonomy(
                        "UnitTestA bccsource:Organisation",
                        "Organisation",
                        "Test BCC Data Sources",
                        null
                        )),

                    new OpenReferralService_Taxonomy
                    ("UnitTestA9108",
                    null,
                    new OpenReferralTaxonomy(
                        "UnitTestA bccprimaryservicetype:38",
                        "Support",
                        "Test BCC Primary Services",
                        null
                        )),

                    new OpenReferralService_Taxonomy
                    ("UnitTestA9109",
                    null,
                    new OpenReferralTaxonomy(
                        "UnitTestA bccagegroup:37",
                        "Children",
                        "Test BCC Age Groups",
                        null
                        )),

                    new OpenReferralService_Taxonomy
                    ("UnitTestA9110",
                    null,
                    new OpenReferralTaxonomy(
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
