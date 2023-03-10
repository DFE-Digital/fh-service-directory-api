using Ardalis.GuardClauses;
using AutoMapper;
using FamilyHubs.ServiceDirectory.Api.Commands.CreateOrganisation;
using FamilyHubs.ServiceDirectory.Api.Commands.CreateService;
using FamilyHubs.ServiceDirectory.Api.Commands.DeleteService;
using FamilyHubs.ServiceDirectory.Api.Commands.UpdateService;
using FamilyHubs.ServiceDirectory.Api.Queries.GetServices;
using FamilyHubs.ServiceDirectory.Api.Queries.GetServicesByOrganisation;
using FamilyHubs.ServiceDirectory.Core;
using FamilyHubs.ServiceDirectory.Infrastructure.Persistence.Repository;
using FamilyHubs.ServiceDirectory.Shared.Dto;
using FamilyHubs.ServiceDirectory.Shared.Enums;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace FamilyHubs.ServiceDirectoryApi.UnitTests.Services;

public class WhenUsingGetServiceCommand : BaseCreateDbUnitTest
{
    public WhenUsingGetServiceCommand()
    {
        var myProfile = new AutoMappingProfiles();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
        Mapper = new Mapper(configuration);
        MockApplicationDbContext = GetApplicationDbContext();

        MockMediatR = new Mock<ISender>();
        var createServiceCommandHandler = new CreateServiceCommandHandler(MockApplicationDbContext, Mapper, NullLogger<CreateServiceCommandHandler>.Instance);
        var updateServiceCommandHandler = new UpdateServiceCommandHandler(MockApplicationDbContext, Mapper, NullLogger<UpdateServiceCommandHandler>.Instance);
        MockMediatR.Setup(m => m.Send(It.IsAny<CreateServiceCommand>(), It.IsAny<CancellationToken>()))
            .Callback<object, CancellationToken>((notification, cToken) =>
                createServiceCommandHandler.Handle((CreateServiceCommand)notification, cToken).GetAwaiter().GetResult());

        MockMediatR.Setup(m => m.Send(It.IsAny<UpdateServiceCommand>(), It.IsAny<CancellationToken>()))
            .Callback<object, CancellationToken>((notification, cToken) =>
                updateServiceCommandHandler.Handle((UpdateServiceCommand)notification, cToken).GetAwaiter().GetResult());

        TestOrganisation = TestDataProvider.GetTestCountyCouncilDto();
    }

    private OrganisationWithServicesDto TestOrganisation { get; }

    private Mock<ISender> MockMediatR { get; }

    private ApplicationDbContext MockApplicationDbContext { get; }

    private IMapper Mapper { get; }
    private static NullLogger<T> GetLogger<T>() => new NullLogger<T>();

    private void CreateOrganisation()
    {
        var createOrganisationCommand = new CreateOrganisationCommand(TestOrganisation);

        var handler = new CreateOrganisationCommandHandler(MockApplicationDbContext, Mapper, MockMediatR.Object, GetLogger<CreateOrganisationCommandHandler>());

        handler.Handle(createOrganisationCommand, new CancellationToken()).GetAwaiter().GetResult();
    }

    [Fact]
    public async Task ThenGetService()
    {
        //Arrange
        CreateOrganisation();

        var command = new GetServicesCommand(ServiceType.InformationSharing, ServiceStatusType.Active, "XTEST", null, null, null,
            null, null, null, 1, 10, null, null, null, null, null, null, null, null);
        var handler = new GetServicesCommandHandler(MockApplicationDbContext, Mapper);

        //Act
        var results = await handler.Handle(command, new CancellationToken());

        //Assert
        results.Should().NotBeNull();
        ArgumentNullException.ThrowIfNull(TestOrganisation);
        ArgumentNullException.ThrowIfNull(TestOrganisation.Services);
        results.Items[0].Should().BeEquivalentTo(TestOrganisation.Services.ElementAt(0));
    }

    [Fact]
    public async Task ThenGetServicesByOrganisationId()
    {
        //Arrange
        CreateOrganisation();

        var command = new GetServicesByOrganisationIdCommand(TestOrganisation.Id);
        var handler = new GetServicesByOrganisationIdCommandHandler(MockApplicationDbContext, Mapper);

        //Act
        var results = await handler.Handle(command, new CancellationToken());

        //Assert
        results.Should().NotBeNull();
        ArgumentNullException.ThrowIfNull(TestOrganisation);
        ArgumentNullException.ThrowIfNull(TestOrganisation.Services);
        results[0].Should().BeEquivalentTo(TestOrganisation.Services.ElementAt(0));
    }

    [Fact]
    public async Task ThenGetServicesByOrganisationId_ShouldThrowExceptionWhenNoOrganisations()
    {
        //Arrange
        var command = new GetServicesByOrganisationIdCommand(TestOrganisation.Id);
        var handler = new GetServicesByOrganisationIdCommandHandler(MockApplicationDbContext, Mapper);

        // Act 
        // Assert
        await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, new CancellationToken()));

    }

    [Fact]
    public async Task ThenGetServicesByOrganisationId_ShouldThrowExceptionWhenNoServices()
    {
        //Arrange
        var command = new GetServicesByOrganisationIdCommand(TestOrganisation.Id);
        var handler = new GetServicesByOrganisationIdCommandHandler(MockApplicationDbContext, Mapper);

        // Act 
        // Assert
        await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, new CancellationToken()));

    }

    [Fact]
    public async Task ThenGetServiceThatArePaidFor()
    {
        //Arrange
        CreateOrganisation();

        var command = new GetServicesCommand(ServiceType.InformationSharing, ServiceStatusType.Active, "XTEST", null, null, null,
            null, null, null, 1, 10, null, null, true, null, null, null, null, null);
        var handler = new GetServicesCommandHandler(MockApplicationDbContext, Mapper);

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
        CreateOrganisation();

        var command = new GetServicesCommand(ServiceType.InformationSharing, ServiceStatusType.Active, "XTEST", null, null, null,
            null, null, null, 1, 10, null, null, false, null, null, null, null, null);
        var handler = new GetServicesCommandHandler(MockApplicationDbContext, Mapper);

        //Act
        var results = await handler.Handle(command, new CancellationToken());

        //Assert
        results.Should().NotBeNull();
        ArgumentNullException.ThrowIfNull(TestOrganisation);
        ArgumentNullException.ThrowIfNull(TestOrganisation.Services);
        results.Items[0].Should().BeEquivalentTo(TestOrganisation.Services.ElementAt(0));
    }

    [Fact]
    public async Task ThenDeleteService()
    {
        //Arrange
        CreateOrganisation();

        var command = new DeleteServiceByIdCommand(1);
        var handler = new DeleteServiceByIdCommandHandler(MockApplicationDbContext, new Mock<ILogger<DeleteServiceByIdCommandHandler>>().Object);

        //Act
        var results = await handler.Handle(command, new CancellationToken());

        //Assert
        results.Should().Be(true);

    }

    [Fact]
    public async Task ThenDeleteServiceThatDoesNotExist()
    {
        //Arrange
        var command = new DeleteServiceByIdCommand(Random.Shared.Next());
        var handler = new DeleteServiceByIdCommandHandler(MockApplicationDbContext, new Mock<ILogger<DeleteServiceByIdCommandHandler>>().Object);

        // Act 
        // Assert
        await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, new CancellationToken()));

    }
}
