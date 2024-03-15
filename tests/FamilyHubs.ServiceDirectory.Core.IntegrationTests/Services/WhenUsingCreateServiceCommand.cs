using FamilyHubs.ServiceDirectory.Core.Commands.Organisations.CreateOrganisation;
using FamilyHubs.ServiceDirectory.Core.Commands.Services.CreateService;
using FamilyHubs.ServiceDirectory.Core.Exceptions;
using FamilyHubs.ServiceDirectory.Shared.Dto;
using FluentAssertions;
using FluentAssertions.Equivalency;
using Microsoft.Extensions.Logging;
using Moq;

namespace FamilyHubs.ServiceDirectory.Core.IntegrationTests.Services;

public class WhenUsingCreateServiceCommand : DataIntegrationTestBase
{
    [Fact]
    public async Task ThenCreateService()
    {
        //Arrange
        await CreateOrganisation();
        var newService = TestDataProvider.GetTestCountyCouncilServicesChangeDto2(Mapper, TestOrganisationWithoutAnyServices.Id);

        var command = new CreateServiceCommand(newService);

        var handler = new CreateServiceCommandHandler(TestDbContext, Mapper, new Mock<ILogger<CreateServiceCommandHandler>>().Object);

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

    //todo: need to handle the generic case of having the same entity in the object twice
    //[Fact]
    //public async Task ThenCreateServiceWithSameContactInGraphTwice()
    //{
    //    //Arrange
    //    await CreateOrganisation();
    //    var newService = TestDataProvider.GetTestCountyCouncilServicesDto2(TestOrganisationWithoutAnyServices.Id);
    //    var location = newService.Locations.ElementAt(0);

    //    var expected = location.Contacts.ElementAt(0);

    //    expected.Name = "Existing contact already Saved in DB";
    //    expected.Id = await CreateContact(expected);
    //    location.Contacts.Add(expected);

    //    var command = new CreateServiceCommand(newService);
    //    var handler = new CreateServiceCommandHandler(TestDbContext, Mapper, GetLogger<CreateServiceCommandHandler>());

    //    //Act
    //    var serviceId = await handler.Handle(command, new CancellationToken());

    //    //Assert
    //    serviceId.Should().NotBe(0);

    //    var actualService = TestDbContext.Services.SingleOrDefault(s => s.Name == newService.Name);
    //    actualService.Should().NotBeNull();
    //    actualService!.Locations.First().Contacts.Count.Should().Be(2);

    //    //var actualEntity = TestDbContext.Locations.SingleOrDefault(s => s.Name == expected.Name);
    //    //actualEntity.Should().NotBeNull();
    //    //actualEntity.Should().BeEquivalentTo(expected, options =>
    //    //    options.Excluding((IMemberInfo info) => info.Name.Contains("Id"))
    //    //        .Excluding((IMemberInfo info) => info.Name.Contains("Distance")));
    //}

    //[Fact]
    //public async Task ThenCreateServiceUpdateExistingLocationWithSameLocationInGraphTwice()
    //{
    //    //Arrange
    //    await CreateOrganisation();
    //    var newService = TestDataProvider.GetTestCountyCouncilServicesChangeDto2(Mapper, TestOrganisationWithoutAnyServices.Id);

    //    var expected = newService.Locations.ElementAt(0);

    //    expected.Name = "Existing Location already Saved in DB";
    //    expected.Id = await CreateLocation(expected);
    //    newService.Locations.Add(expected);

    //    var command = new CreateServiceCommand(newService);
    //    var handler = new CreateServiceCommandHandler(TestDbContext, Mapper, GetLogger<CreateServiceCommandHandler>());

    //    //Act
    //    var serviceId = await handler.Handle(command, new CancellationToken());

    //    //Assert
    //    serviceId.Should().NotBe(0);

    //    var actualService = TestDbContext.Services.SingleOrDefault(s => s.Name == newService.Name);
    //    actualService.Should().NotBeNull();
    //    actualService!.Locations.Count.Should().Be(2);

    //    var actualEntity = TestDbContext.Locations.SingleOrDefault(s => s.Name == expected.Name);
    //    actualEntity.Should().NotBeNull();
    //    actualEntity.Should().BeEquivalentTo(expected, options =>
    //        options.Excluding((IMemberInfo info) => info.Name.Contains("Id"))
    //            .Excluding((IMemberInfo info) => info.Name.Contains("Distance")));
    //}

    //[Fact]
    //public async Task ThenCreateServiceAndAttachExistingTaxonomy()
    //{
    //    //Arrange
    //    await CreateOrganisation();
    //    var newService = TestDataProvider.GetTestCountyCouncilServicesDto2(TestOrganisationWithoutAnyServices.Id);

    //    var expected = new TaxonomyDto
    //    {
    //        Name = "Existing Taxonomy already Saved in DB"
    //    };
    //    expected.Id = await CreateTaxonomy(expected);

    //    newService.Taxonomies.Add(expected);

    //    var createServiceCommand = new CreateServiceCommand(newService);
    //    var handler = new CreateServiceCommandHandler(TestDbContext, Mapper, GetLogger<CreateServiceCommandHandler>());

    //    //Act
    //    var organisationId = await handler.Handle(createServiceCommand, new CancellationToken());

    //    //Assert
    //    organisationId.Should().NotBe(0);

    //    var actualService = TestDbContext.Services.SingleOrDefault(s => s.Name == newService.Name);
    //    actualService.Should().NotBeNull();
    //    actualService!.Taxonomies.Count.Should().Be(5);

    //    var actualEntity = TestDbContext.Taxonomies.SingleOrDefault(s => s.Name == expected.Name);
    //    actualEntity.Should().NotBeNull();
    //    actualEntity.Should().BeEquivalentTo(expected);
    //}

    //[Fact]
    //public async Task ThenCreateDuplicateService_ShouldThrowException()
    //{
    //    //Arrange
    //    await CreateOrganisationDetails();

    //    var command = new CreateOrganisationCommand(TestOrganisation);
    //    var handler = new CreateOrganisationCommandHandler(TestDbContext, Mapper, GetLogger<CreateOrganisationCommandHandler>());

    //    // Act 
    //    // Assert
    //    await Assert.ThrowsAsync<AlreadyExistsException>(() => handler.Handle(command, new CancellationToken()));
    //}
}