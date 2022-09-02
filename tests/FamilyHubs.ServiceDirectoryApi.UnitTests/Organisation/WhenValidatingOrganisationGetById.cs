using fh_service_directory_api.api.Queries.GetOpenReferralOrganisationById;
using FluentAssertions;

namespace FamilyHubs.ServiceDirectoryApi.UnitTests.Organisation;

public class WhenValidatingOrganisationGetById
{
    [Fact]
    public void ThenShouldNotErrorWhenModelIsValid()
    {
        //Arrange
        var validator = new GetOpenReferralOrganisationByIdCommandValidator();
        var testModel = new GetOpenReferralOrganisationByIdCommand { Id = "Id"};

        //Act
        var result = validator.Validate(testModel);

        //Assert
        result.Errors.Any().Should().BeFalse();
    }

    [Fact]
    public void ThenShouldErrorWhenThereIsNoId()
    {
        //Arrange
        var validator = new GetOpenReferralOrganisationByIdCommandValidator();
        var testModel = new GetOpenReferralOrganisationByIdCommand();

        //Act
        var result = validator.Validate(testModel);

        //Assert
        result.Errors.Any(x => x.PropertyName == "Id").Should().BeTrue();
    }
}
