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

public class WhenUsingCreateServiceCommand : BaseCreateDbUnitTest
{
    public WhenUsingCreateServiceCommand()
    {
        var myProfile = new AutoMappingProfiles();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
        Mapper = new Mapper(configuration);
        MockApplicationDbContext = GetApplicationDbContext();

        MockMediatR = new Mock<ISender>();
        var createServiceCommandHandler = new CreateServiceCommandHandler(MockApplicationDbContext, Mapper, NullLogger<CreateServiceCommandHandler>.Instance);
        var updateServiceCommandHandler = new UpdateServiceCommandHandler(MockApplicationDbContext, Mapper, NullLogger<UpdateServiceCommandHandler>.Instance);
        MockMediatR.Setup(m => m.Send(It.IsAny<CreateServiceCommand>(), It.IsAny<CancellationToken>()))
            .Callback<object, CancellationToken>((notification, cToken) =>
                createServiceCommandHandler.Handle((CreateServiceCommand)notification, cToken).GetAwaiter().GetResult());

        MockMediatR.Setup(m => m.Send(It.IsAny<UpdateServiceCommand>(), It.IsAny<CancellationToken>()))
            .Callback<object, CancellationToken>((notification, cToken) =>
                updateServiceCommandHandler.Handle((UpdateServiceCommand)notification, cToken).GetAwaiter().GetResult());

        TestOrganisation = TestDataProvider.GetTestCountyCouncilDto();
    }

    private OrganisationWithServicesDto TestOrganisation { get; }

    private Mock<ISender> MockMediatR { get; }

    private ApplicationDbContext MockApplicationDbContext { get; }

    private IMapper Mapper { get; }
    private static NullLogger<T> GetLogger<T>() => new NullLogger<T>();

    private void CreateOrganisation()
    {
        var createOrganisationCommand = new CreateOrganisationCommand(TestOrganisation);

        var handler = new CreateOrganisationCommandHandler(MockApplicationDbContext, Mapper, MockMediatR.Object, GetLogger<CreateOrganisationCommandHandler>());

        handler.Handle(createOrganisationCommand, new CancellationToken()).GetAwaiter().GetResult();
    }

    [Fact]
    public async Task ThenCreateService()
    {
        //Arrange
        CreateOrganisation();

        var anotherService = TestDataProvider.GetTestCountyCouncilServicesDto2(TestOrganisation.Id);

        var command = new CreateServiceCommand(anotherService);

        var handler = new CreateServiceCommandHandler(MockApplicationDbContext, Mapper, new Mock<ILogger<CreateServiceCommandHandler>>().Object);

        //Act
        var result = await handler.Handle(command, new CancellationToken());

        //Assert
        result.Should().NotBe(0);
        result.Should().Be(anotherService.Id);
    }

    [Fact]
    public async Task ThenCreateNewServiceWithExistingContact()
    {
        //Arrange
        CreateOrganisation();

        var anotherService = TestDataProvider.GetTestCountyCouncilServicesDto2(TestOrganisation.Id);

        var contact = Mapper.Map<Contact>(anotherService.Contacts.ElementAt(0));
        MockApplicationDbContext.Contacts.Add(contact);
        await MockApplicationDbContext.SaveChangesAsync();

        var command = new CreateServiceCommand(anotherService);

        var handler = new CreateServiceCommandHandler(MockApplicationDbContext, Mapper, new Mock<ILogger<CreateServiceCommandHandler>>().Object);

        //Act
        MockApplicationDbContext.Contacts
            .Where(c => c.Id == contact.Id)
            .ToList().Count.Should().Be(1);

        var result = await handler.Handle(command, new CancellationToken());

        //Assert
        result.Should().NotBe(0);
        result.Should().Be(anotherService.Id);

        MockApplicationDbContext.Contacts
            .Where(c => c.Id == contact.Id)
            .ToList().Count.Should().Be(1);

        var linkContacts = MockApplicationDbContext.LinkContacts.Where(lc => lc.LinkId == anotherService.Id).ToList();

        linkContacts.Should().HaveCount(1);
        linkContacts.ElementAt(0).ContactId.Should().Be(contact.Id);
    }

