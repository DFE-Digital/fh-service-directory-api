using AutoMapper;
using FamilyHubs.ServiceDirectory.Api.Commands.CreateOrganisation;
using FamilyHubs.ServiceDirectory.Api.Commands.CreateService;
using FamilyHubs.ServiceDirectory.Api.Commands.UpdateService;
using FamilyHubs.ServiceDirectory.Core;
using FamilyHubs.ServiceDirectory.Core.Entities;
using FamilyHubs.ServiceDirectory.Infrastructure.Persistence.Repository;
using FamilyHubs.ServiceDirectory.Shared.Dto;
using FamilyHubs.ServiceDirectory.Shared.Enums;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace FamilyHubs.ServiceDirectoryApi.UnitTests.Services;

public class WhenUsingUpdateServiceCommand : BaseCreateDbUnitTest
{
    public WhenUsingUpdateServiceCommand()
    {
        var myProfile = new AutoMappingProfiles();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
        Mapper = new Mapper(configuration);
        var logger = new Mock<ILogger<CreateOrganisationCommandHandler>>();
        MockApplicationDbContext = GetApplicationDbContext();

        MockMediator = new Mock<IMediator>();
        var createServiceCommandHandler = new CreateServiceCommandHandler(MockApplicationDbContext, Mapper, NullLogger<CreateServiceCommandHandler>.Instance);
        var updateServiceCommandHandler = new UpdateServiceCommandHandler(MockApplicationDbContext, Mapper, NullLogger<UpdateServiceCommandHandler>.Instance);
        MockMediator.Setup(m => m.Send(It.IsAny<CreateServiceCommand>(), It.IsAny<CancellationToken>()))
            .Callback<object, CancellationToken>((notification, cToken) =>
                createServiceCommandHandler.Handle((CreateServiceCommand)notification, cToken).GetAwaiter().GetResult());

        MockMediator.Setup(m => m.Send(It.IsAny<UpdateServiceCommand>(), It.IsAny<CancellationToken>()))
            .Callback<object, CancellationToken>((notification, cToken) =>
                updateServiceCommandHandler.Handle((UpdateServiceCommand)notification, cToken).GetAwaiter().GetResult());

        TestOrganisation = TestDataProvider.GetTestCountyCouncilDto();
        var command = new CreateOrganisationCommand(TestOrganisation);
        var handler = new CreateOrganisationCommandHandler(MockApplicationDbContext, Mapper, MockMediator.Object, logger.Object);

        handler.Handle(command, new CancellationToken()).GetAwaiter().GetResult();

        UpdateLogger = new Mock<ILogger<UpdateServiceCommandHandler>>();
    }

    private Mock<ILogger<UpdateServiceCommandHandler>> UpdateLogger { get; }

    private OrganisationWithServicesDto TestOrganisation { get; }

    private Mock<IMediator> MockMediator { get; }

    private IMapper Mapper { get; }

    private ApplicationDbContext MockApplicationDbContext { get; }

    [Fact]
    public async Task ThenUpdateServiceOnly()
    {
        //Arrange

        var service = TestDataProvider.GetTestCountyCouncilServicesDto(TestOrganisation.Id);

        service.Name = "Unit Test Update Service Name";
        service.Description = "Unit Test Update Service Name";
        var updateCommand = new UpdateServiceCommand(service.Id, service);
        var updateHandler = new UpdateServiceCommandHandler(MockApplicationDbContext, Mapper, UpdateLogger.Object);

        //Act
        var result = await updateHandler.Handle(updateCommand, new CancellationToken());

        //Assert
        result.Should().NotBe(0);
        result.Should().Be(service.Id);
    }

    [Fact]
    public async Task ThenUpdateServiceWithExistingContact()
    {
        //Arrange
        var existingContact = TestOrganisation.Services.ElementAt(0).Contacts.ElementAt(0);
        var service = TestDataProvider.GetTestCountyCouncilServicesDto(TestOrganisation.Id);

        service.Name = "Unit Test Update Service Name";
        service.Description = "Unit Test Update Service Name";
        service.Contacts.Add(existingContact);

        var updateCommand = new UpdateServiceCommand(service.Id, service);
        var updateHandler = new UpdateServiceCommandHandler(MockApplicationDbContext, Mapper, UpdateLogger.Object);

        //Act
        var result = await updateHandler.Handle(updateCommand, new CancellationToken());

        //Assert
        result.Should().NotBe(0);
        result.Should().Be(service.Id);

        var linkContacts = MockApplicationDbContext.LinkContacts.Where(lc => lc.LinkId == service.Id).ToList();

        linkContacts.Should().HaveCount(2);
        linkContacts.ElementAt(0).ContactId.Should().Be(existingContact.Id);
    }

