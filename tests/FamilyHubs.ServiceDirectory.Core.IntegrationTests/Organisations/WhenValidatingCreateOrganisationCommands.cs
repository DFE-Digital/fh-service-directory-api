using FamilyHubs.ServiceDirectory.Core.Commands.Organisations.CreateOrganisation;
using FluentAssertions;

namespace FamilyHubs.ServiceDirectory.Core.IntegrationTests.Organisations;

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

    [Fact]
    public void ThenShouldCreateOrganisationCommandNotErrorWhenNameIsLessThen255Char()
    {
        //Arrange
        var testOrganisation = TestDataProvider.GetTestCountyCouncilDto2();
        testOrganisation.Name = string.Join(string.Empty, Enumerable.Range(0, 254).Select(_ => "a"));
        var validator = new CreateOrganisationCommandValidator();
        var testModel = new CreateOrganisationCommand(testOrganisation);

        //Act
        var result = validator.Validate(testModel);

        //Assert
        result.Errors.Any().Should().BeFalse();
    }

    [Fact]
    public void ThenShouldCreateOrganisationCommandHasErrorsWhenNameIsGreaterThen255Char()
    {
        //Arrange
        var testOrganisation = TestDataProvider.GetTestCountyCouncilDto2();
        testOrganisation.Name = string.Join(string.Empty, Enumerable.Range(0, 256).Select(_ => "a"));
        var validator = new CreateOrganisationCommandValidator();
        var testModel = new CreateOrganisationCommand(testOrganisation);

        //Act
        var result = validator.Validate(testModel);

        //Assert
        result.Errors.Any().Should().BeTrue();
    }
}
