using FamilyHubs.ServiceDirectory.Core.Commands.Services.UpdateService;
using FamilyHubs.ServiceDirectory.Shared.CreateUpdateDto;
using FamilyHubs.ServiceDirectory.Shared.Dto;
using FamilyHubs.ServiceDirectory.Shared.Enums;
using FluentAssertions;
using FluentAssertions.Equivalency;
using Microsoft.Extensions.Logging;
using Moq;

namespace FamilyHubs.ServiceDirectory.Core.IntegrationTests.Services;

public class WhenUsingUpdateServiceCommand : DataIntegrationTestBase
{
    public readonly Mock<ILogger<UpdateServiceCommandHandler>> UpdateLogger = new();

    [Fact]
    public async Task ThenUpdateServiceOnly()
    {
        //Arrange
        await CreateOrganisationDetails();
        var service = TestOrganisation.Services.ElementAt(0);
        service.Name = "Unit Test Update Service Name";
        service.Description = "Unit Test Update Service Name";

        var serviceChange = Mapper.Map<ServiceChangeDto>(service);

        var updateCommand = new UpdateServiceCommand(service.Id, serviceChange);
        var updateHandler = new UpdateServiceCommandHandler(TestDbContext, Mapper, UpdateLogger.Object);

        //Act
        var result = await updateHandler.Handle(updateCommand, new CancellationToken());

        //Assert
        result.Should().NotBe(0);
        result.Should().Be(service.Id);
        var actualService = TestDbContext.Services.SingleOrDefault(s => s.Name == service.Name);
        actualService.Should().NotBeNull();
        actualService!.Description.Should().Be(service.Description);
    }

    //[Fact]
    //public async Task ThenUpdateServiceAddAndDeleteEligibilities()
    //{
    //    //Arrange
    //    await CreateOrganisationDetails();
    //    var service = TestOrganisation.Services.ElementAt(0);
    //    var existingItem = service.Eligibilities.ElementAt(0);

    //    var expected = new EligibilityDto
    //    {
    //        //ServiceId = service.Id,
    //        MaximumAge = 2,
    //        MinimumAge = 0,
    //        EligibilityType = null
    //    };
    //    service.Eligibilities.Clear();
    //    service.Eligibilities.Add(expected);

    //    var updateCommand = new UpdateServiceCommand(service.Id, service);
    //    var updateHandler = new UpdateServiceCommandHandler(TestDbContext, Mapper, UpdateLogger.Object);

    //    //Act
    //    var result = await updateHandler.Handle(updateCommand, new CancellationToken());

    //    //Assert
    //    result.Should().NotBe(0);
    //    result.Should().Be(service.Id);

    //    var actualService = TestDbContext.Services.SingleOrDefault(s => s.Name == service.Name);
    //    actualService.Should().NotBeNull();
    //    actualService!.Eligibilities.Count.Should().Be(1);

    //    var actualEntity = TestDbContext.Eligibilities.SingleOrDefault(s => s.EligibilityType == expected.EligibilityType);
    //    actualEntity.Should().NotBeNull();
    //    actualEntity.Should().BeEquivalentTo(expected, options =>
    //        options.Excluding(info => info.Name.Contains("Id"))
    //            .Excluding(info => info.Name.Contains("Distance")));

    //    var unexpectedEntity = TestDbContext.Eligibilities.Where(lc => lc.Id == existingItem.Id).ToList();
    //    unexpectedEntity.Should().HaveCount(0);
    //}

    //[Fact]
    //public async Task ThenUpdateServiceUpdatedEligibilities()
    //{
    //    //Arrange
    //    await CreateOrganisationDetails();
    //    var service = TestOrganisation.Services.ElementAt(0);

    //    var expected = service.Eligibilities.ElementAt(0);
    //    expected.MinimumAge = 500;
    //    expected.MaximumAge = 5000;

    //    var updateCommand = new UpdateServiceCommand(service.Id, service);
    //    var updateHandler = new UpdateServiceCommandHandler(TestDbContext, Mapper, UpdateLogger.Object);

    //    //Act
    //    var result = await updateHandler.Handle(updateCommand, new CancellationToken());

    //    //Assert
    //    result.Should().NotBe(0);
    //    result.Should().Be(service.Id);

    //    var actualService = TestDbContext.Services.SingleOrDefault(s => s.Name == service.Name);
    //    actualService.Should().NotBeNull();
    //    actualService!.Eligibilities.Count.Should().Be(1);

