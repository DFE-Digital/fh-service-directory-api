using FamilyHubs.ServiceDirectory.Api.Commands.CreateLocation;
using FluentAssertions;

namespace FamilyHubs.ServiceDirectoryApi.UnitTests.Locations;

public class WhenValidatingCreateLocationCommands
{
    [Fact]
    public void ThenShouldNotErrorWhenModelIsValid()
    {
        //Arrange
        var testLocation = WhenUsingLocationCommand.GetTestLocationDto();
        var validator = new CreateLocationCommandValidator();
        var testModel = new CreateLocationCommand(testLocation);

        //Act
        var result = validator.Validate(testModel);

        //Assert
        result.Errors.Any().Should().BeFalse();
    }
}
