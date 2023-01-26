using FamilyHubs.ServiceDirectory.Api.Queries.GetOrganisationById;
using FluentAssertions;

namespace FamilyHubs.ServiceDirectoryApi.UnitTests.Organisation;

public class WhenValidatingOrganisationGetById
{
    [Fact]
    public void ThenShouldNotErrorWhenModelIsValid()
    {
        //Arrange
        var validator = new GetOrganisationByIdCommandValidator();
        var testModel = new GetOrganisationByIdCommand { Id = "Id"};

        //Act
        var result = validator.Validate(testModel);

        //Assert
        result.Errors.Any().Should().BeFalse();
    }

    [Fact]
    public void ThenShouldErrorWhenThereIsNoId()
    {
        //Arrange
        var validator = new GetOrganisationByIdCommandValidator();
        var testModel = new GetOrganisationByIdCommand();

        //Act
        var result = validator.Validate(testModel);

        //Assert
        result.Errors.Any(x => x.PropertyName == "Id").Should().BeTrue();
    }
}
