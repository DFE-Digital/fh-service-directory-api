using AutoMapper;
using fh_service_directory_api.api.Queries.FxSearch;
using fh_service_directory_api.core;
using fh_service_directory_api.core.Entities;
using fh_service_directory_api.infrastructure.Persistence.Repository;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace FamilyHubs.ServiceDirectoryApi.UnitTests.Services;

public class WhenUsingFxSearchCommands : BaseCreateDbUnitTest
{
    //Becoming Obsolete

    [Fact]
    public async Task ThenGetFamilyHubs()
    {
        //Arange
        var myProfile = new AutoMappingProfiles();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
        IMapper mapper = new Mapper(configuration);
        var logger = new Mock<ILogger<ApplicationDbContextInitialiser>>();
        var mockApplicationDbContext = GetApplicationDbContext();
        ApplicationDbContextInitialiser applicationDbContextInitialiser = new(logger.Object, mockApplicationDbContext);
        await applicationDbContextInitialiser.SeedAsync();
        mockApplicationDbContext.ModelLinks.AddRange(SeedModelLinks());
        await mockApplicationDbContext.SaveChangesAsync();

        FxSearchCommand searchCommand = new("E08000006", -2.3D, 53.6D);
        FxSearchCommandHandler searchCommandHandler = new(mockApplicationDbContext);
        

        //Act
        var results = await searchCommandHandler.Handle(searchCommand, new CancellationToken());

        //Assert
        results.Should().NotBeNull();
        results.Where(r => r.First != null).Count().Should().Be(3); // Services
        results.Where(r => r.Second != null).Count().Should().Be(3);    // Family Hubs
    }

    private IReadOnlyCollection<ModelLink> SeedModelLinks()
    {
        List<ModelLink> modelLinks = new()
        {
            new ModelLink("ebe8647d-e06d-478d-a6af-948dd7a289c7", fh_service_directory_api.core.StaticContants.Location_Taxonomy, "964ea451-6146-4add-913e-dff23a1bd7b6", "d242700a-b2ad-42fe-8848-61534002156c"),
            new ModelLink("760f05de-07df-4e58-a429-20922416b221", fh_service_directory_api.core.StaticContants.Location_Taxonomy, "74c37f53-dbc0-4958-8c97-baee41a022bf", "d242700a-b2ad-42fe-8848-61534002156c"),
            new ModelLink("9c8df56e-bebf-486f-8544-39f8deccbe94", fh_service_directory_api.core.StaticContants.Location_Taxonomy, "1b4a625b-54bb-407d-a508-f90cade1e96f", "d242700a-b2ad-42fe-8848-61534002156c"),

            new ModelLink("dc5e2bdb-ed5a-4278-b3a3-f0622502b5e4", fh_service_directory_api.core.StaticContants.Location_Organisation, "964ea451-6146-4add-913e-dff23a1bd7b6", "ca8ddaeb-b5e5-46c4-b94d-43a8e2ccc066"),
            new ModelLink("4cb330f6-f68f-4529-8a55-dffa8b9d48af", fh_service_directory_api.core.StaticContants.Location_Organisation, "74c37f53-dbc0-4958-8c97-baee41a022bf", "ca8ddaeb-b5e5-46c4-b94d-43a8e2ccc066"),
            new ModelLink("06bf080c-b161-4fc3-b015-81535ced46d8", fh_service_directory_api.core.StaticContants.Location_Organisation, "1b4a625b-54bb-407d-a508-f90cade1e96f", "ca8ddaeb-b5e5-46c4-b94d-43a8e2ccc066"),

        };

        return modelLinks;
    }
}