    [Fact]
    public async Task ThenCreateNewServiceWithExistingContactForServiceAtLocation()
    {
        //Arrange
        CreateOrganisation();

        var anotherService = TestDataProvider.GetTestCountyCouncilServicesDto2(TestOrganisation.Id);

        var contact = Mapper.Map<Contact>(anotherService.Locations.ElementAt(0).Contacts.ElementAt(0));
        MockApplicationDbContext.Contacts.Add(contact);
        await MockApplicationDbContext.SaveChangesAsync();

        var command = new CreateServiceCommand(anotherService);

        var handler = new CreateServiceCommandHandler(MockApplicationDbContext, Mapper, new Mock<ILogger<CreateServiceCommandHandler>>().Object);

        //Act
        MockApplicationDbContext.Contacts
            .Where(c => c.Id == contact.Id)
            .ToList().Count.Should().Be(1);

        var result = await handler.Handle(command, new CancellationToken());

        //Assert
        result.Should().NotBe(0);
        result.Should().Be(anotherService.Id);

        MockApplicationDbContext.Contacts
            .Where(c => c.Id == contact.Id)
            .ToList().Count.Should().Be(1);

        var linkContacts = MockApplicationDbContext.LinkContacts.Where(lc => lc.LinkId == anotherService.Locations.ElementAt(0).Id).ToList();

        linkContacts.Should().HaveCount(1);
        linkContacts.ElementAt(0).ContactId.Should().Be(contact.Id);
    }

    [Fact]
    public async Task ThenCreateNewServiceWithExistingLocation()
    {
        //Arrange
        CreateOrganisation();

        var anotherService = TestDataProvider.GetTestCountyCouncilServicesDto2(TestOrganisation.Id);
        var existingLocation = TestOrganisation.Services.ElementAt(0).Locations.ElementAt(0);

        anotherService.Locations.Clear();
        anotherService.Locations.Add(existingLocation);

        var command = new CreateServiceCommand(anotherService);

        var handler = new CreateServiceCommandHandler(MockApplicationDbContext, Mapper, new Mock<ILogger<CreateServiceCommandHandler>>().Object);

        //Act
        MockApplicationDbContext.ServiceAtLocations
            .Where(c => c.LocationId == existingLocation.Id)
            .ToList().Count.Should().Be(1);

        var result = await handler.Handle(command, new CancellationToken());

        //Assert
        result.Should().NotBe(0);
        result.Should().Be(anotherService.Id);

        MockApplicationDbContext.ServiceAtLocations
            .Where(c => c.LocationId == existingLocation.Id)
            .ToList().Count.Should().Be(2);

        var actualEntity = MockApplicationDbContext.Locations.Where(lc => lc.Id == existingLocation.Id).ToList();

        actualEntity.Should().HaveCount(1);
        actualEntity.ElementAt(0).Id.Should().Be(existingLocation.Id);
    }

    [Fact]
    public async Task ThenCreateNewServiceWithExistingAddress()
    {
        //Arrange
        CreateOrganisation();

        var anotherService = TestDataProvider.GetTestCountyCouncilServicesDto2(TestOrganisation.Id);
        var existingAddress = TestOrganisation.Services.ElementAt(0).Locations.ElementAt(0);

        var anotherLocation = anotherService.Locations.ElementAt(0);

        anotherLocation.Name = existingAddress.Name;
        anotherLocation.Address1 = existingAddress.Address1;
        anotherLocation.City = existingAddress.City;
        anotherLocation.PostCode = existingAddress.PostCode;
        anotherLocation.Country = existingAddress.Country;
        anotherLocation.StateProvince = existingAddress.StateProvince;

        var command = new CreateServiceCommand(anotherService);

        var handler = new CreateServiceCommandHandler(MockApplicationDbContext, Mapper, new Mock<ILogger<CreateServiceCommandHandler>>().Object);

        //Act
        MockApplicationDbContext.Locations.Where(c => c.Id == anotherLocation.Id)
            .ToList().Count.Should().Be(1);

        var result = await handler.Handle(command, new CancellationToken());

        //Assert
        result.Should().NotBe(0);
        result.Should().Be(anotherService.Id);

        var expectedEntity = MockApplicationDbContext.Locations.Where(lc => lc.Id == anotherLocation.Id).ToList();

        expectedEntity.Should().HaveCount(1);
    }

