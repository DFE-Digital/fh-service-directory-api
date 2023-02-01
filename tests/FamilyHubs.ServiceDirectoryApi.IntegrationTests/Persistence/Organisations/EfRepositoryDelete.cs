using FamilyHubs.ServiceDirectory.Core.Entities;
using Xunit;

namespace FamilyHubs.ServiceDirectoryApi.IntegrationTests.Persistence.Organisations;

public class EfRepositoryDelete : BaseEfRepositoryTestFixture
{
    [Fact]
    public async Task DeletesOrOganisationAfterAddingIt()
    {
        // Arrange
        var testData = new TestData();
        var newOrganisation = testData.GetTestCountyCouncil();
        ArgumentNullException.ThrowIfNull(newOrganisation);
        var organisationId = newOrganisation.Id;
        var repository = GetRepository<Organisation>();
        ArgumentNullException.ThrowIfNull(repository);
        await repository.AddAsync(newOrganisation);

        // Act
        await repository.DeleteAsync(newOrganisation);

        // Assert
        Assert.DoesNotContain(await repository.ListAsync(),
            organisation => organisation.Id == organisationId);
    }
}
