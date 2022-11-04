using AutoMapper;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.ModelLink;
using fh_service_directory_api.api.Commands.CreateModelLink;
using fh_service_directory_api.core;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace FamilyHubs.ServiceDirectoryApi.UnitTests.Locations;

public class WhenUsingModelLinkCommands : BaseCreateDbUnitTest
{
    [Fact]
    public async Task ThenCreateModelLink()
    {
        //Arrange
        var myProfile = new AutoMappingProfiles();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
        IMapper mapper = new Mapper(configuration);
        var logger = new Mock<ILogger<CreateModelLinkCommandHandler>>();
        var mockApplicationDbContext = GetApplicationDbContext();

        CreateModelLinkCommand command = new(new ModelLinkDto("d3ee9abb-6cbc-4ca8-9119-6ec3133c166b", "TestLink", "ModelOne", "ModelTwo"));
        CreateModelLinkCommandHandler handler = new(mockApplicationDbContext, mapper, logger.Object);

        //Act
        var result = await handler.Handle(command, new System.Threading.CancellationToken());

        //Assert
        result.Should().NotBeNullOrEmpty();
        result.Should().Be("d3ee9abb-6cbc-4ca8-9119-6ec3133c166b");
    }
}
