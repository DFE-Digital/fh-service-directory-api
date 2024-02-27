using FamilyHubs.ServiceDirectory.Core.Commands.Locations.UpdateLocation;
using FamilyHubs.ServiceDirectory.Data.Entities;
using FamilyHubs.ServiceDirectory.Shared.Dto;
using FluentAssertions;
using FluentAssertions.Equivalency;
using Microsoft.Extensions.Logging;
using Moq;

namespace FamilyHubs.ServiceDirectory.Core.IntegrationTests.Locations;

public class WhenUsingUpdateLocationCommand : DataIntegrationTestBase
{
    public readonly Mock<ILogger<UpdateLocationCommandHandler>> UpdateLogger = new();

    [Fact]
    public async Task ThenUpdateLocationOnly()
    {
        //Arrange
        var testLocationDto = TestOrganisation.Services.ElementAt(0).Locations.ElementAt(0);

        var location = await CreateLocation(testLocationDto, TestDbContext.Organisations.First().Id);
        testLocationDto.Id = location.Id;
        testLocationDto.OrganisationId = location.OrganisationId;
        testLocationDto.Name = "Unit Test Update Service Name";
        testLocationDto.Description = "Unit Test Update Service Name";

        var updateCommand = new UpdateLocationCommand(testLocationDto.Id, testLocationDto);
        var updateHandler = new UpdateLocationCommandHandler(TestDbContext, Mapper, UpdateLogger.Object);

        //Act
        var result = await updateHandler.Handle(updateCommand, new CancellationToken());

        //Assert
        result.Should().NotBe(0);
        result.Should().Be(testLocationDto.Id);

        var actualService = TestDbContext.Locations.SingleOrDefault(s => s.Name == testLocationDto.Name);
        actualService.Should().NotBeNull();
        actualService!.Description.Should().Be(testLocationDto.Description);
    }

    [Fact]
    public async Task ThenUpdateLocationAddAndDeleteContacts()
    {
        //Arrange
        var testLocation = TestOrganisation.Services.ElementAt(0).Locations.ElementAt(0);
        var location = await CreateLocation(testLocation, TestDbContext.Organisations.First().Id);
        testLocation.Id = location.Id;
        testLocation.OrganisationId = location.OrganisationId;
        var existingItem = testLocation.Contacts.ElementAt(0);
        var contact = new ContactDto
        {
            Id = 0,
            Name = "New Contact",
            Telephone = "New Telephone"
        };
        testLocation.Contacts.Clear();
        testLocation.Contacts.Add(contact);

        var updateCommand = new UpdateLocationCommand(testLocation.Id, testLocation);
        var updateHandler = new UpdateLocationCommandHandler(TestDbContext, Mapper, UpdateLogger.Object);

        //Act
        var result = await updateHandler.Handle(updateCommand, new CancellationToken());

        //Assert
        result.Should().NotBe(0);
        result.Should().Be(testLocation.Id);

        var actualService = TestDbContext.Locations.SingleOrDefault(s => s.Name == testLocation.Name);
        actualService.Should().NotBeNull();
        actualService!.Contacts.Count.Should().Be(1);

        var actualContact = TestDbContext.Contacts.SingleOrDefault(s => s.Name == contact.Name);
        actualContact.Should().NotBeNull();
        actualContact.Should().BeEquivalentTo(contact, options =>
            options.Excluding((IMemberInfo info) => info.Name.Contains("Id"))
                .Excluding((IMemberInfo info) => info.Name.Contains("Distance")));

        var unexpectedEntity = TestDbContext.Contacts.Where(lc => lc.Id == existingItem.Id).ToList();
        unexpectedEntity.Should().HaveCount(0);
    }

