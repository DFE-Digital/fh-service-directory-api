//using AutoFixture;
//using fh_service_directory_api.core.OpenReferralOrganisationAggregate.Entities;
//using Xunit;

//namespace FamilyHubs.ServiceDirectoryApi.IntegrationTests.Persistence.OpenReferralOrganisations;

//public class EfRepositoryAdd : BaseEfRepositoryTestFixture
//{
//    private readonly Fixture _fixture = new Fixture();

//    [Fact]
//    public async Task AddsOrOpenReferralOrganisation()
//    {
//        // Arrange
//        var OpenReferralOrganisation = _fixture.Create<OpenReferralOrganisation>();
//        ArgumentNullException.ThrowIfNull(OpenReferralOrganisation, nameof(OpenReferralOrganisation));

//        var repository = GetRepository<OpenReferralOrganisation>();
//        ArgumentNullException.ThrowIfNull(repository, nameof(repository));

//        // Act
//        await repository.AddAsync(OpenReferralOrganisation);

//        var addedOpenReferralOrganisation = await repository.GetByIdAsync(OpenReferralOrganisation.Id);
//        ArgumentNullException.ThrowIfNull(addedOpenReferralOrganisation, nameof(addedOpenReferralOrganisation));

//        await repository.SaveChangesAsync();

//        // Assert
//        Assert.Equal(OpenReferralOrganisation, addedOpenReferralOrganisation);
//        Assert.True(!string.IsNullOrEmpty(addedOpenReferralOrganisation.Id));
//    }
//}