    //    var actualEntity = TestDbContext.Eligibilities.SingleOrDefault(s => s.EligibilityType == expected.EligibilityType);
    //    actualEntity.Should().NotBeNull();
    //    actualEntity.Should().BeEquivalentTo(expected);
    //}

    //[Fact]
    //public async Task ThenUpdateServiceAddAndDeleteServiceAreas()
    //{
    //    //Arrange
    //    await CreateOrganisationDetails();
    //    var service = TestOrganisation.Services.ElementAt(0);
    //    var existingItem = service.ServiceAreas.ElementAt(0);
    //    var expected = new ServiceAreaDto
    //    {
    //        ServiceAreaName = "ServiceAreaName",
    //        Extent = "Extent",
    //        Uri = "Uri"
    //    };
    //    service.ServiceAreas.Clear();
    //    service.ServiceAreas.Add(expected);

    //    var updateCommand = new UpdateServiceCommand(service.Id, service);
    //    var updateHandler = new UpdateServiceCommandHandler(TestDbContext, Mapper, UpdateLogger.Object);

    //    //Act
    //    var result = await updateHandler.Handle(updateCommand, new CancellationToken());

    //    //Assert
    //    result.Should().NotBe(0);
    //    result.Should().Be(service.Id);

    //    var actualService = TestDbContext.Services.SingleOrDefault(s => s.Name == service.Name);
    //    actualService.Should().NotBeNull();
    //    actualService!.ServiceAreas.Count.Should().Be(1);

    //    var actualEntity = TestDbContext.ServiceAreas.SingleOrDefault(s => s.ServiceAreaName == expected.ServiceAreaName);
    //    actualEntity.Should().NotBeNull();
    //    actualEntity.Should().BeEquivalentTo(expected, options =>
    //        options.Excluding(info => info.Name.Contains("Id"))
    //            .Excluding(info => info.Name.Contains("Distance")));

    //    var unexpectedEntity = TestDbContext.ServiceAreas.Where(lc => lc.Id == existingItem.Id).ToList();
    //    unexpectedEntity.Should().HaveCount(0);
    //}

    //[Fact]
    //public async Task ThenUpdateServiceUpdatedServiceAreas()
    //{
    //    //Arrange
    //    await CreateOrganisationDetails();
    //    var service = TestOrganisation.Services.ElementAt(0);

    //    var expected = service.ServiceAreas.ElementAt(0);
    //    expected.ServiceAreaName = "Updated ServiceAreaName";
    //    expected.Extent = "Updated Extent";

    //    var updateCommand = new UpdateServiceCommand(service.Id, service);
    //    var updateHandler = new UpdateServiceCommandHandler(TestDbContext, Mapper, UpdateLogger.Object);

    //    //Act
    //    var result = await updateHandler.Handle(updateCommand, new CancellationToken());

    //    //Assert
    //    result.Should().NotBe(0);
    //    result.Should().Be(service.Id);

    //    var actualService = TestDbContext.Services.SingleOrDefault(s => s.Name == service.Name);
    //    actualService.Should().NotBeNull();
    //    actualService!.ServiceAreas.Count.Should().Be(1);

    //    var actualEntity = TestDbContext.ServiceAreas.SingleOrDefault(s => s.ServiceAreaName == expected.ServiceAreaName);
    //    actualEntity.Should().NotBeNull();
    //    actualEntity.Should().BeEquivalentTo(expected);
    //}

    //[Fact]
    //public async Task ThenUpdateServiceAddAndDeleteServiceDeliveries()
    //{
    //    //Arrange
    //    await CreateOrganisationDetails();
    //    var service = TestOrganisation.Services.ElementAt(0);
    //    var existingItem = service.ServiceDeliveries.ElementAt(0);

    //    service.ServiceDeliveries.Clear();

    //    var updateCommand = new UpdateServiceCommand(service.Id, service);
    //    var updateHandler = new UpdateServiceCommandHandler(TestDbContext, Mapper, UpdateLogger.Object);

    //    //Act
    //    var result = await updateHandler.Handle(updateCommand, new CancellationToken());

    //    //Assert
    //    result.Should().NotBe(0);
    //    result.Should().Be(service.Id);

    //    var actualService = TestDbContext.Services.SingleOrDefault(s => s.Name == service.Name);
    //    actualService.Should().NotBeNull();
    //    actualService!.ServiceDeliveries.Count.Should().Be(0);

