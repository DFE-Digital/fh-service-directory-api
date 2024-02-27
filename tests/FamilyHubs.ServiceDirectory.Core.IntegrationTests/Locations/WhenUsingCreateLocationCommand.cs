using FamilyHubs.ServiceDirectory.Core.Commands.Locations.CreateLocation;
using FluentAssertions;
using FluentAssertions.Equivalency;

namespace FamilyHubs.ServiceDirectory.Core.IntegrationTests.Locations;

public class WhenUsingCreateLocationCommand : DataIntegrationTestBase
{
    [Fact]
    public async Task ThenCreateLocation()
    {
        //Arrange
        var testLocation = TestOrganisation.Services.ElementAt(0).Locations.ElementAt(0);
        testLocation.OrganisationId = TestDbContext.Organisations.First().Id;
        var createLocationCommand = new CreateLocationCommand(testLocation);
        var handler = new CreateLocationCommandHandler(TestDbContext, Mapper, GetLogger<CreateLocationCommandHandler>());

        //Act
        var result = await handler.Handle(createLocationCommand, new CancellationToken());

        //Assert
        result.Should().NotBe(0);
        var actualLocation = TestDbContext.Locations.SingleOrDefault(o => o.Id == result);
        actualLocation.Should().NotBeNull();
        actualLocation.Should().BeEquivalentTo(testLocation, options =>
            options.Excluding((IMemberInfo info) => info.Name.Contains("Id"))
                .Excluding((IMemberInfo info) => info.Name.Contains("Distance")));
    }

    [Fact]
    public async Task ThenCreateLocationWithoutAnyChildEntities()
    {
        var testLocation = TestOrganisation.Services.ElementAt(0).Locations.ElementAt(0);
        testLocation.Contacts.Clear();
        testLocation.AccessibilityForDisabilities.Clear();
        testLocation.Schedules.Clear();
        //Arrange
        var createLocationCommand = new CreateLocationCommand(testLocation);
        var handler = new CreateLocationCommandHandler(TestDbContext, Mapper, GetLogger<CreateLocationCommandHandler>());

        //Act
        var result = await handler.Handle(createLocationCommand, new CancellationToken());

        //Assert
        result.Should().NotBe(0);

        var actualLocation = TestDbContext.Locations.SingleOrDefault(o => o.Id == result);
        actualLocation.Should().NotBeNull();
        actualLocation!.Contacts.Count.Should().Be(0);
        actualLocation.Schedules.Count.Should().Be(0);
        actualLocation.AccessibilityForDisabilities.Count.Should().Be(0);

        actualLocation.Should().BeEquivalentTo(testLocation, options =>
            options.Excluding((IMemberInfo info) => info.Name.Contains("Id"))
                .Excluding((IMemberInfo info) => info.Name.Contains("Distance")));
    }

    [Fact]
    public async Task ThenCreateDuplicateLocation_ShouldThrowException()
    {
        //Arrange
        var testLocation = TestOrganisation.Services.ElementAt(0).Locations.ElementAt(0);

        testLocation.Id = await CreateLocation(testLocation);

        var command = new CreateLocationCommand(testLocation);
        var handler = new CreateLocationCommandHandler(TestDbContext, Mapper, GetLogger<CreateLocationCommandHandler>());

        // Act 
        // Assert
        await Assert.ThrowsAsync<ArgumentException>(() => handler.Handle(command, new CancellationToken()));
    }
}