using fh_service_directory_api.api.Commands.UpdateOpenReferralOrganisation;
using FluentAssertions;

namespace FamilyHubs.ServiceDirectoryApi.UnitTests.Organisation;

public class WhenValidatingUpdateOrganisationCommands
{
    [Fact]
    public void ThenShouldNotErrorWhenModelIsValid()
    {
        //Arrange
        var testOrganisation = WhenUsingOrganisationCommands.GetTestCountyCouncilRecord();
        var validator = new UpdateOpenReferralOrganisationCommandValidator();
        var testModel = new UpdateOpenReferralOrganisationCommand("Id", testOrganisation);

        //Act
        var result = validator.Validate(testModel);

        //Assert
        result.Errors.Any().Should().BeFalse();
    }

    [Fact]
    public void ThenShouldErrorWhenThereIsNoId()
    {
        //Arrange
        var testOrganisation = WhenUsingOrganisationCommands.GetTestCountyCouncilRecord();
        var validator = new UpdateOpenReferralOrganisationCommandValidator();
        var testModel = new UpdateOpenReferralOrganisationCommand("", testOrganisation);

        //Act
        var result = validator.Validate(testModel);

        //Assert
        result.Errors.Any(x => x.PropertyName == "Id").Should().BeTrue();
    }

    [Fact]
    public void ThenShouldErrorWhenModelHasNoId()
    {
        //Arrange
        var testOrganisation = WhenUsingOrganisationCommands.GetTestCountyCouncilRecord();
        testOrganisation.Id = string.Empty;
        var validator = new UpdateOpenReferralOrganisationCommandValidator();
        var testModel = new UpdateOpenReferralOrganisationCommand("Id", testOrganisation);

        //Act
        var result = validator.Validate(testModel);

        //Assert
        result.Errors.Any(x => x.PropertyName == "OpenReferralOrganisation.Id").Should().BeTrue();
    }

    [Fact]
    public void ThenShouldErrorWhenModelHasNoName()
    {
        //Arrange
        var testOrganisation = WhenUsingOrganisationCommands.GetTestCountyCouncilRecord();
        testOrganisation.Name = string.Empty;
        var validator = new UpdateOpenReferralOrganisationCommandValidator();
        var testModel = new UpdateOpenReferralOrganisationCommand("Id", testOrganisation);

        //Act
        var result = validator.Validate(testModel);

        //Assert
        result.Errors.Any(x => x.PropertyName == "OpenReferralOrganisation.Name").Should().BeTrue();
    }
}
