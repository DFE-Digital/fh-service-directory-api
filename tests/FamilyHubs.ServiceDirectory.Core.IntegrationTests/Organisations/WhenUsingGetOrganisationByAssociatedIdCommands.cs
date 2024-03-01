using FamilyHubs.ServiceDirectory.Core.Queries.Organisations.GetOrganisationsByAssociatedId;
using FluentAssertions;

namespace FamilyHubs.ServiceDirectory.Core.IntegrationTests.Organisations;

public class WhenUsingGetOrganisationByAssociatedIdCommands : DataIntegrationTestBase
{
    [Fact]
    public async Task ThenGetOrganisationById()
    {
        //Arrange
        await CreateOrganisationDetails();
        var parent = TestDbContext.Organisations.First();
        var child = CreateChildOrganisation(parent);
        TestDbContext.Organisations.Add(child);
        await TestDbContext.SaveChangesAsync();

        var getCommand = new GetOrganisationsByAssociatedIdCommand(parent.Id );
        var getHandler = new GetOrganisationsByAssociatedIdCommandHandler(TestDbContext, Mapper);

        //Act
        var result = await getHandler.Handle(getCommand, new CancellationToken());

        //Assert
        result.Should().NotBeNull();
        Assert.True(result.Count(x => x.Id == parent.Id) == 1);
        Assert.True(result.Count(x => x.Id == child.Id) == 1);
        Assert.True(result.Count == 2);
    }

    [Fact]
    public async Task ThenGetOrganisationById_ShouldReturnEmptyListIfNotFound()
    {
        //Arrange
        await CreateOrganisationDetails();

        var getCommand = new GetOrganisationsByAssociatedIdCommand(-1);
        var getHandler = new GetOrganisationsByAssociatedIdCommandHandler(TestDbContext, Mapper);

        //Act
        var result = await getHandler.Handle(getCommand, new CancellationToken());

        //Assert
        result.Should().NotBeNull();
        Assert.False(result.Any());
    }

}