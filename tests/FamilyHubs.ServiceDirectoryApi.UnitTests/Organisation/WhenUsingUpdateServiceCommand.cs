using AutoMapper;
using FamilyHubs.ServiceDirectory.Api.Commands.CreateOrganisation;
using FamilyHubs.ServiceDirectory.Api.Commands.CreateService;
using FamilyHubs.ServiceDirectory.Api.Commands.UpdateService;
using FamilyHubs.ServiceDirectory.Core;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
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
        var mapper = new Mapper(configuration);
        var logger = new Mock<ILogger<CreateOrganisationCommandHandler>>();
        var mockApplicationDbContext = GetApplicationDbContext();
        
        var mockMediator = new Mock<ISender>();
        var createServiceCommandHandler = new CreateServiceCommandHandler(mockApplicationDbContext, mapper, NullLogger<CreateServiceCommandHandler>.Instance);
        var updateServiceCommandHandler = new UpdateServiceCommandHandler(mockApplicationDbContext, mapper, NullLogger<UpdateServiceCommandHandler>.Instance);
        mockMediator.Setup(m => m.Send(It.IsAny<CreateServiceCommand>(), It.IsAny<CancellationToken>()))
            .Callback<object, CancellationToken>((notification, cToken) => 
                createServiceCommandHandler.Handle((CreateServiceCommand)notification, cToken).GetAwaiter().GetResult());

        mockMediator.Setup(m => m.Send(It.IsAny<UpdateServiceCommand>(), It.IsAny<CancellationToken>()))
            .Callback<object, CancellationToken>((notification, cToken) => 
                updateServiceCommandHandler.Handle((UpdateServiceCommand)notification, cToken).GetAwaiter().GetResult());
        
        var testOrganisation = TestDataProvider.GetTestCountyCouncilDto();
        var updateLogger = new Mock<ILogger<UpdateServiceCommandHandler>>();
        var command = new CreateOrganisationCommand(testOrganisation);
        var handler = new CreateOrganisationCommandHandler(mockApplicationDbContext, mapper, mockMediator.Object, logger.Object);
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
