using AutoMapper;
using FamilyHubs.ServiceDirectory.Api.Commands.CreateOrganisation;
using FamilyHubs.ServiceDirectory.Api.Commands.CreateService;
using FamilyHubs.ServiceDirectory.Api.Commands.UpdateService;
using FamilyHubs.ServiceDirectory.Core;
using FamilyHubs.ServiceDirectory.Infrastructure.Persistence.Repository;
using FamilyHubs.ServiceDirectory.Shared.Dto;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace FamilyHubs.ServiceDirectoryApi.UnitTests.Organisation;

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
}
