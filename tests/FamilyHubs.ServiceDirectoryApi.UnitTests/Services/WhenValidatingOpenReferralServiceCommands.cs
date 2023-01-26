using FamilyHubs.ServiceDirectoryApi.UnitTests.Organisation;
using fh_service_directory_api.api.Commands.CreateOpenReferralService;
using fh_service_directory_api.api.Commands.DeleteOpenReferralService;
using fh_service_directory_api.api.Commands.UpdateOpenReferralService;
using fh_service_directory_api.api.Queries.GetOpenReferralService;
using FluentAssertions;

namespace FamilyHubs.ServiceDirectoryApi.UnitTests.Services;

public class WhenValidatingOpenReferralServiceCommands
{
    [Fact]
    public void ThenShouldCreateOpenReferralServiceCommandNotErrorWhenModelIsValid()
    {
        //Arrange
        var testService = WhenUsingOrganisationCommands.GetTestCountyCouncilServicesDto("56e62852-1b0b-40e5-ac97-54a67ea957dc");
        var validator = new CreateOpenReferralServiceCommandValidator();
        var testModel = new CreateOpenReferralServiceCommand(testService);

        //Act
        var result = validator.Validate(testModel);

        //Assert
        result.Errors.Any().Should().BeFalse();
    }

    [Fact]
    public void ThenShouldUpdateOpenReferralServiceCommandNotErrorWhenModelIsValid()
    {
        //Arrange
        var testService = WhenUsingOrganisationCommands.GetTestCountyCouncilServicesDto("56e62852-1b0b-40e5-ac97-54a67ea957dc");
        var validator = new UpdateOpenReferralServiceCommandValidator();
        var testModel = new UpdateOpenReferralServiceCommand(testService.Id, testService);

        //Act
        var result = validator.Validate(testModel);

        //Assert
        result.Errors.Any().Should().BeFalse();
    }

    [Fact]
    public void ThenShouldDeleteOpenReferralServiceByIdCommandNotErrorWhenModelIsValid()
    {
        //Arrange
        var validator = new DeleteOpenReferralServiceByIdCommandValidator();
        var testModel = new DeleteOpenReferralServiceByIdCommand("1");

        //Act
        var result = validator.Validate(testModel);

        //Assert
        result.Errors.Any().Should().BeFalse();
    }

    [Fact]
    public void ThenShouldGetOpenReferralServiceByIdCommandNotErrorWhenModelIsValid()
    {
        //Arrange
        var validator = new GetOpenReferralServiceByIdCommandValidator();
        var testModel = new GetOpenReferralServiceByIdCommand("3010521b-6e0a-41b0-b610-200edbbeeb14");

        //Act
        var result = validator.Validate(testModel);

        //Assert
        result.Errors.Any().Should().BeFalse();
    }
}
