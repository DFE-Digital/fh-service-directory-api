using FamilyHubs.ServiceDirectory.Api.Queries.GetUiCacheById;
using FluentAssertions;

namespace FamilyHubs.ServiceDirectoryApi.UnitTests.UiCaches;

public class WhenValidatingUiCacheGetById
{
    [Fact]
    public void ThenShouldNotErrorWhenModelIsValid()
    {
        //Arrange
        var validator = new GetUiCacheByIdCommandValidator();
        var testModel = new GetUiCacheByIdCommand("Id");

        //Act
        var result = validator.Validate(testModel);

        //Assert
        result.Errors.Any().Should().BeFalse();
    }

    [Fact]
    public void ThenShouldErrorWhenThereIsNoId()
    {
        //Arrange
        var validator = new GetUiCacheByIdCommandValidator();
        var testModel = new GetUiCacheByIdCommand("");

        //Act
        var result = validator.Validate(testModel);

        //Assert
        result.Errors.Any(x => x.PropertyName == "Id").Should().BeTrue();
    }
}
