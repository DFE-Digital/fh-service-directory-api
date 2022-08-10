using AutoFixture;
using fh_service_directory_api.core.Aggregates.Organisations.Entities;
using Xunit;

namespace IntegrationTests.Persistence.Organisations;

public class EfRepositoryDelete : BaseEfRepositoryTestFixture
{
    private readonly Fixture _fixture = new Fixture();

    [Fact]
    public async Task DeletesOrOganisationAfterAddingIt()
    {
        // Arrange
        var newOrganisation = _fixture.Create<Organisation>();
        ArgumentNullException.ThrowIfNull(newOrganisation, nameof(newOrganisation));
        var organisationId = newOrganisation.Id;
        var repository = GetRepository<Organisation>();
        ArgumentNullException.ThrowIfNull(repository, nameof(repository));
        await repository.AddAsync(newOrganisation);

        // Act
        await repository.DeleteAsync(newOrganisation);

        // Assert
        Assert.DoesNotContain(await repository.ListAsync(),
            newOrganisation => newOrganisation.Id == organisationId);
    }
}
