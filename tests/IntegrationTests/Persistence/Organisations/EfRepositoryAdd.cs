using AutoFixture;
using fh_service_directory_api.core.Aggregates.Organisations.Entities;
using Xunit;

namespace IntegrationTests.Persistence.Organisations;

public class EfRepositoryAdd : BaseEfRepositoryTestFixture
{
    private readonly Fixture _fixture = new Fixture();

    [Fact]
    public async Task AddsOrOrganisation()
    {
        // Arrange
        var organisation = _fixture.Create<Organisation>();
        ArgumentNullException.ThrowIfNull(organisation, nameof(organisation));

        var repository = GetRepository<Organisation>();
        ArgumentNullException.ThrowIfNull(repository, nameof(repository));

        // Act
        await repository.AddAsync(organisation);

        var addedOrganisation = await repository.GetByIdAsync(organisation.Id);
        ArgumentNullException.ThrowIfNull(addedOrganisation, nameof(addedOrganisation));

        await repository.SaveChangesAsync();

        // Assert
        Assert.Equal(organisation, addedOrganisation);
        Assert.True(!string.IsNullOrEmpty(addedOrganisation.Id));
    }
}
