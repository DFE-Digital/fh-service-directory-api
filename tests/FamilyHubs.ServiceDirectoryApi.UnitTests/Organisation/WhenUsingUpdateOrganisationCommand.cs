using AutoMapper;
using FamilyHubs.ServiceDirectory.Api.Commands.CreateOrganisation;
using FamilyHubs.ServiceDirectory.Api.Commands.CreateService;
using FamilyHubs.ServiceDirectory.Api.Commands.UpdateOrganisation;
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
        result.Should().NotBe(0);
        result.Should().Be(TestOrganisation.Id);
    }

    [Fact]
    public async Task ThenUpdateOrganisationWithNewService()
    {
        //Arrange
        CreateOrganisation();

        TestOrganisation.Services.Clear();

        var updateLogger = new Mock<ILogger<UpdateOrganisationCommandHandler>>();

        var updateTestOrganisation = TestDataProvider.GetTestCountyCouncilDto();
        var newService = TestDataProvider.GetTestCountyCouncilServicesDto2(updateTestOrganisation.Id);
        updateTestOrganisation.Services = new List<ServiceDto>
        {
             newService
        };

        var updateCommand = new UpdateOrganisationCommand(updateTestOrganisation.Id, updateTestOrganisation);
        var updateHandler = new UpdateOrganisationCommandHandler(MockApplicationDbContext, updateLogger.Object, MockMediatR.Object, Mapper);

        //Act
        var result = await updateHandler.Handle(updateCommand, new CancellationToken());

        //Assert
        result.Should().NotBe(0);
        result.Should().Be(TestOrganisation.Id);
        var actualServices = MockApplicationDbContext.Services
            .Where(s => s.OrganisationId == updateTestOrganisation.Id).ToList();
        actualServices.Should().NotBeNull();
        actualServices.Count.Should().Be(2);
        actualServices.SingleOrDefault(s => s.ServiceOwnerReferenceId == newService.ServiceOwnerReferenceId).Should().NotBeNull();
    }

    [Fact]
    public async Task ThenUpdateOrganisationWithUpdatedService()
    {
        //Arrange
        CreateOrganisation();

        var updateLogger = new Mock<ILogger<UpdateOrganisationCommandHandler>>();

        var updateTestOrganisation = TestDataProvider.GetTestCountyCouncilDto();
        
        var serviceDto = updateTestOrganisation.Services.ElementAt(0);

        if (serviceDto is null) throw new NullReferenceException("Service Dto is null");
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

        var updateCommand = new UpdateOrganisationCommand(updateTestOrganisation.Id, updateTestOrganisation);
        var updateHandler = new UpdateOrganisationCommandHandler(MockApplicationDbContext, updateLogger.Object, MockMediatR.Object, Mapper);

        //Act
        var result = await updateHandler.Handle(updateCommand, new CancellationToken());

        //Assert
        result.Should().NotBe(0);
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
    public async Task ThenUpdateOrganisationWithUpdatedServiceAndExistingContact()
    {
        //Arrange
        CreateOrganisation();

        var updateLogger = new Mock<ILogger<UpdateOrganisationCommandHandler>>();

        var serviceDto = TestOrganisation.Services.ElementAt(0);

        var existingContactDto = TestOrganisation.Services.ElementAt(0).Contacts.ElementAt(0);
        //trying to add same contact twice
        serviceDto.Contacts.Add(existingContactDto);

        var updateCommand = new UpdateOrganisationCommand(TestOrganisation.Id, TestOrganisation);
        var updateHandler = new UpdateOrganisationCommandHandler(MockApplicationDbContext, updateLogger.Object, MockMediatR.Object, Mapper);

        //Act
        var result = await updateHandler.Handle(updateCommand, new CancellationToken());

        //Assert
        result.Should().NotBe(0);
        result.Should().Be(TestOrganisation.Id);
        
        var actualServices = MockApplicationDbContext.Services.Where(s => s.Id == serviceDto.Id).ToList();
        actualServices.Should().NotBeNull();
        actualServices.Count.Should().Be(1);
        
        var service = actualServices.SingleOrDefault(s => s.Id == serviceDto.Id);

        service.Should().NotBeNull();
        var linkContacts = MockApplicationDbContext.LinkContacts.Where(lc => lc.LinkId == serviceDto.Id).ToList();

        linkContacts.Should().HaveCount(1);
        linkContacts.ElementAt(0).Should().NotBeNull();
        linkContacts.ElementAt(0).ContactId.Should().Be(existingContactDto.Id);
    }

    [Fact]
    public async Task ThenUpdateOrganisationWithUpdatedServiceAndExistingContactForServiceAtLocation()
    {
        //Arrange
        CreateOrganisation();

        var updateLogger = new Mock<ILogger<UpdateOrganisationCommandHandler>>();

        var serviceDto = TestOrganisation.Services.ElementAt(0);
        var serviceAtLocationDto = serviceDto.Locations.ElementAt(0);
        var existingContactDto = serviceAtLocationDto.Contacts.ElementAt(0);

        serviceAtLocationDto.Contacts.Add(existingContactDto);

        var updateCommand = new UpdateOrganisationCommand(TestOrganisation.Id, TestOrganisation);
        var updateHandler = new UpdateOrganisationCommandHandler(MockApplicationDbContext, updateLogger.Object, MockMediatR.Object, Mapper);

        //Act
        var result = await updateHandler.Handle(updateCommand, new CancellationToken());

        //Assert
        result.Should().NotBe(0);
        result.Should().Be(TestOrganisation.Id);
        
        var linkContacts = MockApplicationDbContext.LinkContacts.Where(lc => lc.LinkId == serviceAtLocationDto.Id).ToList();

        linkContacts.Should().HaveCount(1);
        linkContacts.ElementAt(0).Should().NotBeNull();
        linkContacts.ElementAt(0).ContactId.Should().Be(existingContactDto.Id);
    }
}