    [Fact]
    public async Task ThenUpdateServiceWithNewServiceAtLocation()
    {
        //Arrange
        var serviceDto = TestOrganisation.Services.ElementAt(0);

        serviceDto.Description = "Updated Description";
        serviceDto.Fees = "Updated Fees";
        serviceDto.Name = "Updated Name";
        var newLocation = new LocationDto
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
        serviceDto.Locations.Add(newLocation);
        var command = new UpdateServiceCommand(TestOrganisation.Services.ElementAt(0).Id, TestOrganisation.Services.ElementAt(0));
        var handler = new UpdateServiceCommandHandler(MockApplicationDbContext, Mapper, new Mock<ILogger<UpdateServiceCommandHandler>>().Object);


        //Act
        var result = await handler.Handle(command, new CancellationToken());

        //Assert
        result.Should().NotBe(0);
        result.Should().Be(TestOrganisation.Services.ElementAt(0).Id);

        var actualServices = MockApplicationDbContext.Services.Where(s => s.Id == serviceDto.Id).ToList();
        actualServices.Should().NotBeNull();
        actualServices.Count.Should().Be(1);

        var service = actualServices.SingleOrDefault(s => s.Id == serviceDto.Id);

        service.Should().NotBeNull();
        service!.Description.Should().Be("Updated Description");
        service.Fees.Should().Be("Updated Fees");
        service.Name.Should().Be("Updated Name");
        service.Description.Should().Be("Updated Description");

        service.Locations.Should().Contain(s => s.Name == newLocation.Name && s.PostCode == newLocation.PostCode);
        var location = service.Locations.Single(s => s.Name == newLocation.Name && s.PostCode == newLocation.PostCode);

        location.Name.Should().Be("New Location");
        location.Description.Should().Be("new Description");
        location.Address1.Should().Be("Address1");
        location.City.Should().Be("City");
        location.PostCode.Should().Be("PostCode");
        location.StateProvince.Should().Be("StateProvince");
    }

    [Fact]
    public async Task ThenUpdateServiceWithNewServiceAtLocationAndExistingLocation()
    {
        //Arrange
        var serviceDto = TestOrganisation.Services.ElementAt(0);
        var existingLocation = serviceDto.Locations.ElementAt(0);

        serviceDto.Description = "Updated Description";
        serviceDto.Fees = "Updated Fees";
        serviceDto.Name = "Updated Name";
        serviceDto.Locations.Add(existingLocation);

        var command = new UpdateServiceCommand(TestOrganisation.Services.ElementAt(0).Id, TestOrganisation.Services.ElementAt(0));
        var handler = new UpdateServiceCommandHandler(MockApplicationDbContext, Mapper, new Mock<ILogger<UpdateServiceCommandHandler>>().Object);


        //Act
        var result = await handler.Handle(command, new CancellationToken());

        //Assert
        result.Should().NotBe(0);
        result.Should().Be(TestOrganisation.Services.ElementAt(0).Id);

        var actualServices = MockApplicationDbContext.Services.Where(s => s.Id == serviceDto.Id).ToList();
        actualServices.Should().NotBeNull();
        actualServices.Count.Should().Be(1);

        var service = actualServices.SingleOrDefault(s => s.Id == serviceDto.Id);

        service.Should().NotBeNull();
        service!.Description.Should().Be("Updated Description");
        service.Fees.Should().Be("Updated Fees");
        service.Name.Should().Be("Updated Name");
        service.Description.Should().Be("Updated Description");

        service.Locations.Should().Contain(s => s.Id == existingLocation.Id);
    }

