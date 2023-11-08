using Ardalis.GuardClauses;
using FamilyHubs.ServiceDirectory.Core.Commands.Services.DeleteService;
using FamilyHubs.ServiceDirectory.Core.Queries.Services.GetServiceByOwnerReferenceIdCommand;
using FamilyHubs.ServiceDirectory.Core.Queries.Services.GetServices;
using FamilyHubs.ServiceDirectory.Core.Queries.Services.GetServicesByOrganisationId;
using FamilyHubs.ServiceDirectory.Shared.Dto;
using FamilyHubs.ServiceDirectory.Shared.Enums;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace FamilyHubs.ServiceDirectory.Core.IntegrationTests.Services;

public class WhenUsingGetServiceCommand : DataIntegrationTestBase
{
    [Fact]
    public async Task ThenGetService()
    {
        //Arrange
        await CreateOrganisation();

        var command = new GetServicesCommand(ServiceType.InformationSharing, ServiceStatusType.Active, "XTEST", null, null, null,
            null, null, null, 1, 10, null, null, null, null, null, null, null, null);
        var handler = new GetServicesCommandHandler(TestDbContext, Mapper);

        //Act
        var results = await handler.Handle(command, new CancellationToken());

        //Assert
        results.Should().NotBeNull();
        ArgumentNullException.ThrowIfNull(TestOrganisation);
        ArgumentNullException.ThrowIfNull(TestOrganisation.Services);
        results.Items[0].Should().BeEquivalentTo(TestOrganisation.Services.ElementAt(0));
    }

    [Fact]
    public async Task ThenGetServiceByOwnerReferenceId()
    {
        //Arrange
        await CreateOrganisation();
        const string serviceId = "3010521b-6e0a-41b0-b610-200edbbeeb14";

        var command = new GetServiceByOwnerReferenceIdCommand
        {
            OwnerReferenceId = serviceId
        };
        var handler = new GetServiceByOwnerReferenceIdCommandHandler(TestDbContext, Mapper);

        //Act
        var results = await handler.Handle(command, new CancellationToken());

        //Assert
        results.Should().NotBeNull();
        results.Should().BeEquivalentTo(TestOrganisation.Services.ElementAt(0));
    }

    [Fact]
    public async Task ThenGetServicesByOrganisationId()
    {
        //Arrange
        await CreateOrganisation();

        var command = new GetServiceNamesCommand
        {
            OrganisationId = TestOrganisation.Id,
            PageNumber = 1,
            PageSize = 10,
            Order = SortOrder.ascending
        };
        var handler = new GetServiceNamesCommandHandler(TestDbContext, Mapper);

        //Act
        var results = await handler.Handle(command, new CancellationToken());

        //Assert
        results.Should().NotBeNull();

        var expectedService = TestOrganisation.Services.ElementAt(0);
        var expectedServiceNameDto = new ServiceNameDto
        {
            Id = expectedService.Id,
            Name = expectedService.Name,
        };
        results.Items[0].Should().BeEquivalentTo(expectedServiceNameDto);
    }

    [Fact]
    public async Task ThenGetServiceThatArePaidFor()
    {
        //Arrange
        await CreateOrganisation(TestDataProvider.GetTestCountyCouncilDto2());

        var command = new GetServicesCommand(ServiceType.InformationSharing, ServiceStatusType.Active, "XTEST", null, null, null,
            null, null, null, 1, 10, null, null, true, null, null, null, null, null);
        var handler = new GetServicesCommandHandler(TestDbContext, Mapper);

        //Act
        var results = await handler.Handle(command, new CancellationToken());

        //Assert
        results.Should().NotBeNull();
        results.Items.Count.Should().Be(1);
    }

    [Fact]
    public async Task ThenGetServiceThatAreFree()
    {
        //Arrange
        await CreateOrganisation();

        var command = new GetServicesCommand(ServiceType.InformationSharing, ServiceStatusType.Active, "XTEST", null, null, null,
            null, null, null, 1, 10, null, null, false, null, null, null, null, null);
        var handler = new GetServicesCommandHandler(TestDbContext, Mapper);

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
        await CreateOrganisation();

        var command = new DeleteServiceByIdCommand(1);
        var handler = new DeleteServiceByIdCommandHandler(TestDbContext, new Mock<ILogger<DeleteServiceByIdCommandHandler>>().Object);

        //Act
        var results = await handler.Handle(command, new CancellationToken());

        //Assert
        results.Should().Be(true);
    }

    [Fact]
    public async Task ThenDeleteServiceThatDoesNotExist_ShouldThrowException()
    {
        //Arrange
        var command = new DeleteServiceByIdCommand(Random.Shared.Next());
        var handler = new DeleteServiceByIdCommandHandler(TestDbContext, new Mock<ILogger<DeleteServiceByIdCommandHandler>>().Object);

        // Act 
        // Assert
        await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, new CancellationToken()));
    }
}