    [Fact]
    public async Task ThenCreateNewServiceWithNewAdditionalAddress()
    {
        //Arrange
        CreateOrganisation();

        var anotherService = TestDataProvider.GetTestCountyCouncilServicesDto2(TestOrganisation.Id);

        //add new address with new ID should still attach the address to existing address
        var newLocation = new LocationDto
        {
            LocationType = LocationType.NotSet,
            Name = "",
            Latitude = 0,
            Longitude = 0,
            Address1 = "Address1",
            City = "City",
            PostCode = "PostCode",
            StateProvince = "StateProvince",
            Country = "Country",
        };
        anotherService.Locations.Add(newLocation);

        anotherService.Locations.Count.Should().Be(2);

        MockApplicationDbContext.Locations
            .Where(c => c.Name == newLocation.Name && c.PostCode == newLocation.PostCode)
            .ToList().Count.Should().Be(0);

        //Act
        var command = new CreateServiceCommand(anotherService);
        var handler = new CreateServiceCommandHandler(MockApplicationDbContext, Mapper, new Mock<ILogger<CreateServiceCommandHandler>>().Object);

        var result = await handler.Handle(command, new CancellationToken());

        //Assert
        result.Should().NotBe(0);
        result.Should().Be(anotherService.Id);

        MockApplicationDbContext.Locations
            .Where(c => c.Name == newLocation.Name && c.PostCode == newLocation.PostCode)
            .ToList().Count.Should().Be(1);

        var actualEntity = MockApplicationDbContext.ServiceAtLocations.Where(lc => lc.ServiceId == anotherService.Id).ToList();

        actualEntity.Should().HaveCount(2);
    }

    [Fact]
    public async Task ThenCreateNewServiceWithExistingButUpdateAddress()
    {
        //Arrange
        CreateOrganisation();

        var anotherService = TestDataProvider.GetTestCountyCouncilServicesDto2(TestOrganisation.Id);
        var existingLocation = TestOrganisation.Services.ElementAt(0).Locations.ElementAt(0);
        //add existing address with new ID should still attach the address to existing address

        existingLocation.Address1 = "Address1";
        existingLocation.City = "City";
        existingLocation.PostCode = "PostCode";
        existingLocation.Country = "Country";
        existingLocation.StateProvince = "StateProvince";

        //Act
        var dbAddress = MockApplicationDbContext.Locations.Where(c => c.Id == existingLocation.Id).ToList();
        dbAddress.Count.Should().Be(1);
        dbAddress.ElementAt(0).Address1.Should().NotBe("Address1");

        var command = new CreateServiceCommand(anotherService);

        var handler = new CreateServiceCommandHandler(MockApplicationDbContext, Mapper, new Mock<ILogger<CreateServiceCommandHandler>>().Object);

        var result = await handler.Handle(command, new CancellationToken());

        //Assert
        result.Should().NotBe(0);
        result.Should().Be(anotherService.Id);

        var actualEntity = MockApplicationDbContext.Locations.Where(c => c.Id == existingLocation.Id).ToList();

        actualEntity.Should().HaveCount(1);
        actualEntity.ElementAt(0).Id.Should().Be(existingLocation.Id);

        actualEntity.ElementAt(0).Address1.Should().Be("Address1");
        actualEntity.ElementAt(0).City.Should().Be("City");
        actualEntity.ElementAt(0).PostCode.Should().Be("PostCode");
        actualEntity.ElementAt(0).Country.Should().Be("Country");
        actualEntity.ElementAt(0).StateProvince.Should().Be("StateProvince");
    }

    [Fact]
    public async Task ThenCreateNewServiceWithExistingEligibility()
    {
        //Arrange
        CreateOrganisation();

        var anotherService = TestDataProvider.GetTestCountyCouncilServicesDto2(TestOrganisation.Id);

        var entity = Mapper.Map<Eligibility>(anotherService.Eligibilities.ElementAt(0));
        entity.ServiceId = anotherService.Id;
        MockApplicationDbContext.Eligibilities.Add(entity);
        await MockApplicationDbContext.SaveChangesAsync();

        var command = new CreateServiceCommand(anotherService);

        var handler = new CreateServiceCommandHandler(MockApplicationDbContext, Mapper, new Mock<ILogger<CreateServiceCommandHandler>>().Object);

        //Act
        MockApplicationDbContext.Eligibilities
            .Where(c => c.Id == entity.Id)
            .ToList().Count.Should().Be(1);

        var result = await handler.Handle(command, new CancellationToken());

        //Assert
        result.Should().NotBe(0);
        result.Should().Be(anotherService.Id);

        MockApplicationDbContext.Eligibilities
            .Where(c => c.Id == entity.Id)
            .ToList().Count.Should().Be(1);

        var actualEntity = MockApplicationDbContext.Eligibilities.Where(lc => lc.ServiceId == anotherService.Id).ToList();

        actualEntity.Should().HaveCount(1);
        actualEntity.ElementAt(0).Id.Should().Be(entity.Id);
    }

