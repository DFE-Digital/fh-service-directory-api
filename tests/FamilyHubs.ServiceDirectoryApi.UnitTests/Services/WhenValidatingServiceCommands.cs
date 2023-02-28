using FamilyHubs.ServiceDirectory.Api.Commands.CreateService;
using FamilyHubs.ServiceDirectory.Api.Commands.DeleteService;
using FamilyHubs.ServiceDirectory.Api.Commands.UpdateService;
using FamilyHubs.ServiceDirectory.Api.Queries.GetService;
using FluentAssertions;

namespace FamilyHubs.ServiceDirectoryApi.UnitTests.Services;

public class WhenValidatingServiceCommands
{
    [Fact]
    public void ThenShouldCreateServiceCommandNotErrorWhenModelIsValid()
    {
        //Arrange
        var testService = TestDataProvider.GetTestCountyCouncilServicesDto2("56e62852-1b0b-40e5-ac97-54a67ea957dc");
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
        var testService = TestDataProvider.GetTestCountyCouncilServicesDto2("56e62852-1b0b-40e5-ac97-54a67ea957dc");
        var validator = new UpdateServiceCommandValidator();
        var testModel = new UpdateServiceCommand(testService.Id, testService);

        //Act
        var result = validator.Validate(testModel);

        //Assert
        result.Errors.Any().Should().BeFalse();
    }

    [Fact]
    public void ThenShouldDeleteServiceByIdCommandNotErrorWhenModelIsValid()
    {
        //Arrange
        var validator = new DeleteServiceByIdCommandValidator();
        var testModel = new DeleteServiceByIdCommand("1");

        //Act
        var result = validator.Validate(testModel);

        //Assert
        result.Errors.Any().Should().BeFalse();
    }

    [Fact]
    public void ThenShouldGetServiceByIdCommandNotErrorWhenModelIsValid()
    {
        //Arrange
        var validator = new GetServiceByIdCommandValidator();
        var testModel = new GetServiceByIdCommand("3010521b-6e0a-41b0-b610-200edbbeeb14");

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
        var testService = TestDataProvider.GetTestCountyCouncilServicesDto2("56e62852-1b0b-40e5-ac97-54a67ea957dc");
        if (testService.ServiceAtLocations is not null)
        {
            foreach (var serviceAtLocation in testService.ServiceAtLocations)
            {
                if (serviceAtLocation is not null && serviceAtLocation.LinkContacts is not null)
                {
                    foreach (var item in serviceAtLocation.LinkContacts)
                    {
                        item.Contact.Url = url;
                    }
                }
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
        var testService = TestDataProvider.GetTestCountyCouncilServicesDto2("56e62852-1b0b-40e5-ac97-54a67ea957dc");
        if (testService.ServiceAtLocations is not null)
        {
            foreach (var serviceAtLocation in testService.ServiceAtLocations)
            {
                if (serviceAtLocation is not null && serviceAtLocation.LinkContacts is not null)
                {
                    foreach (var item in serviceAtLocation.LinkContacts)
                    {
                        item.Contact.Url = url;
                    }
                }
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
        var testService = TestDataProvider.GetTestCountyCouncilServicesDto2("56e62852-1b0b-40e5-ac97-54a67ea957dc");
        if (testService.ServiceAtLocations is not null)
        {
            foreach (var serviceAtLocation in testService.ServiceAtLocations)
            {
                if (serviceAtLocation is not null && serviceAtLocation.LinkContacts is not null)
                {
                    foreach (var item in serviceAtLocation.LinkContacts)
                    {
                        item.Contact.Url = url;
                    }
                }
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
        var testService = TestDataProvider.GetTestCountyCouncilServicesDto2("56e62852-1b0b-40e5-ac97-54a67ea957dc");
        if (testService.ServiceAtLocations is not null)
        {
            foreach (var serviceAtLocation in testService.ServiceAtLocations)
            {
                if (serviceAtLocation is not null && serviceAtLocation.LinkContacts is not null)
                {
                    foreach (var item in serviceAtLocation.LinkContacts)
                    {
                        item.Contact.Url = url;
                    }
                }
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
