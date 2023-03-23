using FamilyHubs.ServiceDirectory.Core.Commands.Organisations.UpdateOrganisation;
using FluentAssertions;

namespace FamilyHubs.ServiceDirectory.Core.IntegrationTests.Organisations;

public class WhenValidatingUpdateOrganisationCommands
{
    [Fact]
    public void ThenShouldNotErrorWhenModelIsValid()
    {
        //Arrange
        var testOrganisation = TestDataProvider.GetTestCountyCouncilDto2();
        testOrganisation.Id = 1;

        var validator = new UpdateOrganisationCommandValidator();
        var testModel = new UpdateOrganisationCommand(1, testOrganisation);

        //Act
        var result = validator.Validate(testModel);

        //Assert
        result.Errors.Any().Should().BeFalse();
    }

    [Fact]
    public void ThenShouldErrorWhenModelHasNoId()
    {
        //Arrange
        var testOrganisation = TestDataProvider.GetTestCountyCouncilDto2();
        testOrganisation.Id = 0;
        var validator = new UpdateOrganisationCommandValidator();
        var testModel = new UpdateOrganisationCommand(Random.Shared.Next(), testOrganisation);

        //Act
        var result = validator.Validate(testModel);

        //Assert
        result.Errors.Any(x => x.PropertyName == "Organisation.Id").Should().BeTrue();
    }

    [Fact]
    public void ThenShouldErrorWhenModelHasNoName()
    {
        //Arrange
        var testOrganisation = TestDataProvider.GetTestCountyCouncilDto2();
        testOrganisation.Name = string.Empty;
        var validator = new UpdateOrganisationCommandValidator();
        var testModel = new UpdateOrganisationCommand(Random.Shared.Next(), testOrganisation);

        //Act
        var result = validator.Validate(testModel);

        //Assert
        result.Errors.Any(x => x.PropertyName == "Organisation.Name").Should().BeTrue();
    }

    [Fact]
    public void ThenShouldUpdateOrganisationCommandNotErrorWhenNameIsLessThen255Char()
    {
        //Arrange
        var testOrganisation = TestDataProvider.GetTestCountyCouncilDto2();
        testOrganisation.Id = Random.Shared.Next();
        testOrganisation.Name = string.Join(string.Empty, Enumerable.Range(0, 254).Select(_ => "a"));
        var validator = new UpdateOrganisationCommandValidator();
        var testModel = new UpdateOrganisationCommand(testOrganisation.Id, testOrganisation);

        //Act
        var result = validator.Validate(testModel);

        //Assert
        result.Errors.Any().Should().BeFalse();
    }

    [Fact]
    public void ThenShouldUpdateOrganisationCommandHasErrorsWhenNameIsGreaterThen255Char()
    {
        //Arrange
        var testOrganisation = TestDataProvider.GetTestCountyCouncilDto2();
        testOrganisation.Name = string.Join(string.Empty, Enumerable.Range(0, 256).Select(_ => "a"));
        var validator = new UpdateOrganisationCommandValidator();
        var testModel = new UpdateOrganisationCommand(testOrganisation.Id, testOrganisation);

        //Act
        var result = validator.Validate(testModel);

        //Assert
        result.Errors.Any().Should().BeTrue();
    }
}
