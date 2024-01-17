﻿using FamilyHubs.ServiceDirectory.Core.Commands.Locations.UpdateLocation;
using FamilyHubs.ServiceDirectory.Shared.Dto;
using FluentAssertions;
using FluentAssertions.Equivalency;
using Microsoft.Extensions.Logging;
using Moq;

namespace FamilyHubs.ServiceDirectory.Core.IntegrationTests.Locations;

public class WhenUsingUpdateLocationCommand : DataIntegrationTestBase
{
    public readonly Mock<ILogger<UpdateLocationCommandHandler>> UpdateLogger = new Mock<ILogger<UpdateLocationCommandHandler>>();

    [Fact]
    public async Task ThenUpdateLocationOnly()
    {
        //Arrange
        var testLocation = TestOrganisation.Services.ElementAt(0).Locations.ElementAt(0);
        testLocation.Id = await CreateLocation(testLocation);
        testLocation.Name = "Unit Test Update Service Name";
        testLocation.Description = "Unit Test Update Service Name";

        var updateCommand = new UpdateLocationCommand(testLocation.Id, testLocation);
        var updateHandler = new UpdateLocationCommandHandler(TestDbContext, Mapper, UpdateLogger.Object);

        //Act
        var result = await updateHandler.Handle(updateCommand, new CancellationToken());

        //Assert
        result.Should().NotBe(0);
        result.Should().Be(testLocation.Id);

        var actualService = TestDbContext.Locations.SingleOrDefault(s => s.Name == testLocation.Name);
        actualService.Should().NotBeNull();
        actualService!.Description.Should().Be(testLocation.Description);
    }

    [Fact]
    public async Task ThenUpdateLocationAddAndDeleteContacts()
    {
        //Arrange
        var testLocation = TestOrganisation.Services.ElementAt(0).Locations.ElementAt(0);
        testLocation.Id = await CreateLocation(testLocation);
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
        actualContact.Should().BeEquivalentTo(contact , options => 
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
            options.Excluding((IMemberInfo info ) => info.Name.Contains("Id")));
    }

    [Fact]
    public async Task ThenUpdateLocationAddAndDeleteRegularSchedules()
    {
        //Arrange
        var testLocation = TestOrganisation.Services.ElementAt(0).Locations.ElementAt(0);
        testLocation.Id = await CreateLocation(testLocation);
        var existingItem = testLocation.RegularSchedules.ElementAt(0);
        var expected = new RegularScheduleDto
        {
            ValidFrom = DateTime.UtcNow,
            ValidTo = DateTime.UtcNow,
            ByDay = "New ByDay",
            ByMonthDay = "New ByMonthDay"
        };

        testLocation.RegularSchedules.Clear();
        testLocation.RegularSchedules.Add(expected);

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
    public async Task ThenUpdateLocationUpdatedRegularSchedules()
    {
        //Arrange
        var testLocation = TestOrganisation.Services.ElementAt(0).Locations.ElementAt(0);
        testLocation.Id = await CreateLocation(testLocation);

        var expected = testLocation.RegularSchedules.ElementAt(0);
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
            options.Excluding((IMemberInfo info ) => info.Name.Contains("Id")));
    }

    [Fact]
    public async Task ThenUpdateLocationAddAndDeleteHolidaySchedules()
    {
        //Arrange
        var testLocation = TestOrganisation.Services.ElementAt(0).Locations.ElementAt(0);
        testLocation.Id = await CreateLocation(testLocation);

        var existingItem = testLocation.HolidaySchedules.ElementAt(0);
        var expected = new HolidayScheduleDto
        {
            StartDate = new DateTime(2023, 1, 2).ToUniversalTime(),
            EndDate = new DateTime(2023, 1, 2).ToUniversalTime(),
        };
        testLocation.HolidaySchedules.Clear();
        testLocation.HolidaySchedules.Add(expected);

        var updateCommand = new UpdateLocationCommand(testLocation.Id, testLocation);
        var updateHandler = new UpdateLocationCommandHandler(TestDbContext, Mapper, UpdateLogger.Object);

        //Act
        var result = await updateHandler.Handle(updateCommand, new CancellationToken());

        //Assert
        result.Should().NotBe(0);
        result.Should().Be(testLocation.Id);

        var actualService = TestDbContext.Locations.SingleOrDefault(s => s.Name == testLocation.Name);
        actualService.Should().NotBeNull();
        actualService!.HolidaySchedules.Count.Should().Be(1);

        var actualEntity = TestDbContext.HolidaySchedules.SingleOrDefault(s => s.LocationId == testLocation.Id);
        actualEntity.Should().NotBeNull();
        actualEntity.Should().BeEquivalentTo(expected, options => 
            options.Excluding(info => info.Name.Contains("Id"))
                .Excluding(info => info.Name.Contains("Distance")));

        var unexpectedEntity = TestDbContext.HolidaySchedules.Where(lc => lc.Id == existingItem.Id).ToList();
        unexpectedEntity.Should().HaveCount(0);
    }

    [Fact]
    public async Task ThenUpdateLocationUpdatedHolidaySchedules()
    {
        //Arrange
        var testLocation = TestOrganisation.Services.ElementAt(0).Locations.ElementAt(0);
        testLocation.Id = await CreateLocation(testLocation);

        var expected = testLocation.HolidaySchedules.ElementAt(0);
        expected.StartDate = new DateTime(2023, 1, 2).ToUniversalTime();
        expected.EndDate = new DateTime(2023, 1, 2).ToUniversalTime();
        
        var updateCommand = new UpdateLocationCommand(testLocation.Id, testLocation);
        var updateHandler = new UpdateLocationCommandHandler(TestDbContext, Mapper, UpdateLogger.Object);

        //Act
        var result = await updateHandler.Handle(updateCommand, new CancellationToken());

        //Assert
        result.Should().NotBe(0);
        result.Should().Be(testLocation.Id);

        var actualService = TestDbContext.Locations.SingleOrDefault(s => s.Name == testLocation.Name);
        actualService.Should().NotBeNull();
        actualService!.HolidaySchedules.Count.Should().Be(1);

        var actualEntity = TestDbContext.HolidaySchedules.SingleOrDefault(s => s.LocationId == testLocation.Id);
        actualEntity.Should().NotBeNull();
        actualEntity.Should().BeEquivalentTo(expected, options => 
            options.Excluding((IMemberInfo info ) => info.Name.Contains("Id")));
    }
}