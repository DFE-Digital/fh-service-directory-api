using Ardalis.GuardClauses;
using AutoMapper;
using FamilyHubs.ServiceDirectory.Api.Commands.CreateOrganisation;
using FamilyHubs.ServiceDirectory.Api.Commands.CreateService;
using FamilyHubs.ServiceDirectory.Api.Commands.DeleteService;
using FamilyHubs.ServiceDirectory.Api.Commands.UpdateService;
using FamilyHubs.ServiceDirectory.Api.Queries.GetServices;
using FamilyHubs.ServiceDirectory.Api.Queries.GetServicesByOrganisation;
using FamilyHubs.ServiceDirectory.Core;
using FamilyHubs.ServiceDirectoryApi.UnitTests.Organisation;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace FamilyHubs.ServiceDirectoryApi.UnitTests.Services;

public class WhenUsingServiceCommands : BaseCreateDbUnitTest
{
    [Fact]
    public async Task ThenCreateService()
    {
        //Arange
        var myProfile = new AutoMappingProfiles();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
        IMapper mapper = new Mapper(configuration);
        var logger = new Mock<ILogger<CreateOrganisationCommandHandler>>();
        var mockApplicationDbContext = GetApplicationDbContext();
        var testOrganisation = WhenUsingOrganisationCommands.GetTestCountyCouncilDto();
        testOrganisation.Services = default!;
        CreateOrganisationCommand orgcommand = new(testOrganisation);
        CreateOrganisationCommandHandler orghandler = new(mockApplicationDbContext, mapper, logger.Object);
        await orghandler.Handle(orgcommand, new CancellationToken());
        testOrganisation = WhenUsingOrganisationCommands.GetTestCountyCouncilDto();
        CreateServiceCommand command = new(testOrganisation?.Services?.ElementAt(0) ?? default!);
        CreateServiceCommandHandler handler = new(mockApplicationDbContext, mapper, new Mock<ILogger<CreateServiceCommandHandler>>().Object);


        //Act
        var result = await handler.Handle(command, new CancellationToken());

        //Assert
        result.Should().NotBeNull();
        result.Should().Be(testOrganisation?.Services?.ElementAt(0).Id);
    }

    [Fact]
    public async Task ThenGetService()
    {
        //Arange
        var myProfile = new AutoMappingProfiles();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
        IMapper mapper = new Mapper(configuration);
        var logger = new Mock<ILogger<CreateOrganisationCommandHandler>>();
        var mockApplicationDbContext = GetApplicationDbContext();
        var testOrganisation = WhenUsingOrganisationCommands.GetTestCountyCouncilDto();
        CreateOrganisationCommand createcommand = new(testOrganisation);
        CreateOrganisationCommandHandler createhandler = new(mockApplicationDbContext, mapper, logger.Object);
        var id = await createhandler.Handle(createcommand, new CancellationToken());


        GetServicesCommand command = new("Information Sharing", "active", "XTEST", null, null, null, null, null, null, 1, 10, null, null, null, null, null, null, null, null);
        GetServicesCommandHandler handler = new(mockApplicationDbContext);

        //Act
        var results = await handler.Handle(command, new CancellationToken());

        //Assert
        results.Should().NotBeNull();
        ArgumentNullException.ThrowIfNull(testOrganisation, nameof(testOrganisation));
        ArgumentNullException.ThrowIfNull(testOrganisation.Services, nameof(testOrganisation.Services));
        results.Items[0].Should().BeEquivalentTo(testOrganisation.Services.ElementAt(0));
    }

    [Fact]
    public async Task ThenGetServicesByOrganisationId()
    {
        //Arange
        var myProfile = new AutoMappingProfiles();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
        IMapper mapper = new Mapper(configuration);
        var logger = new Mock<ILogger<CreateOrganisationCommandHandler>>();
        var mockApplicationDbContext = GetApplicationDbContext();
        var testOrganisation = WhenUsingOrganisationCommands.GetTestCountyCouncilDto();
        CreateOrganisationCommand createcommand = new(testOrganisation);
        CreateOrganisationCommandHandler createhandler = new(mockApplicationDbContext, mapper, logger.Object);
        var id = await createhandler.Handle(createcommand, new CancellationToken());


        GetServicesByOrganisationIdCommand command = new(testOrganisation.Id);
        GetServicesByOrganisationIdCommandHandler handler = new(mockApplicationDbContext);

        //Act
        var results = await handler.Handle(command, new CancellationToken());

        //Assert
        results.Should().NotBeNull();
        ArgumentNullException.ThrowIfNull(testOrganisation, nameof(testOrganisation));
        ArgumentNullException.ThrowIfNull(testOrganisation.Services, nameof(testOrganisation.Services));
        results[0].Should().BeEquivalentTo(testOrganisation.Services.ElementAt(0));
    }

