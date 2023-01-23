using fh_service_directory_api.api.Commands.CreateLocation;
using FluentAssertions;

namespace FamilyHubs.ServiceDirectoryApi.UnitTests.Locations;

public class WhenValidatingCreateLocationCommands
{
    [Fact]
    public void ThenShouldNotErrorWhenModelIsValid()
    {
        //Arrange
        var testLocation = WhenUsingLocationCommand.GetTestOpenReferralLocationDto();
        var validator = new CreateOpenReferralLocationCommandValidator();
        var testModel = new CreateOpenReferralLocationCommand(testLocation);

        //Act
        var result = validator.Validate(testModel);

        //Assert
        result.Errors.Any().Should().BeFalse();
    }
}
