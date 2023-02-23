using Ardalis.GuardClauses;
using AutoMapper;
using FamilyHubs.ServiceDirectory.Api.Commands.CreateOrganisation;
using FamilyHubs.ServiceDirectory.Api.Commands.CreateService;
using FamilyHubs.ServiceDirectory.Api.Commands.UpdateOrganisation;
using FamilyHubs.ServiceDirectory.Api.Commands.UpdateService;
using FamilyHubs.ServiceDirectory.Api.Queries.GetOrganisationAdminByOrganisationId;
using FamilyHubs.ServiceDirectory.Api.Queries.GetOrganisationById;
using FamilyHubs.ServiceDirectory.Api.Queries.GetOrganisationTypes;
using FamilyHubs.ServiceDirectory.Api.Queries.ListOrganisation;
using FamilyHubs.ServiceDirectory.Core;
using FamilyHubs.ServiceDirectory.Core.Entities;
using FamilyHubs.ServiceDirectory.Infrastructure.Persistence.Repository;
using FamilyHubs.ServiceDirectory.Shared.Dto;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace FamilyHubs.ServiceDirectoryApi.UnitTests.Organisation;

public class WhenUsingUpdateOrganisationCommand : BaseCreateDbUnitTest
{
    public WhenUsingUpdateOrganisationCommand()
    {
        TestOrganisation = TestDataProvider.GetTestCountyCouncilDto();

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
    }

    private void CreateOrganisation()
    {
        var createOrganisationCommand = new CreateOrganisationCommand(TestOrganisation);
        var handler = new CreateOrganisationCommandHandler(MockApplicationDbContext, Mapper, MockMediatR.Object,
            GetLogger<CreateOrganisationCommandHandler>());
        handler.Handle(createOrganisationCommand, new CancellationToken()).GetAwaiter().GetResult();
    }

    private OrganisationWithServicesDto TestOrganisation { get; }
    private IMapper Mapper { get; }
    private Mock<ISender> MockMediatR { get; }
    private ApplicationDbContext MockApplicationDbContext { get; }
    private static NullLogger<T> GetLogger<T>() => new NullLogger<T>();

    [Fact]
    public async Task ThenUpdateOrganisation()
    {
        //Arrange
        CreateOrganisation();

        var updateLogger = new Mock<ILogger<UpdateOrganisationCommandHandler>>();
        
        var updateTestOrganisation = TestDataProvider.GetTestCountyCouncilDto(true);


        var updateCommand = new UpdateOrganisationCommand(updateTestOrganisation.Id, updateTestOrganisation);
        var updateHandler = new UpdateOrganisationCommandHandler(MockApplicationDbContext, updateLogger.Object, MockMediatR.Object, Mapper);

        //Act
        var result = await updateHandler.Handle(updateCommand, new CancellationToken());

        //Assert
        result.Should().NotBeNull();
        result.Should().Be(TestOrganisation.Id);
    }

    [Fact]
    public async Task ThenUpdateOrganisationWithNewService()
    {
        //Arrange
        CreateOrganisation();

        TestOrganisation.Services = default;
        var updateLogger = new Mock<ILogger<UpdateOrganisationCommandHandler>>();

        var updateTestOrganisation = TestDataProvider.GetTestCountyCouncilDto();
        
        updateTestOrganisation.Services = new List<ServiceDto>
        {
             TestDataProvider.GetTestCountyCouncilServicesDto2("56e62852-1b0b-40e5-ac97-54a67ea957dc")
        };

        var updateCommand = new UpdateOrganisationCommand(updateTestOrganisation.Id, updateTestOrganisation);
        var updateHandler = new UpdateOrganisationCommandHandler(MockApplicationDbContext, updateLogger.Object, MockMediatR.Object, Mapper);

        //Act
        var result = await updateHandler.Handle(updateCommand, new CancellationToken());

        //Assert
        result.Should().NotBeNull();
        result.Should().Be(TestOrganisation.Id);
        var actualServices = MockApplicationDbContext.Services
            .Where(s => s.OrganisationId == updateTestOrganisation.Id).ToList();
        actualServices.Should().NotBeNull();
        actualServices.Count.Should().Be(2);
        actualServices.SingleOrDefault(s => s.Id == "5059a0b2-ad5d-4288-b7c1-e30d35345b0e").Should().NotBeNull();
    }

