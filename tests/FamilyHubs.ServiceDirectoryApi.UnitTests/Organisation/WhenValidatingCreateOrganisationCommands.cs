using FamilyHubs.ServiceDirectory.Api.Commands.CreateOrganisation;
using FluentAssertions;

namespace FamilyHubs.ServiceDirectoryApi.UnitTests.Organisation;

public class WhenValidatingCreateOrganisationCommands
{
    [Fact]
    public void ThenShouldNotErrorWhenModelIsValid()
    {
        //Arrange
        var testOrganisation = TestDataProvider.GetTestCountyCouncilDto();
        var validator = new CreateOrganisationCommandValidator();
        var testModel = new CreateOrganisationCommand(testOrganisation);

        //Act
        var result = validator.Validate(testModel);

        //Assert
        result.Errors.Any().Should().BeFalse();
    }

    [Fact]
    public void ThenShouldErrorWhenModelHasNoId()
    {
        //Arrange
        var testOrganisation = TestDataProvider.GetTestCountyCouncilDto();
        testOrganisation.Id = string.Empty;
        var validator = new CreateOrganisationCommandValidator();
        var testModel = new CreateOrganisationCommand(testOrganisation);

        //Act
        var result = validator.Validate(testModel);

        //Assert
        result.Errors.Any(x => x.PropertyName == "Organisation.Id").Should().BeTrue();
    }

    [Fact]
    public void ThenShouldErrorWhenModelHasNoName()
    {
        //Arrange
        var testOrganisation = TestDataProvider.GetTestCountyCouncilDto();
        testOrganisation.Name = string.Empty;
        var validator = new CreateOrganisationCommandValidator();
        var testModel = new CreateOrganisationCommand(testOrganisation);

        //Act
        var result = validator.Validate(testModel);

        //Assert
        result.Errors.Any(x => x.PropertyName == "Organisation.Name").Should().BeTrue();
    }
}
