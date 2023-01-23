using AutoMapper;
using FamilyHubs.ServiceDirectoryApi.UnitTests.Organisation;
using fh_service_directory_api.api.Commands.CreateOpenReferralOrganisation;
using fh_service_directory_api.api.Commands.DeleteOpenReferralService;
using fh_service_directory_api.api.Queries.GetServices;
using fh_service_directory_api.core;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace FamilyHubs.ServiceDirectoryApi.UnitTests.Services;

public class WhenUsingServiceCommands : BaseCreateDbUnitTest
{
    [Fact]
    public async Task ThenGetOpenReferralService()
    {
        //Arange
        var myProfile = new AutoMappingProfiles();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
        IMapper mapper = new Mapper(configuration);
        var logger = new Mock<ILogger<CreateOpenReferralOrganisationCommandHandler>>();
        var mockApplicationDbContext = GetApplicationDbContext();
        var testOrganisation = WhenUsingOrganisationCommands.GetTestCountyCouncilDto();
        CreateOpenReferralOrganisationCommand createcommand = new(testOrganisation);
        CreateOpenReferralOrganisationCommandHandler createhandler = new(mockApplicationDbContext, mapper, logger.Object);
        var id = await createhandler.Handle(createcommand, new CancellationToken());


        GetOpenReferralServicesCommand command = new("Information Sharing", "active", "XTEST", null, null, null, null, null, null, 1, 10, null, null, null, null, null, null, null, null);
        GetOpenReferralServicesCommandHandler handler = new(mockApplicationDbContext);

        //Act
        var results = await handler.Handle(command, new CancellationToken());

        //Assert
        results.Should().NotBeNull();
        ArgumentNullException.ThrowIfNull(testOrganisation, nameof(testOrganisation));
        ArgumentNullException.ThrowIfNull(testOrganisation.Services, nameof(testOrganisation.Services));
        results.Items[0].Should().BeEquivalentTo(testOrganisation.Services.ElementAt(0));
    }

    [Fact]
    public async Task ThenGetOpenReferralServiceThatArePaidFor()
    {
        //Arange
        var myProfile = new AutoMappingProfiles();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
        IMapper mapper = new Mapper(configuration);
        var logger = new Mock<ILogger<CreateOpenReferralOrganisationCommandHandler>>();
        var mockApplicationDbContext = GetApplicationDbContext();
        var testOrganisation = WhenUsingOrganisationCommands.GetTestCountyCouncilDto();
        CreateOpenReferralOrganisationCommand createcommand = new(testOrganisation);
        CreateOpenReferralOrganisationCommandHandler createhandler = new(mockApplicationDbContext, mapper, logger.Object);
        var id = await createhandler.Handle(createcommand, new CancellationToken());


        GetOpenReferralServicesCommand command = new("Information Sharing", "active", "XTEST", null, null, null, null, null, null, 1, 10, null, null, true, null, null, null, null, null);
        GetOpenReferralServicesCommandHandler handler = new(mockApplicationDbContext);

        //Act
        var results = await handler.Handle(command, new CancellationToken());

        //Assert
        results.Should().NotBeNull();
        results.Items.Count().Should().Be(0);
    }

    //todo: add similar test for no_cost options
    [Fact]
    public async Task ThenGetOpenReferralServiceThatAreFree()
    {
        //Arange
        var myProfile = new AutoMappingProfiles();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
        IMapper mapper = new Mapper(configuration);
        var logger = new Mock<ILogger<CreateOpenReferralOrganisationCommandHandler>>();
        var mockApplicationDbContext = GetApplicationDbContext();
        var testOrganisation = WhenUsingOrganisationCommands.GetTestCountyCouncilDto();
        CreateOpenReferralOrganisationCommand createcommand = new(testOrganisation);
        CreateOpenReferralOrganisationCommandHandler createhandler = new(mockApplicationDbContext, mapper, logger.Object);
        var id = await createhandler.Handle(createcommand, new CancellationToken());


        GetOpenReferralServicesCommand command = new("Information Sharing", "active", "XTEST", null, null, null, null, null, null, 1, 10, null, null, false, null, null, null, null, null);
        GetOpenReferralServicesCommandHandler handler = new(mockApplicationDbContext);

        //Act
        var results = await handler.Handle(command, new CancellationToken());

        //Assert
        results.Should().NotBeNull();
        ArgumentNullException.ThrowIfNull(testOrganisation, nameof(testOrganisation));
        ArgumentNullException.ThrowIfNull(testOrganisation.Services, nameof(testOrganisation.Services));
        results.Items[0].Should().BeEquivalentTo(testOrganisation.Services.ElementAt(0));
    }

    [Fact]
    public async Task ThenDeleteOpenReferralService()
    {
        //Arange
        var myProfile = new AutoMappingProfiles();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
        IMapper mapper = new Mapper(configuration);
        var logger = new Mock<ILogger<CreateOpenReferralOrganisationCommandHandler>>();
        var mockApplicationDbContext = GetApplicationDbContext();
        var testOrganisation = WhenUsingOrganisationCommands.GetTestCountyCouncilDto();
        CreateOpenReferralOrganisationCommand createcommand = new(testOrganisation);
        CreateOpenReferralOrganisationCommandHandler createhandler = new(mockApplicationDbContext, mapper, logger.Object);
        var id = await createhandler.Handle(createcommand, new CancellationToken());


        DeleteOpenReferralServiceByIdCommand command = new("3010521b-6e0a-41b0-b610-200edbbeeb14");
        DeleteOpenReferralServiceByIdCommandHandler handler = new(mockApplicationDbContext, new Mock<ILogger<DeleteOpenReferralServiceByIdCommandHandler>>().Object);

        //Act
        var results = await handler.Handle(command, new CancellationToken());

        //Assert
        results.Should().Be(true);
        
    }
}
