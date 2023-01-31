using AutoMapper;
using FamilyHubs.ServiceDirectory.Api.Commands.CreateOrganisation;
using FamilyHubs.ServiceDirectory.Api.Commands.UpdateService;
using FamilyHubs.ServiceDirectory.Core;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;

namespace FamilyHubs.ServiceDirectoryApi.UnitTests.Organisation;

public class WhenUsingUpdateServiceCommand : BaseCreateDbUnitTest
{
    [Fact]
    public async Task ThenUpdateServiceOnly()
    {
        //Arange
        var myProfile = new AutoMappingProfiles();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
        IMapper mapper = new Mapper(configuration);
        var logger = new Mock<ILogger<CreateOrganisationCommandHandler>>();
        var mockApplicationDbContext = GetApplicationDbContext();
        var testOrganisation = WhenUsingOrganisationCommands.GetTestCountyCouncilDto();
        var updatelogger = new Mock<ILogger<UpdateServiceCommandHandler>>();
        var mockMediator = new Mock<ISender>();
        CreateOrganisationCommand command = new(testOrganisation);
        CreateOrganisationCommandHandler handler = new(mockApplicationDbContext, mapper, logger.Object);
        var id = await handler.Handle(command, new CancellationToken());

        var service = WhenUsingOrganisationCommands.GetTestCountyCouncilServicesDto2(testOrganisation.Id);


        service.Name = "Unit Test Update Service Name";
        service.Description = "Unit Test Update Service Name";
        UpdateServiceCommand updatecommand = new(service.Id, service);
        UpdateServiceCommandHandler updatehandler = new(mockApplicationDbContext, mapper, updatelogger.Object);

        //Act
        var result = await updatehandler.Handle(updatecommand, new CancellationToken());

        //Assert
        result.Should().NotBeNull();
        result.Should().Be(service.Id);
    }
}
