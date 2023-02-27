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
        CreateOrganisation();

        var command = new GetServicesCommand("Information Sharing", "active", "XTEST", null, null, null,
            null, null, null, 1, 10, null, null, null, null, null, null, null, null);
        var handler = new GetServicesCommandHandler(MockApplicationDbContext);

        //Act
        var results = await handler.Handle(command, new CancellationToken());

        results.Should().NotBeNull();
        ArgumentNullException.ThrowIfNull(TestOrganisation);
        ArgumentNullException.ThrowIfNull(TestOrganisation.Services);
        results.Items[0].Should().BeEquivalentTo(TestOrganisation.Services.ElementAt(0));
    }

    [Fact]
    public async Task ThenGetServicesByOrganisationId()
    {
        CreateOrganisation();

        var command = new GetServicesByOrganisationIdCommand(TestOrganisation.Id);
        var handler = new GetServicesByOrganisationIdCommandHandler(MockApplicationDbContext);

        //Act
        var results = await handler.Handle(command, new CancellationToken());

        results.Should().NotBeNull();
        ArgumentNullException.ThrowIfNull(TestOrganisation);
        ArgumentNullException.ThrowIfNull(TestOrganisation.Services);
        results[0].Should().BeEquivalentTo(TestOrganisation.Services.ElementAt(0));
    }

    [Fact]
    public async Task ThenGetServicesByOrganisationId_ShouldThrowExceptionWhenNoOrganisations()
    {
        var command = new GetServicesByOrganisationIdCommand(TestOrganisation.Id);
        var handler = new GetServicesByOrganisationIdCommandHandler(MockApplicationDbContext);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, new CancellationToken()));
    }

    [Fact]
    public async Task ThenGetServicesByOrganisationId_ShouldThrowExceptionWhenNoServices()
    {
        var command = new GetServicesByOrganisationIdCommand(TestOrganisation.Id);
        var handler = new GetServicesByOrganisationIdCommandHandler(MockApplicationDbContext);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, new CancellationToken()));
    }

    [Fact]
    public async Task ThenGetServiceThatArePaidForWhenThereAreNone()
    {
        CreateOrganisation();

        var command = new GetServicesCommand("Information Sharing", "active", "XTEST", null, null, null,
            null, null, null, 1, 10, null, null, true, null, null, null, null, null);
        var handler = new GetServicesCommandHandler(MockApplicationDbContext);

        //Act
        var results = await handler.Handle(command, new CancellationToken());

        results.Should().NotBeNull();
        results.Items.Count.Should().Be(0);
    }

    [Fact]
    public async Task ThenGetServiceThatArePaidFor()
    {
        TestOrganisation.Services!.ElementAt(0).CostOptions = new List<CostOptionDto>
        {
            new(Guid.NewGuid().ToString(),
                "Session",
                10m,
                default,
                default,
                DateTime.UtcNow,
                DateTime.UtcNow.AddHours(8))
        };

        CreateOrganisation();

        var command = new GetServicesCommand("Information Sharing", "active", "XTEST", null, null, null,
            null, null, null, 1, 10, null, null, true, null, null, null, null, null);
        var handler = new GetServicesCommandHandler(MockApplicationDbContext);

        //Act
        var results = await handler.Handle(command, new CancellationToken());

        results.Should().NotBeNull();
        results.Items.Count.Should().Be(1);
    }

    [Fact]
    public async Task ThenGetServiceThatAreFreeWhenThereAreNoCostOptions()
    {
        TestOrganisation.Services!.ElementAt(0).CostOptions = new List<CostOptionDto>();

        CreateOrganisation();

        var command = new GetServicesCommand("Information Sharing", "active", "XTEST", null, null, null,
            null, null, null, 1, 10, null, null, false, null, null, null, null, null);
        var handler = new GetServicesCommandHandler(MockApplicationDbContext);

        //Act
        var results = await handler.Handle(command, new CancellationToken());

        results.Should().NotBeNull();
        ArgumentNullException.ThrowIfNull(TestOrganisation);
        ArgumentNullException.ThrowIfNull(TestOrganisation.Services);
        results.Items[0].Should().BeEquivalentTo(TestOrganisation.Services.ElementAt(0));
    }

    [Fact]
    public async Task ThenGetServiceThatAreFreeWhenOptionIsFree()
    {
        CreateOrganisation();

        var command = new GetServicesCommand("Information Sharing", "active", "XTEST", null, null, null,
            null, null, null, 1, 10, null, null, false, null, null, null, null, null);
        var handler = new GetServicesCommandHandler(MockApplicationDbContext);

        //Act
        var results = await handler.Handle(command, new CancellationToken());

        results.Should().NotBeNull();
        ArgumentNullException.ThrowIfNull(TestOrganisation);
        ArgumentNullException.ThrowIfNull(TestOrganisation.Services);
        results.Items[0].Should().BeEquivalentTo(TestOrganisation.Services.ElementAt(0));
    }

    [Fact]
    public async Task ThenDeleteService()
    {
        CreateOrganisation();

        var command = new DeleteServiceByIdCommand("3010521b-6e0a-41b0-b610-200edbbeeb14");
        var handler = new DeleteServiceByIdCommandHandler(MockApplicationDbContext, new Mock<ILogger<DeleteServiceByIdCommandHandler>>().Object);

        //Act
        bool results = await handler.Handle(command, new CancellationToken());

        results.Should().Be(true);
    }

    [Fact]
    public async Task ThenDeleteServiceThatDoesNotExist()
    {
        var command = new DeleteServiceByIdCommand(Guid.NewGuid().ToString());
        var handler = new DeleteServiceByIdCommandHandler(MockApplicationDbContext, new Mock<ILogger<DeleteServiceByIdCommandHandler>>().Object);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, new CancellationToken()));
    }
}
