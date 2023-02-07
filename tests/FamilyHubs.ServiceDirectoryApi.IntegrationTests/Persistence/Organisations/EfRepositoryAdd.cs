using AutoFixture;
using FamilyHubs.ServiceDirectory.Core.Entities;
using Xunit;

namespace FamilyHubs.ServiceDirectoryApi.IntegrationTests.Persistence.Organisations;

public class EfRepositoryAdd : BaseEfRepositoryTestFixture
{
    private readonly Fixture _fixture = new Fixture();

    [Fact]
    public async Task AddsOrganisation()
    {
        // Arrange
        var testData = new TestData();
        var organisation = testData.GetTestCountyCouncil();
        ArgumentNullException.ThrowIfNull(organisation);

        var repository = GetRepository<Organisation>();
        ArgumentNullException.ThrowIfNull(repository);

        // Act
        await repository.AddAsync(organisation);

        var addedOrganisation = await repository.GetByIdAsync(organisation.Id);
        ArgumentNullException.ThrowIfNull(addedOrganisation);

        await repository.SaveChangesAsync();

        // Assert
        Assert.Equal(organisation, addedOrganisation);
        Assert.True(!string.IsNullOrEmpty(addedOrganisation.Id));
    }
}
