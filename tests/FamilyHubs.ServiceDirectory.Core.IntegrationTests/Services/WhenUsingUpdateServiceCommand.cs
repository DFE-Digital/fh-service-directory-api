using FamilyHubs.ServiceDirectory.Core.Commands.Services.UpdateService;
using FamilyHubs.ServiceDirectory.Shared.CreateUpdateDto;
using FamilyHubs.ServiceDirectory.Shared.Dto;
using FamilyHubs.ServiceDirectory.Shared.Enums;
using FluentAssertions;
using FluentAssertions.Equivalency;

namespace FamilyHubs.ServiceDirectory.Core.IntegrationTests.Services;

public class WhenUsingUpdateServiceCommand : DataIntegrationTestBase
{
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
        var updateHandler = new UpdateServiceCommandHandler(TestDbContext, Mapper);

        //Act
        var result = await updateHandler.Handle(updateCommand, new CancellationToken());

        //Assert
        result.Should().NotBe(0);
        result.Should().Be(service.Id);
        var actualService = TestDbContext.Services.SingleOrDefault(s => s.Name == service.Name);
        actualService.Should().NotBeNull();
        actualService!.Description.Should().Be(service.Description);
    }

    [Fact]
    public async Task ThenUpdateServiceAddAndDeleteEligibilities()
    {
        //Arrange
        await CreateOrganisationDetails();
        var service = TestOrganisation.Services.ElementAt(0);

        var serviceChange = Mapper.Map<ServiceChangeDto>(service);

        var existingItem = serviceChange.Eligibilities.ElementAt(0);

        var expected = new EligibilityDto
        {
            //ServiceId = service.Id,
            MaximumAge = 2,
            MinimumAge = 0,
            EligibilityType = null
        };
        serviceChange.Eligibilities.Clear();
        serviceChange.Eligibilities.Add(expected);

        var updateCommand = new UpdateServiceCommand(service.Id, serviceChange);
        var updateHandler = new UpdateServiceCommandHandler(TestDbContext, Mapper);

        //Act
        var result = await updateHandler.Handle(updateCommand, new CancellationToken());

        //Assert
        result.Should().NotBe(0);
        result.Should().Be(serviceChange.Id);

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
    public async Task ThenUpdateServiceUpdatedEligibilities()
    {
        //Arrange
        await CreateOrganisationDetails();
        var service = TestOrganisation.Services.ElementAt(0);

        var serviceChange = Mapper.Map<ServiceChangeDto>(service);

        var expected = serviceChange.Eligibilities.ElementAt(0);
        expected.MinimumAge = 500;
        expected.MaximumAge = 5000;

        var updateCommand = new UpdateServiceCommand(service.Id, serviceChange);
        var updateHandler = new UpdateServiceCommandHandler(TestDbContext, Mapper);

        //Act
        var result = await updateHandler.Handle(updateCommand, new CancellationToken());

        //Assert
        result.Should().NotBe(0);
        result.Should().Be(serviceChange.Id);

        var actualService = TestDbContext.Services.SingleOrDefault(s => s.Name == service.Name);
        actualService.Should().NotBeNull();
        actualService!.Eligibilities.Count.Should().Be(1);

        var actualEntity = TestDbContext.Eligibilities.SingleOrDefault(s => s.EligibilityType == expected.EligibilityType);
        actualEntity.Should().NotBeNull();
        actualEntity.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task ThenUpdateServiceAddAndDeleteServiceAreas()
    {
        //Arrange
        await CreateOrganisationDetails();
        var service = TestOrganisation.Services.ElementAt(0);
        var serviceChange = Mapper.Map<ServiceChangeDto>(service);
        var existingItem = serviceChange.ServiceAreas.ElementAt(0);
        var expected = new ServiceAreaDto
        {
            ServiceAreaName = "ServiceAreaName",
            Extent = "Extent",
            Uri = "Uri"
        };
        serviceChange.ServiceAreas.Clear();
        serviceChange.ServiceAreas.Add(expected);

        var updateCommand = new UpdateServiceCommand(service.Id, serviceChange);
        var updateHandler = new UpdateServiceCommandHandler(TestDbContext, Mapper);

        //Act
        var result = await updateHandler.Handle(updateCommand, new CancellationToken());

        //Assert
        result.Should().NotBe(0);
        result.Should().Be(serviceChange.Id);

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
    public async Task ThenUpdateServiceUpdatedServiceAreas()
    {
        //Arrange
        await CreateOrganisationDetails();
        var service = TestOrganisation.Services.ElementAt(0);
        var serviceChange = Mapper.Map<ServiceChangeDto>(service);

        var expected = serviceChange.ServiceAreas.ElementAt(0);
        expected.ServiceAreaName = "Updated ServiceAreaName";
        expected.Extent = "Updated Extent";

        var updateCommand = new UpdateServiceCommand(service.Id, serviceChange);
        var updateHandler = new UpdateServiceCommandHandler(TestDbContext, Mapper);

        //Act
        var result = await updateHandler.Handle(updateCommand, new CancellationToken());

        //Assert
        result.Should().NotBe(0);
        result.Should().Be(serviceChange.Id);

        var actualService = TestDbContext.Services.SingleOrDefault(s => s.Name == service.Name);
        actualService.Should().NotBeNull();
        actualService!.ServiceAreas.Count.Should().Be(1);

        var actualEntity = TestDbContext.ServiceAreas.SingleOrDefault(s => s.ServiceAreaName == expected.ServiceAreaName);
        actualEntity.Should().NotBeNull();
        actualEntity.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task ThenUpdateServiceDeleteServiceDeliveries()
    {
        //Arrange
        await CreateOrganisationDetails();
        var service = TestOrganisation.Services.ElementAt(0);
        var serviceChange = Mapper.Map<ServiceChangeDto>(service);

        var existingItem = serviceChange.ServiceDeliveries.ElementAt(0);

        serviceChange.ServiceDeliveries.Clear();

        var updateCommand = new UpdateServiceCommand(service.Id, serviceChange);
        var updateHandler = new UpdateServiceCommandHandler(TestDbContext, Mapper);

        //Act
        var result = await updateHandler.Handle(updateCommand, new CancellationToken());

        //Assert
        result.Should().NotBe(0);
        result.Should().Be(serviceChange.Id);

        var actualService = TestDbContext.Services.SingleOrDefault(s => s.Name == service.Name);
        actualService.Should().NotBeNull();
        actualService!.ServiceDeliveries.Count.Should().Be(0);

        var unexpectedEntity = TestDbContext.ServiceDeliveries.Where(lc => lc.Id == existingItem.Id).ToList();
        unexpectedEntity.Should().HaveCount(0);
    }

    [Fact]
    public async Task ThenUpdateServiceUpdatedServiceDeliveries()
    {
        //Arrange
        await CreateOrganisationDetails();
        var service = TestOrganisation.Services.ElementAt(0);
        var serviceChange = Mapper.Map<ServiceChangeDto>(service);

        var expected = serviceChange.ServiceDeliveries.ElementAt(0);
        expected.Name = AttendingType.InPerson;

        var updateCommand = new UpdateServiceCommand(service.Id, serviceChange);
        var updateHandler = new UpdateServiceCommandHandler(TestDbContext, Mapper);

        //Act
        var result = await updateHandler.Handle(updateCommand, new CancellationToken());

        //Assert
        result.Should().NotBe(0);
        result.Should().Be(serviceChange.Id);

        var actualService = TestDbContext.Services.SingleOrDefault(s => s.Name == service.Name);
        actualService.Should().NotBeNull();
        actualService!.ServiceDeliveries.Count.Should().Be(1);

        var actualEntity = TestDbContext.ServiceDeliveries.SingleOrDefault(s => s.Name == expected.Name);
        actualEntity.Should().NotBeNull();
        actualEntity.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task ThenUpdateServiceAddAndDeleteLanguages()
    {
        //Arrange
        await CreateOrganisationDetails();
        var service = TestOrganisation.Services.ElementAt(0);
        var serviceChange = Mapper.Map<ServiceChangeDto>(service);

        var existingItem = serviceChange.Languages.ElementAt(0);
        var expected = new LanguageDto()
        {
            Name = "New Language",
            Code = "xx"
        };

        serviceChange.Languages.Clear();
        serviceChange.Languages.Add(expected);

        var updateCommand = new UpdateServiceCommand(service.Id, serviceChange);
        var updateHandler = new UpdateServiceCommandHandler(TestDbContext, Mapper);

        //Act
        var result = await updateHandler.Handle(updateCommand, new CancellationToken());

        //Assert
        result.Should().NotBe(0);
        result.Should().Be(serviceChange.Id);

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
    public async Task ThenUpdateServiceUpdatedLanguages()
    {
        //Arrange
        await CreateOrganisationDetails();
        var service = TestOrganisation.Services.ElementAt(0);
        var serviceChange = Mapper.Map<ServiceChangeDto>(service);

        var expected = serviceChange.Languages.ElementAt(0);
        expected.Name = "Updated Language";
        expected.Code = "UL";

        var updateCommand = new UpdateServiceCommand(service.Id, serviceChange);
        var updateHandler = new UpdateServiceCommandHandler(TestDbContext, Mapper);

        //Act
        var result = await updateHandler.Handle(updateCommand, new CancellationToken());

        //Assert
        result.Should().NotBe(0);
        result.Should().Be(serviceChange.Id);

        var actualService = TestDbContext.Services.SingleOrDefault(s => s.Name == serviceChange.Name);
        actualService.Should().NotBeNull();
        actualService!.Languages.Count.Should().Be(1);

        var actualEntity = TestDbContext.Languages.SingleOrDefault(s => s.Name == expected.Name);
        actualEntity.Should().NotBeNull();
        actualEntity.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task ThenUpdateServiceAddAndDeleteCostOptions()
    {
        //Arrange
        await CreateOrganisationDetails();
        var service = TestOrganisation.Services.ElementAt(0);
        var serviceChange = Mapper.Map<ServiceChangeDto>(service);

        var existingItem = serviceChange.CostOptions.ElementAt(0);
        var expected = new CostOptionDto
        {
            ValidFrom = DateTime.UtcNow,
            ValidTo = DateTime.UtcNow,
            Option = "new Option",
            Amount = 123,
            AmountDescription = "Amount Description"
        };

        serviceChange.CostOptions.Clear();
        serviceChange.CostOptions.Add(expected);

        var updateCommand = new UpdateServiceCommand(service.Id, serviceChange);
        var updateHandler = new UpdateServiceCommandHandler(TestDbContext, Mapper);

        //Act
        var result = await updateHandler.Handle(updateCommand, new CancellationToken());

        //Assert
        result.Should().NotBe(0);
        result.Should().Be(serviceChange.Id);

        var actualService = TestDbContext.Services.SingleOrDefault(s => s.Name == serviceChange.Name);
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
    public async Task ThenUpdateServiceUpdatedCostOptions()
    {
        //Arrange
        await CreateOrganisationDetails();
        var service = TestOrganisation.Services.ElementAt(0);
        var serviceChange = Mapper.Map<ServiceChangeDto>(service);

        var expected = serviceChange.CostOptions.ElementAt(0);
        expected.Amount = 987;
        expected.Option = "Updated Option";
        expected.AmountDescription = "Updated Amount Description";

        var updateCommand = new UpdateServiceCommand(service.Id, serviceChange);
        var updateHandler = new UpdateServiceCommandHandler(TestDbContext, Mapper);

        //Act
        var result = await updateHandler.Handle(updateCommand, new CancellationToken());

        //Assert
        result.Should().NotBe(0);
        result.Should().Be(serviceChange.Id);

        var actualService = TestDbContext.Services.SingleOrDefault(s => s.Name == serviceChange.Name);
        actualService.Should().NotBeNull();
        actualService!.CostOptions.Count.Should().Be(1);

        var actualEntity = TestDbContext.CostOptions.SingleOrDefault(s => s.Option == expected.Option);
        actualEntity.Should().NotBeNull();
        actualEntity.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task ThenUpdateServiceAddAndDeleteContacts()
    {
        //Arrange
        await CreateOrganisationDetails();
        var service = TestOrganisation.Services.ElementAt(0);
        var serviceChange = Mapper.Map<ServiceChangeDto>(service);

        var existingItem = serviceChange.Contacts.ElementAt(0);
        var contact = new ContactDto
        {
            Id = 0,
            Name = "New Contact",
            Telephone = "New Telephone"
        };
        serviceChange.Contacts.Clear();
        serviceChange.Contacts.Add(contact);

        var updateCommand = new UpdateServiceCommand(service.Id, serviceChange);
        var updateHandler = new UpdateServiceCommandHandler(TestDbContext, Mapper);

        //Act
        var result = await updateHandler.Handle(updateCommand, new CancellationToken());

        //Assert
        result.Should().NotBe(0);
        result.Should().Be(serviceChange.Id);

        var actualService = TestDbContext.Services.SingleOrDefault(s => s.Name == serviceChange.Name);
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
    public async Task ThenUpdateServiceWithUpdatedContacts()
    {
        //Arrange
        await CreateOrganisationDetails();
        var service = TestOrganisation.Services.ElementAt(0);
        var serviceChange = Mapper.Map<ServiceChangeDto>(service);

        var contact = serviceChange.Contacts.ElementAt(0);
        contact.Name = "Updated Name";
        contact.Email = "Updated Email";
        contact.Telephone = "Updated Telephone";

        var updateCommand = new UpdateServiceCommand(service.Id, serviceChange);
        var updateHandler = new UpdateServiceCommandHandler(TestDbContext, Mapper);

        //Act
        var result = await updateHandler.Handle(updateCommand, new CancellationToken());

        //Assert
        result.Should().NotBe(0);
        result.Should().Be(serviceChange.Id);

        var actualService = TestDbContext.Services.SingleOrDefault(s => s.Name == serviceChange.Name);
        actualService.Should().NotBeNull();
        actualService!.Contacts.Count.Should().Be(1);

        var actualContact = TestDbContext.Contacts.SingleOrDefault(s => s.Name == contact.Name);
        actualContact.Should().NotBeNull();
        actualContact.Should().BeEquivalentTo(contact);
    }

    [Fact]
    public async Task ThenUpdateServiceAddAndDeleteSchedules()
    {
        //Arrange
        await CreateOrganisationDetails();
        var service = TestOrganisation.Services.ElementAt(0);
        var serviceChange = Mapper.Map<ServiceChangeDto>(service);

        var existingItem = serviceChange.Schedules.ElementAt(0);
        var expected = new ScheduleDto
        {
            ValidFrom = DateTime.UtcNow,
            ValidTo = DateTime.UtcNow,
            ByDay = "New ByDay",
            ByMonthDay = "New ByMonthDay"
        };

        serviceChange.Schedules.Clear();
        serviceChange.Schedules.Add(expected);

        var updateCommand = new UpdateServiceCommand(service.Id, serviceChange);
        var updateHandler = new UpdateServiceCommandHandler(TestDbContext, Mapper);

        //Act
        var result = await updateHandler.Handle(updateCommand, new CancellationToken());

        //Assert
        result.Should().NotBe(0);
        result.Should().Be(serviceChange.Id);

        var actualService = TestDbContext.Services.SingleOrDefault(s => s.Name == serviceChange.Name);
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
    public async Task ThenUpdateServiceUpdatedSchedules()
    {
        //Arrange
        await CreateOrganisationDetails();
        var service = TestOrganisation.Services.ElementAt(0);
        var serviceChange = Mapper.Map<ServiceChangeDto>(service);

        var expected = serviceChange.Schedules.ElementAt(0);
        expected.ByDay = "Updated ByDay";
        expected.ByMonthDay = "Updated ByMonthDay";

        var updateCommand = new UpdateServiceCommand(service.Id, serviceChange);
        var updateHandler = new UpdateServiceCommandHandler(TestDbContext, Mapper);

        //Act
        var result = await updateHandler.Handle(updateCommand, new CancellationToken());

        //Assert
        result.Should().NotBe(0);
        result.Should().Be(serviceChange.Id);

        var actualService = TestDbContext.Services.SingleOrDefault(s => s.Name == serviceChange.Name);
        actualService.Should().NotBeNull();
        actualService!.Schedules.Count.Should().Be(1);

        var actualEntity = TestDbContext.Schedules.SingleOrDefault(s => s.ByDay == expected.ByDay);
        actualEntity.Should().NotBeNull();
        actualEntity.Should().BeEquivalentTo(expected);
    }

    //todo: ideally we'd have one test for add, one for delete (and this one for add and delete
    [Fact]
    public async Task ThenUpdateServiceAddAndDeleteLocations()
    {
        //Arrange
        await CreateOrganisationDetails();
        var service = TestOrganisation.Services.ElementAt(0);
        var serviceChange = Mapper.Map<ServiceChangeDto>(service);

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
            LocationTypeCategory = LocationTypeCategory.NotSet,
            Latitude = 0,
            Longitude = 0,
            LocationType = LocationType.Postal
        };
        var existingLocationId = await CreateLocation(expected);

        serviceChange.ServiceAtLocations.Clear();
        serviceChange.ServiceAtLocations.Add(new ServiceAtLocationChangeDto
        {
            LocationId = existingLocationId
        });

        var updateCommand = new UpdateServiceCommand(service.Id, serviceChange);
        var updateHandler = new UpdateServiceCommandHandler(TestDbContext, Mapper);

        //Act
        var result = await updateHandler.Handle(updateCommand, new CancellationToken());

        //Assert
        result.Should().NotBe(0);
        result.Should().Be(serviceChange.Id);

        var actualService = TestDbContext.Services.SingleOrDefault(s => s.Name == serviceChange.Name);
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
    public async Task ThenUpdateServiceAddAndDeleteLocationsWithSchedules()
    {
        //Arrange
        await CreateOrganisationDetails();
        var service = TestOrganisation.Services.ElementAt(0);
        var serviceChange = Mapper.Map<ServiceChangeDto>(service);

        //todo: don't use hardcoded id?
        await AddServiceAtLocationSchedule(1, new ScheduleDto
        {
            AttendingType = AttendingType.InPerson.ToString(),
            Freq = FrequencyType.WEEKLY,
            ByDay = "MO"
        });

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
            LocationTypeCategory = LocationTypeCategory.NotSet,
            Latitude = 0,
            Longitude = 0,
            LocationType = LocationType.Postal
        };
        var existingLocationId = await CreateLocation(expected);

        serviceChange.ServiceAtLocations.Clear();
        serviceChange.ServiceAtLocations.Add(new ServiceAtLocationChangeDto
        {
            LocationId = existingLocationId,
            Schedules = new List<ScheduleDto>
            {
                new()
                {
                    AttendingType = AttendingType.InPerson.ToString(),
                    Freq = FrequencyType.WEEKLY,
                    ByDay = "TU"
                }
            }
        });

        var updateCommand = new UpdateServiceCommand(service.Id, serviceChange);
        var updateHandler = new UpdateServiceCommandHandler(TestDbContext, Mapper);

        //Act
        var result = await updateHandler.Handle(updateCommand, new CancellationToken());

        //Assert
        result.Should().NotBe(0);
        result.Should().Be(serviceChange.Id);

        var actualService = TestDbContext.Services.SingleOrDefault(s => s.Name == serviceChange.Name);
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
    public async Task ThenUpdateServiceAddAndDeleteTaxonomies()
    {
        //Arrange
        await CreateOrganisationDetails();
        var service = TestOrganisation.Services.ElementAt(0);
        var serviceChange = Mapper.Map<ServiceChangeDto>(service);

        var newTaxonomy = TestDbContext.Taxonomies.First(t => t.Name == "Sports and recreation");

        var taxonomy = service.Taxonomies.ElementAt(0);
        serviceChange.TaxonomyIds.Clear();
        serviceChange.TaxonomyIds.Add(newTaxonomy.Id);

        var updateCommand = new UpdateServiceCommand(service.Id, serviceChange);
        var updateHandler = new UpdateServiceCommandHandler(TestDbContext, Mapper);

        //Act
        var result = await updateHandler.Handle(updateCommand, new CancellationToken());

        //Assert
        result.Should().NotBe(0);
        result.Should().Be(serviceChange.Id);

        var actualService = TestDbContext.Services.SingleOrDefault(s => s.Name == serviceChange.Name);
        actualService.Should().NotBeNull();
        actualService!.Taxonomies.Count.Should().Be(1);
        actualService!.Taxonomies.Select(t => t.Id).Should().BeEquivalentTo(new[] { newTaxonomy.Id });

        // Delete wont cascade delete Taxonomies, so existing will be left behind
        var detachedEntity = TestDbContext.Taxonomies.SingleOrDefault(s => s.Name == taxonomy.Name);
        detachedEntity.Should().NotBeNull();
        detachedEntity.Should().BeEquivalentTo(taxonomy, options =>
            options.Excluding((IMemberInfo info) => info.Name.Contains("Id"))
                .Excluding((IMemberInfo info) => info.Name.Contains("Distance")));
    }
}