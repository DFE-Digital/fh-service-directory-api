using FamilyHubs.ServiceDirectory.Api.Commands.CreateUiCache;
using FamilyHubs.ServiceDirectory.Shared.Dto;
using FluentAssertions;

namespace FamilyHubs.ServiceDirectoryApi.UnitTests.UiCaches;

public class WhenValidatingCreateUiCache
{
    [Fact]
    public void ThenShouldNotErrorWhenModelIsValid()
    {
        //Arrange
        var validator = new CreateUiCacheCommandValidator();
        var testModel = new CreateUiCacheCommand(new UICacheDto("Id","Value"));
        
        //Act
        var result = validator.Validate(testModel);

        //Assert
        result.Errors.Any().Should().BeFalse();
    }

    [Fact]
    public void ThenShouldErrorWhenModelHasNoId()
    {
        //Arrange
        var validator = new CreateUiCacheCommandValidator();
        var testModel = new CreateUiCacheCommand(new UICacheDto(string.Empty,"Value"));

        //Act
        var result = validator.Validate(testModel);

        //Assert
        result.Errors.Any(x => x.PropertyName == "UiCacheDto.Id").Should().BeTrue();
    }

    [Fact]
    public void ThenShouldErrorWhenModelHasNoValue()
    {
        //Arrange
        var validator = new CreateUiCacheCommandValidator();
        var testModel = new CreateUiCacheCommand(new UICacheDto("Id", string.Empty));

        //Act
        var result = validator.Validate(testModel);

        //Assert
        result.Errors.Any(x => x.PropertyName == "UiCacheDto.Value").Should().BeTrue();
    }

}
