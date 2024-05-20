using FamilyHubs.ServiceDirectory.Core.Queries.Organisations.ListOrganisations;
using FamilyHubs.ServiceDirectory.Shared.Dto;
using FluentAssertions;
using FamilyHubs.ServiceDirectory.Shared.Enums;

namespace FamilyHubs.ServiceDirectory.Core.IntegrationTests.Organisations;

public class WhenUsingListOrganisationsCommand : DataIntegrationTestBase
{
    [Fact]
    public async Task ThenListOrganisations()
    {
        //Arrange
        await CreateOrganisationDetails();

        var getCommand = new ListOrganisationsCommand(new List<long>(), null);
        var getHandler = new ListOrganisationCommandHandler(TestDbContext, Mapper);

        //Act
        var result = await getHandler.Handle(getCommand, new CancellationToken());

        //Assert
        result.Should().NotBeNull();
        result.Last().Should().BeEquivalentTo((OrganisationDto)TestOrganisation);
    }

    [Fact]
    public async Task ThenListOrganisationsFilteredByLaOrganisationType()
    {
        //Arrange
        var getCommand = new ListOrganisationsCommand(new List<long>(), null, OrganisationType.LA);
        var getHandler = new ListOrganisationCommandHandler(TestDbContext, Mapper);

        //Act
        var result = await getHandler.Handle(getCommand, new CancellationToken());

        //Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(6);
    }

    [Fact]
    public async Task ThenListOrganisationsFilteredByVcsOrganisationType()
    {
        //Arrange
        TestOrganisation.OrganisationType = OrganisationType.VCFS;
        await CreateOrganisationDetails();

        var getCommand = new ListOrganisationsCommand(new List<long>(), null, OrganisationType.VCFS);
        var getHandler = new ListOrganisationCommandHandler(TestDbContext, Mapper);

        //Act
        var result = await getHandler.Handle(getCommand, new CancellationToken());

        //Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(1);
    }
}