    [Fact]
    public async Task ThenGetServicesByOrganisationId_ShouldThrowExceptionWhenNoOrgaisations()
    {
        //Arange
        var mockApplicationDbContext = GetApplicationDbContext();
        var testOrganisation = WhenUsingOrganisationCommands.GetTestCountyCouncilDto();
        GetServicesByOrganisationIdCommand command = new(testOrganisation.Id);
        GetServicesByOrganisationIdCommandHandler handler = new(mockApplicationDbContext);

        // Act 
        Func<Task> act = () => handler.Handle(command, new CancellationToken());


        // Assert
        await Assert.ThrowsAsync<NotFoundException>(act);

    }

    [Fact]
    public async Task ThenGetServicesByOrganisationId_ShouldThrowExceptionWhenNoServices()
    {
        //Arange
        var myProfile = new AutoMappingProfiles();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
        IMapper mapper = new Mapper(configuration);
        var logger = new Mock<ILogger<CreateOrganisationCommandHandler>>();
        var mockApplicationDbContext = GetApplicationDbContext();
        var testOrganisation = WhenUsingOrganisationCommands.GetTestCountyCouncilDto();
        testOrganisation.Services = default!;
        CreateOrganisationCommand createcommand = new(testOrganisation);
        CreateOrganisationCommandHandler createhandler = new(mockApplicationDbContext, mapper, logger.Object);
        var id = await createhandler.Handle(createcommand, new CancellationToken());

        GetServicesByOrganisationIdCommand command = new(testOrganisation.Id);
        GetServicesByOrganisationIdCommandHandler handler = new(mockApplicationDbContext);

        // Act 
        Func<Task> act = () => handler.Handle(command, new CancellationToken());


        // Assert
        await Assert.ThrowsAsync<NotFoundException>(act);

    }

    [Fact]
    public async Task ThenGetServiceThatArePaidFor()
    {
        //Arange
        var myProfile = new AutoMappingProfiles();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
        IMapper mapper = new Mapper(configuration);
        var logger = new Mock<ILogger<CreateOrganisationCommandHandler>>();
        var mockApplicationDbContext = GetApplicationDbContext();
        var testOrganisation = WhenUsingOrganisationCommands.GetTestCountyCouncilDto();
        CreateOrganisationCommand createcommand = new(testOrganisation);
        CreateOrganisationCommandHandler createhandler = new(mockApplicationDbContext, mapper, logger.Object);
        var id = await createhandler.Handle(createcommand, new CancellationToken());


        GetServicesCommand command = new("Information Sharing", "active", "XTEST", null, null, null, null, null, null, 1, 10, null, null, true, null, null, null, null, null);
        GetServicesCommandHandler handler = new(mockApplicationDbContext);

        //Act
        var results = await handler.Handle(command, new CancellationToken());

        //Assert
        results.Should().NotBeNull();
        results.Items.Count().Should().Be(0);
    }

    //todo: add similar test for no_cost options
    [Fact]
    public async Task ThenGetServiceThatAreFree()
    {
        //Arange
        var myProfile = new AutoMappingProfiles();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
        IMapper mapper = new Mapper(configuration);
        var logger = new Mock<ILogger<CreateOrganisationCommandHandler>>();
        var mockApplicationDbContext = GetApplicationDbContext();
        var testOrganisation = WhenUsingOrganisationCommands.GetTestCountyCouncilDto();
        CreateOrganisationCommand createcommand = new(testOrganisation);
        CreateOrganisationCommandHandler createhandler = new(mockApplicationDbContext, mapper, logger.Object);
        var id = await createhandler.Handle(createcommand, new CancellationToken());


        GetServicesCommand command = new("Information Sharing", "active", "XTEST", null, null, null, null, null, null, 1, 10, null, null, false, null, null, null, null, null);
        GetServicesCommandHandler handler = new(mockApplicationDbContext);

        //Act
        var results = await handler.Handle(command, new CancellationToken());

        //Assert
        results.Should().NotBeNull();
        ArgumentNullException.ThrowIfNull(testOrganisation, nameof(testOrganisation));
        ArgumentNullException.ThrowIfNull(testOrganisation.Services, nameof(testOrganisation.Services));
        results.Items[0].Should().BeEquivalentTo(testOrganisation.Services.ElementAt(0));
    }

