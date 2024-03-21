using FamilyHubs.ServiceDirectory.Core.Commands.Organisations.UpdateOrganisation;
using FamilyHubs.ServiceDirectory.Core.Exceptions;
using FamilyHubs.ServiceDirectory.Shared.Dto;
using FamilyHubs.ServiceDirectory.Shared.Enums;
using FamilyHubs.SharedKernel.Identity;
using FluentAssertions;
using FluentAssertions.Equivalency;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using System.Security.Claims;

namespace FamilyHubs.ServiceDirectory.Core.IntegrationTests.Organisations;

public class WhenUsingUpdateOrganisationCommand : DataIntegrationTestBase
{
    public readonly Mock<IHttpContextAccessor> _mockHttpContextAccessor;
    public readonly Mock<ILogger<UpdateOrganisationCommandHandler>> UpdateLogger = new();

    public WhenUsingUpdateOrganisationCommand()
    {
        _mockHttpContextAccessor = GetMockHttpContextAccessor(-1, RoleTypes.DfeAdmin);
    }

    [Fact]
    public async Task ThenUpdateOrganisationOnly()
    {
        //Arrange
        await CreateOrganisationDetails();
        var service = TestOrganisation.Services.ElementAt(0);
        TestOrganisation.Name = "Unit Test Update TestOrganisation Name";
        TestOrganisation.Description = "Unit Test Update TestOrganisation Name";

        var updateCommand = new UpdateOrganisationCommand(TestOrganisation.Id, TestOrganisation);
        var updateHandler = new UpdateOrganisationCommandHandler(_mockHttpContextAccessor.Object, TestDbContext, Mapper, UpdateLogger.Object);

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
        await CreateOrganisationDetails();
        var service = TestOrganisation.Services.ElementAt(0);
        var existingItem = service.Eligibilities.ElementAt(0);

        var expected = new EligibilityDto
        {
            //ServiceId = service.Id,
            MaximumAge = 2,
            MinimumAge = 0,
            EligibilityType = null
        };
        service.Eligibilities.Clear();
        service.Eligibilities.Add(expected);

        var updateCommand = new UpdateOrganisationCommand(TestOrganisation.Id, TestOrganisation);
        var updateHandler = new UpdateOrganisationCommandHandler(_mockHttpContextAccessor.Object, TestDbContext, Mapper, UpdateLogger.Object);

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
        await CreateOrganisationDetails();
        var service = TestOrganisation.Services.ElementAt(0);

        var expected = service.Eligibilities.ElementAt(0);
        expected.MinimumAge = 500;
        expected.MaximumAge = 5000;

        var updateCommand = new UpdateOrganisationCommand(TestOrganisation.Id, TestOrganisation);
        var updateHandler = new UpdateOrganisationCommandHandler(_mockHttpContextAccessor.Object, TestDbContext, Mapper, UpdateLogger.Object);

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
        await CreateOrganisationDetails();
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
        var updateHandler = new UpdateOrganisationCommandHandler(_mockHttpContextAccessor.Object, TestDbContext, Mapper, UpdateLogger.Object);

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
        await CreateOrganisationDetails();
        var service = TestOrganisation.Services.ElementAt(0);

        var expected = service.ServiceAreas.ElementAt(0);
        expected.ServiceAreaName = "Updated ServiceAreaName";
        expected.Extent = "Updated Extent";

        var updateCommand = new UpdateOrganisationCommand(TestOrganisation.Id, TestOrganisation);
        var updateHandler = new UpdateOrganisationCommandHandler(_mockHttpContextAccessor.Object, TestDbContext, Mapper, UpdateLogger.Object);

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
        await CreateOrganisationDetails();
        var service = TestOrganisation.Services.ElementAt(0);
        var existingItem = service.ServiceDeliveries.ElementAt(0);

        service.ServiceDeliveries.Clear();

        var updateCommand = new UpdateOrganisationCommand(TestOrganisation.Id, TestOrganisation);
        var updateHandler = new UpdateOrganisationCommandHandler(_mockHttpContextAccessor.Object, TestDbContext, Mapper, UpdateLogger.Object);

        //Act
        var result = await updateHandler.Handle(updateCommand, new CancellationToken());

        //Assert
        result.Should().NotBe(0);
        result.Should().Be(TestOrganisation.Id);

        var actualService = TestDbContext.Services.SingleOrDefault(s => s.Name == service.Name);
        actualService.Should().NotBeNull();
        actualService!.ServiceDeliveries.Count.Should().Be(0);

        var unexpectedEntity = TestDbContext.ServiceDeliveries.Where(lc => lc.Id == existingItem.Id).ToList();
        unexpectedEntity.Should().HaveCount(0);
    }

