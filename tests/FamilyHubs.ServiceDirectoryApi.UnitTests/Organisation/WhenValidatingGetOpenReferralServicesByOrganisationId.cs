using fh_service_directory_api.api.Queries.GetOpenReferralServicesByOrganisation;
using FluentAssertions;

namespace FamilyHubs.ServiceDirectoryApi.UnitTests.Organisation;

public class WhenValidatingGetOpenReferralServicesByOrganisationId
{
    [Fact]
    public void ThenShouldNotErrorForGetOpenReferralServicesByOrganisationIdWhenModelIsValid()
    {
        //Arrange
        var validator = new GetOpenReferralServicesByOrganisationIdCommandValidator();
        var testModel = new GetOpenReferralServicesByOrganisationIdCommand("56e62852-1b0b-40e5-ac97-54a67ea957dc");

        //Act
        var result = validator.Validate(testModel);

        //Assert
        result.Errors.Any().Should().BeFalse();
    }
}