    [Fact]
    public async Task ThenUpdateLocationWithUpdatedContacts()
    {
        //Arrange
        var testLocation = TestOrganisation.Services.ElementAt(0).Locations.ElementAt(0);
        testLocation.Id = await CreateLocation(testLocation);
        var contact = testLocation.Contacts.ElementAt(0);
        contact.Name = "Updated Name";
        contact.Email = "Updated Email";
        contact.Telephone = "Updated Telephone";

        var updateCommand = new UpdateLocationCommand(testLocation.Id, testLocation);
        var updateHandler = new UpdateLocationCommandHandler(TestDbContext, Mapper, UpdateLogger.Object);

        //Act
        var result = await updateHandler.Handle(updateCommand, new CancellationToken());

        //Assert
        result.Should().NotBe(0);
        result.Should().Be(testLocation.Id);

        var actualService = TestDbContext.Locations.SingleOrDefault(s => s.Name == testLocation.Name);
        actualService.Should().NotBeNull();
        actualService!.Contacts.Count.Should().Be(1);

        var actualContact = TestDbContext.Contacts.SingleOrDefault(s => s.Name == contact.Name);
        actualContact.Should().NotBeNull();
        actualContact.Should().BeEquivalentTo(contact, options =>
            options.Excluding((IMemberInfo info) => info.Name.Contains("Id")));
    }

    [Fact]
    public async Task ThenUpdateLocationAddAndDeleteSchedules()
    {
        //Arrange
        var testLocation = TestOrganisation.Services.ElementAt(0).Locations.ElementAt(0);
        testLocation.Id = await CreateLocation(testLocation);
        var existingItem = testLocation.Schedules.ElementAt(0);
        var expected = new ScheduleDto
        {
            ValidFrom = DateTime.UtcNow,
            ValidTo = DateTime.UtcNow,
            ByDay = "New ByDay",
            ByMonthDay = "New ByMonthDay"
        };

        testLocation.Schedules.Clear();
        testLocation.Schedules.Add(expected);

        var updateCommand = new UpdateLocationCommand(testLocation.Id, testLocation);
        var updateHandler = new UpdateLocationCommandHandler(TestDbContext, Mapper, UpdateLogger.Object);

        //Act
        var result = await updateHandler.Handle(updateCommand, new CancellationToken());

        //Assert
        result.Should().NotBe(0);
        result.Should().Be(testLocation.Id);

        var actualService = TestDbContext.Locations.SingleOrDefault(s => s.Name == testLocation.Name);
        actualService.Should().NotBeNull();
        actualService!.Schedules.Count.Should().Be(1);

        var actualEntity = TestDbContext.Schedules.SingleOrDefault(s => s.ByDay == expected.ByDay);
        actualEntity.Should().NotBeNull();
        actualEntity.Should().BeEquivalentTo(expected, options =>
            options.Excluding(info => info.Name.Contains("Id"))
                .Excluding(info => info.Name.Contains("Distance")));

        var unexpectedEntity = TestDbContext.Schedules.Where(lc => lc.Id == existingItem.Id).ToList();
        unexpectedEntity.Should().HaveCount(0);
    }

    [Fact]
    public async Task ThenUpdateLocationUpdatedSchedules()
    {
        //Arrange
        var testLocation = TestOrganisation.Services.ElementAt(0).Locations.ElementAt(0);
        testLocation.Id = await CreateLocation(testLocation);

        var expected = testLocation.Schedules.ElementAt(0);
        expected.ByDay = "Updated ByDay";
        expected.ByMonthDay = "Updated ByMonthDay";

        var updateCommand = new UpdateLocationCommand(testLocation.Id, testLocation);
        var updateHandler = new UpdateLocationCommandHandler(TestDbContext, Mapper, UpdateLogger.Object);

        //Act
        var result = await updateHandler.Handle(updateCommand, new CancellationToken());

        //Assert
        result.Should().NotBe(0);
        result.Should().Be(testLocation.Id);

        var actualService = TestDbContext.Locations.SingleOrDefault(s => s.Name == testLocation.Name);
        actualService.Should().NotBeNull();
        actualService!.Schedules.Count.Should().Be(1);

        var actualEntity = TestDbContext.Schedules.SingleOrDefault(s => s.ByDay == expected.ByDay);
        actualEntity.Should().NotBeNull();
        actualEntity.Should().BeEquivalentTo(expected, options =>
            options.Excluding((IMemberInfo info) => info.Name.Contains("Id")));
    }
}