    [Fact]
    public async Task ThenUpdateOrganisationUpdatedServiceDeliveries()
    {
        //Arrange
        await CreateOrganisationDetails();
        var service = TestOrganisation.Services.ElementAt(0);

        var updateCommand = new UpdateOrganisationCommand(TestOrganisation.Id, TestOrganisation);
        var updateHandler = new UpdateOrganisationCommandHandler(_mockHttpContextAccessor.Object, TestDbContext, Mapper, UpdateLogger.Object);

        //Act
        var result = await updateHandler.Handle(updateCommand, new CancellationToken());

        //Assert
        result.Should().NotBe(0);
        result.Should().Be(TestOrganisation.Id);

        var actualService = TestDbContext.Services.SingleOrDefault(s => s.Name == service.Name);
        actualService.Should().NotBeNull();
        actualService!.ServiceDeliveries.Count.Should().Be(1);

        //todo:
        //var actualEntity = TestDbContext.ServiceDeliveries.SingleOrDefault(s => s.Name == expected.Name);
        //actualEntity.Should().NotBeNull();
        //actualEntity.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task ThenUpdateOrganisationAddAndDeleteLanguages()
    {
        //Arrange
        await CreateOrganisationDetails();
        var service = TestOrganisation.Services.ElementAt(0);
        var existingItem = service.Languages.ElementAt(0);
        var expected = new LanguageDto()
        {
            Name = "New Language",
            Code = "xx"
        };

        service.Languages.Clear();
        service.Languages.Add(expected);

        var updateCommand = new UpdateOrganisationCommand(TestOrganisation.Id, TestOrganisation);
        var updateHandler = new UpdateOrganisationCommandHandler(_mockHttpContextAccessor.Object, TestDbContext, Mapper, UpdateLogger.Object);

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
        await CreateOrganisationDetails();
        var service = TestOrganisation.Services.ElementAt(0);

        var expected = service.Languages.ElementAt(0);
        expected.Name = "Updated Language";

        var updateCommand = new UpdateOrganisationCommand(TestOrganisation.Id, TestOrganisation);
        var updateHandler = new UpdateOrganisationCommandHandler(_mockHttpContextAccessor.Object, TestDbContext, Mapper, UpdateLogger.Object);

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
        await CreateOrganisationDetails();
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
        var updateHandler = new UpdateOrganisationCommandHandler(_mockHttpContextAccessor.Object, TestDbContext, Mapper, UpdateLogger.Object);

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
        await CreateOrganisationDetails();
        var service = TestOrganisation.Services.ElementAt(0);

        var expected = service.CostOptions.ElementAt(0);
        expected.Amount = 987;
        expected.Option = "Updated Option";
        expected.AmountDescription = "Updated Amount Description";

        var updateCommand = new UpdateOrganisationCommand(TestOrganisation.Id, TestOrganisation);
        var updateHandler = new UpdateOrganisationCommandHandler(_mockHttpContextAccessor.Object, TestDbContext, Mapper, UpdateLogger.Object);

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
        await CreateOrganisationDetails();
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
        var updateHandler = new UpdateOrganisationCommandHandler(_mockHttpContextAccessor.Object, TestDbContext, Mapper, UpdateLogger.Object);

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
        actualContact.Should().BeEquivalentTo(contact, options =>
            options.Excluding((IMemberInfo info) => info.Name.Contains("Id"))
                .Excluding((IMemberInfo info) => info.Name.Contains("Distance")));

        var unexpectedEntity = TestDbContext.Contacts.Where(lc => lc.Id == existingItem.Id).ToList();
        unexpectedEntity.Should().HaveCount(0);
    }

    [Fact]
    public async Task ThenUpdateOrganisationWithUpdatedContacts()
    {
        //Arrange
        await CreateOrganisationDetails();
        var service = TestOrganisation.Services.ElementAt(0);
        var contact = service.Contacts.ElementAt(0);
        contact.Name = "Updated Name";
        contact.Email = "Updated Email";
        contact.Telephone = "Updated Telephone";

        var updateCommand = new UpdateOrganisationCommand(TestOrganisation.Id, TestOrganisation);
        var updateHandler = new UpdateOrganisationCommandHandler(_mockHttpContextAccessor.Object, TestDbContext, Mapper, UpdateLogger.Object);

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

    //todo: this will go when we have an OrganisationChangeDto
    //[Fact]
    //public async Task ThenUpdateOrganisationAddAndDeleteSchedules()
    //{
    //    //Arrange
    //    await CreateOrganisationDetails();
    //    var service = TestOrganisation.Services.ElementAt(0);
    //    var existingItem = service.Schedules.ElementAt(0);
    //    var expected = new ScheduleDto
    //    {
    //        ValidFrom = DateTime.UtcNow,
    //        ValidTo = DateTime.UtcNow,
    //        ByDay = "New ByDay",
    //        ByMonthDay = "New ByMonthDay"
    //    };

    //    service.Schedules.Clear();
    //    service.Schedules.Add(expected);

    //    var updateCommand = new UpdateOrganisationCommand(TestOrganisation.Id, TestOrganisation);
    //    var updateHandler = new UpdateOrganisationCommandHandler(_mockHttpContextAccessor.Object, TestDbContext, Mapper, UpdateLogger.Object);

    //    //Act
    //    var result = await updateHandler.Handle(updateCommand, new CancellationToken());

    //    //Assert
    //    result.Should().NotBe(0);
    //    result.Should().Be(TestOrganisation.Id);

    //    var actualService = TestDbContext.Services.SingleOrDefault(s => s.Name == service.Name);
    //    actualService.Should().NotBeNull();
    //    actualService!.Schedules.Count.Should().Be(1);

    //    var actualEntity = TestDbContext.Schedules.SingleOrDefault(s => s.ByDay == expected.ByDay);
    //    actualEntity.Should().NotBeNull();
    //    actualEntity.Should().BeEquivalentTo(expected, options =>
    //        options.Excluding(info => info.Name.Contains("Id"))
    //            .Excluding(info => info.Name.Contains("Distance")));

    //    var unexpectedEntity = TestDbContext.Schedules.Where(lc => lc.Id == existingItem.Id).ToList();
    //    unexpectedEntity.Should().HaveCount(0);
    //}

    [Fact]
    public async Task ThenUpdateOrganisationUpdatedSchedules()
    {
        //Arrange
        await CreateOrganisationDetails();
        var service = TestOrganisation.Services.ElementAt(0);

        var expected = service.Schedules.ElementAt(0);
        expected.ByDay = "Updated ByDay";
        expected.ByMonthDay = "Updated ByMonthDay";

        var updateCommand = new UpdateOrganisationCommand(TestOrganisation.Id, TestOrganisation);
        var updateHandler = new UpdateOrganisationCommandHandler(_mockHttpContextAccessor.Object, TestDbContext, Mapper, UpdateLogger.Object);

        //Act
        var result = await updateHandler.Handle(updateCommand, new CancellationToken());

        //Assert
        result.Should().NotBe(0);
        result.Should().Be(TestOrganisation.Id);

        var actualService = TestDbContext.Services.SingleOrDefault(s => s.Name == service.Name);
        actualService.Should().NotBeNull();
        actualService!.Schedules.Count.Should().Be(1);

        var actualEntity = TestDbContext.Schedules.SingleOrDefault(s => s.ByDay == expected.ByDay);
        actualEntity.Should().NotBeNull();
        actualEntity.Should().BeEquivalentTo(expected);
    }

    //todo: we need an OrganisationChangeDto & it would be better perhaps to just allow it to reference existing services/locations
    // services and locations have their own api, not sure it makes sense to allow updated throughout the object graph, i.e. don't use update organisation to change a service's contact for example
    //[Fact]
    //public async Task ThenUpdateOrganisationAddAndDeleteLocations()
    //{
    //    //Arrange
    //    await CreateOrganisationDetails();
    //    var service = TestOrganisation.Services.ElementAt(0);
    //    var location = service.Locations.ElementAt(0);
    //    var expected = new LocationDto
    //    {
    //        Name = "New Location",
    //        Description = "new Description",
    //        Address1 = "Address1",
    //        City = "City",
    //        Country = "Country",
    //        PostCode = "PostCode",
    //        StateProvince = "StateProvince",
    //        LocationTypeCategory = LocationTypeCategory.NotSet,
    //        Latitude = 0,
    //        Longitude = 0,
    //        LocationType= LocationType.Postal
    //    };
    //    service.Locations.Clear();
    //    service.Locations.Add(expected);

    //    var updateCommand = new UpdateOrganisationCommand(TestOrganisation.Id, TestOrganisation);
    //    var updateHandler = new UpdateOrganisationCommandHandler(_mockHttpContextAccessor.Object, TestDbContext, Mapper, UpdateLogger.Object);

    //    //Act
    //    var result = await updateHandler.Handle(updateCommand, new CancellationToken());

    //    //Assert
    //    result.Should().NotBe(0);
    //    result.Should().Be(TestOrganisation.Id);

    //    var actualService = TestDbContext.Services.SingleOrDefault(s => s.Name == service.Name);
    //    actualService.Should().NotBeNull();
    //    actualService!.Locations.Count.Should().Be(1);

    //    var actualEntity = TestDbContext.Locations.SingleOrDefault(s => s.Name == expected.Name);
    //    actualEntity.Should().NotBeNull();
    //    actualEntity.Should().BeEquivalentTo(expected, options =>
    //        options.Excluding((IMemberInfo info) => info.Name.Contains("Id"))
    //            .Excluding((IMemberInfo info) => info.Name.Contains("Distance")));

    //    //Delete wont cascade delete Locations, so existing will be left behind
    //    var detachedEntity = TestDbContext.Locations.SingleOrDefault(s => s.Name == location.Name);
    //    detachedEntity.Should().NotBeNull();
    //    detachedEntity.Should().BeEquivalentTo(location, options =>
    //        options.Excluding((IMemberInfo info) => info.Name.Contains("Id"))
    //            .Excluding((IMemberInfo info) => info.Name.Contains("Distance")));
    //}

    [Fact]
    public async Task ThenUpdateOrganisationUpdatedLocations()
    {
        //Arrange
        await CreateOrganisationDetails();
        var service = TestOrganisation.Services.ElementAt(0);

        var expected = service.Locations.ElementAt(0);
        expected.Name = "Updated Name";
        expected.Description = "Updated Description";

        var expectedOtherInstance = TestOrganisation.Locations.First();
        expectedOtherInstance.Name = "Updated Name";
        expectedOtherInstance.Description = "Updated Description";

        var updateCommand = new UpdateOrganisationCommand(TestOrganisation.Id, TestOrganisation);
        var updateHandler = new UpdateOrganisationCommandHandler(_mockHttpContextAccessor.Object, TestDbContext, Mapper, UpdateLogger.Object);

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
        await CreateOrganisationDetails();
        var expected = TestDataProvider.GetTestCountyCouncilDto2().Services.ElementAt(0).Locations.ElementAt(0) with { Name = "Existing Location already Saved in DB" };
        expected.OrganisationId = TestOrganisation.Id;
        expected.Id = await CreateLocation(expected);

        var service = TestOrganisation.Services.ElementAt(0);
        service.Locations.Add(expected);

        var updateCommand = new UpdateOrganisationCommand(TestOrganisation.Id, TestOrganisation);
        var updateHandler = new UpdateOrganisationCommandHandler(_mockHttpContextAccessor.Object, TestDbContext, Mapper, UpdateLogger.Object);

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
        await CreateOrganisationDetails();
        var service = TestOrganisation.Services.ElementAt(0);
        var taxonomy = service.Taxonomies.ElementAt(0);
        var expected = new TaxonomyDto
        {
            Name = "New Taxonomy"
        };
        service.Taxonomies.Clear();
        service.Taxonomies.Add(expected);

        var updateCommand = new UpdateOrganisationCommand(TestOrganisation.Id, TestOrganisation);
        var updateHandler = new UpdateOrganisationCommandHandler(_mockHttpContextAccessor.Object, TestDbContext, Mapper, UpdateLogger.Object);

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
        await CreateOrganisationDetails();
        var service = TestOrganisation.Services.ElementAt(0);

        var expected = service.Taxonomies.ElementAt(0);
        expected.Name = "Updated Name";

        var updateCommand = new UpdateOrganisationCommand(TestOrganisation.Id, TestOrganisation);
        var updateHandler = new UpdateOrganisationCommandHandler(_mockHttpContextAccessor.Object, TestDbContext, Mapper, UpdateLogger.Object);

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
        await CreateOrganisationDetails();
        var expected = new TaxonomyDto
        {
            Name = "New Taxonomy Name",
            TaxonomyType = TaxonomyType.ServiceCategory
        };
        expected.Id = await CreateTaxonomy(expected);

        var service = TestOrganisation.Services.ElementAt(0);
        service.Taxonomies.Add(expected);

        var updateCommand = new UpdateOrganisationCommand(TestOrganisation.Id, TestOrganisation);
        var updateHandler = new UpdateOrganisationCommandHandler(_mockHttpContextAccessor.Object, TestDbContext, Mapper, UpdateLogger.Object);

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
        await CreateOrganisationDetails();
        var service = TestOrganisation.Services.ElementAt(0);
        var location = TestOrganisation.Locations.ElementAt(0);
        var existingItem = location.Contacts.ElementAt(0);
        var contact = new ContactDto
        {
            Id = 0,
            Name = "New Contact",
            Telephone = "New Telephone"
        };

        location.Contacts.Clear();
        location.Contacts.Add(contact);

        service.Locations.ElementAt(0).Contacts.Clear();
        service.Locations.ElementAt(0).Contacts.Add(contact);

        existingItem = service.Locations.ElementAt(0).Contacts.ElementAt(0);
        service.Locations.ElementAt(0).Contacts.Clear();
        service.Locations.ElementAt(0).Contacts.Add(contact);

        var updateCommand = new UpdateOrganisationCommand(TestOrganisation.Id, TestOrganisation);
        var updateHandler = new UpdateOrganisationCommandHandler(_mockHttpContextAccessor.Object, TestDbContext, Mapper, UpdateLogger.Object);

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
        await CreateOrganisationDetails();
        var locationContact = TestOrganisation.Locations.ElementAt(0).Contacts.ElementAt(0);
        var service = TestOrganisation.Services.ElementAt(0);
        var contact = service.Locations.ElementAt(0).Contacts.ElementAt(0);
        contact.Name = "Updated Name";
        contact.Email = "Updated Email";
        contact.Telephone = "Updated Telephone";

        locationContact.Name = "Updated Name";
        locationContact.Email = "Updated Email";
        locationContact.Telephone = "Updated Telephone";

        var updateCommand = new UpdateOrganisationCommand(TestOrganisation.Id, TestOrganisation);
        var updateHandler = new UpdateOrganisationCommandHandler(_mockHttpContextAccessor.Object, TestDbContext, Mapper, UpdateLogger.Object);

        //Act
        var result = await updateHandler.Handle(updateCommand, new CancellationToken());

        //Assert
        result.Should().NotBe(0);
        result.Should().Be(TestOrganisation.Id);

        var actualService = TestDbContext.Services.SingleOrDefault(s => s.Name == service.Name);
        actualService.Should().NotBeNull();
        actualService!.Locations[0].Contacts.Count.Should().Be(1);

        var actualContact = TestDbContext.Contacts.SingleOrDefault(s => s.Name == contact.Name);
        actualContact.Should().NotBeNull();
        actualContact.Should().BeEquivalentTo(contact);
    }

    //todo:
    //[Fact]
    //public async Task ThenUpdateOrganisationLocationsAddAndDeleteSchedules()
    //{
    //    //Arrange
    //    await CreateOrganisationDetails();
    //    var location = TestOrganisation.Locations.ElementAt(0);
    //    var service = TestOrganisation.Services.ElementAt(0);
    //    var existingItem = service.Locations.ElementAt(0).Schedules.ElementAt(0);
    //    var expected = new ScheduleDto
    //    {
    //        ValidFrom = DateTime.UtcNow,
    //        ValidTo = DateTime.UtcNow,
    //        ByDay = "New ByDay",
    //        ByMonthDay = "New ByMonthDay"
    //    };

    //    location.Schedules.Clear();
    //    location.Schedules.Add(expected);

    //    service.Locations.ElementAt(0).Schedules.Clear();
    //    service.Locations.ElementAt(0).Schedules.Add(expected);

    //    var updateCommand = new UpdateOrganisationCommand(TestOrganisation.Id, TestOrganisation);
    //    var updateHandler = new UpdateOrganisationCommandHandler(_mockHttpContextAccessor.Object, TestDbContext, Mapper, UpdateLogger.Object);

    //    //Act
    //    var result = await updateHandler.Handle(updateCommand, new CancellationToken());

    //    //Assert
    //    result.Should().NotBe(0);
    //    result.Should().Be(TestOrganisation.Id);

    //    var actualService = TestDbContext.Services.SingleOrDefault(s => s.Name == service.Name);
    //    actualService.Should().NotBeNull();
    //    actualService!.Locations[0].Schedules.Count.Should().Be(1);

    //    var actualEntity = TestDbContext.Schedules.SingleOrDefault(s => s.ByDay == expected.ByDay);
    //    actualEntity.Should().NotBeNull();
    //    actualEntity.Should().BeEquivalentTo(expected, options =>
    //        options.Excluding(info => info.Name.Contains("Id"))
    //            .Excluding(info => info.Name.Contains("Distance")));

    //    var unexpectedEntity = TestDbContext.Schedules.Where(lc => lc.Id == existingItem.Id).ToList();
    //    unexpectedEntity.Should().HaveCount(0);
    //}

    [Fact]
    public async Task ThenUpdateOrganisationLocationsUpdatedSchedules()
    {
        //Arrange
        await CreateOrganisationDetails();
        var service = TestOrganisation.Services.ElementAt(0);

        // we have to ensure that each instance of the same schedule is updated
        var expected = TestOrganisation.Locations.First().Schedules.First();
        expected.ByDay = "Updated ByDay";
        expected.ByMonthDay = "Updated ByMonthDay";

        expected = service.Locations.ElementAt(0).Schedules.ElementAt(0);
        expected.ByDay = "Updated ByDay";
        expected.ByMonthDay = "Updated ByMonthDay";

        var updateCommand = new UpdateOrganisationCommand(TestOrganisation.Id, TestOrganisation);
        var updateHandler = new UpdateOrganisationCommandHandler(_mockHttpContextAccessor.Object, TestDbContext, Mapper, UpdateLogger.Object);

        //Act
        var result = await updateHandler.Handle(updateCommand, new CancellationToken());

        //Assert
        result.Should().NotBe(0);
        result.Should().Be(TestOrganisation.Id);

        var actualService = TestDbContext.Services.SingleOrDefault(s => s.Name == service.Name);
        actualService.Should().NotBeNull();
        actualService!.Locations[0].Schedules.Count.Should().Be(1);

        var actualEntity = TestDbContext.Schedules.SingleOrDefault(s => s.ByDay == expected.ByDay);
        actualEntity.Should().NotBeNull();
        actualEntity.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task ThenUpdateOrganisation_ThrowsForbiddenException()
    {
        //Arrange
        await CreateOrganisationDetails();
        var mockHttpContextAccessor = GetMockHttpContextAccessor(50, RoleTypes.LaManager);

        var updateCommand = new UpdateOrganisationCommand(TestOrganisation.Id, TestOrganisation);
        var updateHandler = new UpdateOrganisationCommandHandler(mockHttpContextAccessor.Object, TestDbContext, Mapper, UpdateLogger.Object);

        //Act / Assert
        await Assert.ThrowsAsync<ForbiddenException>(async () => await updateHandler.Handle(updateCommand, new CancellationToken()));
    }

    private Mock<IHttpContextAccessor> GetMockHttpContextAccessor(long organisationId, string userRole)
    {
        var mockUser = new Mock<ClaimsPrincipal>();
        var claims = new List<Claim>();
        claims.Add(new Claim(FamilyHubsClaimTypes.OrganisationId, organisationId.ToString()));
        claims.Add(new Claim(FamilyHubsClaimTypes.Role, userRole));

        mockUser.SetupGet(x => x.Claims).Returns(claims);


        var mockHttpContext = new Mock<HttpContext>();
        mockHttpContext.SetupGet(x => x.User).Returns(mockUser.Object);

        var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
        mockHttpContextAccessor.SetupGet(x => x.HttpContext).Returns(mockHttpContext.Object);

        return mockHttpContextAccessor;
    }
}