using FamilyHubs.ServiceDirectory.Api.Commands.UpdateUiCache;
using FamilyHubs.ServiceDirectory.Shared.Dto;
using FluentAssertions;

namespace FamilyHubs.ServiceDirectoryApi.UnitTests.UiCaches;

public class WhenValidatingUpdateUiCache
{
    [Fact]
    public void ThenShouldNotErrorWhenModelIsValid()
    {
        //Arrange
        var validator = new UpdateUiCacheCommandValidator();
        var testModel = new UpdateUiCacheCommand("Id", new UICacheDto("Id", "Value"));

        //Act
        var result = validator.Validate(testModel);

        //Assert
        result.Errors.Any().Should().BeFalse();
    }

    [Fact]
    public void ThenShouldErrorWhenThereIsNoId()
    {
        //Arrange
        var validator = new UpdateUiCacheCommandValidator();
        var testModel = new UpdateUiCacheCommand("", new UICacheDto("Id", "Value"));

        //Act
        var result = validator.Validate(testModel);

        //Assert
        result.Errors.Any(x => x.PropertyName == "Id").Should().BeTrue();
    }

    [Fact]
    public void ThenShouldErrorWhenModelHasNoId()
    {
        //Arrange
        var validator = new UpdateUiCacheCommandValidator();
        var testModel = new UpdateUiCacheCommand("Id", new UICacheDto(string.Empty, "Value"));

        //Act
        var result = validator.Validate(testModel);

        //Assert
        result.Errors.Any(x => x.PropertyName == "UiCacheDto.Id").Should().BeTrue();
    }

    [Fact]
    public void ThenShouldErrorWhenModelHasNoValue()
    {
        //Arrange
        var validator = new UpdateUiCacheCommandValidator();
        var testModel = new UpdateUiCacheCommand("Id", new UICacheDto("Id", string.Empty));

        //Act
        var result = validator.Validate(testModel);

        //Assert
        result.Errors.Any(x => x.PropertyName == "UiCacheDto.Value").Should().BeTrue();
    }
}
