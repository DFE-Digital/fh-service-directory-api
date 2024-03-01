using Ardalis.GuardClauses;
using FamilyHubs.ServiceDirectory.Core.Queries.Organisations.GetOrganisationAdminAreaById;
using FamilyHubs.ServiceDirectory.Core.Queries.Organisations.GetOrganisationById;
using FamilyHubs.ServiceDirectory.Core.Queries.Organisations.ListOrganisations;
using FamilyHubs.ServiceDirectory.Shared.Dto;
using FluentAssertions;

namespace FamilyHubs.ServiceDirectory.Core.IntegrationTests.Organisations;

public class WhenUsingGetLocationCommands : DataIntegrationTestBase
{
    [Fact]
    public async Task ThenGetOrganisationById()
    {
        //Arrange
        await CreateOrganisationDetails();

        var getCommand = new GetOrganisationByIdCommand { Id = TestOrganisation.Id };
        var getHandler = new GetOrganisationByIdHandler(TestDbContext, Mapper);

        //Act
        var result = await getHandler.Handle(getCommand, new CancellationToken());

        //Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(TestOrganisation, opts => opts.Excluding(si => si.AdminAreaCode));
    }

    [Fact]
    public async Task ThenGetOrganisationById_ShouldThrowExceptionWhenIdDoesNotExist()
    {
        //Arrange
        var getCommand = new GetOrganisationByIdCommand { Id = Random.Shared.Next() };
        var getHandler = new GetOrganisationByIdHandler(TestDbContext, Mapper);

        // Act 
        // Assert
        await Assert.ThrowsAsync<NotFoundException>(() => getHandler.Handle(getCommand, new CancellationToken()));
    }

    [Fact]
    public async Task ThenListOrganisations()
    {
        //Arrange
        await CreateOrganisationDetails();

        var getCommand = new ListOrganisationsCommand();
        var getHandler = new ListOrganisationCommandHandler(TestDbContext, Mapper);

        //Act
        var result = await getHandler.Handle(getCommand, new CancellationToken());

        //Assert
        result.Should().NotBeNull();
        result.Last().Should().BeEquivalentTo((OrganisationDto)TestOrganisation);
    }

    [Fact]
    public async Task ThenGetAdminByOrganisationId()
    {
        //Arrange
        await CreateOrganisationDetails();

        var getCommand = new GetOrganisationAdminAreaByIdCommand { OrganisationId = TestOrganisation.Id };
        var getHandler = new GetOrganisationAdminAreaByIdCommandHandler(TestDbContext);
        TestOrganisation.Logo = "";

        //Act
        var result = await getHandler.Handle(getCommand, new CancellationToken());

        //Assert
        result.Should().NotBeNull();
        result.Should().Be("XTEST");
    }
}