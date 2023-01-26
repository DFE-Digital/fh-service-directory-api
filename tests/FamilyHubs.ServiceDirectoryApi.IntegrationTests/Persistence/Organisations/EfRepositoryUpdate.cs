using AutoFixture;
using FamilyHubs.ServiceDirectory.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace FamilyHubs.ServiceDirectoryApi.IntegrationTests.Persistence.Organisations;

public class EfRepositoryUpdate : BaseEfRepositoryTestFixture
{
    private readonly Fixture _fixture = new Fixture();

    [Fact]
    public async Task UpdatesOrganisationAfterAddingIt()
    {
        // Arrange
        var testData = new TestData();
        var organisation = testData.GetTestCountyCouncil();
        ArgumentNullException.ThrowIfNull(organisation, nameof(organisation));

        var repository = GetRepository<Organisation>();
        ArgumentNullException.ThrowIfNull(repository, nameof(repository));
        await repository.AddAsync(organisation);

        DbContext.Entry(organisation).State = EntityState.Detached;             // detach the item so we get a different instance

        var addedOrganisation = await repository.GetByIdAsync(organisation.Id); // fetch the Organisation and update its name
        if (addedOrganisation == null)
        {
            Assert.NotNull(addedOrganisation);
            return;
        }

        Assert.NotSame(organisation, addedOrganisation);

        // Act
        addedOrganisation.Name = "Brum Council";
        await repository.UpdateAsync(addedOrganisation);
        var updatedOrganisation = await repository.GetByIdAsync(addedOrganisation.Id);

        // Assert
        Assert.NotNull(updatedOrganisation);
        Assert.NotEqual(organisation.Name, updatedOrganisation?.Name);
        Assert.Equal(organisation.Id, updatedOrganisation?.Id);
    }
}