    //    var unexpectedEntity = TestDbContext.ServiceDeliveries.Where(lc => lc.Id == existingItem.Id).ToList();
    //    unexpectedEntity.Should().HaveCount(0);
    //}

    //[Fact]
    //public async Task ThenUpdateServiceUpdatedServiceDeliveries()
    //{
    //    //Arrange
    //    await CreateOrganisationDetails();
    //    var service = TestOrganisation.Services.ElementAt(0);

    //    var updateCommand = new UpdateServiceCommand(service.Id, service);
    //    var updateHandler = new UpdateServiceCommandHandler(TestDbContext, Mapper, UpdateLogger.Object);

    //    //Act
    //    var result = await updateHandler.Handle(updateCommand, new CancellationToken());

    //    //Assert
    //    result.Should().NotBe(0);
    //    result.Should().Be(service.Id);

    //    var actualService = TestDbContext.Services.SingleOrDefault(s => s.Name == service.Name);
    //    actualService.Should().NotBeNull();
    //    actualService!.ServiceDeliveries.Count.Should().Be(1);

    //    //todo:
    //    //var actualEntity = TestDbContext.ServiceDeliveries.SingleOrDefault(s => s.Name == expected.Name);
    //    //actualEntity.Should().NotBeNull();
    //    //actualEntity.Should().BeEquivalentTo(expected);
    //}

    //[Fact]
    //public async Task ThenUpdateServiceAddAndDeleteLanguages()
    //{
    //    //Arrange
    //    await CreateOrganisationDetails();
    //    var service = TestOrganisation.Services.ElementAt(0);
    //    var existingItem = service.Languages.ElementAt(0);
    //    var expected = new LanguageDto()
    //    {
    //        Name = "New Language",
    //        Code = "xx"
    //    };

    //    service.Languages.Clear();
    //    service.Languages.Add(expected);

    //    var updateCommand = new UpdateServiceCommand(service.Id, service);
    //    var updateHandler = new UpdateServiceCommandHandler(TestDbContext, Mapper, UpdateLogger.Object);

    //    //Act
    //    var result = await updateHandler.Handle(updateCommand, new CancellationToken());

    //    //Assert
    //    result.Should().NotBe(0);
    //    result.Should().Be(service.Id);

    //    var actualService = TestDbContext.Services.SingleOrDefault(s => s.Name == service.Name);
    //    actualService.Should().NotBeNull();
    //    actualService!.Languages.Count.Should().Be(1);

    //    var actualEntity = TestDbContext.Languages.SingleOrDefault(s => s.Name == expected.Name);
    //    actualEntity.Should().NotBeNull();
    //    actualEntity.Should().BeEquivalentTo(expected, options =>
    //        options.Excluding((IMemberInfo info) => info.Name.Contains("Id"))
    //            .Excluding((IMemberInfo info) => info.Name.Contains("Distance")));

    //    var unexpectedEntity = TestDbContext.Languages.Where(lc => lc.Id == existingItem.Id).ToList();
    //    unexpectedEntity.Should().HaveCount(0);
    //}

    //[Fact]
    //public async Task ThenUpdateServiceUpdatedLanguages()
    //{
    //    //Arrange
    //    await CreateOrganisationDetails();
    //    var service = TestOrganisation.Services.ElementAt(0);

    //    var expected = service.Languages.ElementAt(0);
    //    expected.Name = "Updated Language";

    //    var updateCommand = new UpdateServiceCommand(service.Id, service);
    //    var updateHandler = new UpdateServiceCommandHandler(TestDbContext, Mapper, UpdateLogger.Object);

    //    //Act
    //    var result = await updateHandler.Handle(updateCommand, new CancellationToken());

    //    //Assert
    //    result.Should().NotBe(0);
    //    result.Should().Be(service.Id);

    //    var actualService = TestDbContext.Services.SingleOrDefault(s => s.Name == service.Name);
    //    actualService.Should().NotBeNull();
    //    actualService!.Languages.Count.Should().Be(1);

    //    var actualEntity = TestDbContext.Languages.SingleOrDefault(s => s.Name == expected.Name);
    //    actualEntity.Should().NotBeNull();
    //    actualEntity.Should().BeEquivalentTo(expected);
    //}

    //[Fact]
    //public async Task ThenUpdateServiceAddAndDeleteCostOptions()
    //{
    //    //Arrange
    //    await CreateOrganisationDetails();
    //    var service = TestOrganisation.Services.ElementAt(0);
    //    var existingItem = service.CostOptions.ElementAt(0);
    //    var expected = new CostOptionDto
    //    {
    //        ValidFrom = DateTime.UtcNow,
    //        ValidTo = DateTime.UtcNow,
    //        Option = "new Option",
    //        Amount = 123,
    //        AmountDescription = "Amount Description"
    //    };