    [Fact]
    public async Task ThenUpdateOrganisationWithUpdatedService()
    {
        //Arrange
        CreateOrganisation();

        var updateLogger = new Mock<ILogger<UpdateOrganisationCommandHandler>>();

        var updateTestOrganisation = TestDataProvider.GetTestCountyCouncilDto();
        
        var serviceDto = updateTestOrganisation.Services!.ElementAt(0);

        if (serviceDto is null) throw new NullReferenceException("Service Dto is null");
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

        var updateCommand = new UpdateOrganisationCommand(updateTestOrganisation.Id, updateTestOrganisation);
        var updateHandler = new UpdateOrganisationCommandHandler(MockApplicationDbContext, updateLogger.Object, MockMediatR.Object, Mapper);

        //Act
        var result = await updateHandler.Handle(updateCommand, new CancellationToken());

        //Assert
        result.Should().NotBeNull();
        result.Should().Be(TestOrganisation.Id);
        
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
    public async Task ThenUpdateOrganisationWithUpdatedServiceAndExistingContact()
    {
        //Arrange
        CreateOrganisation();

        var updateLogger = new Mock<ILogger<UpdateOrganisationCommandHandler>>();

        var serviceDto = TestOrganisation.Services!.ElementAt(0);

        var existingContactDto = TestOrganisation.Services!.ElementAt(0).LinkContacts!.ElementAt(0).Contact;

        serviceDto.LinkContacts!.Add(new LinkContactDto(
            Guid.NewGuid().ToString(),
            serviceDto.Id,
            "Service",
            existingContactDto
        ));

        var updateCommand = new UpdateOrganisationCommand(TestOrganisation.Id, TestOrganisation);
        var updateHandler = new UpdateOrganisationCommandHandler(MockApplicationDbContext, updateLogger.Object, MockMediatR.Object, Mapper);

        //Act
        var result = await updateHandler.Handle(updateCommand, new CancellationToken());

        //Assert
        result.Should().NotBeNull();
        result.Should().Be(TestOrganisation.Id);
        
        var actualServices = MockApplicationDbContext.Services.Where(s => s.Id == serviceDto.Id).ToList();
        actualServices.Should().NotBeNull();
        actualServices.Count.Should().Be(1);
        
        var service = actualServices.SingleOrDefault(s => s.Id == serviceDto.Id);

        service.Should().NotBeNull();
        var linkContacts = MockApplicationDbContext.LinkContacts.Where(lc => lc.LinkId == serviceDto.Id).ToList();

        linkContacts.Should().HaveCount(2);
        linkContacts.ElementAt(0).Contact.Should().NotBeNull();
        linkContacts.ElementAt(1).Contact.Should().NotBeNull();

        linkContacts.ElementAt(0).Contact!.Id.Should().Be(existingContactDto.Id);
        linkContacts.ElementAt(1).Contact!.Id.Should().Be(existingContactDto.Id);
    }

    [Fact]
    public async Task ThenUpdateOrganisationWithUpdatedServiceAndExistingContactForServiceAtLocation()
    {
        //Arrange
        CreateOrganisation();

        var updateLogger = new Mock<ILogger<UpdateOrganisationCommandHandler>>();

        var serviceDto = TestOrganisation.Services!.ElementAt(0);
        var serviceAtLocationDto = serviceDto.ServiceAtLocations!.ElementAt(0);
        var existingContactDto = serviceAtLocationDto.LinkContacts!.ElementAt(0).Contact;

        serviceAtLocationDto.LinkContacts!.Add(new LinkContactDto(
            Guid.NewGuid().ToString(),
            serviceAtLocationDto.Id,
            "ServiceAtLocation",
            existingContactDto
        ));

        var updateCommand = new UpdateOrganisationCommand(TestOrganisation.Id, TestOrganisation);
        var updateHandler = new UpdateOrganisationCommandHandler(MockApplicationDbContext, updateLogger.Object, MockMediatR.Object, Mapper);

        //Act
        var result = await updateHandler.Handle(updateCommand, new CancellationToken());

        //Assert
        result.Should().NotBeNull();
        result.Should().Be(TestOrganisation.Id);
        
        var linkContacts = MockApplicationDbContext.LinkContacts.Where(lc => lc.LinkId == serviceAtLocationDto.Id).ToList();

        linkContacts.Should().HaveCount(2);
        linkContacts.ElementAt(0).Contact.Should().NotBeNull();
        linkContacts.ElementAt(1).Contact.Should().NotBeNull();

        linkContacts.ElementAt(0).Contact!.Id.Should().Be(existingContactDto.Id);
        linkContacts.ElementAt(1).Contact!.Id.Should().Be(existingContactDto.Id);
    }
}