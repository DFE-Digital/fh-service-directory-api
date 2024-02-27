using FamilyHubs.ServiceDirectory.Core.Commands.Organisations.CreateOrganisation;
using FamilyHubs.ServiceDirectory.Core.Exceptions;
using FamilyHubs.ServiceDirectory.Shared.Dto;
using FamilyHubs.ServiceDirectory.Shared.Enums;
using FluentAssertions;
using FluentAssertions.Equivalency;

namespace FamilyHubs.ServiceDirectory.Core.IntegrationTests.Organisations;

public class WhenUsingCreateLocationCommand : DataIntegrationTestBase
{
    [Fact]
    public async Task ThenCreateOrganisation()
    {
        //Arrange
        var createOrganisationCommand = new CreateOrganisationCommand(TestOrganisation);
        var handler = new CreateOrganisationCommandHandler(TestDbContext, Mapper, GetLogger<CreateOrganisationCommandHandler>());

        //Act
        var result = await handler.Handle(createOrganisationCommand, new CancellationToken());

        //Assert
        result.Should().NotBe(0);
        var actualOrganisation = TestDbContext.Organisations.SingleOrDefault(o => o.Id == result);
        actualOrganisation.Should().NotBeNull();
        actualOrganisation.Should().BeEquivalentTo(TestOrganisation, options => 
            options.Excluding((IMemberInfo info) => info.Name.Contains("Id"))
                .Excluding((IMemberInfo info) => info.Name.Contains("Distance")));
    }

    [Fact]
    public async Task ThenCreateOrganisationWithoutAnyServices()
    {
        var service = TestOrganisation.Services.ElementAt(0);
        TestOrganisation.Services.Clear();
        //Arrange
        var createOrganisationCommand = new CreateOrganisationCommand(TestOrganisation);
        var handler = new CreateOrganisationCommandHandler(TestDbContext, Mapper, GetLogger<CreateOrganisationCommandHandler>());

        //Act
        var result = await handler.Handle(createOrganisationCommand, new CancellationToken());

        //Assert
        result.Should().NotBe(0);

        var actualOrganisation = TestDbContext.Organisations.SingleOrDefault(o => o.Id == result);
        actualOrganisation.Should().NotBeNull();
        actualOrganisation!.Services.Count.Should().Be(0);
        actualOrganisation.Should().BeEquivalentTo(TestOrganisation, options => 
            options.Excluding((IMemberInfo info) => info.Name.Contains("Id"))
                .Excluding((IMemberInfo info) => info.Name.Contains("Distance")));

        var unexpectedService = TestDbContext.Services.SingleOrDefault(s => s.Name == service.Name);
        unexpectedService.Should().BeNull();
    }

    [Fact]
    public async Task ThenCreateOrganisationWithNewServiceAndAttachExistingLocation()
    {
        //Arrange
        var expected = TestDataProvider.GetTestCountyCouncilDto2().Services.ElementAt(0).Locations.ElementAt(0);
        expected.Name = "Existing Location already Saved in DB";
        expected.Id = await CreateLocation(expected);

        var service = TestOrganisation.Services.ElementAt(0);
        service.Locations.Add(expected);

        var createOrganisationCommand = new CreateOrganisationCommand(TestOrganisation);
        var handler = new CreateOrganisationCommandHandler(TestDbContext, Mapper, GetLogger<CreateOrganisationCommandHandler>());

        //Act
        var organisationId = await handler.Handle(createOrganisationCommand, new CancellationToken());
        
        //Assert
        organisationId.Should().NotBe(0);

        var actualService = TestDbContext.Services.SingleOrDefault(s => s.Name == service.Name);
        actualService.Should().NotBeNull();
        actualService!.Locations.Count.Should().Be(2);

        var actualEntity = TestDbContext.Locations.SingleOrDefault(s => s.Name == expected.Name);
        actualEntity.Should().NotBeNull();
        actualEntity.Should().BeEquivalentTo(expected, options => 
            options.Excluding((IMemberInfo info) => info.Name.Contains("Id"))
                .Excluding((IMemberInfo info) => info.Name.Contains("Distance")));
    }

    [Fact]
    public async Task ThenCreateOrganisationWithNewServiceAndAttachExistingTaxonomy()
    {
        //Arrange
        var expected = new TaxonomyDto
        {
            Name = "Existing Taxonomy already Saved in DB"
        };
        expected.Id = await CreateTaxonomy(expected);

        var service = TestOrganisation.Services.ElementAt(0);
        service.Taxonomies.Add(expected);

        var createOrganisationCommand = new CreateOrganisationCommand(TestOrganisation);
        var handler = new CreateOrganisationCommandHandler(TestDbContext, Mapper, GetLogger<CreateOrganisationCommandHandler>());

        //Act
        var organisationId = await handler.Handle(createOrganisationCommand, new CancellationToken());
        
        //Assert
        organisationId.Should().NotBe(0);

        var actualService = TestDbContext.Services.SingleOrDefault(s => s.Name == service.Name);
        actualService.Should().NotBeNull();
        actualService!.Taxonomies.Count.Should().Be(5);

        var actualEntity = TestDbContext.Taxonomies.SingleOrDefault(s => s.Name == expected.Name);
        actualEntity.Should().NotBeNull();
        actualEntity.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task ThenCreateRelatedOrganisationWithCorrectAdminAreaCode()
    {
        //Arrange
        await CreateOrganisation();

        var relatedOrganisation = new OrganisationDetailsDto
        {
            OrganisationType = OrganisationType.VCFS,
            Name = "Related VCS",
            Description = "Related VCS",
            Logo = null,
            Uri = new Uri("https://www.relatedvcs.gov.uk/").ToString(),
            Url = "https://www.related.gov.uk/",
            AdminAreaCode = "XTEST",
            AssociatedOrganisationId = 1
        };

        var command = new CreateOrganisationCommand(relatedOrganisation);
        var handler = new CreateOrganisationCommandHandler(TestDbContext, Mapper, GetLogger<CreateOrganisationCommandHandler>());

        //Act
        var result = await handler.Handle(command, new CancellationToken());

        //Assert
        result.Should().NotBe(0);

        var actualOrganisation = TestDbContext.Organisations.SingleOrDefault(c => c.AssociatedOrganisationId == 1);
        actualOrganisation.Should().NotBeNull();
        actualOrganisation!.AdminAreaCode.Should().Be("E06000023");
    }

    [Fact]
    public async Task ThenCreateDuplicateOrganisation_ShouldThrowException()
    {
        //Arrange
        await CreateOrganisation();

        var command = new CreateOrganisationCommand(TestOrganisation);
        var handler = new CreateOrganisationCommandHandler(TestDbContext, Mapper, GetLogger<CreateOrganisationCommandHandler>());

        // Act 
        // Assert
        await Assert.ThrowsAsync<AlreadyExistsException>(() => handler.Handle(command, new CancellationToken()));
    }
}