using FamilyHubs.ServiceDirectory.Api.Commands.UpdateOrganisation;
using FluentAssertions;

namespace FamilyHubs.ServiceDirectoryApi.UnitTests.Organisation;

public class WhenValidatingUpdateOrganisationCommands
{
    [Fact]
    public void ThenShouldNotErrorWhenModelIsValid()
    {
        //Arrange
        var testOrganisation = WhenUsingOrganisationCommands.GetTestCountyCouncilRecord();
        var validator = new UpdateOrganisationCommandValidator();
        var testModel = new UpdateOrganisationCommand("Id", testOrganisation);

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
        var validator = new UpdateOrganisationCommandValidator();
        var testModel = new UpdateOrganisationCommand("", testOrganisation);

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
        var validator = new UpdateOrganisationCommandValidator();
        var testModel = new UpdateOrganisationCommand("Id", testOrganisation);

        //Act
        var result = validator.Validate(testModel);

        //Assert
        result.Errors.Any(x => x.PropertyName == "Organisation.Id").Should().BeTrue();
    }

    [Fact]
    public void ThenShouldErrorWhenModelHasNoName()
    {
        //Arrange
        var testOrganisation = WhenUsingOrganisationCommands.GetTestCountyCouncilRecord();
        testOrganisation.Name = string.Empty;
        var validator = new UpdateOrganisationCommandValidator();
        var testModel = new UpdateOrganisationCommand("Id", testOrganisation);

        //Act
        var result = validator.Validate(testModel);

        //Assert
        result.Errors.Any(x => x.PropertyName == "Organisation.Name").Should().BeTrue();
    }
}