    //    service.CostOptions.Clear();
    //    service.CostOptions.Add(expected);

    //    var updateCommand = new UpdateServiceCommand(service.Id, service);
    //    var updateHandler = new UpdateServiceCommandHandler(TestDbContext, Mapper, UpdateLogger.Object);

    //    //Act
    //    var result = await updateHandler.Handle(updateCommand, new CancellationToken());

    //    //Assert
    //    result.Should().NotBe(0);
    //    result.Should().Be(service.Id);

    //    var actualService = TestDbContext.Services.SingleOrDefault(s => s.Name == service.Name);
    //    actualService.Should().NotBeNull();
    //    actualService!.CostOptions.Count.Should().Be(1);

    //    var actualEntity = TestDbContext.CostOptions.SingleOrDefault(s => s.Option == expected.Option);
    //    actualEntity.Should().NotBeNull();
    //    actualEntity.Should().BeEquivalentTo(expected, options =>
    //        options.Excluding(info => info.Name.Contains("Id"))
    //            .Excluding(info => info.Name.Contains("Distance")));

    //    var unexpectedEntity = TestDbContext.CostOptions.Where(lc => lc.Id == existingItem.Id).ToList();
    //    unexpectedEntity.Should().HaveCount(0);
    //}

    //[Fact]
    //public async Task ThenUpdateServiceUpdatedCostOptions()
    //{
    //    //Arrange
    //    await CreateOrganisationDetails();
    //    var service = TestOrganisation.Services.ElementAt(0);

    //    var expected = service.CostOptions.ElementAt(0);
    //    expected.Amount = 987;
    //    expected.Option = "Updated Option";
    //    expected.AmountDescription = "Updated Amount Description";

    //    var updateCommand = new UpdateServiceCommand(service.Id, service);
    //    var updateHandler = new UpdateServiceCommandHandler(TestDbContext, Mapper, UpdateLogger.Object);

    //    //Act
    //    var result = await updateHandler.Handle(updateCommand, new CancellationToken());

    //    //Assert
    //    result.Should().NotBe(0);
    //    result.Should().Be(service.Id);

    //    var actualService = TestDbContext.Services.SingleOrDefault(s => s.Name == service.Name);
    //    actualService.Should().NotBeNull();
    //    actualService!.CostOptions.Count.Should().Be(1);

    //    var actualEntity = TestDbContext.CostOptions.SingleOrDefault(s => s.Option == expected.Option);
    //    actualEntity.Should().NotBeNull();
    //    actualEntity.Should().BeEquivalentTo(expected);
    //}

    //[Fact]
    //public async Task ThenUpdateServiceAddAndDeleteContacts()
    //{
    //    //Arrange
    //    await CreateOrganisationDetails();
    //    var service = TestOrganisation.Services.ElementAt(0);
    //    var existingItem = service.Contacts.ElementAt(0);
    //    var contact = new ContactDto
    //    {
    //        Id = 0,
    //        Name = "New Contact",
    //        Telephone = "New Telephone"
    //    };
    //    service.Contacts.Clear();
    //    service.Contacts.Add(contact);

    //    var updateCommand = new UpdateServiceCommand(service.Id, service);
    //    var updateHandler = new UpdateServiceCommandHandler(TestDbContext, Mapper, UpdateLogger.Object);

    //    //Act
    //    var result = await updateHandler.Handle(updateCommand, new CancellationToken());

    //    //Assert
    //    result.Should().NotBe(0);
    //    result.Should().Be(service.Id);

    //    var actualService = TestDbContext.Services.SingleOrDefault(s => s.Name == service.Name);
    //    actualService.Should().NotBeNull();
    //    actualService!.Contacts.Count.Should().Be(1);

    //    var actualContact = TestDbContext.Contacts.SingleOrDefault(s => s.Name == contact.Name);
    //    actualContact.Should().NotBeNull();
    //    actualContact.Should().BeEquivalentTo(contact, options =>
    //        options.Excluding((IMemberInfo info) => info.Name.Contains("Id"))
    //            .Excluding((IMemberInfo info) => info.Name.Contains("Distance")));

