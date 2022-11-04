using AutoMapper;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralLocations;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralPhysicalAddresses;
using FamilyHubs.ServiceDirectoryApi.UnitTests.Organisation;
using fh_service_directory_api.api.Commands.CreateLocation;
using fh_service_directory_api.api.Commands.CreateModelLink;
using fh_service_directory_api.api.Commands.CreateOpenReferralOrganisation;
using fh_service_directory_api.core;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;

namespace FamilyHubs.ServiceDirectoryApi.UnitTests.Locations;

public class WhenUsingLocationCommands : BaseCreateDbUnitTest
{
    [Fact]
    public async Task ThenCreateOpenReferralLocationWithFamilyHub()
    {
        //Arange
        var myProfile = new AutoMappingProfiles();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
        IMapper mapper = new Mapper(configuration);
        var logger = new Mock<ILogger<CreateOpenReferralOrganisationCommandHandler>>();
        var mockApplicationDbContext = GetApplicationDbContext();
        var testOrganisation = WhenUsingOrganisationCommands.GetTestCountyCouncilDto();
        CreateOpenReferralOrganisationCommand createOrgCommand = new(testOrganisation);
        CreateOpenReferralOrganisationCommandHandler orghandler = new(mockApplicationDbContext, mapper, logger.Object);
        var orgresult = await orghandler.Handle(createOrgCommand, new System.Threading.CancellationToken());
        var mockMediator = new Mock<ISender>();
        mockMediator.Setup(x => x.Send(It.IsAny<CreateModelLinkCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync("All Done");
        
        var addresses = new List<OpenReferralPhysicalAddressDto>() { new OpenReferralPhysicalAddressDto("e823d7ef-a9c4-4782-ad52-91e642ebb895", "Test Street", "Manchester", "M7 7BQ", "United Kingdom", "Salford") };
        OpenReferralLocationDto location = new OpenReferralLocationDto("0c1111fd-7817-49ae-b599-4d15e504fe8b", "Test Family Hub", "Test Hub", -2.359764D, 53.407025D, addresses);
        CreateOpenReferralLocationCommand command = new CreateOpenReferralLocationCommand(location, "d242700a-b2ad-42fe-8848-61534002156c", "56e62852-1b0b-40e5-ac97-54a67ea957dc");
        CreateOpenReferralLocationCommandHandler handler = new(mockApplicationDbContext, mapper, mockMediator.Object, new Mock<ILogger<CreateOpenReferralLocationCommandHandler>>().Object);

        //Act
        var result = await handler.Handle(command, new System.Threading.CancellationToken());

        //Assert
        result.Should().NotBeNullOrEmpty();
        result.Should().Be("0c1111fd-7817-49ae-b599-4d15e504fe8b");

    }
}
