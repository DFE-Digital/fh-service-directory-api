using Ardalis.GuardClauses;
using FamilyHubs.ServiceDirectory.Core.Queries.Locations.GetLocationById;
using FamilyHubs.ServiceDirectory.Core.Queries.Locations.GetLocationsByOrganisationId;
using FamilyHubs.ServiceDirectory.Core.Queries.Locations.GetLocationsByServiceId;
using FamilyHubs.ServiceDirectory.Core.Queries.Locations.ListLocations;
using FluentAssertions;
using FluentAssertions.Equivalency;

namespace FamilyHubs.ServiceDirectory.Core.IntegrationTests.Locations;

public class WhenUsingGetLocationCommands : DataIntegrationTestBase
{
    [Fact]
    public async Task ThenGetLocationById()
    {
        //Arrange
        var testLocation = GetTestLocation();
        testLocation.Id = await CreateLocation(testLocation);

        var getCommand = new GetLocationByIdCommand { Id = testLocation.Id };
        var getHandler = new GetLocationByIdCommandHandler(TestDbContext, Mapper);

        //Act
        var result = await getHandler.Handle(getCommand, new CancellationToken());

        //Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(testLocation, options =>
            options.Excluding((IMemberInfo info) => info.Name.Contains("Id"))
                .Excluding((IMemberInfo info) => info.Name.Contains("Distance")));
    }

    [Fact]
    public async Task ThenGetLocationByServiceId()
    {
        //Arrange
        await CreateOrganisationDetails();

        var getCommand = new GetLocationsByServiceIdCommand
        {
            ServiceId = TestOrganisation.Services.ElementAt(0).Id
        };
        var getHandler = new GetLocationsByServiceIdCommandHandler(TestDbContext, Mapper);

        //Act
        var result = await getHandler.Handle(getCommand, new CancellationToken());

        //Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(TestOrganisation.Services.ElementAt(0).Locations);
    }



    [Fact]
    public async Task ThenGetLocationByOrganisationId()
    {
        //Arrange
        await CreateOrganisationDetails();

        var getCommand = new GetLocationsByOrganisationIdCommand(TestOrganisation.Id, null, null, null, null, null,null);

        var getHandler = new GetLocationsByOrganisationIdCommandHandler(TestDbContext, Mapper);

        //Act
        var result = await getHandler.Handle(getCommand, new CancellationToken());

        //Assert
        result.Should().NotBeNull();
        result.Items.Should().BeEquivalentTo(TestOrganisation.Services.SelectMany(s => s.Locations));
    }

    [Fact]
    public async Task ThenListLocations()
    {
        //Arrange
        var services = await CreateManyTestServicesQueryTesting();

        var getCommand = new ListLocationsCommand(null, null, null, null,null, null);
        var getHandler = new ListLocationCommandHandler(TestDbContext, Mapper);

        //Act
        var result = await getHandler.Handle(getCommand, new CancellationToken());

        //Assert
        result.Should().NotBeNull();
        result.Items.Should().BeEquivalentTo(services.SelectMany(s => s.Locations), options =>
            options.Excluding((IMemberInfo info) => info.Name.Contains("Id"))
                .Excluding((IMemberInfo info) => info.Name.Contains("Distance"))
                .Excluding((IMemberInfo info) => info.Name.Contains("Created"))
                .Excluding((IMemberInfo info) => info.Name.Contains("CreatedBy"))
                .Excluding((IMemberInfo info) => info.Name.Contains("LastModified"))
                .Excluding((IMemberInfo info) => info.Name.Contains("LastModifiedBy")));
    }

    [Fact]
    public async Task ThenGetLocationById_ShouldThrowExceptionWhenIdDoesNotExist()
    {
        //Arrange
        var getCommand = new GetLocationByIdCommand { Id = Random.Shared.Next() };
        var getHandler = new GetLocationByIdCommandHandler(TestDbContext, Mapper);

        // Act 
        // Assert
        await Assert.ThrowsAsync<NotFoundException>(() => getHandler.Handle(getCommand, new CancellationToken()));
    }

    [Fact]
    public async Task ThenGetLocationByServiceId_ShouldThrowExceptionWhenIdDoesNotExist()
    {
        //Arrange
        var getCommand = new GetLocationsByServiceIdCommand { ServiceId = Random.Shared.Next() };
        var getHandler = new GetLocationsByServiceIdCommandHandler(TestDbContext, Mapper);

        // Act 
        // Assert
        await Assert.ThrowsAsync<NotFoundException>(() => getHandler.Handle(getCommand, new CancellationToken()));
    }
}