    [Fact]
    public async Task ThenUpdateServiceWithNewLinkContactAndExistingContact()
    {
        //Arrange
        var serviceDto = TestOrganisation.Services.ElementAt(0);
        var existingContact = serviceDto.Contacts.ElementAt(0);

        serviceDto.Contacts.Add(existingContact);

        var command = new UpdateServiceCommand(serviceDto.Id, serviceDto);
        var handler = new UpdateServiceCommandHandler(MockApplicationDbContext, Mapper, new Mock<ILogger<UpdateServiceCommandHandler>>().Object);

        //Act
        var result = await handler.Handle(command, new CancellationToken());

        //Assert
        result.Should().NotBe(0);
        result.Should().Be(TestOrganisation.Services.ElementAt(0).Id);

        MockApplicationDbContext.Contacts
            .Where(c => c.Id == existingContact.Id)
            .ToList().Count.Should().Be(1);

        var linkContacts = MockApplicationDbContext.LinkContacts.Where(lc => lc.ContactId == existingContact.Id).ToList();

        linkContacts.Should().HaveCount(1);
        linkContacts.ElementAt(0).ContactId.Should().Be(existingContact.Id);
    }

    [Fact]
    public async Task ThenUpdateServiceWithExistingContactForServiceAtLocation()
    {
        //Arrange
        var serviceDto = TestOrganisation.Services.ElementAt(0);
        var serviceAtLocationDto = serviceDto.Locations.ElementAt(0);
        var existingContact = serviceAtLocationDto.Contacts.ElementAt(0);

        serviceDto.Locations.ElementAt(0).Contacts.Add(existingContact);

        var command = new UpdateServiceCommand(serviceDto.Id, serviceDto);
        var handler = new UpdateServiceCommandHandler(MockApplicationDbContext, Mapper, new Mock<ILogger<UpdateServiceCommandHandler>>().Object);

        //Act
        var result = await handler.Handle(command, new CancellationToken());

        //Assert
        result.Should().NotBe(0);
        result.Should().Be(TestOrganisation.Services.ElementAt(0).Id);

        var linkContacts = MockApplicationDbContext.LinkContacts.Where(lc => lc.LinkId == serviceAtLocationDto.Id).ToList();

        linkContacts.Should().HaveCount(2);
        linkContacts.ElementAt(0).ContactId.Should().Be(existingContact.Id);
        linkContacts.ElementAt(1).ContactId.Should().Be(existingContact.Id);
    }

    [Fact]
    public async Task ThenUpdateServiceAddNewAddress()
    {
        //Arrange
        var newLocation = new LocationDto
        {
            LocationType = LocationType.NotSet,
            Name = "Name",
            Latitude = 0,
            Longitude = 0,
            Address1 = "address1",
            City = "city",
            PostCode = "postcode",
            StateProvince = "stateProvince",
            Country = "country"
        };

        TestOrganisation.Services.ElementAt(0).Locations.Add(newLocation);

        var command = new UpdateServiceCommand(TestOrganisation.Services.ElementAt(0).Id, TestOrganisation.Services.ElementAt(0));
        var handler = new UpdateServiceCommandHandler(MockApplicationDbContext, Mapper, new Mock<ILogger<UpdateServiceCommandHandler>>().Object);

        //Act
        var result = await handler.Handle(command, new CancellationToken());

        //Assert
        result.Should().NotBe(0);
        result.Should().Be(TestOrganisation.Services.ElementAt(0).Id);

        var expectedEntity = MockApplicationDbContext.Locations.Where(s => s.Name == newLocation.Name && s.PostCode == newLocation.PostCode).ToList();

        expectedEntity.Should().HaveCount(1);
        expectedEntity.ElementAt(0).Should().NotBeNull();

        expectedEntity.ElementAt(0).Id.Should().Be(newLocation.Id);
    }

    [Fact]
    public async Task ThenUpdateServiceUpdateExistingAddress()
    {
        //Arrange
        var addressDto = TestOrganisation.Services.ElementAt(0).Locations.ElementAt(0);

        addressDto.Address1 = "address1";
        addressDto.City = "city";
        addressDto.PostCode = "postcode";
        addressDto.Country = "country";
        addressDto.StateProvince = "stateProvince";

        var command = new UpdateServiceCommand(TestOrganisation.Services.ElementAt(0).Id, TestOrganisation.Services.ElementAt(0));
        var handler = new UpdateServiceCommandHandler(MockApplicationDbContext, Mapper, new Mock<ILogger<UpdateServiceCommandHandler>>().Object);

        //Act
        var result = await handler.Handle(command, new CancellationToken());

        //Assert
        result.Should().NotBe(0);
        result.Should().Be(TestOrganisation.Services.ElementAt(0).Id);

        var expectedEntity = MockApplicationDbContext.Locations.Where(lc => lc.Id == addressDto.Id).ToList();

        expectedEntity.Should().HaveCount(1);
        expectedEntity.ElementAt(0).Should().NotBeNull();

        expectedEntity.ElementAt(0).Id.Should().Be(addressDto.Id);
        expectedEntity.ElementAt(0).Should().BeEquivalentTo(addressDto);
    }

