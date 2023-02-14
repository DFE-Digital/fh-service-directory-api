using AutoMapper;
using FamilyHubs.ServiceDirectory.Api.Commands.CreateOrganisation;
using FamilyHubs.ServiceDirectory.Api.Commands.CreateService;
using FamilyHubs.ServiceDirectory.Api.Commands.UpdateService;
using FamilyHubs.ServiceDirectory.Core;
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
        result.Should().NotBeNull();
        result.Should().Be(service.Id);
    }

    [Fact]
    public async Task ThenUpdateServiceWithExistingContact()
    {
        //Arrange
        var existingContact = TestOrganisation.Services!.ElementAt(0).LinkContacts!.ElementAt(0).Contact;
        var service = TestDataProvider.GetTestCountyCouncilServicesDto(TestOrganisation.Id);

        service.Name = "Unit Test Update Service Name";
        service.Description = "Unit Test Update Service Name";
        service.LinkContacts!.Add(new LinkContactDto(
            Guid.NewGuid().ToString(),
            service.Id,
            "Service",
            existingContact
            ));

        var updateCommand = new UpdateServiceCommand(service.Id, service);
        var updateHandler = new UpdateServiceCommandHandler(MockApplicationDbContext, Mapper, UpdateLogger.Object);

        //Act
        var result = await updateHandler.Handle(updateCommand, new CancellationToken());

        //Assert
        result.Should().NotBeNull();
        result.Should().Be(service.Id);

        var linkContacts = MockApplicationDbContext.LinkContacts.Where(lc => lc.LinkId == service.Id).ToList();

        linkContacts.Should().HaveCount(2);
        linkContacts.ElementAt(0).Contact.Should().NotBeNull();
        linkContacts.ElementAt(1).Contact.Should().NotBeNull();

        linkContacts.ElementAt(0).Contact!.Id.Should().Be(existingContact.Id);
        linkContacts.ElementAt(1).Contact!.Id.Should().Be(existingContact.Id);
    }

    [Fact]
    public async Task ThenUpdateServiceWithNewServiceAtLocation()
    {
        //Arrange
        var serviceDto = TestOrganisation.Services!.ElementAt(0);

        var serviceAtLocationId = Guid.NewGuid().ToString();
        var locationId = Guid.NewGuid().ToString();
        var physicalAddressId = Guid.NewGuid().ToString();

        serviceDto.Description = "Updated Description";
        serviceDto.Fees = "Updated Fees";
        serviceDto.Name = "Updated Name";
        serviceDto.ServiceAtLocations!.Add(new ServiceAtLocationDto
        {
            Id = serviceAtLocationId,
            Location = new LocationDto
            {
                Id = locationId,
                Name = "New Location",
                Description = "new Description",
                PhysicalAddresses = new List<PhysicalAddressDto>
                {
                    new PhysicalAddressDto
                    {
                        Id = physicalAddressId,
                        Address1 = "Address1",
                        City = "City",
                        Country = "Country",
                        PostCode = "PostCode",
                        StateProvince = "StateProvince"
                    }
                }
            }
        });
        var command = new UpdateServiceCommand(TestOrganisation.Services?.ElementAt(0).Id ?? string.Empty, TestOrganisation.Services?.ElementAt(0) ?? default!);
        var handler = new UpdateServiceCommandHandler(MockApplicationDbContext, Mapper, new Mock<ILogger<UpdateServiceCommandHandler>>().Object);


        //Act
        var result = await handler.Handle(command, new CancellationToken());

        //Assert
        result.Should().NotBeNull();
        result.Should().Be(TestOrganisation.Services?.ElementAt(0).Id);

        var actualServices = MockApplicationDbContext.Services.Where(s => s.Id == serviceDto.Id).ToList();
        actualServices.Should().NotBeNull();
        actualServices.Count.Should().Be(1);

        var service = actualServices.SingleOrDefault(s => s.Id == serviceDto.Id);

        service.Should().NotBeNull();
        service!.Description.Should().Be("Updated Description");
        service.Fees.Should().Be("Updated Fees");
        service.Name.Should().Be("Updated Name");
        service.Description.Should().Be("Updated Description");

        service.ServiceAtLocations.Should().Contain(s => s.Id == serviceAtLocationId);
        service.ServiceAtLocations.Single(s => s.Id == serviceAtLocationId)
            .Location.Id.Should().Be(locationId);

        var location = service.ServiceAtLocations.Single(s => s.Id == serviceAtLocationId)
            .Location;

        location.Name.Should().Be("New Location");
        location.Description.Should().Be("new Description");
        location.PhysicalAddresses.Should().NotBeEmpty();
        location.PhysicalAddresses!.ElementAt(0).Address1.Should().Be("Address1");
        location.PhysicalAddresses!.ElementAt(0).City.Should().Be("City");
        location.PhysicalAddresses!.ElementAt(0).PostCode.Should().Be("PostCode");
        location.PhysicalAddresses!.ElementAt(0).StateProvince.Should().Be("StateProvince");
    }

    [Fact]
    public async Task ThenUpdateServiceWithNewLinkContactAndExistingContact()
    {
        //Arrange
        var serviceDto = TestOrganisation.Services!.ElementAt(0);
        var existingContact = serviceDto.LinkContacts!.ElementAt(0).Contact;

        var newLinkContactId = Guid.NewGuid().ToString();
        serviceDto.LinkContacts!.Add(new LinkContactDto(
            newLinkContactId,
            serviceDto.Id,
            "Service",
            existingContact
            ));

        var command = new UpdateServiceCommand(serviceDto.Id, serviceDto);
        var handler = new UpdateServiceCommandHandler(MockApplicationDbContext, Mapper, new Mock<ILogger<UpdateServiceCommandHandler>>().Object);

        //Act
        var result = await handler.Handle(command, new CancellationToken());

        //Assert
        result.Should().NotBeNull();
        result.Should().Be(TestOrganisation.Services?.ElementAt(0).Id);

        MockApplicationDbContext.Contacts
            .Where(c => c.Id == existingContact.Id)
            .ToList().Count.Should().Be(1);

        var linkContacts = MockApplicationDbContext.LinkContacts.Where(lc => lc.Id == newLinkContactId).ToList();

        linkContacts.Should().HaveCount(1);
        linkContacts.ElementAt(0).Contact.Should().NotBeNull();

        linkContacts.ElementAt(0).Contact!.Id.Should().Be(existingContact.Id);
    }

    [Fact]
    public async Task ThenUpdateServiceWithExistingContactForServiceAtLocation()
    {
        //Arrange
        var serviceDto = TestOrganisation.Services!.ElementAt(0);
        var serviceAtLocationDto = serviceDto.ServiceAtLocations!.ElementAt(0);
        var existingContact = serviceAtLocationDto.LinkContacts!.ElementAt(0).Contact;

        var newLinkContactId = Guid.NewGuid().ToString();
        serviceDto.ServiceAtLocations!.ElementAt(0)
            .LinkContacts!.Add(new LinkContactDto(
            newLinkContactId,
            serviceAtLocationDto.Id,
            "ServiceAtLocation",
            existingContact
            ));

        var command = new UpdateServiceCommand(serviceDto.Id, serviceDto);
        var handler = new UpdateServiceCommandHandler(MockApplicationDbContext, Mapper, new Mock<ILogger<UpdateServiceCommandHandler>>().Object);

        //Act
        var result = await handler.Handle(command, new CancellationToken());

        //Assert
        result.Should().NotBeNull();
        result.Should().Be(TestOrganisation.Services?.ElementAt(0).Id);

        var linkContacts = MockApplicationDbContext.LinkContacts.Where(lc => lc.LinkId == serviceAtLocationDto.Id).ToList();

        linkContacts.Should().HaveCount(2);
        linkContacts.ElementAt(0).Contact.Should().NotBeNull();
        linkContacts.ElementAt(0).Contact!.Id.Should().Be(existingContact.Id);
        linkContacts.ElementAt(1).Contact.Should().NotBeNull();
        linkContacts.ElementAt(1).Contact!.Id.Should().Be(existingContact.Id);
    }

    [Fact]
    public async Task ThenUpdateServiceWithNewNestedRecords()
    {
        //Arrange
        var command = new UpdateServiceCommand(TestOrganisation.Services?.ElementAt(0).Id ?? string.Empty, TestOrganisation.Services?.ElementAt(0) ?? default!);
        var handler = new UpdateServiceCommandHandler(MockApplicationDbContext, Mapper, new Mock<ILogger<UpdateServiceCommandHandler>>().Object);


        //Act
        var result = await handler.Handle(command, new CancellationToken());

        //Assert
        result.Should().NotBeNull();
        result.Should().Be(TestOrganisation.Services?.ElementAt(0).Id);
    }

    [Fact]
    public async Task ThenUpdateServiceWithNewLinkContactAndExistingEligibilities()
    {
        //Arrange
        var serviceDto = TestOrganisation.Services!.ElementAt(0);
        var newId = Guid.NewGuid().ToString();

        var newItem = new EligibilityDto(
            newId,
            "Test",
            1,
            2
        );

        serviceDto.Eligibilities!.Add(newItem);

        var command = new UpdateServiceCommand(serviceDto.Id, serviceDto);
        var handler = new UpdateServiceCommandHandler(MockApplicationDbContext, Mapper, new Mock<ILogger<UpdateServiceCommandHandler>>().Object);

        //Act
        var result = await handler.Handle(command, new CancellationToken());

        //Assert
        result.Should().NotBeNull();
        result.Should().Be(TestOrganisation.Services?.ElementAt(0).Id);

        MockApplicationDbContext.Eligibilities
            .Where(c => c.Id == newItem.Id)
            .ToList().Count.Should().Be(1);

        var expectedEntity = MockApplicationDbContext.Eligibilities.Where(lc => lc.Id == newId).ToList();

        expectedEntity.Should().HaveCount(1);
        expectedEntity.ElementAt(0).Should().NotBeNull();

        expectedEntity.ElementAt(0).Id.Should().Be(newItem.Id);
        expectedEntity.ElementAt(0).ServiceId.Should().Be(serviceDto.Id);
    }

    [Fact]
    public async Task ThenUpdateServiceWithNewLinkContactAndExistingServiceAreas()
    {
        //Arrange
        var serviceDto = TestOrganisation.Services!.ElementAt(0);
        var newId = Guid.NewGuid().ToString();

        var newItem = new ServiceAreaDto(
            newId,
            "Test",
            "Test",
            "Test"
        );

        serviceDto.ServiceAreas!.Add(newItem);

        var command = new UpdateServiceCommand(serviceDto.Id, serviceDto);
        var handler = new UpdateServiceCommandHandler(MockApplicationDbContext, Mapper, new Mock<ILogger<UpdateServiceCommandHandler>>().Object);

        //Act
        var result = await handler.Handle(command, new CancellationToken());

        //Assert
        result.Should().NotBeNull();
        result.Should().Be(TestOrganisation.Services?.ElementAt(0).Id);

        MockApplicationDbContext.ServiceAreas
            .Where(c => c.Id == newItem.Id)
            .ToList().Count.Should().Be(1);

        var expectedEntity = MockApplicationDbContext.ServiceAreas.Where(lc => lc.Id == newId).ToList();

        expectedEntity.Should().HaveCount(1);
        expectedEntity.ElementAt(0).Should().NotBeNull();

        expectedEntity.ElementAt(0).Id.Should().Be(newItem.Id);
        expectedEntity.ElementAt(0).ServiceId.Should().Be(serviceDto.Id);
    }

    [Fact]
    public async Task ThenUpdateServiceWithNewLinkContactAndExistingServiceDeliveries()
    {
        //Arrange
        var serviceDto = TestOrganisation.Services!.ElementAt(0);
        var newId = Guid.NewGuid().ToString();

        var newItem = new ServiceDeliveryDto(
            newId,
            ServiceDeliveryType.Online
        );
        
        serviceDto.ServiceDeliveries!.Add(newItem);

        var command = new UpdateServiceCommand(serviceDto.Id, serviceDto);
        var handler = new UpdateServiceCommandHandler(MockApplicationDbContext, Mapper, new Mock<ILogger<UpdateServiceCommandHandler>>().Object);

        //Act
        var result = await handler.Handle(command, new CancellationToken());

        //Assert
        result.Should().NotBeNull();
        result.Should().Be(TestOrganisation.Services?.ElementAt(0).Id);

        MockApplicationDbContext.ServiceDeliveries
            .Where(c => c.Id == newItem.Id)
            .ToList().Count.Should().Be(1);

        var expectedEntity = MockApplicationDbContext.ServiceDeliveries.Where(lc => lc.Id == newId).ToList();

        expectedEntity.Should().HaveCount(1);
        expectedEntity.ElementAt(0).Should().NotBeNull();

        expectedEntity.ElementAt(0).Id.Should().Be(newItem.Id);
        expectedEntity.ElementAt(0).ServiceId.Should().Be(serviceDto.Id);
    }

    [Fact]
    public async Task ThenUpdateServiceWithNewLinkContactAndExistingLanguages()
    {
        //Arrange
        var serviceDto = TestOrganisation.Services!.ElementAt(0);
        var newId = Guid.NewGuid().ToString();

        var newItem = new LanguageDto(
            newId,
            "Name"
        );
        
        serviceDto.Languages!.Add(newItem);

        var command = new UpdateServiceCommand(serviceDto.Id, serviceDto);
        var handler = new UpdateServiceCommandHandler(MockApplicationDbContext, Mapper, new Mock<ILogger<UpdateServiceCommandHandler>>().Object);

        //Act
        var result = await handler.Handle(command, new CancellationToken());

        //Assert
        result.Should().NotBeNull();
        result.Should().Be(TestOrganisation.Services?.ElementAt(0).Id);

        MockApplicationDbContext.Languages
            .Where(c => c.Id == newItem.Id)
            .ToList().Count.Should().Be(1);

        var expectedEntity = MockApplicationDbContext.Languages.Where(lc => lc.Id == newId).ToList();

        expectedEntity.Should().HaveCount(1);
        expectedEntity.ElementAt(0).Should().NotBeNull();

        expectedEntity.ElementAt(0).Id.Should().Be(newItem.Id);
        expectedEntity.ElementAt(0).ServiceId.Should().Be(serviceDto.Id);
    }

    [Fact]
    public async Task ThenUpdateServiceWithNewLinkContactAndExistingServiceTaxonomies()
    {
        //Arrange
        var serviceDto = TestOrganisation.Services!.ElementAt(0);
        var newId = Guid.NewGuid().ToString();

        var newItem = new ServiceTaxonomyDto(
            newId,
            null
        );
        
        serviceDto.ServiceTaxonomies!.Add(newItem);

        var command = new UpdateServiceCommand(serviceDto.Id, serviceDto);
        var handler = new UpdateServiceCommandHandler(MockApplicationDbContext, Mapper, new Mock<ILogger<UpdateServiceCommandHandler>>().Object);

        //Act
        var result = await handler.Handle(command, new CancellationToken());

        //Assert
        result.Should().NotBeNull();
        result.Should().Be(TestOrganisation.Services?.ElementAt(0).Id);

        MockApplicationDbContext.ServiceTaxonomies
            .Where(c => c.Id == newItem.Id)
            .ToList().Count.Should().Be(1);

        var expectedEntity = MockApplicationDbContext.ServiceTaxonomies.Where(lc => lc.Id == newId).ToList();

        expectedEntity.Should().HaveCount(1);
        expectedEntity.ElementAt(0).Should().NotBeNull();

        expectedEntity.ElementAt(0).Id.Should().Be(newItem.Id);
        expectedEntity.ElementAt(0).ServiceId.Should().Be(serviceDto.Id);
    }

    [Fact]
    public async Task ThenUpdateServiceWithNewLinkContactAndExistingCostOptions()
    {
        //Arrange
        var serviceDto = TestOrganisation.Services!.ElementAt(0);
        var newId = Guid.NewGuid().ToString();

        var newItem = new CostOptionDto(
            newId,
            "test",
            12,
            null,
            null,
            null,
            null
        );
        
        serviceDto.CostOptions!.Add(newItem);

        var command = new UpdateServiceCommand(serviceDto.Id, serviceDto);
        var handler = new UpdateServiceCommandHandler(MockApplicationDbContext, Mapper, new Mock<ILogger<UpdateServiceCommandHandler>>().Object);

        //Act
        var result = await handler.Handle(command, new CancellationToken());

        //Assert
        result.Should().NotBeNull();
        result.Should().Be(TestOrganisation.Services?.ElementAt(0).Id);

        MockApplicationDbContext.CostOptions
            .Where(c => c.Id == newItem.Id)
            .ToList().Count.Should().Be(1);

        var expectedEntity = MockApplicationDbContext.CostOptions.Where(lc => lc.Id == newId).ToList();

        expectedEntity.Should().HaveCount(1);
        expectedEntity.ElementAt(0).Should().NotBeNull();

        expectedEntity.ElementAt(0).Id.Should().Be(newItem.Id);
        expectedEntity.ElementAt(0).ServiceId.Should().Be(serviceDto.Id);
    }

    [Fact]
    public async Task ThenUpdateServiceWithNewLinkContactAndExistingRegularSchedules()
    {
        //Arrange
        var serviceDto = TestOrganisation.Services!.ElementAt(0);
        var newId = Guid.NewGuid().ToString();

        var newItem = new RegularScheduleDto(
            newId,
            "test",
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null
        );
        
        serviceDto.RegularSchedules!.Add(newItem);

        var command = new UpdateServiceCommand(serviceDto.Id, serviceDto);
        var handler = new UpdateServiceCommandHandler(MockApplicationDbContext, Mapper, new Mock<ILogger<UpdateServiceCommandHandler>>().Object);

        //Act
        var result = await handler.Handle(command, new CancellationToken());

        //Assert
        result.Should().NotBeNull();
        result.Should().Be(TestOrganisation.Services?.ElementAt(0).Id);

        MockApplicationDbContext.RegularSchedules
            .Where(c => c.Id == newItem.Id)
            .ToList().Count.Should().Be(1);

        var expectedEntity = MockApplicationDbContext.RegularSchedules.Where(lc => lc.Id == newId).ToList();

        expectedEntity.Should().HaveCount(1);
        expectedEntity.ElementAt(0).Should().NotBeNull();

        expectedEntity.ElementAt(0).Id.Should().Be(newItem.Id);
        expectedEntity.ElementAt(0).ServiceId.Should().Be(serviceDto.Id);
    }

    [Fact]
    public async Task ThenUpdateServiceWithNewLinkContactAndExistingHolidaySchedules()
    {
        //Arrange
        var serviceDto = TestOrganisation.Services!.ElementAt(0);
        var newId = Guid.NewGuid().ToString();

        var newItem = new HolidayScheduleDto(
            newId,
            false,
            null,
            null,
            null,
            null
        );
        
        serviceDto.HolidaySchedules!.Add(newItem);

        var command = new UpdateServiceCommand(serviceDto.Id, serviceDto);
        var handler = new UpdateServiceCommandHandler(MockApplicationDbContext, Mapper, new Mock<ILogger<UpdateServiceCommandHandler>>().Object);

        //Act
        var result = await handler.Handle(command, new CancellationToken());

        //Assert
        result.Should().NotBeNull();
        result.Should().Be(TestOrganisation.Services?.ElementAt(0).Id);

        MockApplicationDbContext.HolidaySchedules
            .Where(c => c.Id == newItem.Id)
            .ToList().Count.Should().Be(1);

        var expectedEntity = MockApplicationDbContext.HolidaySchedules.Where(lc => lc.Id == newId).ToList();

        expectedEntity.Should().HaveCount(1);
        expectedEntity.ElementAt(0).Should().NotBeNull();

        expectedEntity.ElementAt(0).Id.Should().Be(newItem.Id);
        expectedEntity.ElementAt(0).ServiceId.Should().Be(serviceDto.Id);
    }
}