    //    var unexpectedEntity = TestDbContext.Contacts.Where(lc => lc.Id == existingItem.Id).ToList();
    //    unexpectedEntity.Should().HaveCount(0);
    //}

    //[Fact]
    //public async Task ThenUpdateServiceWithUpdatedContacts()
    //{
    //    //Arrange
    //    await CreateOrganisationDetails();
    //    var service = TestOrganisation.Services.ElementAt(0);
    //    var contact = service.Contacts.ElementAt(0);
    //    contact.Name = "Updated Name";
    //    contact.Email = "Updated Email";
    //    contact.Telephone = "Updated Telephone";

    //    var updateCommand = new UpdateServiceCommand(service.Id, service);
    //    var updateHandler = new UpdateServiceCommandHandler(TestDbContext, Mapper, UpdateLogger.Object);

    //    //Act
    //    var result = await updateHandler.Handle(updateCommand, new CancellationToken());

    //    //Assert
    //    result.Should().NotBe(0);
    //    result.Should().Be(service.Id);

    //    var actualService = TestDbContext.Services.SingleOrDefault(s => s.Name == service.Name);
    //    actualService.Should().NotBeNull();
    //    actualService!.Contacts.Count.Should().Be(1);

    //    var actualContact = TestDbContext.Contacts.SingleOrDefault(s => s.Name == contact.Name);
    //    actualContact.Should().NotBeNull();
    //    actualContact.Should().BeEquivalentTo(contact);
    //}

    //[Fact]
    //public async Task ThenUpdateServiceAddAndDeleteSchedules()
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

    //    var updateCommand = new UpdateServiceCommand(service.Id, service);
    //    var updateHandler = new UpdateServiceCommandHandler(TestDbContext, Mapper, UpdateLogger.Object);

    //    //Act
    //    var result = await updateHandler.Handle(updateCommand, new CancellationToken());

    //    //Assert
    //    result.Should().NotBe(0);
    //    result.Should().Be(service.Id);

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

    //[Fact]
    //public async Task ThenUpdateServiceUpdatedSchedules()
    //{
    //    //Arrange
    //    await CreateOrganisationDetails();
    //    var service = TestOrganisation.Services.ElementAt(0);

    //    var expected = service.Schedules.ElementAt(0);
    //    expected.ByDay = "Updated ByDay";
    //    expected.ByMonthDay = "Updated ByMonthDay";

    //    var updateCommand = new UpdateServiceCommand(service.Id, service);
    //    var updateHandler = new UpdateServiceCommandHandler(TestDbContext, Mapper, UpdateLogger.Object);

    //    //Act
    //    var result = await updateHandler.Handle(updateCommand, new CancellationToken());

    //    //Assert
    //    result.Should().NotBe(0);
    //    result.Should().Be(service.Id);

    //    var actualService = TestDbContext.Services.SingleOrDefault(s => s.Name == service.Name);
    //    actualService.Should().NotBeNull();
    //    actualService!.Schedules.Count.Should().Be(1);

    //    var actualEntity = TestDbContext.Schedules.SingleOrDefault(s => s.ByDay == expected.ByDay);
    //    actualEntity.Should().NotBeNull();
    //    actualEntity.Should().BeEquivalentTo(expected);
    //}

    //[Fact]
    //public async Task ThenUpdateServiceAddAndDeleteLocations()
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
    //        LocationType = LocationType.Postal
    //    };
    //    service.Locations.Clear();
    //    service.Locations.Add(expected);

    //    var updateCommand = new UpdateServiceCommand(service.Id, service);
    //    var updateHandler = new UpdateServiceCommandHandler(TestDbContext, Mapper, UpdateLogger.Object);

    //    //Act
    //    var result = await updateHandler.Handle(updateCommand, new CancellationToken());

    //    //Assert
    //    result.Should().NotBe(0);
    //    result.Should().Be(service.Id);

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

    //[Fact]
    //public async Task ThenUpdateServiceUpdatedLocations()
    //{
    //    //Arrange
    //    await CreateOrganisationDetails();
    //    var service = TestOrganisation.Services.ElementAt(0);

    //    var expected = service.Locations.ElementAt(0);
    //    expected.Name = "Updated Name";
    //    expected.Description = "Updated Description";

    //    var updateCommand = new UpdateServiceCommand(service.Id, service);
    //    var updateHandler = new UpdateServiceCommandHandler(TestDbContext, Mapper, UpdateLogger.Object);

    //    //Act
    //    var result = await updateHandler.Handle(updateCommand, new CancellationToken());

