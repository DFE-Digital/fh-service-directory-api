using AutoMapper;
using fh_service_directory_api.api.Queries.FxSearch;
using fh_service_directory_api.core;
using fh_service_directory_api.infrastructure.Persistence.Repository;
using FluentAssertions;
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
        var logger = new Mock<ILogger<ApplicationDbContextInitialiser>>();
        var mockApplicationDbContext = GetApplicationDbContext();
        ApplicationDbContextInitialiser applicationDbContextInitialiser = new ApplicationDbContextInitialiser(logger.Object, mockApplicationDbContext);
        await applicationDbContextInitialiser.SeedAsync();


        FxSearchCommand searchCommand = new("E08000006", -2.3D, 53.6D);
        FxSearchCommandHandler searchCommandHandler = new(mockApplicationDbContext);
        

        //Act
        var results = await searchCommandHandler.Handle(searchCommand, new CancellationToken());

        //Assert
        results.Should().NotBeNull();
        results.Count().Should().BeGreaterThan(2);
    }
}

