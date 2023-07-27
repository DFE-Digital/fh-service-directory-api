using FamilyHubs.ServiceDirectory.Core.Commands;
using FamilyHubs.ServiceDirectory.Shared.Dto;
using FamilyHubs.ServiceDirectory.Shared.Enums;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;

namespace FamilyHubs.ServiceDirectory.Core.IntegrationTests;

public class WhenUsingSendEventGridMessageCommandHandlerTests
{
    [Fact]
    public async Task ThenHandle_ValidRequest_ReturnsContent()
    {
        // Arrange
        var configurationMock = new Mock<IConfiguration>();
        configurationMock.SetupGet(x => x["EventGridUrl"]).Returns("http://example.com/eventgrid");
        configurationMock.SetupGet(x => x["aeg-sas-key"]).Returns("dummy-key");

        var handler = new SendEventGridMessageCommandHandler(configurationMock.Object,new Mock<ILogger<SendEventGridMessageCommandHandler>>().Object);
        handler.IsUnitTesting = true;

        var organisationDto = new OrganisationDto
        {
            Id = 3,
            OrganisationType = OrganisationType.LA,
            Name = "Grid Unit Test A County Council",
            Description = "Grid Unit Test A County Council",
            AdminAreaCode = "XTEST",
            Uri = new Uri("https://www.unittesta.gov.uk/").ToString(),
            Url = "https://www.unittesta.gov.uk/"
        };

        var request = new SendEventGridMessageCommand(organisationDto);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task ThenHandle_MissingEventGridUrl_ThrowsArgumentException()
    {
        // Arrange
        var configurationMock = new Mock<IConfiguration>();
        configurationMock.SetupGet(x => x["EventGridUrl"]).Returns(default(string));
        configurationMock.SetupGet(x => x["aeg-sas-key"]).Returns("dummy-key");

        var handler = new SendEventGridMessageCommandHandler(configurationMock.Object, new Mock<ILogger<SendEventGridMessageCommandHandler>>().Object);
        handler.IsUnitTesting = true;

        var organisationDto = new OrganisationDto
        {
            Id = 3,
            OrganisationType = OrganisationType.LA,
            Name = "Grid Unit Test A County Council",
            Description = "Grid Unit Test A County Council",
            AdminAreaCode = "XTEST",
            Uri = new Uri("https://www.unittesta.gov.uk/").ToString(),
            Url = "https://www.unittesta.gov.uk/"
        };

        var request = new SendEventGridMessageCommand(organisationDto);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => handler.Handle(request, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_MissingAegSasKey_ThrowsArgumentException()
    {
        // Arrange
        var configurationMock = new Mock<IConfiguration>();
        configurationMock.SetupGet(x => x["EventGridUrl"]).Returns("http://example.com/eventgrid");
        configurationMock.SetupGet(x => x["aeg-sas-key"]).Returns(default(string));

        var handler = new SendEventGridMessageCommandHandler(configurationMock.Object, new Mock<ILogger<SendEventGridMessageCommandHandler>>().Object);
        handler.IsUnitTesting = true;

        var organisationDto = new OrganisationDto
        {
            Id = 3,
            OrganisationType = OrganisationType.LA,
            Name = "Grid Unit Test A County Council",
            Description = "Grid Unit Test A County Council",
            AdminAreaCode = "XTEST",
            Uri = new Uri("https://www.unittesta.gov.uk/").ToString(),
            Url = "https://www.unittesta.gov.uk/"
        };

        var request = new SendEventGridMessageCommand(organisationDto);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => handler.Handle(request, CancellationToken.None));
    }
}
