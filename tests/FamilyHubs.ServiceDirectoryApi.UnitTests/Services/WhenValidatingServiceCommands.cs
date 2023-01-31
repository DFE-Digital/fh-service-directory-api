using FamilyHubs.ServiceDirectory.Api.Commands.CreateService;
using FamilyHubs.ServiceDirectory.Api.Commands.DeleteService;
using FamilyHubs.ServiceDirectory.Api.Commands.UpdateService;
using FamilyHubs.ServiceDirectory.Api.Queries.GetService;
using FamilyHubs.ServiceDirectoryApi.UnitTests.Organisation;
using FluentAssertions;

namespace FamilyHubs.ServiceDirectoryApi.UnitTests.Services;

public class WhenValidatingServiceCommands
{
    [Fact]
    public void ThenShouldCreateServiceCommandNotErrorWhenModelIsValid()
    {
        //Arrange
        var testService = WhenUsingOrganisationCommands.GetTestCountyCouncilServicesDto2("56e62852-1b0b-40e5-ac97-54a67ea957dc");
        var validator = new CreateServiceCommandValidator();
        var testModel = new CreateServiceCommand(testService);

        //Act
        var result = validator.Validate(testModel);

        //Assert
        result.Errors.Any().Should().BeFalse();
    }

    [Fact]
    public void ThenShouldUpdateServiceCommandNotErrorWhenModelIsValid()
    {
        //Arrange
        var testService = WhenUsingOrganisationCommands.GetTestCountyCouncilServicesDto2("56e62852-1b0b-40e5-ac97-54a67ea957dc");
        var validator = new UpdateServiceCommandValidator();
        var testModel = new UpdateServiceCommand(testService.Id, testService);

        //Act
        var result = validator.Validate(testModel);

        //Assert
        result.Errors.Any().Should().BeFalse();
    }

    [Fact]
    public void ThenShouldDeleteServiceByIdCommandNotErrorWhenModelIsValid()
    {
        //Arrange
        var validator = new DeleteServiceByIdCommandValidator();
        var testModel = new DeleteServiceByIdCommand("1");

        //Act
        var result = validator.Validate(testModel);

        //Assert
        result.Errors.Any().Should().BeFalse();
    }

    [Fact]
    public void ThenShouldGetServiceByIdCommandNotErrorWhenModelIsValid()
    {
        //Arrange
        var validator = new GetServiceByIdCommandValidator();
        var testModel = new GetServiceByIdCommand("3010521b-6e0a-41b0-b610-200edbbeeb14");

        //Act
        var result = validator.Validate(testModel);

        //Assert
        result.Errors.Any().Should().BeFalse();
    }
}