    //    //Assert
    //    result.Should().NotBe(0);
    //    result.Should().Be(service.Id);

    //    var actualService = TestDbContext.Services.SingleOrDefault(s => s.Name == service.Name);
    //    actualService.Should().NotBeNull();
    //    actualService!.Locations.Count.Should().Be(1);

    //    var actualEntity = TestDbContext.Locations.SingleOrDefault(s => s.Name == expected.Name);
    //    actualEntity.Should().NotBeNull();
    //    actualEntity.Should().BeEquivalentTo(expected, options =>
    //        options.Excluding((IMemberInfo info) => info.Name.Contains("Id"))
    //            .Excluding((IMemberInfo info) => info.Name.Contains("Distance")));
    //}

    ////todo: this is now basically the same as ThenUpdateServiceUpdatedLocations
    //// what was it trying to do, having 2 locations on the service with the same id, but different properties? didn't make sense
    //[Fact]
    //public async Task ThenUpdateServiceAttachExistingLocations()
    //{
    //    //Arrange
    //    await CreateOrganisationDetails();

    //    var service = TestOrganisation.Services.First();
    //    var expected = service.Locations.First();
    //    expected.Name = "Existing Location already Saved in DB";

    //    var updateCommand = new UpdateServiceCommand(service.Id, service);
    //    var updateHandler = new UpdateServiceCommandHandler(TestDbContext, Mapper, UpdateLogger.Object);

    //    //Act
    //    var result = await updateHandler.Handle(updateCommand, new CancellationToken());

    //    //Assert
    //    result.Should().NotBe(0);
    //    result.Should().Be(service.Id);

    //    var actualService = TestDbContext.Services.SingleOrDefault(s => s.Name == service.Name);
    //    actualService.Should().NotBeNull();
    //    actualService!.Locations.Count.Should().Be(1);

    //    var actualEntity = TestDbContext.Locations.SingleOrDefault(s => s.Name == expected.Name);
    //    actualEntity.Should().NotBeNull();
    //    actualEntity.Should().BeEquivalentTo(expected, options =>
    //        options.Excluding((IMemberInfo info) => info.Name.Contains("Id"))
    //            .Excluding((IMemberInfo info) => info.Name.Contains("Distance")));
    //}

    //[Fact]
    //public async Task ThenUpdateServiceAddAndDeleteTaxonomies()
    //{
    //    //Arrange
    //    await CreateOrganisationDetails();
    //    var service = TestOrganisation.Services.ElementAt(0);
    //    var taxonomy = service.Taxonomies.ElementAt(0);
    //    var expected = new TaxonomyDto
    //    {
    //        Name = "New Taxonomy"
    //    };
    //    service.Taxonomies.Clear();
    //    service.Taxonomies.Add(expected);

    //    var updateCommand = new UpdateServiceCommand(service.Id, service);
    //    var updateHandler = new UpdateServiceCommandHandler(TestDbContext, Mapper, UpdateLogger.Object);

    //    //Act
    //    var result = await updateHandler.Handle(updateCommand, new CancellationToken());

    //    //Assert
    //    result.Should().NotBe(0);
    //    result.Should().Be(service.Id);

    //    var actualService = TestDbContext.Services.SingleOrDefault(s => s.Name == service.Name);
    //    actualService.Should().NotBeNull();
    //    actualService!.Taxonomies.Count.Should().Be(1);

    //    var actualEntity = TestDbContext.Taxonomies.SingleOrDefault(s => s.Name == expected.Name);
    //    actualEntity.Should().NotBeNull();
    //    actualEntity.Should().BeEquivalentTo(expected, options =>
    //        options.Excluding((IMemberInfo info) => info.Name.Contains("Id"))
    //            .Excluding((IMemberInfo info) => info.Name.Contains("Distance")));

    //    //Delete wont cascade delete Taxonomies, so existing will be left behind
    //    var detachedEntity = TestDbContext.Taxonomies.SingleOrDefault(s => s.Name == taxonomy.Name);
    //    detachedEntity.Should().NotBeNull();
    //    detachedEntity.Should().BeEquivalentTo(taxonomy, options =>
    //        options.Excluding((IMemberInfo info) => info.Name.Contains("Id"))
    //            .Excluding((IMemberInfo info) => info.Name.Contains("Distance")));
    //}

    //[Fact]
    //public async Task ThenUpdateServiceUpdatedTaxonomies()
    //{
    //    //Arrange
    //    await CreateOrganisationDetails();
    //    var service = TestOrganisation.Services.ElementAt(0);

