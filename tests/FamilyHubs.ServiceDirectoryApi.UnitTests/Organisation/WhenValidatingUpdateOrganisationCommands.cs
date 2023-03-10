﻿using FamilyHubs.ServiceDirectory.Api.Commands.UpdateOrganisation;
using FluentAssertions;

namespace FamilyHubs.ServiceDirectoryApi.UnitTests.Organisation;

public class WhenValidatingUpdateOrganisationCommands
{
    [Fact]
    public void ThenShouldNotErrorWhenModelIsValid()
    {
        //Arrange
        var testOrganisation = TestDataProvider.GetTestCountyCouncilRecord();
        var validator = new UpdateOrganisationCommandValidator();
        var testModel = new UpdateOrganisationCommand(Random.Shared.Next(), testOrganisation);

        //Act
        var result = validator.Validate(testModel);

        //Assert
        result.Errors.Any().Should().BeFalse();
    }

    [Fact]
    public void ThenShouldErrorWhenThereIsNoId()
    {
        //Arrange
        var testOrganisation = TestDataProvider.GetTestCountyCouncilRecord();
        var validator = new UpdateOrganisationCommandValidator();
        var testModel = new UpdateOrganisationCommand(Random.Shared.Next(), testOrganisation);

        //Act
        var result = validator.Validate(testModel);

        //Assert
        result.Errors.Any(x => x.PropertyName == "Id").Should().BeTrue();
    }

    [Fact]
    public void ThenShouldErrorWhenModelHasNoId()
    {
        //Arrange
        var testOrganisation = TestDataProvider.GetTestCountyCouncilRecord();
        testOrganisation.Id = Random.Shared.Next();
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
        var testOrganisation = TestDataProvider.GetTestCountyCouncilRecord();
        testOrganisation.Name = string.Empty;
        var validator = new UpdateOrganisationCommandValidator();
        var testModel = new UpdateOrganisationCommand(Random.Shared.Next(), testOrganisation);

        //Act
        var result = validator.Validate(testModel);

        //Assert
        result.Errors.Any(x => x.PropertyName == "Organisation.Name").Should().BeTrue();
    }
}
