using AutoMapper;
using FamilyHubs.ServiceDirectory.Api.Commands.CreateOrganisation;
using FamilyHubs.ServiceDirectory.Api.Commands.UpdateService;
using FamilyHubs.ServiceDirectory.Core;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace FamilyHubs.ServiceDirectoryApi.UnitTests.Organisation;

public class WhenUsingUpdateServiceCommand : BaseCreateDbUnitTest
{
    [Fact]
    public async Task ThenUpdateServiceOnly()
    {
        //Arrange
        var myProfile = new AutoMappingProfiles();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
        IMapper mapper = new Mapper(configuration);
        var logger = new Mock<ILogger<CreateOrganisationCommandHandler>>();
        var mockApplicationDbContext = GetApplicationDbContext();
        var testOrganisation = TestDataProvider.GetTestCountyCouncilDto(false, false, true);
        var updateLogger = new Mock<ILogger<UpdateServiceCommandHandler>>();
        var command = new CreateOrganisationCommand(testOrganisation);
        var handler = new CreateOrganisationCommandHandler(mockApplicationDbContext, mapper, logger.Object);
        await handler.Handle(command, new CancellationToken());

        var service = TestDataProvider.GetTestCountyCouncilServicesDto(testOrganisation.Id);


        service.Name = "Unit Test Update Service Name";
        service.Description = "Unit Test Update Service Name";
        var updateCommand = new UpdateServiceCommand(service.Id, service);
        var updateHandler = new UpdateServiceCommandHandler(mockApplicationDbContext, mapper, updateLogger.Object);

        //Act
        var result = await updateHandler.Handle(updateCommand, new CancellationToken());

        //Assert
        result.Should().NotBeNull();
        result.Should().Be(service.Id);
    }
}
