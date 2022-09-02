using AutoMapper;
using FamilyHubs.ServiceDirectoryApi.UnitTests.Organisation;
using fh_service_directory_api.api.Commands.CreateOpenReferralOrganisation;
using fh_service_directory_api.api.Queries.GetServices;
using fh_service_directory_api.core;
using FluentAssertions;

namespace FamilyHubs.ServiceDirectoryApi.UnitTests.Services;

public class WhenUsingServiceCommands : BaseCreateDbUnitTest
{
    [Fact]
    public async Task ThenGetOpenReferralService()
    {
        //Arange
        var myProfile = new AutoMappingProfiles();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
        IMapper mapper = new Mapper(configuration);
        var mockApplicationDbContext = GetApplicationDbContext();
        var testOrganisation = WhenUsingOrganisationCommands.GetTestCountyCouncilDto();
        CreateOpenReferralOrganisationCommand createcommand = new(testOrganisation);
        CreateOpenReferralOrganisationCommandHandler createhandler = new(mockApplicationDbContext, mapper);
        var id = await createhandler.Handle(createcommand, new System.Threading.CancellationToken());


        GetOpenReferralServicesCommand command = new("active", null, null, null, null, null, 1, 10, null);
        GetOpenReferralServicesCommandHandler handler = new(mockApplicationDbContext);

        //Act
        var results = await handler.Handle(command, new System.Threading.CancellationToken());

        //Assert
        results.Should().NotBeNull();
        ArgumentNullException.ThrowIfNull(testOrganisation, nameof(testOrganisation));
        ArgumentNullException.ThrowIfNull(testOrganisation.Services, nameof(testOrganisation.Services));
        results.Items[0].Should().BeEquivalentTo(testOrganisation.Services.ElementAt(0));
    }
}
