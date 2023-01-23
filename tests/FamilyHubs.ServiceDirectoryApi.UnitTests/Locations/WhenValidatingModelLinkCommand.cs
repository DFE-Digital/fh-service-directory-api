using FamilyHubs.ServiceDirectory.Shared.Models.Api.ModelLink;
using fh_service_directory_api.api.Commands.CreateModelLink;
using FluentAssertions;

namespace FamilyHubs.ServiceDirectoryApi.UnitTests.Locations;

public class WhenValidatingModelLinkCommand
{
    [Fact]
    public void ThenShouldNotErrorWhenModelIsValid()
    {
        //Arrange
        var validator = new CreateModelLinkCommandValidator();
        CreateModelLinkCommand testModel = new(new ModelLinkDto("d3ee9abb-6cbc-4ca8-9119-6ec3133c166b", "TestLink", "ModelOne", "ModelTwo"));
   

        //Act
        var result = validator.Validate(testModel);

        //Assert
        result.Errors.Any().Should().BeFalse();
    }
}
