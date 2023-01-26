using FamilyHubs.ServiceDirectory.Api.Commands.UpdateUICache;
using FamilyHubs.ServiceDirectory.Shared.Dto;
using FluentAssertions;

namespace FamilyHubs.ServiceDirectoryApi.UnitTests.UICaches;

public class WhenValidatingUpdateUICache
{
    [Fact]
    public void ThenShouldNotErrorWhenModelIsValid()
    {
        //Arrange
        var validator = new UpdateUICacheCommandValidator();
        var testModel = new UpdateUICacheCommand("Id", new UICacheDto("Id", "Value"));

        //Act
        var result = validator.Validate(testModel);

        //Assert
        result.Errors.Any().Should().BeFalse();
    }

    [Fact]
    public void ThenShouldErrorWhenThereIsNoId()
    {
        //Arrange
        var validator = new UpdateUICacheCommandValidator();
        var testModel = new UpdateUICacheCommand("", new UICacheDto("Id", "Value"));

        //Act
        var result = validator.Validate(testModel);

        //Assert
        result.Errors.Any(x => x.PropertyName == "Id").Should().BeTrue();
    }

    [Fact]
    public void ThenShouldErrorWhenModelHasNoId()
    {
        //Arrange
        var validator = new UpdateUICacheCommandValidator();
        var testModel = new UpdateUICacheCommand("Id", new UICacheDto(string.Empty, "Value"));

        //Act
        var result = validator.Validate(testModel);

        //Assert
        result.Errors.Any(x => x.PropertyName == "UICacheDto.Id").Should().BeTrue();
    }

    [Fact]
    public void ThenShouldErrorWhenModelHasNoValue()
    {
        //Arrange
        var validator = new UpdateUICacheCommandValidator();
        var testModel = new UpdateUICacheCommand("Id", new UICacheDto("Id", string.Empty));

        //Act
        var result = validator.Validate(testModel);

        //Assert
        result.Errors.Any(x => x.PropertyName == "UICacheDto.Value").Should().BeTrue();
    }
}