    //    var expected = service.Taxonomies.ElementAt(0);
    //    expected.Name = "Updated Name";

    //    var updateCommand = new UpdateServiceCommand(service.Id, service);
    //    var updateHandler = new UpdateServiceCommandHandler(TestDbContext, Mapper, UpdateLogger.Object);

    //    //Act
    //    var result = await updateHandler.Handle(updateCommand, new CancellationToken());

    //    //Assert
    //    result.Should().NotBe(0);
    //    result.Should().Be(service.Id);

    //    var actualService = TestDbContext.Services.SingleOrDefault(s => s.Name == service.Name);
    //    actualService.Should().NotBeNull();
    //    actualService!.Taxonomies.Count.Should().Be(4);

    //    var actualEntity = TestDbContext.Taxonomies.SingleOrDefault(s => s.Name == expected.Name);
    //    actualEntity.Should().NotBeNull();
    //    actualEntity.Should().BeEquivalentTo(expected);
    //}

    //[Fact]
    //public async Task ThenUpdateServiceAttachExistingTaxonomies()
    //{
    //    //Arrange
    //    await CreateOrganisationDetails();
    //    var expected = new TaxonomyDto
    //    {
    //        Name = "New Taxonomy Name",
    //        TaxonomyType = TaxonomyType.ServiceCategory
    //    };
    //    expected.Id = await CreateTaxonomy(expected);

    //    var service = TestOrganisation.Services.ElementAt(0);
    //    service.Taxonomies.Add(expected);

    //    var updateCommand = new UpdateServiceCommand(service.Id, service);
    //    var updateHandler = new UpdateServiceCommandHandler(TestDbContext, Mapper, UpdateLogger.Object);

    //    //Act
    //    var result = await updateHandler.Handle(updateCommand, new CancellationToken());

    //    //Assert
    //    result.Should().NotBe(0);
    //    result.Should().Be(service.Id);

    //    var actualService = TestDbContext.Services.SingleOrDefault(s => s.Name == service.Name);
    //    actualService.Should().NotBeNull();
    //    actualService!.Taxonomies.Count.Should().Be(5);

    //    var actualEntity = TestDbContext.Taxonomies.SingleOrDefault(s => s.Name == expected.Name);
    //    actualEntity.Should().NotBeNull();
    //    actualEntity.Should().BeEquivalentTo(expected);
    //}

    //[Fact]
    //public async Task ThenUpdateServiceLocationsAddAndDeleteContacts()
    //{
    //    //Arrange
    //    await CreateOrganisationDetails();
    //    var service = TestOrganisation.Services.ElementAt(0);
    //    var existingItem = service.Locations.ElementAt(0).Contacts.ElementAt(0);
    //    var contact = new ContactDto
    //    {
    //        Id = 0,
    //        Name = "New Contact",
    //        Telephone = "New Telephone"
    //    };
    //    service.Locations.ElementAt(0).Contacts.Clear();
    //    service.Locations.ElementAt(0).Contacts.Add(contact);

    //    var updateCommand = new UpdateServiceCommand(service.Id, service);
    //    var updateHandler = new UpdateServiceCommandHandler(TestDbContext, Mapper, UpdateLogger.Object);

    //    //Act
    //    var result = await updateHandler.Handle(updateCommand, new CancellationToken());

    //    //Assert
    //    result.Should().NotBe(0);
    //    result.Should().Be(service.Id);

    //    var actualService = TestDbContext.Services.SingleOrDefault(s => s.Name == service.Name);
    //    actualService.Should().NotBeNull();
    //    actualService!.Locations.Count.Should().Be(1);

    //    var actualContact = TestDbContext.Contacts.SingleOrDefault(s => s.Name == contact.Name);
    //    actualContact.Should().NotBeNull();
    //    actualContact.Should().BeEquivalentTo(contact, options =>
    //        options.Excluding((IMemberInfo info) => info.Name.Contains("Id"))
    //            .Excluding((IMemberInfo info) => info.Name.Contains("Distance")));

    //    var unexpectedEntity = TestDbContext.Contacts.Where(lc => lc.Id == existingItem.Id).ToList();
    //    unexpectedEntity.Should().HaveCount(0);
    //}