    [Fact]
    public async Task ThenCreateNewServiceWithExistingServiceAreas()
    {
        //Arrange
        CreateOrganisation();

        var anotherService = TestDataProvider.GetTestCountyCouncilServicesDto2(TestOrganisation.Id);

        var entity = Mapper.Map<ServiceArea>(anotherService.ServiceAreas.ElementAt(0));
        entity.ServiceId = anotherService.Id;
        MockApplicationDbContext.ServiceAreas.Add(entity);
        await MockApplicationDbContext.SaveChangesAsync();

        var command = new CreateServiceCommand(anotherService);

        var handler = new CreateServiceCommandHandler(MockApplicationDbContext, Mapper, new Mock<ILogger<CreateServiceCommandHandler>>().Object);

        //Act
        MockApplicationDbContext.ServiceAreas
            .Where(c => c.Id == entity.Id)
            .ToList().Count.Should().Be(1);

        var result = await handler.Handle(command, new CancellationToken());

        //Assert
        result.Should().NotBe(0);
        result.Should().Be(anotherService.Id);

        MockApplicationDbContext.ServiceAreas
            .Where(c => c.Id == entity.Id)
            .ToList().Count.Should().Be(1);

        var actualEntity = MockApplicationDbContext.ServiceAreas.Where(lc => lc.ServiceId == anotherService.Id).ToList();

        actualEntity.Should().HaveCount(1);
        actualEntity.ElementAt(0).Id.Should().Be(entity.Id);
    }

    [Fact]
    public async Task ThenCreateNewServiceWithExistingLanguages()
    {
        //Arrange
        CreateOrganisation();

        var anotherService = TestDataProvider.GetTestCountyCouncilServicesDto2(TestOrganisation.Id);

        var entity = Mapper.Map<Language>(anotherService.Languages.ElementAt(0));
        entity.ServiceId = anotherService.Id;
        MockApplicationDbContext.Languages.Add(entity);
        await MockApplicationDbContext.SaveChangesAsync();

        var command = new CreateServiceCommand(anotherService);

        var handler = new CreateServiceCommandHandler(MockApplicationDbContext, Mapper, new Mock<ILogger<CreateServiceCommandHandler>>().Object);

        //Act
        MockApplicationDbContext.Languages
            .Where(c => c.Id == entity.Id)
            .ToList().Count.Should().Be(1);

        var result = await handler.Handle(command, new CancellationToken());

        //Assert
        result.Should().NotBe(0);
        result.Should().Be(anotherService.Id);

        MockApplicationDbContext.Languages
            .Where(c => c.Id == entity.Id)
            .ToList().Count.Should().Be(1);

        var actualEntity = MockApplicationDbContext.Languages.Where(lc => lc.ServiceId == anotherService.Id).ToList();

        actualEntity.Should().HaveCount(1);
        actualEntity.ElementAt(0).Id.Should().Be(entity.Id);
    }

    [Fact]
    public async Task ThenCreateNewServiceWithExistingCostOptions()
    {
        //Arrange
        CreateOrganisation();

        var anotherService = TestDataProvider.GetTestCountyCouncilServicesDto2(TestOrganisation.Id);

        var entity = Mapper.Map<CostOption>(anotherService.CostOptions.ElementAt(0));
        entity.ServiceId = anotherService.Id;
        MockApplicationDbContext.CostOptions.Add(entity);
        await MockApplicationDbContext.SaveChangesAsync();

        var command = new CreateServiceCommand(anotherService);

        var handler = new CreateServiceCommandHandler(MockApplicationDbContext, Mapper, new Mock<ILogger<CreateServiceCommandHandler>>().Object);

        //Act
        MockApplicationDbContext.CostOptions
            .Where(c => c.Id == entity.Id)
            .ToList().Count.Should().Be(1);

        var result = await handler.Handle(command, new CancellationToken());

        //Assert
        result.Should().NotBe(0);
        result.Should().Be(anotherService.Id);

        MockApplicationDbContext.CostOptions
            .Where(c => c.Id == entity.Id)
            .ToList().Count.Should().Be(1);

        var actualEntity = MockApplicationDbContext.CostOptions.Where(lc => lc.ServiceId == anotherService.Id).ToList();

        actualEntity.Should().HaveCount(1);
        actualEntity.ElementAt(0).Id.Should().Be(entity.Id);
    }

