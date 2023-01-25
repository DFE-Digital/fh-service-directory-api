using fh_service_directory_api.core.Entities;
using Xunit;

namespace FamilyHubs.ServiceDirectoryApi.IntegrationTests.Persistence.OpenReferralOrganisations;

public class EfRepositoryDelete : BaseEfRepositoryTestFixture
{
    [Fact]
    public async Task DeletesOrOganisationAfterAddingIt()
    {
        // Arrange
        var openReferralData = new OpenReferralData();
        var newOpenReferralOrganisation = openReferralData.GetTestCountyCouncil();
        ArgumentNullException.ThrowIfNull(newOpenReferralOrganisation, nameof(newOpenReferralOrganisation));
        var OpenReferralOrganisationId = newOpenReferralOrganisation.Id;
        var repository = GetRepository<OpenReferralOrganisation>();
        ArgumentNullException.ThrowIfNull(repository, nameof(repository));
        await repository.AddAsync(newOpenReferralOrganisation);

        // Act
        await repository.DeleteAsync(newOpenReferralOrganisation);

        // Assert
        Assert.DoesNotContain(await repository.ListAsync(),
            newOpenReferralOrganisation => newOpenReferralOrganisation.Id == OpenReferralOrganisationId);
    }
}