    [Fact]
    public async Task ThenUpdateServiceAttachExistingAddress()
    {
        //Arrange
        var locationDto = TestOrganisation.Services.ElementAt(0).Locations.ElementAt(0);
        var newItem = new LocationDto
        {
            LocationType = LocationType.NotSet,
            Name = "Name",
            Latitude = 0,
            Longitude = 0,
            Address1 = "address1",
            City = "city",
            PostCode = "postcode",
            StateProvince = "stateProvince",
            Country = "country"
        };

        TestOrganisation.Services.ElementAt(0).Locations.Clear();
        TestOrganisation.Services.ElementAt(0).Locations.Add(newItem);

        var newEntity = new Location
        {
            LocationType = LocationType.NotSet,
            Name = "Name",
            Latitude = 0,
            Longitude = 0,
            Address1 = "address1",
            City = "city",
            PostCode = "postcode",
            StateProvince = "stateProvince",
            Country = "country"
        };

        MockApplicationDbContext.Locations.Add(newEntity);

        await MockApplicationDbContext.SaveChangesAsync();

        var command = new UpdateServiceCommand(TestOrganisation.Services.ElementAt(0).Id, TestOrganisation.Services.ElementAt(0));
        var handler = new UpdateServiceCommandHandler(MockApplicationDbContext, Mapper, new Mock<ILogger<UpdateServiceCommandHandler>>().Object);

        //Act
        var result = await handler.Handle(command, new CancellationToken());

        //Assert
        result.Should().NotBe(0);
        result.Should().Be(TestOrganisation.Services.ElementAt(0).Id);

        var expectedEntity = MockApplicationDbContext.Locations.Where(lc => lc.Id == locationDto.Id).ToList();

        expectedEntity.Should().HaveCount(1);
        expectedEntity.ElementAt(0).Should().NotBeNull();

        expectedEntity.ElementAt(0).Id.Should().Be(newItem.Id);
        expectedEntity.ElementAt(0).Should().BeEquivalentTo(newItem);
    }

    [Fact]
    public async Task ThenUpdateServiceWithNewNestedRecords()
    {
        //Arrange
        var command = new UpdateServiceCommand(TestOrganisation.Services.ElementAt(0).Id, TestOrganisation.Services.ElementAt(0));
        var handler = new UpdateServiceCommandHandler(MockApplicationDbContext, Mapper, new Mock<ILogger<UpdateServiceCommandHandler>>().Object);


        //Act
        var result = await handler.Handle(command, new CancellationToken());

        //Assert
        result.Should().NotBe(0);
        result.Should().Be(TestOrganisation.Services.ElementAt(0).Id);
    }

    [Fact]
    public async Task ThenUpdateServiceExistingEligibilities()
    {
        //Arrange
        var serviceDto = TestOrganisation.Services.ElementAt(0);

        var newItem = new EligibilityDto
        {
            ServiceId = serviceDto.Id,
            MaximumAge = 2,
            MinimumAge = 0,
            EligibilityType = EligibilityType.NotSet
        };

        serviceDto.Eligibilities.Add(newItem);

        var command = new UpdateServiceCommand(serviceDto.Id, serviceDto);
        var handler = new UpdateServiceCommandHandler(MockApplicationDbContext, Mapper, new Mock<ILogger<UpdateServiceCommandHandler>>().Object);

        //Act
        var result = await handler.Handle(command, new CancellationToken());

        //Assert
        result.Should().NotBe(0);
        result.Should().Be(TestOrganisation.Services.ElementAt(0).Id);

        MockApplicationDbContext.Eligibilities
            .Where(c => c.Id == newItem.Id)
            .ToList().Count.Should().Be(1);

        var expectedEntity = MockApplicationDbContext.Eligibilities.Where(lc => lc.ServiceId == serviceDto.Id).ToList();

        expectedEntity.Should().HaveCount(1);
        expectedEntity.ElementAt(0).Should().NotBeNull();

        expectedEntity.ElementAt(0).Id.Should().Be(newItem.Id);
        expectedEntity.ElementAt(0).ServiceId.Should().Be(serviceDto.Id);
    }