    [Fact]
    public async Task ThenCreateNewServiceWithExistingRegularSchedules()
    {
        //Arrange
        CreateOrganisation();

        var anotherService = TestDataProvider.GetTestCountyCouncilServicesDto2(TestOrganisation.Id);

        var entity = Mapper.Map<RegularSchedule>(anotherService.RegularSchedules.ElementAt(0));
        entity.ServiceId = anotherService.Id;
        MockApplicationDbContext.RegularSchedules.Add(entity);
        await MockApplicationDbContext.SaveChangesAsync();

        var command = new CreateServiceCommand(anotherService);

        var handler = new CreateServiceCommandHandler(MockApplicationDbContext, Mapper, new Mock<ILogger<CreateServiceCommandHandler>>().Object);

        //Act
        MockApplicationDbContext.RegularSchedules
            .Where(c => c.Id == entity.Id)
            .ToList().Count.Should().Be(1);

        var result = await handler.Handle(command, new CancellationToken());

        //Assert
        result.Should().NotBe(0);
        result.Should().Be(anotherService.Id);

        MockApplicationDbContext.RegularSchedules
            .Where(c => c.Id == entity.Id)
            .ToList().Count.Should().Be(1);

        var actualEntity = MockApplicationDbContext.RegularSchedules.Where(lc => lc.ServiceId == anotherService.Id).ToList();

        actualEntity.Should().HaveCount(1);
        actualEntity.ElementAt(0).Id.Should().Be(entity.Id);
    }

    [Fact]
    public async Task ThenCreateNewServiceWithExistingHolidaySchedules()
    {
        //Arrange
        CreateOrganisation();

        var anotherService = TestDataProvider.GetTestCountyCouncilServicesDto2(TestOrganisation.Id);

        var entity = Mapper.Map<HolidaySchedule>(anotherService.HolidaySchedules.ElementAt(0));
        entity.ServiceId = anotherService.Id;
        MockApplicationDbContext.HolidaySchedules.Add(entity);
        await MockApplicationDbContext.SaveChangesAsync();

        var command = new CreateServiceCommand(anotherService);

        var handler = new CreateServiceCommandHandler(MockApplicationDbContext, Mapper, new Mock<ILogger<CreateServiceCommandHandler>>().Object);

        //Act
        MockApplicationDbContext.HolidaySchedules
            .Where(c => c.Id == entity.Id)
            .ToList().Count.Should().Be(1);

        var result = await handler.Handle(command, new CancellationToken());

        //Assert
        result.Should().NotBe(0);
        result.Should().Be(anotherService.Id);

        MockApplicationDbContext.HolidaySchedules
            .Where(c => c.Id == entity.Id)
            .ToList().Count.Should().Be(1);

        var actualEntity = MockApplicationDbContext.HolidaySchedules.Where(lc => lc.ServiceId == anotherService.Id).ToList();

        actualEntity.Should().HaveCount(1);
        actualEntity.ElementAt(0).Id.Should().Be(entity.Id);
    }

    [Fact]
    public async Task ThenCreateNewServiceWithExistingServiceDeliveries()
    {
        //Arrange
        CreateOrganisation();

        var anotherService = TestDataProvider.GetTestCountyCouncilServicesDto2(TestOrganisation.Id);

        var entity = Mapper.Map<ServiceDelivery>(anotherService.ServiceDeliveries.ElementAt(0));
        entity.ServiceId = anotherService.Id;
        MockApplicationDbContext.ServiceDeliveries.Add(entity);
        await MockApplicationDbContext.SaveChangesAsync();

        var command = new CreateServiceCommand(anotherService);

        var handler = new CreateServiceCommandHandler(MockApplicationDbContext, Mapper, new Mock<ILogger<CreateServiceCommandHandler>>().Object);

        //Act
        MockApplicationDbContext.ServiceDeliveries
            .Where(c => c.Id == entity.Id)
            .ToList().Count.Should().Be(1);

        var result = await handler.Handle(command, new CancellationToken());

        //Assert
        result.Should().NotBe(0);
        result.Should().Be(anotherService.Id);

        MockApplicationDbContext.ServiceDeliveries
            .Where(c => c.Id == entity.Id)
            .ToList().Count.Should().Be(1);

        var actualEntity = MockApplicationDbContext.ServiceDeliveries.Where(lc => lc.ServiceId == anotherService.Id).ToList();

        actualEntity.Should().HaveCount(1);
        actualEntity.ElementAt(0).Id.Should().Be(entity.Id);
    }
}