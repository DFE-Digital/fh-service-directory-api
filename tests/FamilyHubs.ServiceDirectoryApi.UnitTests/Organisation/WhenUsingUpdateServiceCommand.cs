using AutoMapper;
using fh_service_directory_api.api.Commands.CreateOpenReferralOrganisation;
using fh_service_directory_api.api.Commands.UpdateOpenReferralService;
using fh_service_directory_api.core;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;

namespace FamilyHubs.ServiceDirectoryApi.UnitTests.Organisation;

public class WhenUsingUpdateServiceCommand : BaseCreateDbUnitTest
{
    [Fact]
    public async Task ThenUpdateOpenReferralServiceOnly()
    {
        //Arange
        var myProfile = new AutoMappingProfiles();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
        IMapper mapper = new Mapper(configuration);
        var logger = new Mock<ILogger<CreateOpenReferralOrganisationCommandHandler>>();
        var mockApplicationDbContext = GetApplicationDbContext();
        var testOrganisation = WhenUsingOrganisationCommands.GetTestCountyCouncilDto();
        var updatelogger = new Mock<ILogger<UpdateOpenReferralServiceCommandHandler>>();
        var mockMediator = new Mock<ISender>();
        CreateOpenReferralOrganisationCommand command = new(testOrganisation);
        CreateOpenReferralOrganisationCommandHandler handler = new(mockApplicationDbContext, mapper, logger.Object);
        var id = await handler.Handle(command, new CancellationToken());

        var openReferralService = WhenUsingOrganisationCommands.GetTestCountyCouncilServicesDto(testOrganisation.Id);


        openReferralService.Name = "Unit Test Update Service Name";
        openReferralService.Description = "Unit Test Update Service Name";
        UpdateOpenReferralServiceCommand updatecommand = new(openReferralService.Id, openReferralService);
        UpdateOpenReferralServiceCommandHandler updatehandler = new(mockApplicationDbContext, mapper, updatelogger.Object);

        //Act
        var result = await updatehandler.Handle(updatecommand, new CancellationToken());

        //Assert
        result.Should().NotBeNull();
        result.Should().Be(openReferralService.Id);
    }
}