    [Fact]
    public async Task ThenUpdateServiceExistingServiceAreas()
    {
        //Arrange
        var serviceDto = TestOrganisation.Services.ElementAt(0);

        var newItem = new ServiceAreaDto
        {
            Uri = "Test",
            Extent = "Test",
            ServiceAreaName = "Test",
            ServiceId = serviceDto.Id
        };

        serviceDto.ServiceAreas.Add(newItem);

        var command = new UpdateServiceCommand(serviceDto.Id, serviceDto);
        var handler = new UpdateServiceCommandHandler(MockApplicationDbContext, Mapper, new Mock<ILogger<UpdateServiceCommandHandler>>().Object);

        //Act
        var result = await handler.Handle(command, new CancellationToken());

        //Assert
        result.Should().NotBe(0);
        result.Should().Be(TestOrganisation.Services.ElementAt(0).Id);

        MockApplicationDbContext.ServiceAreas
            .Where(c => c.Id == newItem.Id)
            .ToList().Count.Should().Be(1);

        var expectedEntity = MockApplicationDbContext.ServiceAreas.Where(lc => lc.ServiceId == serviceDto.Id).ToList();

        expectedEntity.Should().HaveCount(1);
        expectedEntity.ElementAt(0).Should().NotBeNull();

        expectedEntity.ElementAt(0).Id.Should().Be(newItem.Id);
        expectedEntity.ElementAt(0).ServiceId.Should().Be(serviceDto.Id);
    }

    [Fact]
    public async Task ThenUpdateServiceExistingServiceDeliveries()
    {
        //Arrange
        var serviceDto = TestOrganisation.Services.ElementAt(0);

        var newItem = new ServiceDeliveryDto
        {
            Name = ServiceDeliveryType.Online,
            ServiceId = serviceDto.Id
        };
        
        serviceDto.ServiceDeliveries.Add(newItem);

        var command = new UpdateServiceCommand(serviceDto.Id, serviceDto);
        var handler = new UpdateServiceCommandHandler(MockApplicationDbContext, Mapper, new Mock<ILogger<UpdateServiceCommandHandler>>().Object);

        //Act
        var result = await handler.Handle(command, new CancellationToken());

        //Assert
        result.Should().NotBe(0);
        result.Should().Be(TestOrganisation.Services.ElementAt(0).Id);

        MockApplicationDbContext.ServiceDeliveries
            .Where(c => c.Id == newItem.Id)
            .ToList().Count.Should().Be(1);

        var expectedEntity = MockApplicationDbContext.ServiceDeliveries.Where(lc => lc.ServiceId == serviceDto.Id).ToList();

        expectedEntity.Should().HaveCount(1);
        expectedEntity.ElementAt(0).Should().NotBeNull();

        expectedEntity.ElementAt(0).Id.Should().Be(newItem.Id);
        expectedEntity.ElementAt(0).ServiceId.Should().Be(serviceDto.Id);
    }

    [Fact]
    public async Task ThenUpdateServiceExistingLanguages()
    {
        //Arrange
        var serviceDto = TestOrganisation.Services.ElementAt(0);

        var newItem = new LanguageDto
        {
            ServiceId = serviceDto.Id,
            Name = "Name"
        };
        
        serviceDto.Languages.Add(newItem);

        var command = new UpdateServiceCommand(serviceDto.Id, serviceDto);
        var handler = new UpdateServiceCommandHandler(MockApplicationDbContext, Mapper, new Mock<ILogger<UpdateServiceCommandHandler>>().Object);

        //Act
        var result = await handler.Handle(command, new CancellationToken());

        //Assert
        result.Should().NotBe(0);
        result.Should().Be(TestOrganisation.Services.ElementAt(0).Id);

        MockApplicationDbContext.Languages
            .Where(c => c.Id == newItem.Id)
            .ToList().Count.Should().Be(1);

        var expectedEntity = MockApplicationDbContext.Languages.Where(lc => lc.ServiceId == serviceDto.Id).ToList();

        expectedEntity.Should().HaveCount(1);
        expectedEntity.ElementAt(0).Should().NotBeNull();

        expectedEntity.ElementAt(0).Id.Should().Be(newItem.Id);
        expectedEntity.ElementAt(0).ServiceId.Should().Be(serviceDto.Id);
    }

