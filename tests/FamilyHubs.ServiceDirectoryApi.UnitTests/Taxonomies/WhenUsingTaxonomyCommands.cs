using AutoMapper;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralTaxonomys;
using fh_service_directory_api.api.Commands.CreateOpenReferralTaxonomy;
using fh_service_directory_api.api.Commands.UpdateOpenReferralTaxonomy;
using fh_service_directory_api.api.Queries.GetOpenReferralTaxonomies;
using fh_service_directory_api.core;
using fh_service_directory_api.core.Entities;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace FamilyHubs.ServiceDirectoryApi.UnitTests.Taxonomies;

public class WhenUsingTaxonomyCommands : BaseCreateDbUnitTest
{
    [Fact]
    public async Task ThenCreateTaxonomy()
    {
        //Arange
        var myProfile = new AutoMappingProfiles();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
        IMapper mapper = new Mapper(configuration);
        var logger = new Mock<ILogger<CreateOpenReferralTaxonomyCommandHandler>>();
        var mockApplicationDbContext = GetApplicationDbContext();
        var testTaxonomy = GetTestTaxonomyDto();
        CreateOpenReferralTaxonomyCommand command = new(testTaxonomy);
        CreateOpenReferralTaxonomyCommandHandler handler = new(mockApplicationDbContext, mapper, logger.Object);

        //Act
        var result = await handler.Handle(command, new System.Threading.CancellationToken());

        //Assert
        result.Should().NotBeNull();
        result.Should().Be(testTaxonomy.Id);
    }

    [Fact]
    public async Task ThenUpdateTaxonomy()
    {
        var mockApplicationDbContext = GetApplicationDbContext();
        var dbTaxonomy = new OpenReferralTaxonomy("a3226044-5c89-4257-8b07-f29745a22e2c", "Test 1 Taxonomy", "Test 1 Vocabulary", null);
        mockApplicationDbContext.OpenReferralTaxonomies.Add(dbTaxonomy);
        mockApplicationDbContext.SaveChanges();
        var testTaxonomy = GetTestTaxonomyDto();
        var logger = new Mock<ILogger<UpdateOpenReferralTaxonomyCommandHandler>>();

        UpdateOpenReferralTaxonomyCommand command = new("a3226044-5c89-4257-8b07-f29745a22e2c", testTaxonomy);
        UpdateOpenReferralTaxonomyCommandHandler handler = new(mockApplicationDbContext, logger.Object);

        //Act
        var result = await handler.Handle(command, new System.Threading.CancellationToken());

        //Assert
        result.Should().NotBeNull();
        result.Should().Be(testTaxonomy.Id);
    }

    [Fact]
    public async Task ThenGetTaxonomies()
    {
        var mockApplicationDbContext = GetApplicationDbContext();
        var dbTaxonomy = new OpenReferralTaxonomy("a3226044-5c89-4257-8b07-f29745a22e2c", "Test 1 Taxonomy", "Test 1 Vocabulary", null);
        mockApplicationDbContext.OpenReferralTaxonomies.Add(dbTaxonomy);
        mockApplicationDbContext.SaveChanges();


        GetOpenReferralTaxonomiesCommand command = new(1, 1, null);
        GetOpenReferralTaxonomiesCommandHandler handler = new(mockApplicationDbContext);

        //Act
        var result = await handler.Handle(command, new System.Threading.CancellationToken());

        //Assert
        result.Should().NotBeNull();
        result.Items[0].Id.Should().Be("a3226044-5c89-4257-8b07-f29745a22e2c");
        result.Items[0].Name.Should().Be("Test 1 Taxonomy");
        result.Items[0].Vocabulary.Should().Be("Test 1 Vocabulary");
    }

    private static OpenReferralTaxonomyDto GetTestTaxonomyDto()
    {
        return new OpenReferralTaxonomyDto("a3226044-5c89-4257-8b07-f29745a22e2c", "Test Taxonomy", "Test Vocabulary", null);
    }
}
