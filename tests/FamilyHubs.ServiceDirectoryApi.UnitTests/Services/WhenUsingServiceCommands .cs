﻿using Ardalis.GuardClauses;
using AutoMapper;
using FamilyHubs.ServiceDirectory.Api.Commands.CreateOrganisation;
using FamilyHubs.ServiceDirectory.Api.Commands.CreateService;
using FamilyHubs.ServiceDirectory.Api.Commands.DeleteService;
using FamilyHubs.ServiceDirectory.Api.Commands.UpdateService;
using FamilyHubs.ServiceDirectory.Api.Queries.GetServices;
using FamilyHubs.ServiceDirectory.Api.Queries.GetServicesByOrganisation;
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

public class WhenUsingServiceCommands : BaseCreateDbUnitTest
{
    public WhenUsingServiceCommands()
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
    public async Task ThenGetService()
    {
        //Arrange
        CreateOrganisation();

        var command = new GetServicesCommand("Information Sharing", "active", "XTEST", null, null, null,
            null, null, null, 1, 10, null, null, null, null, null, null, null, null);
        var handler = new GetServicesCommandHandler(MockApplicationDbContext);

        //Act
        var results = await handler.Handle(command, new CancellationToken());

        //Assert
        results.Should().NotBeNull();
        ArgumentNullException.ThrowIfNull(TestOrganisation);
        ArgumentNullException.ThrowIfNull(TestOrganisation.Services);
        results.Items[0].Should().BeEquivalentTo(TestOrganisation.Services.ElementAt(0));
    }

    [Fact]
    public async Task ThenGetServicesByOrganisationId()
    {
        //Arrange
        CreateOrganisation();

        var command = new GetServicesByOrganisationIdCommand(TestOrganisation.Id);
        var handler = new GetServicesByOrganisationIdCommandHandler(MockApplicationDbContext);

        //Act
        var results = await handler.Handle(command, new CancellationToken());

        //Assert
        results.Should().NotBeNull();
        ArgumentNullException.ThrowIfNull(TestOrganisation);
        ArgumentNullException.ThrowIfNull(TestOrganisation.Services);
        results[0].Should().BeEquivalentTo(TestOrganisation.Services.ElementAt(0));
    }

    [Fact]
    public async Task ThenGetServicesByOrganisationId_ShouldThrowExceptionWhenNoOrganisations()
    {
        //Arrange
        var command = new GetServicesByOrganisationIdCommand(TestOrganisation.Id);
        var handler = new GetServicesByOrganisationIdCommandHandler(MockApplicationDbContext);

        // Act 
        // Assert
        await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, new CancellationToken()));

    }

    [Fact]
    public async Task ThenGetServicesByOrganisationId_ShouldThrowExceptionWhenNoServices()
    {
        //Arrange
        var command = new GetServicesByOrganisationIdCommand(TestOrganisation.Id);
        var handler = new GetServicesByOrganisationIdCommandHandler(MockApplicationDbContext);

        // Act 
        // Assert
        await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, new CancellationToken()));

    }

    [Fact]
    public async Task ThenGetServiceThatArePaidFor()
    {
        //Arrange
        CreateOrganisation();

        var command = new GetServicesCommand("Information Sharing", "active", "XTEST", null, null, null,
            null, null, null, 1, 10, null, null, true, null, null, null, null, null);
        var handler = new GetServicesCommandHandler(MockApplicationDbContext);

        //Act
        var results = await handler.Handle(command, new CancellationToken());

        //Assert
        results.Should().NotBeNull();
        results.Items.Count.Should().Be(0);
    }

    [Fact]
    public async Task ThenGetServiceThatAreFree()
    {
        //Arrange
        CreateOrganisation();

        var command = new GetServicesCommand("Information Sharing", "active", "XTEST", null, null, null,
            null, null, null, 1, 10, null, null, false, null, null, null, null, null);
        var handler = new GetServicesCommandHandler(MockApplicationDbContext);

        //Act
        var results = await handler.Handle(command, new CancellationToken());

        //Assert
        results.Should().NotBeNull();
        ArgumentNullException.ThrowIfNull(TestOrganisation);
        ArgumentNullException.ThrowIfNull(TestOrganisation.Services);
        results.Items[0].Should().BeEquivalentTo(TestOrganisation.Services.ElementAt(0));
    }

    [Fact]
    public async Task ThenDeleteService()
    {
        //Arrange
        CreateOrganisation();

        var command = new DeleteServiceByIdCommand("3010521b-6e0a-41b0-b610-200edbbeeb14");
        var handler = new DeleteServiceByIdCommandHandler(MockApplicationDbContext, new Mock<ILogger<DeleteServiceByIdCommandHandler>>().Object);

        //Act
        var results = await handler.Handle(command, new CancellationToken());

        //Assert
        results.Should().Be(true);

    }

    [Fact]
    public async Task ThenDeleteServiceThatDoesNotExist()
    {
        //Arrange
        var command = new DeleteServiceByIdCommand(Guid.NewGuid().ToString());
        var handler = new DeleteServiceByIdCommandHandler(MockApplicationDbContext, new Mock<ILogger<DeleteServiceByIdCommandHandler>>().Object);

        // Act 
        // Assert
        await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, new CancellationToken()));

    }

    [Fact]
    public async Task ThenUpdateService()
    {
        //Arrange
        CreateOrganisation();

        var command = new UpdateServiceCommand(TestOrganisation.Services?.ElementAt(0).Id ?? string.Empty, TestOrganisation.Services?.ElementAt(0) ?? default!);
        var handler = new UpdateServiceCommandHandler(MockApplicationDbContext, Mapper, new Mock<ILogger<UpdateServiceCommandHandler>>().Object);


        //Act
        var result = await handler.Handle(command, new CancellationToken());

        //Assert
        result.Should().NotBeNull();
        result.Should().Be(TestOrganisation.Services?.ElementAt(0).Id);
    }

    [Fact]
    public async Task ThenUpdateServiceWithExistingContact()
    {
        //Arrange
        CreateOrganisation();

        var serviceDto = TestOrganisation.Services!.ElementAt(0);
        var existingContact = serviceDto.LinkContacts!.ElementAt(0).Contact;

        if (serviceDto is null) throw new NullReferenceException("Service Dto is null");
        if (existingContact is null) throw new NullReferenceException("Contact Dto is null");

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

        MockApplicationDbContext.Contacts
            .Where(c => c.Id == existingContact.Id)
            .ToList().Count.Should().Be(1);

        var linkContacts = MockApplicationDbContext.LinkContacts.Where(lc => lc.Id == newLinkContactId).ToList();

        linkContacts.Should().HaveCount(1);
        linkContacts.ElementAt(0).Contact.Should().NotBeNull();

        linkContacts.ElementAt(0).Contact!.Id.Should().Be(existingContact.Id);
    }

    [Fact]
    public async Task ThenUpdateServiceWithNewNestedRecords()
    {
        //Arrange
        CreateOrganisation();

        var command = new UpdateServiceCommand(TestOrganisation.Services?.ElementAt(0).Id ?? string.Empty, TestOrganisation.Services?.ElementAt(0) ?? default!);
        var handler = new UpdateServiceCommandHandler(MockApplicationDbContext, Mapper, new Mock<ILogger<UpdateServiceCommandHandler>>().Object);


        //Act
        var result = await handler.Handle(command, new CancellationToken());

        //Assert
        result.Should().NotBeNull();
        result.Should().Be(TestOrganisation.Services?.ElementAt(0).Id);
    }
}
