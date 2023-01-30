using Ardalis.GuardClauses;
using AutoMapper;
using FamilyHubs.ServiceDirectoryApi.UnitTests.Organisation;
using fh_service_directory_api.api.Commands.CreateOpenReferralOrganisation;
using fh_service_directory_api.api.Commands.CreateOpenReferralService;
using fh_service_directory_api.api.Commands.DeleteOpenReferralService;
using fh_service_directory_api.api.Commands.UpdateOpenReferralService;
using fh_service_directory_api.api.Queries.GetOpenReferralServicesByOrganisation;
using fh_service_directory_api.api.Queries.GetServices;
using fh_service_directory_api.core;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace FamilyHubs.ServiceDirectoryApi.UnitTests.Services;

public class WhenUsingServiceCommands : BaseCreateDbUnitTest
{
    [Fact]
    public async Task ThenCreateOpenReferralService()
    {
        //Arange
        var myProfile = new AutoMappingProfiles();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
        IMapper mapper = new Mapper(configuration);
        var logger = new Mock<ILogger<CreateOpenReferralOrganisationCommandHandler>>();
        var mockApplicationDbContext = GetApplicationDbContext();
        var testOrganisation = WhenUsingOrganisationCommands.GetTestCountyCouncilDto();
        testOrganisation.Services = default!;
        CreateOpenReferralOrganisationCommand orgcommand = new(testOrganisation);
        CreateOpenReferralOrganisationCommandHandler orghandler = new(mockApplicationDbContext, mapper, logger.Object);
        var orgresult = await orghandler.Handle(orgcommand, new System.Threading.CancellationToken());
        testOrganisation = WhenUsingOrganisationCommands.GetTestCountyCouncilDto();
        CreateOpenReferralServiceCommand command = new(testOrganisation?.Services?.ElementAt(0) ?? default!);
        CreateOpenReferralServiceCommandHandler handler = new(mockApplicationDbContext, mapper, new Mock<ILogger<CreateOpenReferralServiceCommandHandler>>().Object);


        //Act
        var result = await handler.Handle(command, new System.Threading.CancellationToken());

        //Assert
        result.Should().NotBeNull();
        result.Should().Be(testOrganisation?.Services?.ElementAt(0).Id);
    }

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
    public async Task ThenGetOpenReferralServicesByOrganisationId()
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


        GetOpenReferralServicesByOrganisationIdCommand command = new(testOrganisation.Id);
        GetOpenReferralServicesByOrganisationIdCommandHandler handler = new(mockApplicationDbContext);

        //Act
        var results = await handler.Handle(command, new CancellationToken());