    //[Fact]
    //public async Task ThenUpdateServiceLocationsWithUpdatedContacts()
    //{
    //    //Arrange
    //    await CreateOrganisationDetails();
    //    var service = TestOrganisation.Services.ElementAt(0);
    //    var contact = service.Locations.ElementAt(0).Contacts.ElementAt(0);
    //    contact.Name = "Updated Name";
    //    contact.Email = "Updated Email";
    //    contact.Telephone = "Updated Telephone";

    //    var updateCommand = new UpdateServiceCommand(service.Id, service);
    //    var updateHandler = new UpdateServiceCommandHandler(TestDbContext, Mapper, UpdateLogger.Object);

    //    //Act
    //    var result = await updateHandler.Handle(updateCommand, new CancellationToken());

    //    //Assert
    //    result.Should().NotBe(0);
    //    result.Should().Be(service.Id);

    //    var actualService = TestDbContext.Services.SingleOrDefault(s => s.Name == service.Name);
    //    actualService.Should().NotBeNull();
    //    actualService!.Locations.ElementAt(0).Contacts.Count.Should().Be(1);

    //    var actualContact = TestDbContext.Contacts.SingleOrDefault(s => s.Name == contact.Name);
    //    actualContact.Should().NotBeNull();
    //    actualContact.Should().BeEquivalentTo(contact);
    //}

    //[Fact]
    //public async Task ThenUpdateServiceLocationsAddAndDeleteSchedules()
    //{
    //    //Arrange
    //    await CreateOrganisationDetails();
    //    var service = TestOrganisation.Services.ElementAt(0);
    //    var existingItem = service.Locations.ElementAt(0).Schedules.ElementAt(0);
    //    var expected = new ScheduleDto
    //    {
    //        ValidFrom = DateTime.UtcNow,
    //        ValidTo = DateTime.UtcNow,
    //        ByDay = "New ByDay",
    //        ByMonthDay = "New ByMonthDay"
    //    };

    //    service.Locations.ElementAt(0).Schedules.Clear();
    //    service.Locations.ElementAt(0).Schedules.Add(expected);

    //    var updateCommand = new UpdateServiceCommand(service.Id, service);
    //    var updateHandler = new UpdateServiceCommandHandler(TestDbContext, Mapper, UpdateLogger.Object);

    //    //Act
    //    var result = await updateHandler.Handle(updateCommand, new CancellationToken());

    //    //Assert
    //    result.Should().NotBe(0);
    //    result.Should().Be(service.Id);

    //    var actualService = TestDbContext.Services.SingleOrDefault(s => s.Name == service.Name);
    //    actualService.Should().NotBeNull();
    //    actualService!.Locations.ElementAt(0).Schedules.Count.Should().Be(1);

    //    var actualEntity = TestDbContext.Schedules.SingleOrDefault(s => s.ByDay == expected.ByDay);
    //    actualEntity.Should().NotBeNull();
    //    actualEntity.Should().BeEquivalentTo(expected, options =>
    //        options.Excluding(info => info.Name.Contains("Id"))
    //            .Excluding(info => info.Name.Contains("Distance")));

    //    var unexpectedEntity = TestDbContext.Schedules.Where(lc => lc.Id == existingItem.Id).ToList();
    //    unexpectedEntity.Should().HaveCount(0);
    //}

    //[Fact]
    //public async Task ThenUpdateServiceLocationsUpdatedSchedules()
    //{
    //    //Arrange
    //    await CreateOrganisationDetails();
    //    var service = TestOrganisation.Services.ElementAt(0);

    //    var expected = service.Locations.ElementAt(0).Schedules.ElementAt(0);
    //    expected.ByDay = "Updated ByDay";
    //    expected.ByMonthDay = "Updated ByMonthDay";

    //    var updateCommand = new UpdateServiceCommand(service.Id, service);
    //    var updateHandler = new UpdateServiceCommandHandler(TestDbContext, Mapper, UpdateLogger.Object);

    //    //Act
    //    var result = await updateHandler.Handle(updateCommand, new CancellationToken());

    //    //Assert
    //    result.Should().NotBe(0);
    //    result.Should().Be(service.Id);

    //    var actualService = TestDbContext.Services.SingleOrDefault(s => s.Name == service.Name);
    //    actualService.Should().NotBeNull();
    //    actualService!.Locations.ElementAt(0).Schedules.Count.Should().Be(1);

    //    var actualEntity = TestDbContext.Schedules.SingleOrDefault(s => s.ByDay == expected.ByDay);
    //    actualEntity.Should().NotBeNull();
    //    actualEntity.Should().BeEquivalentTo(expected);
    //}
}