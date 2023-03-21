﻿using FamilyHubs.ServiceDirectory.Core.Commands.Organisations.UpdateOrganisation;
using FamilyHubs.ServiceDirectory.Shared.Dto;
using FamilyHubs.ServiceDirectory.Shared.Enums;
using FluentAssertions;
using FluentAssertions.Equivalency;
using Microsoft.Extensions.Logging;
using Moq;

namespace FamilyHubs.ServiceDirectory.Core.IntegrationTests.Organisations;

public class WhenUsingUpdateLocationCommand : DataIntegrationTestBase
{
    public readonly Mock<ILogger<UpdateOrganisationCommandHandler>> UpdateLogger = new Mock<ILogger<UpdateOrganisationCommandHandler>>();

    [Fact]
    public async Task ThenUpdateOrganisationOnly()
    {
        //Arrange
        await CreateOrganisation();
        var service = TestOrganisation.Services.ElementAt(0);
        TestOrganisation.Name = "Unit Test Update TestOrganisation Name";
        TestOrganisation.Description = "Unit Test Update TestOrganisation Name";

        var updateCommand = new UpdateOrganisationCommand(TestOrganisation.Id, TestOrganisation);
        var updateHandler = new UpdateOrganisationCommandHandler(TestDbContext, Mapper, UpdateLogger.Object);

        //Act
        var result = await updateHandler.Handle(updateCommand, new CancellationToken());

        //Assert
        result.Should().NotBe(0);
        result.Should().Be(TestOrganisation.Id);

        var actualOrganisation = TestDbContext.Organisations.SingleOrDefault(s => s.Name == TestOrganisation.Name);
        actualOrganisation.Should().NotBeNull();
        actualOrganisation!.Description.Should().Be(TestOrganisation.Description);

        var actualService = TestDbContext.Services.SingleOrDefault(s => s.Name == service.Name);
        actualService.Should().NotBeNull();
        actualService!.Description.Should().Be(service.Description);
    }

    [Fact]
    public async Task ThenUpdateOrganisationAddAndDeleteEligibilities()
    {
        //Arrange
        await CreateOrganisation();
        var service = TestOrganisation.Services.ElementAt(0);
        var existingItem = service.Eligibilities.ElementAt(0);

        var expected = new EligibilityDto
        {
            //ServiceId = service.Id,
            MaximumAge = 2,
            MinimumAge = 0,
            EligibilityType = EligibilityType.Adult
        };
        service.Eligibilities.Clear();
        service.Eligibilities.Add(expected);

        var updateCommand = new UpdateOrganisationCommand(TestOrganisation.Id, TestOrganisation);
        var updateHandler = new UpdateOrganisationCommandHandler(TestDbContext, Mapper, UpdateLogger.Object);

        //Act
        var result = await updateHandler.Handle(updateCommand, new CancellationToken());

        //Assert
        result.Should().NotBe(0);
        result.Should().Be(TestOrganisation.Id);

        var actualService = TestDbContext.Services.SingleOrDefault(s => s.Name == service.Name);
        actualService.Should().NotBeNull();
        actualService!.Eligibilities.Count.Should().Be(1);

        var actualEntity = TestDbContext.Eligibilities.SingleOrDefault(s => s.EligibilityType == expected.EligibilityType);
        actualEntity.Should().NotBeNull();
        actualEntity.Should().BeEquivalentTo(expected, options => 
            options.Excluding(info => info.Name.Contains("Id"))
                .Excluding(info => info.Name.Contains("Distance")));

        var unexpectedEntity = TestDbContext.Eligibilities.Where(lc => lc.Id == existingItem.Id).ToList();
        unexpectedEntity.Should().HaveCount(0);
    }

