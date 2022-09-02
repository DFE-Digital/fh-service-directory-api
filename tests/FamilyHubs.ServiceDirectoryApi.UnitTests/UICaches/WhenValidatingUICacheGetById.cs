using fh_service_directory_api.api.Queries.GetUICacheById;
using FluentAssertions;

namespace FamilyHubs.ServiceDirectoryApi.UnitTests.UICaches;

public class WhenValidatingUICacheGetById
{
    [Fact]
    public void ThenShouldNotErrorWhenModelIsValid()
    {
        //Arrange
        var validator = new GetUICacheByIdCommandValidator();
        var testModel = new GetUICacheByIdCommand("Id");

        //Act
        var result = validator.Validate(testModel);

        //Assert
        result.Errors.Any().Should().BeFalse();
    }

    [Fact]
    public void ThenShouldErrorWhenThereIsNoId()
    {
        //Arrange
        var validator = new GetUICacheByIdCommandValidator();
        var testModel = new GetUICacheByIdCommand("");

        //Act
        var result = validator.Validate(testModel);

        //Assert
        result.Errors.Any(x => x.PropertyName == "Id").Should().BeTrue();
    }
}
