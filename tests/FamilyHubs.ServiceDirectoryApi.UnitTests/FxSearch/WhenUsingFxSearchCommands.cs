using AutoMapper;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralLocations;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralPhysicalAddresses;
using FamilyHubs.ServiceDirectoryApi.UnitTests.Locations;
using FamilyHubs.ServiceDirectoryApi.UnitTests.Organisation;
using fh_service_directory_api.api.Commands.CreateModelLink;
using fh_service_directory_api.api.Commands.CreateOpenReferralOrganisation;
using fh_service_directory_api.api.Queries.FxSearch;
using fh_service_directory_api.api.Queries.GetServices;
using fh_service_directory_api.core;
using fh_service_directory_api.infrastructure.Persistence.Repository;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;

namespace FamilyHubs.ServiceDirectoryApi.UnitTests.Services;

public class WhenUsingFxSearchCommands : BaseCreateDbUnitTest
{
    [Fact]
    public async Task ThenGetFamilyHubs()
    {
        //Arange
        var myProfile = new AutoMappingProfiles();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
        IMapper mapper = new Mapper(configuration);
        var logger = new Mock<ILogger<FxSearchCommandHandler>>();
        var mockApplicationDbContext = GetApplicationDbContext();

        var testLocation = await GetTestFamilyHub(mockApplicationDbContext, mapper);

        FxSearchCommand searchCommand = new("XTEST", 1.0, 2.0);
        FxSearchCommandHandler searchCommandHandler = new(mockApplicationDbContext);

        //Act
        var results = await searchCommandHandler.Handle(searchCommand, new CancellationToken());

        //Assert
        results.Should().NotBeNull();

        results[0].First.Should().NotBeNull();
        results[0].Second.Should().NotBeNull();

        results.Count().Should().Be(2);
        //results.Items[0].Should().BeEquivalentTo(testOrganisation.Services.ElementAt(0));
    }

    public async Task<OpenReferralLocationDto> GetTestFamilyHub(ApplicationDbContext mockApplicationDbContext, IMapper mapper)
    {
        var logger = new Mock<ILogger<CreateOpenReferralOrganisationCommandHandler>>();
        var testOrganisation = WhenUsingOrganisationCommands.GetTestCountyCouncilDto();
        CreateOpenReferralOrganisationCommand createOrgCommand = new(testOrganisation);
        CreateOpenReferralOrganisationCommandHandler orghandler = new(mockApplicationDbContext, mapper, logger.Object);
        
        var orgresult = await orghandler.Handle(createOrgCommand, new System.Threading.CancellationToken());

        var mockMediator = new Mock<ISender>();
        mockMediator.Setup(x => x.Send(It.IsAny<CreateModelLinkCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync("All Done");

        var addresses = new List<OpenReferralPhysicalAddressDto>() { new OpenReferralPhysicalAddressDto("e823d7ef-a9c4-4782-ad52-91e642ebb895", "Test Street", "Manchester", "M7 7BQ", "United Kingdom", "Salford") };
        OpenReferralLocationDto location = new OpenReferralLocationDto("0c1111fd-7817-49ae-b599-4d15e504fe8b", "Test Family Hub", "Test Hub", -2.359764D, 53.407025D, addresses);

        return location;
    }
}