    [Fact]
    public async Task ThenUpdateOrganisationUpdatedEligibilities()
    {
        //Arrange
        await CreateOrganisation();
        var service = TestOrganisation.Services.ElementAt(0);

        var expected = service.Eligibilities.ElementAt(0);
        expected.MinimumAge = 500;
        expected.MaximumAge = 5000;

        var updateCommand = new UpdateOrganisationCommand(TestOrganisation.Id, TestOrganisation);
        var updateHandler = new UpdateOrganisationCommandHandler(TestDbContext, Mapper, UpdateLogger.Object);

        //Act
        var result = await updateHandler.Handle(updateCommand, new CancellationToken());

        //Assert
        result.Should().NotBe(0);
        result.Should().Be(TestOrganisation.Id);

        var actualService = TestDbContext.Services.SingleOrDefault(s => s.Name == service.Name);
        actualService.Should().NotBeNull();
        actualService!.Eligibilities.Count.Should().Be(1);

        var actualEntity = TestDbContext.Eligibilities.SingleOrDefault(s => s.EligibilityType == expected.EligibilityType);
        actualEntity.Should().NotBeNull();
        actualEntity.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task ThenUpdateOrganisationAddAndDeleteServiceAreas()
    {
        //Arrange
        await CreateOrganisation();
        var service = TestOrganisation.Services.ElementAt(0);
        var existingItem = service.ServiceAreas.ElementAt(0);
        var expected = new ServiceAreaDto
        {
            ServiceAreaName = "ServiceAreaName",
            Extent = "Extent",
            Uri = "Uri"
        };
        service.ServiceAreas.Clear();
        service.ServiceAreas.Add(expected);

        var updateCommand = new UpdateOrganisationCommand(TestOrganisation.Id, TestOrganisation);
        var updateHandler = new UpdateOrganisationCommandHandler(TestDbContext, Mapper, UpdateLogger.Object);

        //Act
        var result = await updateHandler.Handle(updateCommand, new CancellationToken());

        //Assert
        result.Should().NotBe(0);
        result.Should().Be(TestOrganisation.Id);

        var actualService = TestDbContext.Services.SingleOrDefault(s => s.Name == service.Name);
        actualService.Should().NotBeNull();
        actualService!.ServiceAreas.Count.Should().Be(1);

        var actualEntity = TestDbContext.ServiceAreas.SingleOrDefault(s => s.ServiceAreaName == expected.ServiceAreaName);
        actualEntity.Should().NotBeNull();
        actualEntity.Should().BeEquivalentTo(expected, options => 
            options.Excluding(info => info.Name.Contains("Id"))
                .Excluding(info => info.Name.Contains("Distance")));

        var unexpectedEntity = TestDbContext.ServiceAreas.Where(lc => lc.Id == existingItem.Id).ToList();
        unexpectedEntity.Should().HaveCount(0);
    }

    [Fact]
    public async Task ThenUpdateOrganisationUpdatedServiceAreas()
    {
        //Arrange
        await CreateOrganisation();
        var service = TestOrganisation.Services.ElementAt(0);

        var expected = service.ServiceAreas.ElementAt(0);
        expected.ServiceAreaName = "Updated ServiceAreaName";
        expected.Extent = "Updated Extent";
        
        var updateCommand = new UpdateOrganisationCommand(TestOrganisation.Id, TestOrganisation);
        var updateHandler = new UpdateOrganisationCommandHandler(TestDbContext, Mapper, UpdateLogger.Object);

        //Act
        var result = await updateHandler.Handle(updateCommand, new CancellationToken());

        //Assert
        result.Should().NotBe(0);
        result.Should().Be(TestOrganisation.Id);

        var actualService = TestDbContext.Services.SingleOrDefault(s => s.Name == service.Name);
        actualService.Should().NotBeNull();
        actualService!.ServiceAreas.Count.Should().Be(1);

        var actualEntity = TestDbContext.ServiceAreas.SingleOrDefault(s => s.ServiceAreaName == expected.ServiceAreaName);
        actualEntity.Should().NotBeNull();
        actualEntity.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task ThenUpdateOrganisationAddAndDeleteServiceDeliveries()
    {
        //Arrange
        await CreateOrganisation();
        var service = TestOrganisation.Services.ElementAt(0);
        var existingItem = service.ServiceDeliveries.ElementAt(0);
        var expected = new ServiceDeliveryDto
        {
            Name = ServiceDeliveryType.NotSet
        };

        service.ServiceDeliveries.Clear();
        service.ServiceDeliveries.Add(expected);

        var updateCommand = new UpdateOrganisationCommand(TestOrganisation.Id, TestOrganisation);
        var updateHandler = new UpdateOrganisationCommandHandler(TestDbContext, Mapper, UpdateLogger.Object);

        //Act
        var result = await updateHandler.Handle(updateCommand, new CancellationToken());

        //Assert
        result.Should().NotBe(0);
        result.Should().Be(TestOrganisation.Id);

        var actualService = TestDbContext.Services.SingleOrDefault(s => s.Name == service.Name);
        actualService.Should().NotBeNull();
        actualService!.ServiceDeliveries.Count.Should().Be(1);

        var actualEntity = TestDbContext.ServiceDeliveries.SingleOrDefault(s => s.Name == expected.Name);
        actualEntity.Should().NotBeNull();
        actualEntity.Should().BeEquivalentTo(expected, options => 
            options.Excluding(info => info.Name.Contains("Id"))
                .Excluding(info => info.Name.Contains("Distance")));

        var unexpectedEntity = TestDbContext.ServiceDeliveries.Where(lc => lc.Id == existingItem.Id).ToList();
        unexpectedEntity.Should().HaveCount(0);
    }

    [Fact]
    public async Task ThenUpdateOrganisationUpdatedServiceDeliveries()
    {
        //Arrange
        await CreateOrganisation();
        var service = TestOrganisation.Services.ElementAt(0);

        var expected = service.ServiceDeliveries.ElementAt(0);
        expected.Name = ServiceDeliveryType.NotSet;
        
        var updateCommand = new UpdateOrganisationCommand(TestOrganisation.Id, TestOrganisation);
        var updateHandler = new UpdateOrganisationCommandHandler(TestDbContext, Mapper, UpdateLogger.Object);

        //Act
        var result = await updateHandler.Handle(updateCommand, new CancellationToken());

        //Assert
        result.Should().NotBe(0);
        result.Should().Be(TestOrganisation.Id);

        var actualService = TestDbContext.Services.SingleOrDefault(s => s.Name == service.Name);
        actualService.Should().NotBeNull();
        actualService!.ServiceDeliveries.Count.Should().Be(1);

        var actualEntity = TestDbContext.ServiceDeliveries.SingleOrDefault(s => s.Name == expected.Name);
        actualEntity.Should().NotBeNull();
        actualEntity.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task ThenUpdateOrganisationAddAndDeleteLanguages()
    {
        //Arrange
        await CreateOrganisation();
        var service = TestOrganisation.Services.ElementAt(0);
        var existingItem = service.Languages.ElementAt(0);
        var expected = new LanguageDto()
        {
            Name = "New Language"
        };

        service.Languages.Clear();
        service.Languages.Add(expected);

        var updateCommand = new UpdateOrganisationCommand(TestOrganisation.Id, TestOrganisation);
        var updateHandler = new UpdateOrganisationCommandHandler(TestDbContext, Mapper, UpdateLogger.Object);

        //Act
        var result = await updateHandler.Handle(updateCommand, new CancellationToken());

        //Assert
        result.Should().NotBe(0);
        result.Should().Be(TestOrganisation.Id);

        var actualService = TestDbContext.Services.SingleOrDefault(s => s.Name == service.Name);
        actualService.Should().NotBeNull();
        actualService!.Languages.Count.Should().Be(1);

        var actualEntity = TestDbContext.Languages.SingleOrDefault(s => s.Name == expected.Name);
        actualEntity.Should().NotBeNull();
        actualEntity.Should().BeEquivalentTo(expected, options => 
            options.Excluding((IMemberInfo info) => info.Name.Contains("Id"))
                .Excluding((IMemberInfo info) => info.Name.Contains("Distance")));

        var unexpectedEntity = TestDbContext.Languages.Where(lc => lc.Id == existingItem.Id).ToList();
        unexpectedEntity.Should().HaveCount(0);
    }

    [Fact]
    public async Task ThenUpdateOrganisationUpdatedLanguages()
    {
        //Arrange
        await CreateOrganisation();
        var service = TestOrganisation.Services.ElementAt(0);

        var expected = service.Languages.ElementAt(0);
        expected.Name = "Updated Language";
        
        var updateCommand = new UpdateOrganisationCommand(TestOrganisation.Id, TestOrganisation);
        var updateHandler = new UpdateOrganisationCommandHandler(TestDbContext, Mapper, UpdateLogger.Object);

        //Act
        var result = await updateHandler.Handle(updateCommand, new CancellationToken());

        //Assert
        result.Should().NotBe(0);
        result.Should().Be(TestOrganisation.Id);

        var actualService = TestDbContext.Services.SingleOrDefault(s => s.Name == service.Name);
        actualService.Should().NotBeNull();
        actualService!.Languages.Count.Should().Be(1);

        var actualEntity = TestDbContext.Languages.SingleOrDefault(s => s.Name == expected.Name);
        actualEntity.Should().NotBeNull();
        actualEntity.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task ThenUpdateOrganisationAddAndDeleteCostOptions()
    {
        //Arrange
        await CreateOrganisation();
        var service = TestOrganisation.Services.ElementAt(0);
        var existingItem = service.CostOptions.ElementAt(0);
        var expected = new CostOptionDto
        {
            ValidFrom = DateTime.UtcNow,
            ValidTo = DateTime.UtcNow,
            Option = "new Option",
            Amount = 123,
            AmountDescription = "Amount Description"
        };

        service.CostOptions.Clear();
        service.CostOptions.Add(expected);

        var updateCommand = new UpdateOrganisationCommand(TestOrganisation.Id, TestOrganisation);
        var updateHandler = new UpdateOrganisationCommandHandler(TestDbContext, Mapper, UpdateLogger.Object);

        //Act
        var result = await updateHandler.Handle(updateCommand, new CancellationToken());

        //Assert
        result.Should().NotBe(0);
        result.Should().Be(TestOrganisation.Id);

        var actualService = TestDbContext.Services.SingleOrDefault(s => s.Name == service.Name);
        actualService.Should().NotBeNull();
        actualService!.CostOptions.Count.Should().Be(1);

        var actualEntity = TestDbContext.CostOptions.SingleOrDefault(s => s.Option == expected.Option);
        actualEntity.Should().NotBeNull();
        actualEntity.Should().BeEquivalentTo(expected, options => 
            options.Excluding(info => info.Name.Contains("Id"))
                .Excluding(info => info.Name.Contains("Distance")));

        var unexpectedEntity = TestDbContext.CostOptions.Where(lc => lc.Id == existingItem.Id).ToList();
        unexpectedEntity.Should().HaveCount(0);
    }

    [Fact]
    public async Task ThenUpdateOrganisationUpdatedCostOptions()
    {
        //Arrange
        await CreateOrganisation();
        var service = TestOrganisation.Services.ElementAt(0);

        var expected = service.CostOptions.ElementAt(0);
        expected.Amount = 987;
        expected.Option = "Updated Option";
        expected.AmountDescription = "Updated Amount Description";
        
        var updateCommand = new UpdateOrganisationCommand(TestOrganisation.Id, TestOrganisation);
        var updateHandler = new UpdateOrganisationCommandHandler(TestDbContext, Mapper, UpdateLogger.Object);

        //Act
        var result = await updateHandler.Handle(updateCommand, new CancellationToken());

        //Assert
        result.Should().NotBe(0);
        result.Should().Be(TestOrganisation.Id);

        var actualService = TestDbContext.Services.SingleOrDefault(s => s.Name == service.Name);
        actualService.Should().NotBeNull();
        actualService!.CostOptions.Count.Should().Be(1);

        var actualEntity = TestDbContext.CostOptions.SingleOrDefault(s => s.Option == expected.Option);
        actualEntity.Should().NotBeNull();
        actualEntity.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task ThenUpdateOrganisationAddAndDeleteContacts()
    {
        //Arrange
        await CreateOrganisation();
        var service = TestOrganisation.Services.ElementAt(0);
        var existingItem = service.Contacts.ElementAt(0);
        var contact = new ContactDto
        {
            Id = 0,
            Name = "New Contact",
            Telephone = "New Telephone"
        };
        service.Contacts.Clear();
        service.Contacts.Add(contact);

        var updateCommand = new UpdateOrganisationCommand(TestOrganisation.Id, TestOrganisation);
        var updateHandler = new UpdateOrganisationCommandHandler(TestDbContext, Mapper, UpdateLogger.Object);

        //Act
        var result = await updateHandler.Handle(updateCommand, new CancellationToken());

        //Assert
        result.Should().NotBe(0);
        result.Should().Be(TestOrganisation.Id);

        var actualService = TestDbContext.Services.SingleOrDefault(s => s.Name == service.Name);
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
    public async Task ThenUpdateOrganisationWithUpdatedContacts()
    {
        //Arrange
        await CreateOrganisation();
        var service = TestOrganisation.Services.ElementAt(0);
        var contact = service.Contacts.ElementAt(0);
        contact.Name = "Updated Name";
        contact.Email = "Updated Email";
        contact.Telephone = "Updated Telephone";

        var updateCommand = new UpdateOrganisationCommand(TestOrganisation.Id, TestOrganisation);
        var updateHandler = new UpdateOrganisationCommandHandler(TestDbContext, Mapper, UpdateLogger.Object);

        //Act
        var result = await updateHandler.Handle(updateCommand, new CancellationToken());

        //Assert
        result.Should().NotBe(0);
        result.Should().Be(TestOrganisation.Id);

        var actualService = TestDbContext.Services.SingleOrDefault(s => s.Name == service.Name);
        actualService.Should().NotBeNull();
        actualService!.Contacts.Count.Should().Be(1);

        var actualContact = TestDbContext.Contacts.SingleOrDefault(s => s.Name == contact.Name);
        actualContact.Should().NotBeNull();
        actualContact.Should().BeEquivalentTo(contact);
    }

    [Fact]
    public async Task ThenUpdateOrganisationAddAndDeleteRegularSchedules()
    {
        //Arrange
        await CreateOrganisation();
        var service = TestOrganisation.Services.ElementAt(0);
        var existingItem = service.RegularSchedules.ElementAt(0);
        var expected = new RegularScheduleDto
        {
            ValidFrom = DateTime.UtcNow,
            ValidTo = DateTime.UtcNow,
            ByDay = "New ByDay",
            ByMonthDay = "New ByMonthDay"
        };

        service.RegularSchedules.Clear();
        service.RegularSchedules.Add(expected);

        var updateCommand = new UpdateOrganisationCommand(TestOrganisation.Id, TestOrganisation);
        var updateHandler = new UpdateOrganisationCommandHandler(TestDbContext, Mapper, UpdateLogger.Object);

        //Act
        var result = await updateHandler.Handle(updateCommand, new CancellationToken());

        //Assert
        result.Should().NotBe(0);
        result.Should().Be(TestOrganisation.Id);

        var actualService = TestDbContext.Services.SingleOrDefault(s => s.Name == service.Name);
        actualService.Should().NotBeNull();
        actualService!.RegularSchedules.Count.Should().Be(1);

        var actualEntity = TestDbContext.RegularSchedules.SingleOrDefault(s => s.ByDay == expected.ByDay);
        actualEntity.Should().NotBeNull();
        actualEntity.Should().BeEquivalentTo(expected, options => 
            options.Excluding(info => info.Name.Contains("Id"))
                .Excluding(info => info.Name.Contains("Distance")));

        var unexpectedEntity = TestDbContext.RegularSchedules.Where(lc => lc.Id == existingItem.Id).ToList();
        unexpectedEntity.Should().HaveCount(0);
    }

    [Fact]
    public async Task ThenUpdateOrganisationUpdatedRegularSchedules()
    {
        //Arrange
        await CreateOrganisation();
        var service = TestOrganisation.Services.ElementAt(0);

        var expected = service.RegularSchedules.ElementAt(0);
        expected.ByDay = "Updated ByDay";
        expected.ByMonthDay = "Updated ByMonthDay";
        
        var updateCommand = new UpdateOrganisationCommand(TestOrganisation.Id, TestOrganisation);
        var updateHandler = new UpdateOrganisationCommandHandler(TestDbContext, Mapper, UpdateLogger.Object);

        //Act
        var result = await updateHandler.Handle(updateCommand, new CancellationToken());

        //Assert
        result.Should().NotBe(0);
        result.Should().Be(TestOrganisation.Id);

        var actualService = TestDbContext.Services.SingleOrDefault(s => s.Name == service.Name);
        actualService.Should().NotBeNull();
        actualService!.RegularSchedules.Count.Should().Be(1);

        var actualEntity = TestDbContext.RegularSchedules.SingleOrDefault(s => s.ByDay == expected.ByDay);
        actualEntity.Should().NotBeNull();
        actualEntity.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task ThenUpdateOrganisationAddAndDeleteHolidaySchedules()
    {
        //Arrange
        await CreateOrganisation();
        var service = TestOrganisation.Services.ElementAt(0);
        var existingItem = service.HolidaySchedules.ElementAt(0);
        var expected = new HolidayScheduleDto
        {
            StartDate = new DateTime(2023, 1, 2).ToUniversalTime(),
            EndDate = new DateTime(2023, 1, 2).ToUniversalTime(),
        };
        service.HolidaySchedules.Clear();
        service.HolidaySchedules.Add(expected);

        var updateCommand = new UpdateOrganisationCommand(TestOrganisation.Id, TestOrganisation);
        var updateHandler = new UpdateOrganisationCommandHandler(TestDbContext, Mapper, UpdateLogger.Object);

        //Act
        var result = await updateHandler.Handle(updateCommand, new CancellationToken());

        //Assert
        result.Should().NotBe(0);
        result.Should().Be(TestOrganisation.Id);

        var actualService = TestDbContext.Services.SingleOrDefault(s => s.Name == service.Name);
        actualService.Should().NotBeNull();
        actualService!.HolidaySchedules.Count.Should().Be(1);

        var actualEntity = TestDbContext.HolidaySchedules.SingleOrDefault(s => s.ServiceId == service.Id);
        actualEntity.Should().NotBeNull();
        actualEntity.Should().BeEquivalentTo(expected, options => 
            options.Excluding(info => info.Name.Contains("Id"))
                .Excluding(info => info.Name.Contains("Distance")));

        var unexpectedEntity = TestDbContext.HolidaySchedules.Where(lc => lc.Id == existingItem.Id).ToList();
        unexpectedEntity.Should().HaveCount(0);
    }

    [Fact]
    public async Task ThenUpdateOrganisationUpdatedHolidaySchedules()
    {
        //Arrange
        await CreateOrganisation();
        var service = TestOrganisation.Services.ElementAt(0);

        var expected = service.HolidaySchedules.ElementAt(0);
        expected.StartDate = new DateTime(2023, 1, 2).ToUniversalTime();
        expected.EndDate = new DateTime(2023, 1, 2).ToUniversalTime();
        
        var updateCommand = new UpdateOrganisationCommand(TestOrganisation.Id, TestOrganisation);
        var updateHandler = new UpdateOrganisationCommandHandler(TestDbContext, Mapper, UpdateLogger.Object);

        //Act
        var result = await updateHandler.Handle(updateCommand, new CancellationToken());

        //Assert
        result.Should().NotBe(0);
        result.Should().Be(TestOrganisation.Id);

        var actualService = TestDbContext.Services.SingleOrDefault(s => s.Name == service.Name);
        actualService.Should().NotBeNull();
        actualService!.HolidaySchedules.Count.Should().Be(1);

        var actualEntity = TestDbContext.HolidaySchedules.SingleOrDefault(s => s.ServiceId == service.Id);
        actualEntity.Should().NotBeNull();
        actualEntity.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task ThenUpdateOrganisationAddAndDeleteLocations()
    {
        //Arrange
        await CreateOrganisation();
        var service = TestOrganisation.Services.ElementAt(0);
        var location = service.Locations.ElementAt(0);
        var expected = new LocationDto
        {
            Name = "New Location",
            Description = "new Description",
            Address1 = "Address1",
            City = "City",
            Country = "Country",
            PostCode = "PostCode",
            StateProvince = "StateProvince",
            LocationType = LocationType.NotSet,
            Latitude = 0,
            Longitude = 0
        };
        service.Locations.Clear();
        service.Locations.Add(expected);

        var updateCommand = new UpdateOrganisationCommand(TestOrganisation.Id, TestOrganisation);
        var updateHandler = new UpdateOrganisationCommandHandler(TestDbContext, Mapper, UpdateLogger.Object);

        //Act
        var result = await updateHandler.Handle(updateCommand, new CancellationToken());

        //Assert
        result.Should().NotBe(0);
        result.Should().Be(TestOrganisation.Id);

        var actualService = TestDbContext.Services.SingleOrDefault(s => s.Name == service.Name);
        actualService.Should().NotBeNull();
        actualService!.Locations.Count.Should().Be(1);

        var actualEntity = TestDbContext.Locations.SingleOrDefault(s => s.Name == expected.Name);
        actualEntity.Should().NotBeNull();
        actualEntity.Should().BeEquivalentTo(expected, options => 
            options.Excluding((IMemberInfo info) => info.Name.Contains("Id"))
                .Excluding((IMemberInfo info) => info.Name.Contains("Distance")));

        //Delete wont cascade delete Locations, so existing will be left behind
        var detachedEntity = TestDbContext.Locations.SingleOrDefault(s => s.Name == location.Name);
        detachedEntity.Should().NotBeNull();
        detachedEntity.Should().BeEquivalentTo(location, options => 
            options.Excluding((IMemberInfo info) => info.Name.Contains("Id"))
                .Excluding((IMemberInfo info) => info.Name.Contains("Distance")));
    }

    [Fact]
    public async Task ThenUpdateOrganisationUpdatedLocations()
    {
        //Arrange
        await CreateOrganisation();
        var service = TestOrganisation.Services.ElementAt(0);

        var expected = service.Locations.ElementAt(0);
        expected.Name = "Updated Name";
        expected.Description = "Updated Description";
        
        var updateCommand = new UpdateOrganisationCommand(TestOrganisation.Id, TestOrganisation);
        var updateHandler = new UpdateOrganisationCommandHandler(TestDbContext, Mapper, UpdateLogger.Object);

        //Act
        var result = await updateHandler.Handle(updateCommand, new CancellationToken());

        //Assert
        result.Should().NotBe(0);
        result.Should().Be(TestOrganisation.Id);

        var actualService = TestDbContext.Services.SingleOrDefault(s => s.Name == service.Name);
        actualService.Should().NotBeNull();
        actualService!.Locations.Count.Should().Be(1);

        var actualEntity = TestDbContext.Locations.SingleOrDefault(s => s.Name == expected.Name);
        actualEntity.Should().NotBeNull();
        actualEntity.Should().BeEquivalentTo(expected, options => 
            options.Excluding((IMemberInfo info) => info.Name.Contains("Id"))
                .Excluding((IMemberInfo info) => info.Name.Contains("Distance")));
    }

    [Fact]
    public async Task ThenUpdateOrganisationAttachExistingLocations()
    {
        //Arrange
        await CreateOrganisation();
        var expected = TestDataProvider.GetTestCountyCouncilDto2().Services.ElementAt(0).Locations.ElementAt(0);
        expected.Id = await CreateLocation(expected);
        expected.Name = "Existing Location already Saved in DB";

        var service = TestOrganisation.Services.ElementAt(0);
        service.Locations.Add(expected);
        
        var updateCommand = new UpdateOrganisationCommand(TestOrganisation.Id, TestOrganisation);
        var updateHandler = new UpdateOrganisationCommandHandler(TestDbContext, Mapper, UpdateLogger.Object);

        //Act
        var result = await updateHandler.Handle(updateCommand, new CancellationToken());

        //Assert
        result.Should().NotBe(0);
        result.Should().Be(TestOrganisation.Id);

        var actualService = TestDbContext.Services.SingleOrDefault(s => s.Name == service.Name);
        actualService.Should().NotBeNull();
        actualService!.Locations.Count.Should().Be(2);

        var actualEntity = TestDbContext.Locations.SingleOrDefault(s => s.Name == expected.Name);
        actualEntity.Should().NotBeNull();
        actualEntity.Should().BeEquivalentTo(expected, options => 
            options.Excluding((IMemberInfo info) => info.Name.Contains("Id"))
                .Excluding((IMemberInfo info) => info.Name.Contains("Distance")));
    }

    [Fact]
    public async Task ThenUpdateOrganisationAddAndDeleteTaxonomies()
    {
        //Arrange
        await CreateOrganisation();
        var service = TestOrganisation.Services.ElementAt(0);
        var taxonomy = service.Taxonomies.ElementAt(0);
        var expected = new TaxonomyDto
        {
            Name = "New Taxonomy"
        };
        service.Taxonomies.Clear();
        service.Taxonomies.Add(expected);

        var updateCommand = new UpdateOrganisationCommand(TestOrganisation.Id, TestOrganisation);
        var updateHandler = new UpdateOrganisationCommandHandler(TestDbContext, Mapper, UpdateLogger.Object);

        //Act
        var result = await updateHandler.Handle(updateCommand, new CancellationToken());

        //Assert
        result.Should().NotBe(0);
        result.Should().Be(TestOrganisation.Id);

        var actualService = TestDbContext.Services.SingleOrDefault(s => s.Name == service.Name);
        actualService.Should().NotBeNull();
        actualService!.Taxonomies.Count.Should().Be(1);

        var actualEntity = TestDbContext.Taxonomies.SingleOrDefault(s => s.Name == expected.Name);
        actualEntity.Should().NotBeNull();
        actualEntity.Should().BeEquivalentTo(expected, options => 
            options.Excluding((IMemberInfo info) => info.Name.Contains("Id"))
                .Excluding((IMemberInfo info) => info.Name.Contains("Distance")));

        //Delete wont cascade delete Taxonomies, so existing will be left behind
        var detachedEntity = TestDbContext.Taxonomies.SingleOrDefault(s => s.Name == taxonomy.Name);
        detachedEntity.Should().NotBeNull();
        detachedEntity.Should().BeEquivalentTo(taxonomy, options => 
            options.Excluding((IMemberInfo info) => info.Name.Contains("Id"))
                .Excluding((IMemberInfo info) => info.Name.Contains("Distance")));
    }

    [Fact]
    public async Task ThenUpdateOrganisationUpdatedTaxonomies()
    {
        //Arrange
        await CreateOrganisation();
        var service = TestOrganisation.Services.ElementAt(0);

        var expected = service.Taxonomies.ElementAt(0);
        expected.Name = "Updated Name";
        
        var updateCommand = new UpdateOrganisationCommand(TestOrganisation.Id, TestOrganisation);
        var updateHandler = new UpdateOrganisationCommandHandler(TestDbContext, Mapper, UpdateLogger.Object);

        //Act
        var result = await updateHandler.Handle(updateCommand, new CancellationToken());

        //Assert
        result.Should().NotBe(0);
        result.Should().Be(TestOrganisation.Id);

        var actualService = TestDbContext.Services.SingleOrDefault(s => s.Name == service.Name);
        actualService.Should().NotBeNull();
        actualService!.Taxonomies.Count.Should().Be(4);

        var actualEntity = TestDbContext.Taxonomies.SingleOrDefault(s => s.Name == expected.Name);
        actualEntity.Should().NotBeNull();
        actualEntity.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task ThenUpdateOrganisationAttachExistingTaxonomies()
    {
        //Arrange
        await CreateOrganisation();
        var expected = new TaxonomyDto
        {
            Name = "New Taxonomy Name",
            TaxonomyType = TaxonomyType.ServiceCategory
        };
        expected.Id = await CreateTaxonomy(expected);

        var service = TestOrganisation.Services.ElementAt(0);
        service.Taxonomies.Add(expected);
        
        var updateCommand = new UpdateOrganisationCommand(TestOrganisation.Id, TestOrganisation);
        var updateHandler = new UpdateOrganisationCommandHandler(TestDbContext, Mapper, UpdateLogger.Object);

        //Act
        var result = await updateHandler.Handle(updateCommand, new CancellationToken());

        //Assert
        result.Should().NotBe(0);
        result.Should().Be(TestOrganisation.Id);

        var actualService = TestDbContext.Services.SingleOrDefault(s => s.Name == service.Name);
        actualService.Should().NotBeNull();
        actualService!.Taxonomies.Count.Should().Be(5);

        var actualEntity = TestDbContext.Taxonomies.SingleOrDefault(s => s.Name == expected.Name);
        actualEntity.Should().NotBeNull();
        actualEntity.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task ThenUpdateOrganisationLocationsAddAndDeleteContacts()
    {
        //Arrange
        await CreateOrganisation();
        var service = TestOrganisation.Services.ElementAt(0);
        var existingItem = service.Locations.ElementAt(0).Contacts.ElementAt(0);
        var contact = new ContactDto
        {
            Id = 0,
            Name = "New Contact",
            Telephone = "New Telephone"
        };
        service.Locations.ElementAt(0).Contacts.Clear();
        service.Locations.ElementAt(0).Contacts.Add(contact);

        var updateCommand = new UpdateOrganisationCommand(TestOrganisation.Id, TestOrganisation);
        var updateHandler = new UpdateOrganisationCommandHandler(TestDbContext, Mapper, UpdateLogger.Object);

        //Act
        var result = await updateHandler.Handle(updateCommand, new CancellationToken());

        //Assert
        result.Should().NotBe(0);
        result.Should().Be(TestOrganisation.Id);

        var actualService = TestDbContext.Services.SingleOrDefault(s => s.Name == service.Name);
        actualService.Should().NotBeNull();
        actualService!.Locations.Count.Should().Be(1);

        var actualContact = TestDbContext.Contacts.SingleOrDefault(s => s.Name == contact.Name);
        actualContact.Should().NotBeNull();
        actualContact.Should().BeEquivalentTo(contact, options => 
            options.Excluding((IMemberInfo info) => info.Name.Contains("Id"))
                .Excluding((IMemberInfo info) => info.Name.Contains("Distance")));

        var unexpectedEntity = TestDbContext.Contacts.Where(lc => lc.Id == existingItem.Id).ToList();
        unexpectedEntity.Should().HaveCount(0);
    }

    [Fact]
    public async Task ThenUpdateOrganisationLocationsWithUpdatedContacts()
    {
        //Arrange
        await CreateOrganisation();
        var service = TestOrganisation.Services.ElementAt(0);
        var contact = service.Locations.ElementAt(0).Contacts.ElementAt(0);
        contact.Name = "Updated Name";
        contact.Email = "Updated Email";
        contact.Telephone = "Updated Telephone";

        var updateCommand = new UpdateOrganisationCommand(TestOrganisation.Id, TestOrganisation);
        var updateHandler = new UpdateOrganisationCommandHandler(TestDbContext, Mapper, UpdateLogger.Object);

        //Act
        var result = await updateHandler.Handle(updateCommand, new CancellationToken());

        //Assert
        result.Should().NotBe(0);
        result.Should().Be(TestOrganisation.Id);

        var actualService = TestDbContext.Services.SingleOrDefault(s => s.Name == service.Name);
        actualService.Should().NotBeNull();
        actualService!.Locations.ElementAt(0).Contacts.Count.Should().Be(1);

        var actualContact = TestDbContext.Contacts.SingleOrDefault(s => s.Name == contact.Name);
        actualContact.Should().NotBeNull();
        actualContact.Should().BeEquivalentTo(contact);
    }

    [Fact]
    public async Task ThenUpdateOrganisationLocationsAddAndDeleteRegularSchedules()
    {
        //Arrange
        await CreateOrganisation();
        var service = TestOrganisation.Services.ElementAt(0);
        var existingItem = service.Locations.ElementAt(0).RegularSchedules.ElementAt(0);
        var expected = new RegularScheduleDto
        {
            ValidFrom = DateTime.UtcNow,
            ValidTo = DateTime.UtcNow,
            ByDay = "New ByDay",
            ByMonthDay = "New ByMonthDay"
        };

        service.Locations.ElementAt(0).RegularSchedules.Clear();
        service.Locations.ElementAt(0).RegularSchedules.Add(expected);

        var updateCommand = new UpdateOrganisationCommand(TestOrganisation.Id, TestOrganisation);
        var updateHandler = new UpdateOrganisationCommandHandler(TestDbContext, Mapper, UpdateLogger.Object);

        //Act
        var result = await updateHandler.Handle(updateCommand, new CancellationToken());

        //Assert
        result.Should().NotBe(0);
        result.Should().Be(TestOrganisation.Id);

        var actualService = TestDbContext.Services.SingleOrDefault(s => s.Name == service.Name);
        actualService.Should().NotBeNull();
        actualService!.Locations.ElementAt(0).RegularSchedules.Count.Should().Be(1);

        var actualEntity = TestDbContext.RegularSchedules.SingleOrDefault(s => s.ByDay == expected.ByDay);
        actualEntity.Should().NotBeNull();
        actualEntity.Should().BeEquivalentTo(expected, options => 
            options.Excluding(info => info.Name.Contains("Id"))
                .Excluding(info => info.Name.Contains("Distance")));

        var unexpectedEntity = TestDbContext.RegularSchedules.Where(lc => lc.Id == existingItem.Id).ToList();
        unexpectedEntity.Should().HaveCount(0);
    }

    [Fact]
    public async Task ThenUpdateOrganisationLocationsUpdatedRegularSchedules()
    {
        //Arrange
        await CreateOrganisation();
        var service = TestOrganisation.Services.ElementAt(0);

        var expected = service.Locations.ElementAt(0).RegularSchedules.ElementAt(0);
        expected.ByDay = "Updated ByDay";
        expected.ByMonthDay = "Updated ByMonthDay";
        
        var updateCommand = new UpdateOrganisationCommand(TestOrganisation.Id, TestOrganisation);
        var updateHandler = new UpdateOrganisationCommandHandler(TestDbContext, Mapper, UpdateLogger.Object);

        //Act
        var result = await updateHandler.Handle(updateCommand, new CancellationToken());

        //Assert
        result.Should().NotBe(0);
        result.Should().Be(TestOrganisation.Id);

        var actualService = TestDbContext.Services.SingleOrDefault(s => s.Name == service.Name);
        actualService.Should().NotBeNull();
        actualService!.Locations.ElementAt(0).RegularSchedules.Count.Should().Be(1);

        var actualEntity = TestDbContext.RegularSchedules.SingleOrDefault(s => s.ByDay == expected.ByDay);
        actualEntity.Should().NotBeNull();
        actualEntity.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task ThenUpdateOrganisationLocationsAddAndDeleteHolidaySchedules()
    {
        //Arrange
        await CreateOrganisation();
        var service = TestOrganisation.Services.ElementAt(0);
        var existingItem = service.Locations.ElementAt(0).HolidaySchedules.ElementAt(0);
        var expected = new HolidayScheduleDto
        {
            StartDate = new DateTime(2023, 1, 2).ToUniversalTime(),
            EndDate = new DateTime(2023, 1, 2).ToUniversalTime(),
        };
        service.Locations.ElementAt(0).HolidaySchedules.Clear();
        service.Locations.ElementAt(0).HolidaySchedules.Add(expected);

        var updateCommand = new UpdateOrganisationCommand(TestOrganisation.Id, TestOrganisation);
        var updateHandler = new UpdateOrganisationCommandHandler(TestDbContext, Mapper, UpdateLogger.Object);

        //Act
        var result = await updateHandler.Handle(updateCommand, new CancellationToken());

        //Assert
        result.Should().NotBe(0);
        result.Should().Be(TestOrganisation.Id);

        var actualService = TestDbContext.Services.SingleOrDefault(s => s.Name == service.Name);
        actualService.Should().NotBeNull();
        actualService!.Locations.ElementAt(0).HolidaySchedules.Count.Should().Be(1);

        var actualEntity = TestDbContext.HolidaySchedules.SingleOrDefault(s => s.StartDate == expected.StartDate);
        actualEntity.Should().NotBeNull();
        actualEntity.Should().BeEquivalentTo(expected, options => 
            options.Excluding(info => info.Name.Contains("Id"))
                .Excluding(info => info.Name.Contains("Distance")));

        var unexpectedEntity = TestDbContext.HolidaySchedules.Where(lc => lc.Id == existingItem.Id).ToList();
        unexpectedEntity.Should().HaveCount(0);
    }

    [Fact]
    public async Task ThenUpdateOrganisationLocationsUpdatedHolidaySchedules()
    {
        //Arrange
        await CreateOrganisation();
        var service = TestOrganisation.Services.ElementAt(0);

        var expected = service.Locations.ElementAt(0).HolidaySchedules.ElementAt(0);
        expected.StartDate = new DateTime(2023, 1, 2).ToUniversalTime();
        expected.EndDate = new DateTime(2023, 1, 2).ToUniversalTime();
        
        var updateCommand = new UpdateOrganisationCommand(TestOrganisation.Id, TestOrganisation);
        var updateHandler = new UpdateOrganisationCommandHandler(TestDbContext, Mapper, UpdateLogger.Object);

        //Act
        var result = await updateHandler.Handle(updateCommand, new CancellationToken());

        //Assert
        result.Should().NotBe(0);
        result.Should().Be(TestOrganisation.Id);

        var actualService = TestDbContext.Services.SingleOrDefault(s => s.Name == service.Name);
        actualService.Should().NotBeNull();
        actualService!.Locations.ElementAt(0).HolidaySchedules.Count.Should().Be(1);

        var actualEntity = TestDbContext.HolidaySchedules.SingleOrDefault(s => s.StartDate == expected.StartDate);
        actualEntity.Should().NotBeNull();
        actualEntity.Should().BeEquivalentTo(expected);
    }
}