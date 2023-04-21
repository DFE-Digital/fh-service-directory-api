using FamilyHubs.ServiceDirectory.Core.Commands.Services.CreateService;
using FamilyHubs.ServiceDirectory.Core.Commands.Services.DeleteService;
using FamilyHubs.ServiceDirectory.Core.Commands.Services.UpdateService;
using FluentAssertions;

namespace FamilyHubs.ServiceDirectory.Core.IntegrationTests.Services;

public class WhenValidatingServiceCommands
{
    [Fact]
    public void ThenShouldCreateServiceCommandNotErrorWhenModelIsValid()
    {
        //Arrange
        var testService = TestDataProvider.GetTestCountyCouncilServicesDto2(Random.Shared.Next());
        var validator = new CreateServiceCommandValidator();
        var testModel = new CreateServiceCommand(testService);

        //Act
        var result = validator.Validate(testModel);

        //Assert
        result.Errors.Any().Should().BeFalse();
    }

    [Fact]
    public void ThenShouldUpdateServiceCommandNotErrorWhenModelIsValid()
    {
        //Arrange
        var testService = TestDataProvider.GetTestCountyCouncilServicesDto2(Random.Shared.Next());
        testService.Id = 1;
        var validator = new UpdateServiceCommandValidator();
        var testModel = new UpdateServiceCommand(testService.Id, testService);

        //Act
        var result = validator.Validate(testModel);

        //Assert
        result.Errors.Any().Should().BeFalse();
    }

    [Fact]
    public void ThenShouldCreateServiceCommandNotErrorWhenNameIsLessThen255Char()
    {
        //Arrange
        var testService = TestDataProvider.GetTestCountyCouncilServicesDto2(Random.Shared.Next());
        testService.Name = string.Join(string.Empty, Enumerable.Range(0, 254).Select(_ => "a"));
        var validator = new CreateServiceCommandValidator();
        var testModel = new CreateServiceCommand(testService);

        //Act
        var result = validator.Validate(testModel);

        //Assert
        result.Errors.Any().Should().BeFalse();
    }

    [Fact]
    public void ThenShouldUpdateServiceCommandNotErrorWhenNameIsLessThen255Char()
    {
        //Arrange
        var testService = TestDataProvider.GetTestCountyCouncilServicesDto2(Random.Shared.Next());
        testService.Id = Random.Shared.Next();
        testService.Name = string.Join(string.Empty, Enumerable.Range(0, 254).Select(_ => "a"));
        var validator = new UpdateServiceCommandValidator();
        var testModel = new UpdateServiceCommand(testService.Id, testService);

        //Act
        var result = validator.Validate(testModel);

        //Assert
        result.Errors.Any().Should().BeFalse();
    }

    [Fact]
    public void ThenShouldCreateServiceCommandHasErrorsWhenNameIsGreaterThen255Char()
    {
        //Arrange
        var testService = TestDataProvider.GetTestCountyCouncilServicesDto2(Random.Shared.Next());
        testService.Name = string.Join(string.Empty, Enumerable.Range(0, 256).Select(_ => "a"));
        var validator = new CreateServiceCommandValidator();
        var testModel = new CreateServiceCommand(testService);

        //Act
        var result = validator.Validate(testModel);

        //Assert
        result.Errors.Any().Should().BeTrue();
    }

    [Fact]
    public void ThenShouldUpdateServiceCommandHasErrorsWhenNameIsGreaterThen255Char()
    {
        //Arrange
        var testService = TestDataProvider.GetTestCountyCouncilServicesDto2(Random.Shared.Next());
        testService.Name = string.Join(string.Empty, Enumerable.Range(0, 256).Select(_ => "a"));
        var validator = new UpdateServiceCommandValidator();
        var testModel = new UpdateServiceCommand(testService.Id, testService);

        //Act
        var result = validator.Validate(testModel);

        //Assert
        result.Errors.Any().Should().BeTrue();
    }

    [Fact]
    public void ThenShouldDeleteServiceByIdCommandNotErrorWhenModelIsValid()
    {
        //Arrange
        var validator = new DeleteServiceByIdCommandValidator();
        var testModel = new DeleteServiceByIdCommand(1);

        //Act
        var result = validator.Validate(testModel);

        //Assert
        result.Errors.Any().Should().BeFalse();
    }

    [Theory]
    [InlineData(default!)]
    [InlineData("")]
    [InlineData("http://wwww.google.co.uk")]
    [InlineData("http://wwww.google.com")]
    [InlineData("https://wwww.google.com")]
    public void ThenShouldValidateServiceAtLocationContactUrlWhenCreatingService_ShouldReturnNoErrors(string url)
    {
        //Arrange
        var testService = TestDataProvider.GetTestCountyCouncilServicesDto2(Random.Shared.Next());
        testService.Id = 0;
        foreach (var serviceAtLocation in testService.Locations)
        {
            foreach (var item in serviceAtLocation.Contacts)
            {
                item.Url = url;
            }
        }


        var validator = new CreateServiceCommandValidator();
        var testModel = new CreateServiceCommand(testService);

        //Act
        var result = validator.Validate(testModel);

        //Assert
        result.Errors.Any().Should().BeFalse();
    }

    [Theory]
    [InlineData("someurl")]
    [InlineData("http://someurl")]
    [InlineData("https://someurl")]
    public void ThenShouldValidateServiceAtLocationContactUrlWhenCreatingService_ShouldReturnErrors(string url)
    {
        //Arrange
        var testService = TestDataProvider.GetTestCountyCouncilServicesDto2(Random.Shared.Next());
        testService.Id = 1;
        foreach (var serviceAtLocation in testService.Locations)
        {
            foreach (var item in serviceAtLocation.Contacts)
            {
                item.Url = url;
            }
        }


        var validator = new CreateServiceCommandValidator();
        var testModel = new CreateServiceCommand(testService);

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
    public void ThenShouldValidateServiceAtLocationContactUrlWhenUpdatingService_ShouldReturnNoErrors(string url)
    {
        //Arrange
        var testService = TestDataProvider.GetTestCountyCouncilServicesDto2(Random.Shared.Next());
        testService.Id = 1;
        foreach (var serviceAtLocation in testService.Locations)
        {
            foreach (var item in serviceAtLocation.Contacts)
            {
                item.Url = url;
            }
        }
        var validator = new UpdateServiceCommandValidator();
        var testModel = new UpdateServiceCommand(testService.Id, testService);

        //Act
        var result = validator.Validate(testModel);

        //Assert
        result.Errors.Any().Should().BeFalse();
    }

    [Theory]
    [InlineData("someurl")]
    [InlineData("http://someurl")]
    [InlineData("https://someurl")]
    public void ThenShouldValidateServiceAtLocationContactUrlWhenUpdatingService_ShouldReturnErrors(string url)
    {
        //Arrange
        var testService = TestDataProvider.GetTestCountyCouncilServicesDto2(Random.Shared.Next());
        testService.Id = 1;
        foreach (var serviceAtLocation in testService.Locations)
        {
            foreach (var item in serviceAtLocation.Contacts)
            {
                item.Url = url;
            }
        }
        var validator = new UpdateServiceCommandValidator();
        var testModel = new UpdateServiceCommand(testService.Id, testService);

        //Act
        var result = validator.Validate(testModel);

        //Assert
        result.Errors.Any().Should().BeTrue();
    }
}