        //Assert
        results.Should().NotBeNull();
        ArgumentNullException.ThrowIfNull(testOrganisation, nameof(testOrganisation));
        ArgumentNullException.ThrowIfNull(testOrganisation.Services, nameof(testOrganisation.Services));
        results[0].Should().BeEquivalentTo(testOrganisation.Services.ElementAt(0));
    }

    [Fact]
    public async Task ThenGetOpenReferralServicesByOrganisationId_ShouldThrowExceptionWhenNoOrgaisations()
    {
        //Arange
        var myProfile = new AutoMappingProfiles();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
        IMapper mapper = new Mapper(configuration);
        var logger = new Mock<ILogger<CreateOpenReferralOrganisationCommandHandler>>();
        var mockApplicationDbContext = GetApplicationDbContext();
        var testOrganisation = WhenUsingOrganisationCommands.GetTestCountyCouncilDto();
        GetOpenReferralServicesByOrganisationIdCommand command = new(testOrganisation.Id);
        GetOpenReferralServicesByOrganisationIdCommandHandler handler = new(mockApplicationDbContext);

        // Act 
        Func<Task> act = () => handler.Handle(command, new CancellationToken());


        // Assert
        var exception = await Assert.ThrowsAsync<NotFoundException>(act);

    }

    [Fact]
    public async Task ThenGetOpenReferralServicesByOrganisationId_ShouldThrowExceptionWhenNoServices()
    {
        //Arange
        var myProfile = new AutoMappingProfiles();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
        IMapper mapper = new Mapper(configuration);
        var logger = new Mock<ILogger<CreateOpenReferralOrganisationCommandHandler>>();
        var mockApplicationDbContext = GetApplicationDbContext();
        var testOrganisation = WhenUsingOrganisationCommands.GetTestCountyCouncilDto();
        testOrganisation.Services = default!;
        CreateOpenReferralOrganisationCommand createcommand = new(testOrganisation);
        CreateOpenReferralOrganisationCommandHandler createhandler = new(mockApplicationDbContext, mapper, logger.Object);
        var id = await createhandler.Handle(createcommand, new CancellationToken());

        GetOpenReferralServicesByOrganisationIdCommand command = new(testOrganisation.Id);
        GetOpenReferralServicesByOrganisationIdCommandHandler handler = new(mockApplicationDbContext);

        // Act 
        Func<Task> act = () => handler.Handle(command, new CancellationToken());


        // Assert
        var exception = await Assert.ThrowsAsync<NotFoundException>(act);

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

    [Fact]
    public async Task ThenDeleteOpenReferralServiceThatDoesNotExist()
    {
        //Arange
        var myProfile = new AutoMappingProfiles();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
        IMapper mapper = new Mapper(configuration);
        var logger = new Mock<ILogger<CreateOpenReferralOrganisationCommandHandler>>();
        var mockApplicationDbContext = GetApplicationDbContext();
        DeleteOpenReferralServiceByIdCommand command = new(Guid.NewGuid().ToString());
        DeleteOpenReferralServiceByIdCommandHandler handler = new(mockApplicationDbContext, new Mock<ILogger<DeleteOpenReferralServiceByIdCommandHandler>>().Object);

        // Act 
        Func<Task> act = () => handler.Handle(command, new CancellationToken());


        // Assert
        var exception = await Assert.ThrowsAsync<Exception>(act);

    }

    [Fact]
    public async Task ThenUpdateOpenReferralService()
    {
        //Arange
        var myProfile = new AutoMappingProfiles();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
        IMapper mapper = new Mapper(configuration);
        var logger = new Mock<ILogger<CreateOpenReferralOrganisationCommandHandler>>();
        var mockApplicationDbContext = GetApplicationDbContext();
        var testOrganisation = WhenUsingOrganisationCommands.GetTestCountyCouncilDto();
        CreateOpenReferralOrganisationCommand orgcommand = new(testOrganisation);
        CreateOpenReferralOrganisationCommandHandler orghandler = new(mockApplicationDbContext, mapper, logger.Object);
        var orgresult = await orghandler.Handle(orgcommand, new System.Threading.CancellationToken());
        testOrganisation = WhenUsingOrganisationCommands.GetTestCountyCouncilDto(true);
        UpdateOpenReferralServiceCommand command = new(testOrganisation?.Services?.ElementAt(0)?.Id ?? string.Empty, testOrganisation?.Services?.ElementAt(0) ?? default!);
        UpdateOpenReferralServiceCommandHandler handler = new(mockApplicationDbContext, mapper, new Mock<ILogger<UpdateOpenReferralServiceCommandHandler>>().Object);


        //Act
        var result = await handler.Handle(command, new System.Threading.CancellationToken());

        //Assert
        result.Should().NotBeNull();
        result.Should().Be(testOrganisation?.Services?.ElementAt(0).Id);
    }


    [Fact]
    public async Task ThenUpdateOpenReferralServiceWithNewNestedRecords()
    {
        //Arange
        var myProfile = new AutoMappingProfiles();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
        IMapper mapper = new Mapper(configuration);
        var logger = new Mock<ILogger<CreateOpenReferralOrganisationCommandHandler>>();
        var mockApplicationDbContext = GetApplicationDbContext();
        var testOrganisation = WhenUsingOrganisationCommands.GetTestCountyCouncilDto();
        CreateOpenReferralOrganisationCommand orgcommand = new(testOrganisation);
        CreateOpenReferralOrganisationCommandHandler orghandler = new(mockApplicationDbContext, mapper, logger.Object);
        var orgresult = await orghandler.Handle(orgcommand, new System.Threading.CancellationToken());
        testOrganisation = WhenUsingOrganisationCommands.GetTestCountyCouncilDto(false, true);
        UpdateOpenReferralServiceCommand command = new(testOrganisation?.Services?.ElementAt(0)?.Id ?? string.Empty, testOrganisation?.Services?.ElementAt(0) ?? default!);
        UpdateOpenReferralServiceCommandHandler handler = new(mockApplicationDbContext, mapper, new Mock<ILogger<UpdateOpenReferralServiceCommandHandler>>().Object);


        //Act
        var result = await handler.Handle(command, new System.Threading.CancellationToken());

        //Assert
        result.Should().NotBeNull();
        result.Should().Be(testOrganisation?.Services?.ElementAt(0).Id);
    }
}
