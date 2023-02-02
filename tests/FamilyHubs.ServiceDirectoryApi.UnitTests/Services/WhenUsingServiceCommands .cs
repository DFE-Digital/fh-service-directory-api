using Ardalis.GuardClauses;
using AutoMapper;
using FamilyHubs.ServiceDirectory.Api.Commands.CreateOrganisation;
using FamilyHubs.ServiceDirectory.Api.Commands.CreateService;
using FamilyHubs.ServiceDirectory.Api.Commands.DeleteService;
using FamilyHubs.ServiceDirectory.Api.Commands.UpdateService;
using FamilyHubs.ServiceDirectory.Api.Queries.GetServices;
using FamilyHubs.ServiceDirectory.Api.Queries.GetServicesByOrganisation;
using FamilyHubs.ServiceDirectory.Core;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace FamilyHubs.ServiceDirectoryApi.UnitTests.Services;

public class WhenUsingServiceCommands : BaseCreateDbUnitTest
{
    [Fact]
    public async Task ThenCreateService()
    {
        //Arrange
        var myProfile = new AutoMappingProfiles();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
        var mapper = new Mapper(configuration);
        var logger = new Mock<ILogger<CreateOrganisationCommandHandler>>();
        var mockApplicationDbContext = GetApplicationDbContext();
        var testOrganisation = TestDataProvider.GetTestCountyCouncilDto(false, false, true);
        testOrganisation.Services = default!;
        var orgCommand = new CreateOrganisationCommand(testOrganisation);
        var orgHandler = new CreateOrganisationCommandHandler(mockApplicationDbContext, mapper, logger.Object);
        await orgHandler.Handle(orgCommand, new CancellationToken());
        testOrganisation = TestDataProvider.GetTestCountyCouncilDto();
        var command = new CreateServiceCommand(testOrganisation.Services?.ElementAt(0) ?? default!);
        var handler = new CreateServiceCommandHandler(mockApplicationDbContext, mapper, new Mock<ILogger<CreateServiceCommandHandler>>().Object);


        //Act
        var result = await handler.Handle(command, new CancellationToken());

        //Assert
        result.Should().NotBeNull();
        result.Should().Be(testOrganisation.Services?.ElementAt(0).Id);
    }

    [Fact]
    public async Task ThenGetService()
    {
        //Arrange
        var myProfile = new AutoMappingProfiles();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
        var mapper = new Mapper(configuration);
        var logger = new Mock<ILogger<CreateOrganisationCommandHandler>>();
        var mockApplicationDbContext = GetApplicationDbContext();
        var testOrganisation = TestDataProvider.GetTestCountyCouncilDto(false, false, true);
        var createCommand = new CreateOrganisationCommand(testOrganisation);
        var createHandler = new CreateOrganisationCommandHandler(mockApplicationDbContext, mapper, logger.Object);
        await createHandler.Handle(createCommand, new CancellationToken());


        var command = new GetServicesCommand("Information Sharing", "active", "XTEST", null, null, null,
            null, null, null, 1, 10, null, null, null, null, null, null, null, null);
        var handler = new GetServicesCommandHandler(mockApplicationDbContext);

        //Act
        var results = await handler.Handle(command, new CancellationToken());

        //Assert
        results.Should().NotBeNull();
        ArgumentNullException.ThrowIfNull(testOrganisation);
        ArgumentNullException.ThrowIfNull(testOrganisation.Services);
        results.Items[0].Should().BeEquivalentTo(testOrganisation.Services.ElementAt(0));
    }

    [Fact]
    public async Task ThenGetServicesByOrganisationId()
    {
        //Arrange
        var myProfile = new AutoMappingProfiles();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
        var mapper = new Mapper(configuration);
        var logger = new Mock<ILogger<CreateOrganisationCommandHandler>>();
        var mockApplicationDbContext = GetApplicationDbContext();
        var testOrganisation = TestDataProvider.GetTestCountyCouncilDto(false, false, true);
        var createCommand = new CreateOrganisationCommand(testOrganisation);
        var createHandler = new CreateOrganisationCommandHandler(mockApplicationDbContext, mapper, logger.Object);
        await createHandler.Handle(createCommand, new CancellationToken());


        var command = new GetServicesByOrganisationIdCommand(testOrganisation.Id);
        var handler = new GetServicesByOrganisationIdCommandHandler(mockApplicationDbContext);

        //Act
        var results = await handler.Handle(command, new CancellationToken());

        //Assert
        results.Should().NotBeNull();
        ArgumentNullException.ThrowIfNull(testOrganisation);
        ArgumentNullException.ThrowIfNull(testOrganisation.Services);
        results[0].Should().BeEquivalentTo(testOrganisation.Services.ElementAt(0));
    }

