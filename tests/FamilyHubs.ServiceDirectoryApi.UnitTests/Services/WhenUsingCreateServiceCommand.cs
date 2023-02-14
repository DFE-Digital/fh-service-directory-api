using AutoMapper;
using FamilyHubs.ServiceDirectory.Api.Commands.CreateOrganisation;
using FamilyHubs.ServiceDirectory.Api.Commands.CreateService;
using FamilyHubs.ServiceDirectory.Api.Commands.UpdateService;
using FamilyHubs.ServiceDirectory.Core;
using FamilyHubs.ServiceDirectory.Core.Entities;
using FamilyHubs.ServiceDirectory.Infrastructure.Persistence.Repository;
using FamilyHubs.ServiceDirectory.Shared.Dto;
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
        result.Should().NotBeNull();
        result.Should().Be(anotherService.Id);
    }

    [Fact]
    public async Task ThenCreateNewServiceWithExistingContact()
    {
        //Arrange
        CreateOrganisation();

        var anotherService = TestDataProvider.GetTestCountyCouncilServicesDto2(TestOrganisation.Id);

        var contact = Mapper.Map<Contact>(anotherService.LinkContacts!.ElementAt(0).Contact);
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
        result.Should().NotBeNull();
        result.Should().Be(anotherService.Id);

        MockApplicationDbContext.Contacts
            .Where(c => c.Id == contact.Id)
            .ToList().Count.Should().Be(1);

        var linkContacts = MockApplicationDbContext.LinkContacts.Where(lc => lc.LinkId == anotherService.Id).ToList();

        linkContacts.Should().HaveCount(1);
        linkContacts.ElementAt(0).Contact.Should().NotBeNull();

        linkContacts.ElementAt(0).Contact!.Id.Should().Be(contact.Id);
    }

    [Fact]
    public async Task ThenCreateNewServiceWithExistingContactForServiceAtLocation()
    {
        //Arrange
        CreateOrganisation();

        var anotherService = TestDataProvider.GetTestCountyCouncilServicesDto2(TestOrganisation.Id);

        var contact = Mapper.Map<Contact>(anotherService.ServiceAtLocations!.ElementAt(0).LinkContacts!.ElementAt(0).Contact);
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
        result.Should().NotBeNull();
        result.Should().Be(anotherService.Id);

        MockApplicationDbContext.Contacts
            .Where(c => c.Id == contact.Id)
            .ToList().Count.Should().Be(1);

        var linkContacts = MockApplicationDbContext.LinkContacts.Where(lc => lc.LinkId == anotherService.ServiceAtLocations!.ElementAt(0).Id).ToList();

        linkContacts.Should().HaveCount(1);
        linkContacts.ElementAt(0).Contact.Should().NotBeNull();

        linkContacts.ElementAt(0).Contact!.Id.Should().Be(contact.Id);
    }

    [Fact]
    public async Task ThenCreateNewServiceWithExistingEligibility()
    {
        //Arrange
        CreateOrganisation();

        var anotherService = TestDataProvider.GetTestCountyCouncilServicesDto2(TestOrganisation.Id);

        var entity = Mapper.Map<Eligibility>(anotherService.Eligibilities!.ElementAt(0));
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
        result.Should().NotBeNull();
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

        var entity = Mapper.Map<ServiceArea>(anotherService.ServiceAreas!.ElementAt(0));
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
        result.Should().NotBeNull();
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

        var entity = Mapper.Map<Language>(anotherService.Languages!.ElementAt(0));
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
        result.Should().NotBeNull();
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

        var entity = Mapper.Map<CostOption>(anotherService.CostOptions!.ElementAt(0));
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
        result.Should().NotBeNull();
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

        var entity = Mapper.Map<RegularSchedule>(anotherService.RegularSchedules!.ElementAt(0));
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
        result.Should().NotBeNull();
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

        var entity = Mapper.Map<HolidaySchedule>(anotherService.HolidaySchedules!.ElementAt(0));
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
        result.Should().NotBeNull();
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

        var entity = Mapper.Map<ServiceDelivery>(anotherService.ServiceDeliveries!.ElementAt(0));
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
        result.Should().NotBeNull();
        result.Should().Be(anotherService.Id);

        MockApplicationDbContext.ServiceDeliveries
            .Where(c => c.Id == entity.Id)
            .ToList().Count.Should().Be(1);

        var actualEntity = MockApplicationDbContext.ServiceDeliveries.Where(lc => lc.ServiceId == anotherService.Id).ToList();

        actualEntity.Should().HaveCount(1);
        actualEntity.ElementAt(0).Id.Should().Be(entity.Id);
    }
}