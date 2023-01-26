using AutoMapper;
using FamilyHubs.ServiceDirectory.Api.Commands.CreateOrganisation;
using FamilyHubs.ServiceDirectory.Api.Commands.DeleteService;
using FamilyHubs.ServiceDirectory.Api.Queries.GetServices;
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
        CreateOrganisationCommand command = new(testOrganisation);
        CreateOrganisationCommandHandler handler = new(mockApplicationDbContext, mapper, logger.Object);
        
        //Act
        var result = await handler.Handle(command, new CancellationToken());

        //Assert
        result.Should().NotBeNull();
        result.Should().Be(testOrganisation.Id);
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
}
