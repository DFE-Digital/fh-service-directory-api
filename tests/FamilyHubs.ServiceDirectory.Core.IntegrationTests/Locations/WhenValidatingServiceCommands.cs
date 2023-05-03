using FamilyHubs.ServiceDirectory.Core.Commands.Locations.CreateLocation;
using FamilyHubs.ServiceDirectory.Core.Commands.Locations.UpdateLocation;
using FluentAssertions;

namespace FamilyHubs.ServiceDirectory.Core.IntegrationTests.Locations;

public class WhenValidatingLocationCommands
{
    [Fact]
    public void ThenShouldCreateLocationCommandNotErrorWhenModelIsValid()
    {
        //Arrange
        var testLocation = TestDataProvider.GetTestCountyCouncilServicesDto2(Random.Shared.Next()).Locations.ElementAt(0);
        var validator = new CreateLocationCommandValidator();
        var testModel = new CreateLocationCommand(testLocation);

        //Act
        var result = validator.Validate(testModel);

        //Assert
        result.Errors.Any().Should().BeFalse();
    }

    [Fact]
    public void ThenShouldUpdateLocationCommandNotErrorWhenModelIsValid()
    {
        //Arrange
        var testLocation = TestDataProvider.GetTestCountyCouncilServicesDto2(Random.Shared.Next()).Locations.ElementAt(0);
        testLocation.Id = 1;
        var validator = new UpdateLocationCommandValidator();
        var testModel = new UpdateLocationCommand(testLocation.Id, testLocation);

        //Act
        var result = validator.Validate(testModel);

        //Assert
        result.Errors.Any().Should().BeFalse();
    }

    [Fact]
    public void ThenShouldCreateLocationCommandNotErrorWhenNameIsLessThen255Char()
    {
        //Arrange
        var testLocation = TestDataProvider.GetTestCountyCouncilServicesDto2(Random.Shared.Next()).Locations.ElementAt(0);
        testLocation.Name = string.Join(string.Empty, Enumerable.Range(0, 254).Select(_ => "a"));
        var validator = new CreateLocationCommandValidator();
        var testModel = new CreateLocationCommand(testLocation);

        //Act
        var result = validator.Validate(testModel);

        //Assert
        result.Errors.Any().Should().BeFalse();
    }

    [Fact]
    public void ThenShouldUpdateLocationCommandNotErrorWhenNameIsLessThen255Char()
    {
        //Arrange
        var testLocation = TestDataProvider.GetTestCountyCouncilServicesDto2(Random.Shared.Next()).Locations.ElementAt(0);
        testLocation.Id = Random.Shared.Next();
        testLocation.Name = string.Join(string.Empty, Enumerable.Range(0, 254).Select(_ => "a"));
        var validator = new UpdateLocationCommandValidator();
        var testModel = new UpdateLocationCommand(testLocation.Id, testLocation);

        //Act
        var result = validator.Validate(testModel);

        //Assert
        result.Errors.Any().Should().BeFalse();
    }

    [Fact]
    public void ThenShouldCreateLocationCommandHasErrorsWhenNameIsGreaterThen255Char()
    {
        //Arrange
        var testLocation = TestDataProvider.GetTestCountyCouncilServicesDto2(Random.Shared.Next()).Locations.ElementAt(0);
        testLocation.Name = string.Join(string.Empty, Enumerable.Range(0, 256).Select(_ => "a"));
        var validator = new CreateLocationCommandValidator();
        var testModel = new CreateLocationCommand(testLocation);

        //Act
        var result = validator.Validate(testModel);

        //Assert
        result.Errors.Any().Should().BeTrue();
    }

    [Fact]
    public void ThenShouldUpdateLocationCommandHasErrorsWhenNameIsGreaterThen255Char()
    {
        //Arrange
        var testLocation = TestDataProvider.GetTestCountyCouncilServicesDto2(Random.Shared.Next()).Locations.ElementAt(0);
        testLocation.Name = string.Join(string.Empty, Enumerable.Range(0, 256).Select(_ => "a"));
        var validator = new UpdateLocationCommandValidator();
        var testModel = new UpdateLocationCommand(testLocation.Id, testLocation);

        //Act
        var result = validator.Validate(testModel);

        //Assert
        result.Errors.Any().Should().BeTrue();
    }

    [Theory]
    [InlineData(default!)]
    [InlineData("")]
    [InlineData("http://wwww.google.co.uk")]
    [InlineData("http://wwww.google.com")]
    [InlineData("https://wwww.google.com")]
    public void ThenShouldValidateLocationContactUrlWhenCreatingLocation_ShouldReturnNoErrors(string url)
    {
        //Arrange
        var testLocation = TestDataProvider.GetTestCountyCouncilServicesDto2(Random.Shared.Next()).Locations.ElementAt(0);
        testLocation.Id = 0;
        foreach (var item in testLocation.Contacts)
        {
            item.Url = url;
        }

        var validator = new CreateLocationCommandValidator();
        var testModel = new CreateLocationCommand(testLocation);

        //Act
        var result = validator.Validate(testModel);

        //Assert
        result.Errors.Any().Should().BeFalse();
    }

    [Theory]
    [InlineData("someurl")]
    [InlineData("http://someurl")]
    [InlineData("https://someurl")]
    public void ThenShouldValidateLocationContactUrlWhenCreatingLocation_ShouldReturnErrors(string url)
    {
        //Arrange
        var testLocation = TestDataProvider.GetTestCountyCouncilServicesDto2(Random.Shared.Next()).Locations.ElementAt(0);
        testLocation.Id = 1;

        foreach (var item in testLocation.Contacts)
        {
            item.Url = url;
        }

        var validator = new CreateLocationCommandValidator();
        var testModel = new CreateLocationCommand(testLocation);

        //Act
        var result = validator.Validate(testModel);

        //Assert
        result.Errors.Any().Should().BeTrue();
    }

    [Theory]
    [InlineData(default!)]
    [InlineData("")]
    [InlineData("http://wwww.google.co.uk")]
    [InlineData("http://wwww.google.com")]
    [InlineData("https://wwww.google.com")]
    public void ThenShouldValidateLocationContactUrlWhenUpdatingLocation_ShouldReturnNoErrors(string url)
    {
        //Arrange
        var testLocation = TestDataProvider.GetTestCountyCouncilServicesDto2(Random.Shared.Next()).Locations.ElementAt(0);
        testLocation.Id = 1;

        foreach (var item in testLocation.Contacts)
        {
            item.Url = url;
        }

        var validator = new UpdateLocationCommandValidator();
        var testModel = new UpdateLocationCommand(testLocation.Id, testLocation);

        //Act
        var result = validator.Validate(testModel);

        //Assert
        result.Errors.Any().Should().BeFalse();
    }

    [Theory]
    [InlineData("someurl")]
    [InlineData("http:/someurl")]
    [InlineData("https//someurl")]
    public void ThenShouldValidateLocationContactUrlWhenUpdatingLocation_ShouldReturnErrors(string url)
    {
        //Arrange
        var testLocation = TestDataProvider.GetTestCountyCouncilServicesDto2(Random.Shared.Next()).Locations.ElementAt(0);
        testLocation.Id = 1;

        foreach (var item in testLocation.Contacts)
        {
            item.Url = url;
        }

        var validator = new UpdateLocationCommandValidator();
        var testModel = new UpdateLocationCommand(testLocation.Id, testLocation);

        //Act
        var result = validator.Validate(testModel);

        //Assert
        result.Errors.Any().Should().BeTrue();
    }
}