    [Fact]
    public async Task ThenUpdateServiceExistingServiceTaxonomies()
    {
        //Arrange
        var serviceDto = TestOrganisation.Services.ElementAt(0);

        var newItem = new TaxonomyDto
        {
            Name = "Name",
            TaxonomyType = TaxonomyType.ServiceCategory,
        };
        
        serviceDto.Taxonomies.Add(newItem);

        var command = new UpdateServiceCommand(serviceDto.Id, serviceDto);
        var handler = new UpdateServiceCommandHandler(MockApplicationDbContext, Mapper, new Mock<ILogger<UpdateServiceCommandHandler>>().Object);

        //Act
        var result = await handler.Handle(command, new CancellationToken());

        //Assert
        result.Should().NotBe(0);
        result.Should().Be(TestOrganisation.Services.ElementAt(0).Id);

        MockApplicationDbContext.ServiceTaxonomies
            .Where(c => c.TaxonomyId == newItem.Id)
            .ToList().Count.Should().Be(1);

        var expectedEntity = MockApplicationDbContext.ServiceTaxonomies.Where(lc => lc.ServiceId == serviceDto.Id).ToList();

        expectedEntity.Should().HaveCount(1);
        expectedEntity.ElementAt(0).Should().NotBeNull();

        expectedEntity.ElementAt(0).TaxonomyId.Should().Be(newItem.Id);
        expectedEntity.ElementAt(0).ServiceId.Should().Be(serviceDto.Id);
    }

    [Fact]
    public async Task ThenUpdateServiceExistingCostOptions()
    {
        //Arrange
        var serviceDto = TestOrganisation.Services.ElementAt(0);

        var newItem = new CostOptionDto
        {
            ServiceId = serviceDto.Id,
            Amount = 12,
            AmountDescription = "test",
        };
        
        serviceDto.CostOptions.Add(newItem);

        var command = new UpdateServiceCommand(serviceDto.Id, serviceDto);
        var handler = new UpdateServiceCommandHandler(MockApplicationDbContext, Mapper, new Mock<ILogger<UpdateServiceCommandHandler>>().Object);

        //Act
        var result = await handler.Handle(command, new CancellationToken());

        //Assert
        result.Should().NotBe(0);
        result.Should().Be(TestOrganisation.Services.ElementAt(0).Id);

        MockApplicationDbContext.CostOptions
            .Where(c => c.Id == newItem.Id)
            .ToList().Count.Should().Be(1);

        var expectedEntity = MockApplicationDbContext.CostOptions.Where(lc => lc.ServiceId == serviceDto.Id).ToList();

        expectedEntity.Should().HaveCount(1);
        expectedEntity.ElementAt(0).Should().NotBeNull();

        expectedEntity.ElementAt(0).Id.Should().Be(newItem.Id);
        expectedEntity.ElementAt(0).ServiceId.Should().Be(serviceDto.Id);
    }

    [Fact]
    public async Task ThenUpdateServiceExistingRegularSchedules()
    {
        //Arrange
        var serviceDto = TestOrganisation.Services.ElementAt(0);

        var newItem = new RegularScheduleDto
        {
            ServiceId = serviceDto.Id,
            Description = "Test"
        };
        
        serviceDto.RegularSchedules.Add(newItem);

        var command = new UpdateServiceCommand(serviceDto.Id, serviceDto);
        var handler = new UpdateServiceCommandHandler(MockApplicationDbContext, Mapper, new Mock<ILogger<UpdateServiceCommandHandler>>().Object);

        //Act
        var result = await handler.Handle(command, new CancellationToken());

        //Assert
        result.Should().NotBe(0);
        result.Should().Be(TestOrganisation.Services.ElementAt(0).Id);

        MockApplicationDbContext.RegularSchedules
            .Where(c => c.Id == newItem.Id)
            .ToList().Count.Should().Be(1);

        var expectedEntity = MockApplicationDbContext.RegularSchedules.Where(lc => lc.ServiceId == serviceDto.Id).ToList();

        expectedEntity.Should().HaveCount(1);
        expectedEntity.ElementAt(0).Should().NotBeNull();

        expectedEntity.ElementAt(0).Id.Should().Be(newItem.Id);
        expectedEntity.ElementAt(0).ServiceId.Should().Be(serviceDto.Id);
    }

