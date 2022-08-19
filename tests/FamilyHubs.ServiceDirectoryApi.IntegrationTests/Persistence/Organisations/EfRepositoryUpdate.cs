using AutoFixture;
using fh_service_directory_api.core.OrganisationAggregate.Entities;
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
        var organisation = _fixture.Create<Organisation>();
        ArgumentNullException.ThrowIfNull(organisation, nameof(organisation));

        var repository = GetRepository<Organisation>();
        ArgumentNullException.ThrowIfNull(repository, nameof(repository));
        await repository.AddAsync(organisation);

        DbContext.Entry(organisation).State = EntityState.Detached;             // detach the item so we get a different instance

        var addedOrganisation = await repository.GetByIdAsync(organisation.Id); // fetch the organisation and update its name
        if (addedOrganisation == null)
        {
            Assert.NotNull(addedOrganisation);
            return;
        }

        Assert.NotSame(organisation, addedOrganisation);

        // Act
        addedOrganisation.UpdateOrganisation
        (
            "Brum Council",
            addedOrganisation.Description,
            addedOrganisation.Logo,
            addedOrganisation.Uri,
            addedOrganisation.Url,
            addedOrganisation.OrganisationContacts
        );
        await repository.UpdateAsync(addedOrganisation);
        var updatedOrganisation = await repository.GetByIdAsync(addedOrganisation.Id);

        // Assert
        Assert.NotNull(updatedOrganisation);
        Assert.NotEqual(organisation.Name, updatedOrganisation?.Name);
        Assert.Equal(organisation.Id, updatedOrganisation?.Id);
    }
}
