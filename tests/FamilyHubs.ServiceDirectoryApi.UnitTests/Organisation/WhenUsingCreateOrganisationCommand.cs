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
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace FamilyHubs.ServiceDirectoryApi.UnitTests.Organisation;

public class WhenUsingCreateOrganisationCommand : BaseCreateDbUnitTest
{
    public WhenUsingCreateOrganisationCommand()
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
    public async Task ThenCreateOrganisation()
    {
        //Arrange
        var createOrganisationCommand = new CreateOrganisationCommand(TestOrganisation);
        var handler = new CreateOrganisationCommandHandler(MockApplicationDbContext, Mapper, MockMediatR.Object, GetLogger<CreateOrganisationCommandHandler>());

        //Act
        var result = await handler.Handle(createOrganisationCommand, new CancellationToken());

        //Assert
        result.Should().NotBe(0);
        result.Should().Be(TestOrganisation.Id);
    }

    [Fact]
    public async Task ThenCreateOrganisationWithExistingServiceLinkContact()
    {
        //Arrange
        var contact = Mapper.Map<Contact>(TestOrganisation.Services.ElementAt(0).Contacts.ElementAt(0));
        MockApplicationDbContext.Contacts.Add(contact);
        await MockApplicationDbContext.SaveChangesAsync();

        var createOrganisationCommand = new CreateOrganisationCommand(TestOrganisation);
        var handler = new CreateOrganisationCommandHandler(MockApplicationDbContext, Mapper, MockMediatR.Object, GetLogger<CreateOrganisationCommandHandler>());

        //Act
        MockApplicationDbContext.Contacts
            .Where(c => c.Id == contact.Id)
            .ToList().Count.Should().Be(1);

        var result = await handler.Handle(createOrganisationCommand, new CancellationToken());

        //Assert
        result.Should().NotBe(0);
        result.Should().Be(TestOrganisation.Id);

        MockApplicationDbContext.Contacts
            .Where(c => c.Id == contact.Id)
            .ToList().Count.Should().Be(1);

        var linkContacts = MockApplicationDbContext.LinkContacts.SingleOrDefault(lc => lc.LinkId == TestOrganisation.Services.ElementAt(0).Id);

        linkContacts.Should().NotBeNull();
        linkContacts!.ContactId.Should().Be(contact.Id);
    }

    [Fact]
    public async Task ThenCreateOrganisationWithExistingContactForServiceAtLocation()
    {
        //Arrange
        var serviceAtLocation = TestOrganisation.Services.ElementAt(0).Locations.ElementAt(0);
        var contact = Mapper.Map<Contact>(serviceAtLocation.Contacts.ElementAt(0));
        MockApplicationDbContext.Contacts.Add(contact);
        await MockApplicationDbContext.SaveChangesAsync();

        var createOrganisationCommand = new CreateOrganisationCommand(TestOrganisation);
        var handler = new CreateOrganisationCommandHandler(MockApplicationDbContext, Mapper, MockMediatR.Object, GetLogger<CreateOrganisationCommandHandler>());

        //Act
        MockApplicationDbContext.Contacts
            .Where(c => c.Id == contact.Id)
            .ToList().Count.Should().Be(1);

        var result = await handler.Handle(createOrganisationCommand, new CancellationToken());

        //Assert
        result.Should().NotBe(0);
        result.Should().Be(TestOrganisation.Id);

        MockApplicationDbContext.Contacts
            .Where(c => c.Id == contact.Id)
            .ToList().Count.Should().Be(1);

        var linkContacts = MockApplicationDbContext.LinkContacts.SingleOrDefault(lc => lc.LinkId == serviceAtLocation.Id);

        linkContacts.Should().NotBeNull();
        linkContacts!.ContactId.Should().Be(contact.Id);
    }

    [Fact]
    public async Task ThenCreateRelatedOrganisation()
    {
        //Arrange
        CreateOrganisation();

        var relatedOrganisation = new OrganisationWithServicesDto
        {
            OrganisationType = OrganisationType.VCFS,
            Name = "Related VCS",
            Description = "Related VCS",
            Logo = null,
            Uri = new Uri("https://www.relatedvcs.gov.uk/").ToString(),
            Url = "https://www.related.gov.uk/",
            AdminAreaCode = "XTEST"
        };

        var command = new CreateOrganisationCommand(relatedOrganisation);
        var handler = new CreateOrganisationCommandHandler(MockApplicationDbContext, Mapper, MockMediatR.Object, GetLogger<CreateOrganisationCommandHandler>());

        //Act
        var result = await handler.Handle(command, new CancellationToken());

        //Assert
        result.Should().NotBe(0);
        result.Should().Be(relatedOrganisation.Id);
    }

    [Fact]
    public async Task ThenCreateAnotherOrganisation()
    {
        //Arrange
        CreateOrganisation();
        var anotherOrganisation = TestDataProvider.GetTestCountyCouncilDto2();
        var command = new CreateOrganisationCommand(anotherOrganisation);
        var handler = new CreateOrganisationCommandHandler(MockApplicationDbContext, Mapper, MockMediatR.Object, GetLogger<CreateOrganisationCommandHandler>());

        //Act
        var result = await handler.Handle(command, new CancellationToken());

        //Assert
        result.Should().NotBe(0);
        result.Should().Be(anotherOrganisation.Id);
    }

    [Fact]
    public async Task ThenCreateDuplicateOrganisation_ShouldThrowException()
    {
        //Arrange
        CreateOrganisation();

        var command = new CreateOrganisationCommand(TestOrganisation);
        var handler = new CreateOrganisationCommandHandler(MockApplicationDbContext, Mapper, MockMediatR.Object, GetLogger<CreateOrganisationCommandHandler>());

        // Act 
        // Assert
        await Assert.ThrowsAsync<ArgumentException>(() => handler.Handle(command, new CancellationToken()));
    }
}