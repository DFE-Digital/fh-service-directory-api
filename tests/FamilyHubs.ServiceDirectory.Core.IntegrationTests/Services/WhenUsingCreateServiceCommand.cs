using FamilyHubs.ServiceDirectory.Core.Commands;
using FamilyHubs.ServiceDirectory.Core.Commands.Organisations.CreateOrganisation;
using FamilyHubs.ServiceDirectory.Core.Commands.Services.CreateService;
using FamilyHubs.ServiceDirectory.Core.Exceptions;
using FamilyHubs.ServiceDirectory.Shared.Dto;
using FluentAssertions;
using FluentAssertions.Equivalency;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;

namespace FamilyHubs.ServiceDirectory.Core.IntegrationTests.Services;

public class WhenUsingCreateServiceCommand : DataIntegrationTestBase
{
    [Fact]
    public async Task ThenCreateService()
    {
        //Arrange
        Mock<ISender> sender = new Mock<ISender>();
        sender.Setup(x => x.Send(It.IsAny<SendEventGridMessageCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync("Done");
        await CreateOrganisationWithoutAnyServices();
        var newService = TestDataProvider.GetTestCountyCouncilServicesDto2(TestOrganisationWithoutAnyServices.Id);

        var command = new CreateServiceCommand(newService);

        var handler = new CreateServiceCommandHandler(TestDbContext, Mapper, sender.Object, new Mock<ILogger<CreateServiceCommandHandler>>().Object);

        //Act
        var result = await handler.Handle(command, new CancellationToken());

        //Assert
        result.Should().NotBe(0);

        var actualService = TestDbContext.Services.SingleOrDefault(s => s.Name == newService.Name);
        actualService.Should().NotBeNull();
        actualService.Should().BeEquivalentTo(newService, options => 
            options.Excluding((IMemberInfo info) => info.Name.Contains("Id"))
                .Excluding((IMemberInfo info) => info.Name.Contains("Distance")));
    }

    [Fact]
    public async Task ThenCreateServiceAndAttachExistingLocation()
    {
        //Arrange
        Mock<ISender> sender = new Mock<ISender>();
        sender.Setup(x => x.Send(It.IsAny<SendEventGridMessageCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync("Done");
        await CreateOrganisationWithoutAnyServices();
        var newService = TestDataProvider.GetTestCountyCouncilServicesDto2(TestOrganisationWithoutAnyServices.Id);

        var expected = newService.Locations.ElementAt(0);
        
        expected.Name = "Existing Location already Saved in DB";
        expected.Id = await CreateLocation(expected);
        newService.Locations.Add(expected);

        var command = new CreateServiceCommand(newService);
        var handler = new CreateServiceCommandHandler(TestDbContext, Mapper, sender.Object, GetLogger<CreateServiceCommandHandler>());

        //Act
        var serviceId = await handler.Handle(command, new CancellationToken());
        
        //Assert
        serviceId.Should().NotBe(0);

        var actualService = TestDbContext.Services.SingleOrDefault(s => s.Name == newService.Name);
        actualService.Should().NotBeNull();
        actualService!.Locations.Count.Should().Be(2);

        var actualEntity = TestDbContext.Locations.SingleOrDefault(s => s.Name == expected.Name);
        actualEntity.Should().NotBeNull();
        actualEntity.Should().BeEquivalentTo(expected, options => 
            options.Excluding((IMemberInfo info) => info.Name.Contains("Id"))
                .Excluding((IMemberInfo info) => info.Name.Contains("Distance")));
    }

    [Fact]
    public async Task ThenCreateServiceAndAttachExistingTaxonomy()
    {
        //Arrange
        Mock<ISender> sender = new Mock<ISender>();
        sender.Setup(x => x.Send(It.IsAny<SendEventGridMessageCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync("Done");
        await CreateOrganisationWithoutAnyServices();
        var newService = TestDataProvider.GetTestCountyCouncilServicesDto2(TestOrganisationWithoutAnyServices.Id);
        
        var expected = new TaxonomyDto
        {
            Name = "Existing Taxonomy already Saved in DB"
        };
        expected.Id = await CreateTaxonomy(expected);

        newService.Taxonomies.Add(expected);

        var createServiceCommand = new CreateServiceCommand(newService);
        var handler = new CreateServiceCommandHandler(TestDbContext, Mapper, sender.Object, GetLogger<CreateServiceCommandHandler>());

        //Act
        var organisationId = await handler.Handle(createServiceCommand, new CancellationToken());
        
        //Assert
        organisationId.Should().NotBe(0);

        var actualService = TestDbContext.Services.SingleOrDefault(s => s.Name == newService.Name);
        actualService.Should().NotBeNull();
        actualService!.Taxonomies.Count.Should().Be(5);

        var actualEntity = TestDbContext.Taxonomies.SingleOrDefault(s => s.Name == expected.Name);
        actualEntity.Should().NotBeNull();
        actualEntity.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task ThenCreateDuplicateService_ShouldThrowException()
    {
        //Arrange
        await CreateOrganisation();

        var command = new CreateOrganisationCommand(TestOrganisation);
        Mock<ISender> mockSender = new Mock<ISender>();
        int sendCallBack = 0;
        mockSender.Setup(x => x.Send(It.IsAny<SendEventGridMessageCommand>(), It.IsAny<CancellationToken>()))
            .Callback(() => sendCallBack++).ReturnsAsync("Done");
        var handler = new CreateOrganisationCommandHandler(TestDbContext, Mapper, mockSender.Object, GetLogger<CreateOrganisationCommandHandler>());

        // Act 
        // Assert
        await Assert.ThrowsAsync<AlreadyExistsException>(() => handler.Handle(command, new CancellationToken()));
    }
}