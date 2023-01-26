using AutoFixture;
using fh_service_directory_api.core.Entities;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace FamilyHubs.ServiceDirectoryApi.IntegrationTests.Persistence.OpenReferralOrganisations;

public class EfRepositoryUpdate : BaseEfRepositoryTestFixture
{
    private readonly Fixture _fixture = new Fixture();

    [Fact]
    public async Task UpdatesOpenReferralOrganisationAfterAddingIt()
    {
        // Arrange
        var openReferralData = new OpenReferralData();
        var OpenReferralOrganisation = openReferralData.GetTestCountyCouncil();
        ArgumentNullException.ThrowIfNull(OpenReferralOrganisation, nameof(OpenReferralOrganisation));

        var repository = GetRepository<OpenReferralOrganisation>();
        ArgumentNullException.ThrowIfNull(repository, nameof(repository));
        await repository.AddAsync(OpenReferralOrganisation);

        DbContext.Entry(OpenReferralOrganisation).State = EntityState.Detached;             // detach the item so we get a different instance

        var addedOpenReferralOrganisation = await repository.GetByIdAsync(OpenReferralOrganisation.Id); // fetch the OpenReferralOrganisation and update its name
        if (addedOpenReferralOrganisation == null)
        {
            Assert.NotNull(addedOpenReferralOrganisation);
            return;
        }

        Assert.NotSame(OpenReferralOrganisation, addedOpenReferralOrganisation);

        // Act
        addedOpenReferralOrganisation.Name = "Brum Council";
        await repository.UpdateAsync(addedOpenReferralOrganisation);
        var updatedOpenReferralOrganisation = await repository.GetByIdAsync(addedOpenReferralOrganisation.Id);

        // Assert
        Assert.NotNull(updatedOpenReferralOrganisation);
        Assert.NotEqual(OpenReferralOrganisation.Name, updatedOpenReferralOrganisation?.Name);
        Assert.Equal(OpenReferralOrganisation.Id, updatedOpenReferralOrganisation?.Id);
    }
}