    [Fact]
    public async Task ThenGetServicesByOrganisationId_ShouldThrowExceptionWhenNoOrganisations()
    {
        //Arrange
        var mockApplicationDbContext = GetApplicationDbContext();
        var testOrganisation = TestDataProvider.GetTestCountyCouncilDto(false, false, true);
        var command = new GetServicesByOrganisationIdCommand(testOrganisation.Id);
        var handler = new GetServicesByOrganisationIdCommandHandler(mockApplicationDbContext);

        // Act 
        // Assert
        await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, new CancellationToken()));

    }

    [Fact]
    public async Task ThenGetServicesByOrganisationId_ShouldThrowExceptionWhenNoServices()
    {
        //Arrange
        var myProfile = new AutoMappingProfiles();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
        var mapper = new Mapper(configuration);
        var logger = new Mock<ILogger<CreateOrganisationCommandHandler>>();
        var mockApplicationDbContext = GetApplicationDbContext();
        var testOrganisation = TestDataProvider.GetTestCountyCouncilDto(false, false, true);
        testOrganisation.Services = default!;
        var createCommand = new CreateOrganisationCommand(testOrganisation);
        var createHandler = new CreateOrganisationCommandHandler(mockApplicationDbContext, mapper, logger.Object);
        await createHandler.Handle(createCommand, new CancellationToken());

        var command = new GetServicesByOrganisationIdCommand(testOrganisation.Id);
        var handler = new GetServicesByOrganisationIdCommandHandler(mockApplicationDbContext);

        // Act 
        // Assert
        await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, new CancellationToken()));

    }

    [Fact]
    public async Task ThenGetServiceThatArePaidFor()
    {
        //Arrange
        var myProfile = new AutoMappingProfiles();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
        var mapper = new Mapper(configuration);
        var logger = new Mock<ILogger<CreateOrganisationCommandHandler>>();
        var mockApplicationDbContext = GetApplicationDbContext();
        var testOrganisation = TestDataProvider.GetTestCountyCouncilDto(false, false, true);
        var createCommand = new CreateOrganisationCommand(testOrganisation);
        var createHandler = new CreateOrganisationCommandHandler(mockApplicationDbContext, mapper, logger.Object);
        await createHandler.Handle(createCommand, new CancellationToken());


        var command = new GetServicesCommand("Information Sharing", "active", "XTEST", null, null, null,
            null, null, null, 1, 10, null, null, true, null, null, null, null, null);
        var handler = new GetServicesCommandHandler(mockApplicationDbContext);

        //Act
        var results = await handler.Handle(command, new CancellationToken());

        //Assert
        results.Should().NotBeNull();
        results.Items.Count.Should().Be(0);
    }

    [Fact]
    public async Task ThenGetServiceThatAreFree()
    {
        //Arrange
        var myProfile = new AutoMappingProfiles();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
        var mapper = new Mapper(configuration);
        var logger = new Mock<ILogger<CreateOrganisationCommandHandler>>();
        var mockApplicationDbContext = GetApplicationDbContext();
        var testOrganisation = TestDataProvider.GetTestCountyCouncilDto(false, false, true);
        var createCommand = new CreateOrganisationCommand(testOrganisation);
        var createHandler = new CreateOrganisationCommandHandler(mockApplicationDbContext, mapper, logger.Object);
        await createHandler.Handle(createCommand, new CancellationToken());


        var command = new GetServicesCommand("Information Sharing", "active", "XTEST", null, null, null,
            null, null, null, 1, 10, null, null, false, null, null, null, null, null);
        var handler = new GetServicesCommandHandler(mockApplicationDbContext);

        //Act
        var results = await handler.Handle(command, new CancellationToken());

        //Assert
        results.Should().NotBeNull();
        ArgumentNullException.ThrowIfNull(testOrganisation);
        ArgumentNullException.ThrowIfNull(testOrganisation.Services);
        results.Items[0].Should().BeEquivalentTo(testOrganisation.Services.ElementAt(0));
    }

    [Fact]
    public async Task ThenDeleteService()
    {
        //Arrange
        var myProfile = new AutoMappingProfiles();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
        var mapper = new Mapper(configuration);
        var logger = new Mock<ILogger<CreateOrganisationCommandHandler>>();
        var mockApplicationDbContext = GetApplicationDbContext();
        var testOrganisation = TestDataProvider.GetTestCountyCouncilDto(false, false, true);
        var createCommand = new CreateOrganisationCommand(testOrganisation);
        var createHandler = new CreateOrganisationCommandHandler(mockApplicationDbContext, mapper, logger.Object);
        await createHandler.Handle(createCommand, new CancellationToken());


        var command = new DeleteServiceByIdCommand("3010521b-6e0a-41b0-b610-200edbbeeb14");
        var handler = new DeleteServiceByIdCommandHandler(mockApplicationDbContext, new Mock<ILogger<DeleteServiceByIdCommandHandler>>().Object);

        //Act
        var results = await handler.Handle(command, new CancellationToken());

        //Assert
        results.Should().Be(true);

    }

    [Fact]
    public async Task ThenDeleteServiceThatDoesNotExist()
    {
        //Arrange
        var mockApplicationDbContext = GetApplicationDbContext();
        var command = new DeleteServiceByIdCommand(Guid.NewGuid().ToString());
        var handler = new DeleteServiceByIdCommandHandler(mockApplicationDbContext, new Mock<ILogger<DeleteServiceByIdCommandHandler>>().Object);

        // Act 
        // Assert
        await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, new CancellationToken()));

    }

    [Fact]
    public async Task ThenUpdateService()
    {
        //Arrange
        var myProfile = new AutoMappingProfiles();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
        var mapper = new Mapper(configuration);
        var logger = new Mock<ILogger<CreateOrganisationCommandHandler>>();
        var mockApplicationDbContext = GetApplicationDbContext();
        var testOrganisation = TestDataProvider.GetTestCountyCouncilDto(false, false, true);
        var orgCommand = new CreateOrganisationCommand(testOrganisation);
        var orgHandler = new CreateOrganisationCommandHandler(mockApplicationDbContext, mapper, logger.Object);
        await orgHandler.Handle(orgCommand, new CancellationToken());
        testOrganisation = TestDataProvider.GetTestCountyCouncilDto(true);
        var command = new UpdateServiceCommand(testOrganisation.Services?.ElementAt(0).Id ?? string.Empty, testOrganisation.Services?.ElementAt(0) ?? default!);
        var handler = new UpdateServiceCommandHandler(mockApplicationDbContext, mapper, new Mock<ILogger<UpdateServiceCommandHandler>>().Object);


        //Act
        var result = await handler.Handle(command, new CancellationToken());

        //Assert
        result.Should().NotBeNull();
        result.Should().Be(testOrganisation.Services?.ElementAt(0).Id);
    }


    [Fact]
    public async Task ThenUpdateServiceWithNewNestedRecords()
    {
        //Arrange
        var myProfile = new AutoMappingProfiles();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
        var mapper = new Mapper(configuration);
        var logger = new Mock<ILogger<CreateOrganisationCommandHandler>>();
        var mockApplicationDbContext = GetApplicationDbContext();
        var testOrganisation = TestDataProvider.GetTestCountyCouncilDto(false, false, true);
        var orgCommand = new CreateOrganisationCommand(testOrganisation);
        var orgHandler = new CreateOrganisationCommandHandler(mockApplicationDbContext, mapper, logger.Object);
        await orgHandler.Handle(orgCommand, new CancellationToken());
        testOrganisation = TestDataProvider.GetTestCountyCouncilDto(false, true);
        var command = new UpdateServiceCommand(
            testOrganisation.Services?.ElementAt(0).Id ?? string.Empty,
            testOrganisation.Services?.ElementAt(0) ?? default!);
        var handler = new UpdateServiceCommandHandler(mockApplicationDbContext, mapper,
            new Mock<ILogger<UpdateServiceCommandHandler>>().Object);


        //Act
        var result = await handler.Handle(command, new CancellationToken());

        //Assert
        result.Should().NotBeNull();
        result.Should().Be(testOrganisation.Services?.ElementAt(0).Id);
    }
}
