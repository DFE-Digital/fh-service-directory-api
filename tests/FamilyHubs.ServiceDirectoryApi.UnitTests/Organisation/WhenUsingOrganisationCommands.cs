using Ardalis.GuardClauses;
using AutoMapper;
using FamilyHubs.ServiceDirectory.Api.Commands.CreateOrganisation;
using FamilyHubs.ServiceDirectory.Api.Commands.UpdateOrganisation;
using FamilyHubs.ServiceDirectory.Api.Queries.GetOrganisationAdminByOrganisationId;
using FamilyHubs.ServiceDirectory.Api.Queries.GetOrganisationById;
using FamilyHubs.ServiceDirectory.Api.Queries.GetOrganisationTypes;
using FamilyHubs.ServiceDirectory.Api.Queries.ListOrganisation;
using FamilyHubs.ServiceDirectory.Core;
using FamilyHubs.ServiceDirectory.Infrastructure.Persistence.Repository;
using FamilyHubs.ServiceDirectory.Shared.Dto;
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
        //Arrange
        var myProfile = new AutoMappingProfiles();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
        var mapper = new Mapper(configuration);
        var logger = new Mock<ILogger<CreateOrganisationCommandHandler>>();
        var mockApplicationDbContext = GetApplicationDbContext();
        var testOrganisation = TestDataProvider.GetTestCountyCouncilDto();
        var command = new CreateOrganisationCommand(testOrganisation);
        var handler = new CreateOrganisationCommandHandler(mockApplicationDbContext, mapper, logger.Object);

        //Act
        var result = await handler.Handle(command, new CancellationToken());

        //Assert
        result.Should().NotBeNull();
        result.Should().Be(testOrganisation.Id);
    }

    [Fact]
    public async Task ThenCreateRelatedOrganisation()
    {
        //Arrange
        var myProfile = new AutoMappingProfiles();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
        var mapper = new Mapper(configuration);
        var logger = new Mock<ILogger<CreateOrganisationCommandHandler>>();
        var mockApplicationDbContext = GetApplicationDbContext();
        var testCountyCouncil = TestDataProvider.GetTestCountyCouncilDto();
        var createOrganisationCommand = new CreateOrganisationCommand(testCountyCouncil);
        var handler = new CreateOrganisationCommandHandler(mockApplicationDbContext, mapper, logger.Object);
        await handler.Handle(createOrganisationCommand, new CancellationToken());

        var testOrganisation = new OrganisationWithServicesDto(
            "e0dc6a0c-2f9c-48c6-a222-1232abbf9000",
            new OrganisationTypeDto("2", "VCFS", "Voluntary, Charitable, Faith Sector"),
            "Related VCS",
            "Related VCS",
            null,
            new Uri("https://www.relatedvcs.gov.uk/").ToString(),
            "https://www.related.gov.uk/",
            new List<ServiceDto>())
        {
            AdminAreaCode = "XTEST"
        };

        var command = new CreateOrganisationCommand(testOrganisation);


        //Act
        var result = await handler.Handle(command, new CancellationToken());

        //Assert
        result.Should().NotBeNull();
        result.Should().Be(testOrganisation.Id);
    }

    [Fact]
    public async Task ThenCreateAnotherOrganisation()
    {
        //Arrange
        var myProfile = new AutoMappingProfiles();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
        var mapper = new Mapper(configuration);
        var logger = new Mock<ILogger<CreateOrganisationCommandHandler>>();
        var mockApplicationDbContext = GetApplicationDbContext();
        var initialCommand = new CreateOrganisationCommand(TestDataProvider.GetTestCountyCouncilDto());
        var initialHandler = new CreateOrganisationCommandHandler(mockApplicationDbContext, mapper, logger.Object);
        await initialHandler.Handle(initialCommand, new CancellationToken());

        var testOrganisation = TestDataProvider.GetTestCountyCouncilDto2();
        var command = new CreateOrganisationCommand(TestDataProvider.GetTestCountyCouncilDto2());
        var handler = new CreateOrganisationCommandHandler(mockApplicationDbContext, mapper, logger.Object);

        //Act
        var result = await handler.Handle(command, new CancellationToken());

        //Assert
        result.Should().NotBeNull();
        result.Should().Be(testOrganisation.Id);
    }

    [Fact]
    public async Task ThenCreateDuplicateOrganisation_ShouldThrowException()
    {
        //Arrange
        var myProfile = new AutoMappingProfiles();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
        var mapper = new Mapper(configuration);
        var logger = new Mock<ILogger<CreateOrganisationCommandHandler>>();
        var mockApplicationDbContext = GetApplicationDbContext();
        var testOrganisation = TestDataProvider.GetTestCountyCouncilDto();
        var initialCommand = new CreateOrganisationCommand(testOrganisation);
        var initialHandler = new CreateOrganisationCommandHandler(mockApplicationDbContext, mapper, logger.Object);
        await initialHandler.Handle(initialCommand, new CancellationToken());


        var command = new CreateOrganisationCommand(testOrganisation);
        var handler = new CreateOrganisationCommandHandler(mockApplicationDbContext, mapper, logger.Object);

        // Act 
        // Assert
        await Assert.ThrowsAsync<ArgumentException>(() => handler.Handle(command, new CancellationToken()));

    }

    [Fact]
    public async Task ThenUpdateOrganisation()
    {
        //Arrange
        var myProfile = new AutoMappingProfiles();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
        var mapper = new Mapper(configuration);
        var logger = new Mock<ILogger<CreateOrganisationCommandHandler>>();
        var mockApplicationDbContext = GetApplicationDbContext();
        var testOrganisation = TestDataProvider.GetTestCountyCouncilDto();
        var updateLogger = new Mock<ILogger<UpdateOrganisationCommandHandler>>();
        var mockMediator = new Mock<ISender>();
        var command = new CreateOrganisationCommand(testOrganisation);
        var handler = new CreateOrganisationCommandHandler(mockApplicationDbContext, mapper, logger.Object);
        await handler.Handle(command, new CancellationToken());


        var updateTestOrganisation = TestDataProvider.GetTestCountyCouncilDto(true);


        var updateCommand = new UpdateOrganisationCommand(updateTestOrganisation.Id, updateTestOrganisation);
        var updateHandler = new UpdateOrganisationCommandHandler(mockApplicationDbContext, updateLogger.Object, mockMediator.Object, mapper);

        //Act
        var result = await updateHandler.Handle(updateCommand, new CancellationToken());

        //Assert
        result.Should().NotBeNull();
        result.Should().Be(testOrganisation.Id);
    }

    [Fact]
    public async Task ThenUpdateOrganisationWithNewService()
    {
        //Arrange
        var myProfile = new AutoMappingProfiles();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
        var mapper = new Mapper(configuration);
        var logger = new Mock<ILogger<CreateOrganisationCommandHandler>>();
        var mockApplicationDbContext = GetApplicationDbContext();
        var testOrganisation = TestDataProvider.GetTestCountyCouncilDto();
        testOrganisation.Services = default!;
        var updateLogger = new Mock<ILogger<UpdateOrganisationCommandHandler>>();
        var mockMediator = new Mock<ISender>();
        var command = new CreateOrganisationCommand(testOrganisation);
        var handler = new CreateOrganisationCommandHandler(mockApplicationDbContext, mapper, logger.Object);
        await handler.Handle(command, new CancellationToken());

        var updateTestOrganisation = TestDataProvider.GetTestCountyCouncilDto();
        updateTestOrganisation.Services = new List<ServiceDto>
        {
             TestDataProvider.GetTestCountyCouncilServicesDto2("56e62852-1b0b-40e5-ac97-54a67ea957dc")
        };

        var updateCommand = new UpdateOrganisationCommand(updateTestOrganisation.Id, updateTestOrganisation);
        var updateHandler = new UpdateOrganisationCommandHandler(mockApplicationDbContext, updateLogger.Object, mockMediator.Object, mapper);

        //Act
        var result = await updateHandler.Handle(updateCommand, new CancellationToken());

        //Assert
        result.Should().NotBeNull();
        result.Should().Be(testOrganisation.Id);
    }

    [Fact]
    public async Task ThenGetOrganisationById()
    {
        //Arrange
        var myProfile = new AutoMappingProfiles();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
        var mapper = new Mapper(configuration);
        var logger = new Mock<ILogger<CreateOrganisationCommandHandler>>();
        var mockApplicationDbContext = GetApplicationDbContext();
        var testOrganisation = TestDataProvider.GetTestCountyCouncilDto();
        var command = new CreateOrganisationCommand(testOrganisation);
        var handler = new CreateOrganisationCommandHandler(mockApplicationDbContext, mapper, logger.Object);
        await handler.Handle(command, new CancellationToken());

        var getCommand = new GetOrganisationByIdCommand { Id = testOrganisation.Id };
        var getHandler = new GetOrganisationByIdHandler(mockApplicationDbContext);
        testOrganisation.Logo = "";

        //Act
        var result = await getHandler.Handle(getCommand, new CancellationToken());

        //Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(testOrganisation, opts => opts.Excluding(si => si.AdminAreaCode));
    }

    [Fact]
    public async Task ThenGetOrganisationById_ShouldThrowExceptionWhenIdDoesNotExist()
    {
        //Arrange
        var mockApplicationDbContext = GetApplicationDbContext();
        var getCommand = new GetOrganisationByIdCommand { Id = Guid.NewGuid().ToString() };
        var getHandler = new GetOrganisationByIdHandler(mockApplicationDbContext);


        // Act 
        // Assert
        await Assert.ThrowsAsync<NotFoundException>(() => getHandler.Handle(getCommand, new CancellationToken()));

    }

    [Fact]
    public async Task ThenListOrganisations()
    {
        //Arrange
        var myProfile = new AutoMappingProfiles();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
        var mapper = new Mapper(configuration);
        var logger = new Mock<ILogger<CreateOrganisationCommandHandler>>();
        var mockApplicationDbContext = GetApplicationDbContext();
        var testOrganisation = TestDataProvider.GetTestCountyCouncilDto();
        var command = new CreateOrganisationCommand(testOrganisation);
        var handler = new CreateOrganisationCommandHandler(mockApplicationDbContext, mapper, logger.Object);
        await handler.Handle(command, new CancellationToken());

        var getCommand = new ListOrganisationCommand();
        var getHandler = new ListOrganisationCommandHandler(mockApplicationDbContext);
        testOrganisation.Logo = "";

        //Act
        var result = await getHandler.Handle(getCommand, new CancellationToken());

        //Assert
        result.Should().NotBeNull();
        result[0].Should().BeEquivalentTo(testOrganisation, opts => opts
            .Excluding(si => si.Services)
            .Excluding(si => si.AdminAreaCode)
            .Excluding(si => si.LinkContacts)
        );
    }

    [Fact]
    public async Task ThenListOrganisationTypes()
    {
        //Arrange
        var mockApplicationDbContext = GetApplicationDbContext();
        var seedData = new OrganisationSeedData(false);
        if (!mockApplicationDbContext.AdminAreas.Any())
        {
            mockApplicationDbContext.OrganisationTypes.AddRange(seedData.SeedOrganisationTypes());
            await mockApplicationDbContext.SaveChangesAsync();
        }

        var getCommand = new GetOrganisationTypesCommand();
        var getHandler = new GetOrganisationTypesCommandHandler(mockApplicationDbContext);


        //Act
        var result = await getHandler.Handle(getCommand, new CancellationToken());

        //Assert
        result.Should().NotBeNull();
        result.Count.Should().Be(3);
    }

    [Fact]
    public async Task ThenGetAdminByOrganisationId()
    {
        //Arrange
        var myProfile = new AutoMappingProfiles();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
        var mapper = new Mapper(configuration);
        var logger = new Mock<ILogger<CreateOrganisationCommandHandler>>();
        var mockApplicationDbContext = GetApplicationDbContext();
        var testOrganisation = TestDataProvider.GetTestCountyCouncilDto();
        var command = new CreateOrganisationCommand(testOrganisation);
        var handler = new CreateOrganisationCommandHandler(mockApplicationDbContext, mapper, logger.Object);
        await handler.Handle(command, new CancellationToken());

        var getCommand = new GetOrganisationAdminByOrganisationIdCommand(testOrganisation.Id);
        var getHandler = new GetOrganisationAdminByOrganisationIdCommandHandler(mockApplicationDbContext);
        testOrganisation.Logo = "";

        //Act
        var result = await getHandler.Handle(getCommand, new CancellationToken());

        //Assert
        result.Should().NotBeNull();
        result.Should().Be("XTEST");
    }
}