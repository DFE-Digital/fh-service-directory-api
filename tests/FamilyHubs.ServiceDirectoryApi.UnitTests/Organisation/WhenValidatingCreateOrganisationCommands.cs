using fh_service_directory_api.api.Commands.CreateOpenReferralOrganisation;
using FluentAssertions;

namespace FamilyHubs.ServiceDirectoryApi.UnitTests.Organisation;

public class WhenValidatingCreateOrganisationCommands
{
    [Fact]
    public void ThenShouldNotErrorWhenModelIsValid()
    {
        //Arrange
        var testOrganisation = WhenUsingOrganisationCommands.GetTestCountyCouncilDto();
        var validator = new CreateOpenReferralOrganisationCommandValidator();
        var testModel = new CreateOpenReferralOrganisationCommand(testOrganisation);

        //Act
        var result = validator.Validate(testModel);

        //Assert
        result.Errors.Any().Should().BeFalse();
    }

    [Fact]
    public void ThenShouldErrorWhenModelHasNoId()
    {
        //Arrange
        var testOrganisation = WhenUsingOrganisationCommands.GetTestCountyCouncilDto();
        testOrganisation.Id = string.Empty;
        var validator = new CreateOpenReferralOrganisationCommandValidator();
        var testModel = new CreateOpenReferralOrganisationCommand(testOrganisation);

        //Act
        var result = validator.Validate(testModel);

        //Assert
        result.Errors.Any(x => x.PropertyName == "OpenReferralOrganisation.Id").Should().BeTrue();
    }

    [Fact]
    public void ThenShouldErrorWhenModelHasNoName()
    {
        //Arrange
        var testOrganisation = WhenUsingOrganisationCommands.GetTestCountyCouncilDto();
        testOrganisation.Name = string.Empty;
        var validator = new CreateOpenReferralOrganisationCommandValidator();
        var testModel = new CreateOpenReferralOrganisationCommand(testOrganisation);

        //Act
        var result = validator.Validate(testModel);

        //Assert
        result.Errors.Any(x => x.PropertyName == "OpenReferralOrganisation.Name").Should().BeTrue();
    }
}