    [Fact]
    public async Task ThenUpdateServiceAddAndDeleteRegularSchedules()
    {
        //Arrange
        var serviceDto = TestOrganisation.Services.ElementAt(0);
        var existingItem = TestOrganisation.Services.ElementAt(0).RegularSchedules.ElementAt(0);

        var newItem = new RegularScheduleDto
        {
            ServiceId = serviceDto.Id,
            Description = "Description"
        };
        
        serviceDto.RegularSchedules.Clear();
        serviceDto.RegularSchedules.Add(newItem);

        var command = new UpdateServiceCommand(serviceDto.Id, serviceDto);
        var handler = new UpdateServiceCommandHandler(MockApplicationDbContext, Mapper, new Mock<ILogger<UpdateServiceCommandHandler>>().Object);

        //Act
        var result = await handler.Handle(command, new CancellationToken());

        //Assert
        result.Should().NotBe(0);
        result.Should().Be(TestOrganisation.Services.ElementAt(0).Id);

        MockApplicationDbContext.RegularSchedules
            .Where(c => c.ServiceId == serviceDto.Id)
            .ToList().Count.Should().Be(1);

        var expectedEntity = MockApplicationDbContext.RegularSchedules.Where(lc => lc.ServiceId == serviceDto.Id).ToList();

        expectedEntity.Should().HaveCount(1);
        expectedEntity.ElementAt(0).Should().NotBeNull();

        expectedEntity.ElementAt(0).Id.Should().Be(newItem.Id);
        expectedEntity.ElementAt(0).ServiceId.Should().Be(serviceDto.Id);

        var unexpectedEntity = MockApplicationDbContext.RegularSchedules.Where(lc => existingItem.ServiceId == serviceDto.Id).ToList();
        unexpectedEntity.Should().HaveCount(0);
    }

    [Fact]
    public async Task ThenUpdateServiceUpdateRegularSchedules()
    {
        //Arrange
        var serviceDto = TestOrganisation.Services.ElementAt(0);
        var regularSchedule = TestOrganisation.Services.ElementAt(0).RegularSchedules.ElementAt(0);
        regularSchedule.ByDay = "ByDay";

        var command = new UpdateServiceCommand(serviceDto.Id, serviceDto);
        var handler = new UpdateServiceCommandHandler(MockApplicationDbContext, Mapper, new Mock<ILogger<UpdateServiceCommandHandler>>().Object);

        //Act
        var result = await handler.Handle(command, new CancellationToken());

        //Assert
        result.Should().NotBe(0);
        result.Should().Be(TestOrganisation.Services.ElementAt(0).Id);

        MockApplicationDbContext.RegularSchedules
            .Where(c => c.Id == regularSchedule.Id)
            .ToList().Count.Should().Be(1);

        var expectedEntity = MockApplicationDbContext.RegularSchedules.Where(lc => lc.Id == regularSchedule.Id).ToList();

        expectedEntity.Should().HaveCount(1);
        expectedEntity.ElementAt(0).Should().NotBeNull();

        expectedEntity.ElementAt(0).Id.Should().Be(regularSchedule.Id);
        expectedEntity.ElementAt(0).ServiceId.Should().Be(serviceDto.Id);
        expectedEntity.ElementAt(0).ByDay.Should().Be("ByDay");
    }

    [Fact]
    public async Task ThenUpdateServiceExistingHolidaySchedules()
    {
        //Arrange
        var serviceDto = TestOrganisation.Services.ElementAt(0);

        var newItem = new HolidayScheduleDto
        {
            Closed = false,
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow
        };
        
        serviceDto.HolidaySchedules.Add(newItem);

        var command = new UpdateServiceCommand(serviceDto.Id, serviceDto);
        var handler = new UpdateServiceCommandHandler(MockApplicationDbContext, Mapper, new Mock<ILogger<UpdateServiceCommandHandler>>().Object);

        //Act
        var result = await handler.Handle(command, new CancellationToken());

        //Assert
        result.Should().NotBe(0);
        result.Should().Be(TestOrganisation.Services.ElementAt(0).Id);

        MockApplicationDbContext.HolidaySchedules
            .Where(c => c.Id == newItem.Id)
            .ToList().Count.Should().Be(1);

        var expectedEntity = MockApplicationDbContext.HolidaySchedules.Where(lc => lc.ServiceId == serviceDto.Id).ToList();

        expectedEntity.Should().HaveCount(1);
        expectedEntity.ElementAt(0).Should().NotBeNull();

        expectedEntity.ElementAt(0).Id.Should().Be(newItem.Id);
        expectedEntity.ElementAt(0).ServiceId.Should().Be(serviceDto.Id);
    }
}