    [Fact]
    public async Task ThenDeleteService()
    {
        //Arange
        var myProfile = new AutoMappingProfiles();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
        IMapper mapper = new Mapper(configuration);
        var logger = new Mock<ILogger<CreateOrganisationCommandHandler>>();
        var mockApplicationDbContext = GetApplicationDbContext();
        var testOrganisation = WhenUsingOrganisationCommands.GetTestCountyCouncilDto();
        CreateOrganisationCommand createcommand = new(testOrganisation);
        CreateOrganisationCommandHandler createhandler = new(mockApplicationDbContext, mapper, logger.Object);
        var id = await createhandler.Handle(createcommand, new CancellationToken());


        DeleteServiceByIdCommand command = new("3010521b-6e0a-41b0-b610-200edbbeeb14");
        DeleteServiceByIdCommandHandler handler = new(mockApplicationDbContext, new Mock<ILogger<DeleteServiceByIdCommandHandler>>().Object);

        //Act
        var results = await handler.Handle(command, new CancellationToken());

        //Assert
        results.Should().Be(true);

    }

    [Fact]
    public async Task ThenDeleteServiceThatDoesNotExist()
    {
        //Arange
        var myProfile = new AutoMappingProfiles();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
        IMapper mapper = new Mapper(configuration);
        var logger = new Mock<ILogger<CreateOrganisationCommandHandler>>();
        var mockApplicationDbContext = GetApplicationDbContext();
        DeleteServiceByIdCommand command = new(Guid.NewGuid().ToString());
        DeleteServiceByIdCommandHandler handler = new(mockApplicationDbContext, new Mock<ILogger<DeleteServiceByIdCommandHandler>>().Object);

        // Act 
        Func<Task> act = () => handler.Handle(command, new CancellationToken());


        // Assert
        await Assert.ThrowsAsync<Exception>(act);

    }

    [Fact]
    public async Task ThenUpdateService()
    {
        //Arange
        var myProfile = new AutoMappingProfiles();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
        IMapper mapper = new Mapper(configuration);
        var logger = new Mock<ILogger<CreateOrganisationCommandHandler>>();
        var mockApplicationDbContext = GetApplicationDbContext();
        var testOrganisation = WhenUsingOrganisationCommands.GetTestCountyCouncilDto();
        CreateOrganisationCommand orgcommand = new(testOrganisation);
        CreateOrganisationCommandHandler orghandler = new(mockApplicationDbContext, mapper, logger.Object);
        var orgresult = await orghandler.Handle(orgcommand, new CancellationToken());
        testOrganisation = WhenUsingOrganisationCommands.GetTestCountyCouncilDto(true);
        UpdateServiceCommand command = new(testOrganisation?.Services?.ElementAt(0)?.Id ?? string.Empty, testOrganisation?.Services?.ElementAt(0) ?? default!);
        UpdateServiceCommandHandler handler = new(mockApplicationDbContext, mapper, new Mock<ILogger<UpdateServiceCommandHandler>>().Object);


        //Act
        var result = await handler.Handle(command, new CancellationToken());

        //Assert
        result.Should().NotBeNull();
        result.Should().Be(testOrganisation?.Services?.ElementAt(0).Id);
    }


    [Fact]
    public async Task ThenUpdateServiceWithNewNestedRecords()
    {
        //Arange
        var myProfile = new AutoMappingProfiles();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
        IMapper mapper = new Mapper(configuration);
        var logger = new Mock<ILogger<CreateOrganisationCommandHandler>>();
        var mockApplicationDbContext = GetApplicationDbContext();
        var testOrganisation = WhenUsingOrganisationCommands.GetTestCountyCouncilDto();
        CreateOrganisationCommand orgcommand = new(testOrganisation);
        CreateOrganisationCommandHandler orghandler = new(mockApplicationDbContext, mapper, logger.Object);
        var orgresult = await orghandler.Handle(orgcommand, new CancellationToken());
        testOrganisation = WhenUsingOrganisationCommands.GetTestCountyCouncilDto(false, true);
        UpdateServiceCommand command = new(testOrganisation?.Services?.ElementAt(0)?.Id ?? string.Empty, testOrganisation?.Services?.ElementAt(0) ?? default!);
        UpdateServiceCommandHandler handler = new(mockApplicationDbContext, mapper, new Mock<ILogger<UpdateServiceCommandHandler>>().Object);


        //Act
        var result = await handler.Handle(command, new CancellationToken());

        //Assert
        result.Should().NotBeNull();
        result.Should().Be(testOrganisation?.Services?.ElementAt(0).Id);
    }
}
