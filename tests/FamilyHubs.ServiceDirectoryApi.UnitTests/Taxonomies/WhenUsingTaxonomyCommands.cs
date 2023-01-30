using Ardalis.GuardClauses;
using AutoMapper;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralTaxonomys;
using FamilyHubs.SharedKernel.Interfaces;
using fh_service_directory_api.api.Commands.CreateOpenReferralTaxonomy;
using fh_service_directory_api.api.Commands.UpdateOpenReferralTaxonomy;
using fh_service_directory_api.api.Queries.GetOpenReferralTaxonomies;
using fh_service_directory_api.core;
using fh_service_directory_api.core.Entities;
using fh_service_directory_api.infrastructure.Persistence.Repository;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using StackExchange.Redis;

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
        var result = await handler.Handle(command, new CancellationToken());

        //Assert
        result.Should().NotBeNull();
        result.Should().Be(testTaxonomy.Id);
    }

    [Fact]
    public async Task ThenHandle_ShouldThrowArgumentNullException_WhenEntityIsNull()
    {
        // Arrange
        var context = GetApplicationDbContext();
        var myProfile = new AutoMappingProfiles();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
        IMapper mapper = new Mapper(configuration);
        var logger = new Logger<CreateOpenReferralTaxonomyCommandHandler>(new LoggerFactory());
        var handler = new CreateOpenReferralTaxonomyCommandHandler(context, mapper, logger);
        var command = new CreateOpenReferralTaxonomyCommand(default!);

        // Act
        Func<Task> act = () => handler.Handle(command, CancellationToken.None);
    
        //Assert
        var exception = await Assert.ThrowsAsync<Exception>(act);
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
        var result = await handler.Handle(command, new CancellationToken());

        //Assert
        result.Should().NotBeNull();
        result.Should().Be(testTaxonomy.Id);
    }

    [Fact]
    public async Task ThenHandle_ThrowsException_WhenTaxonomyNotFound()
    {
        // Arrange
        var dbContext = GetApplicationDbContext();
        var dbTaxonomy = new OpenReferralTaxonomy("a3226044-5c89-4257-8b07-f29745a22e2c", "Test 1 Taxonomy", "Test 1 Vocabulary", null);
        dbContext.OpenReferralTaxonomies.Add(dbTaxonomy);
        dbContext.SaveChanges();
        var logger = new Mock<ILogger<UpdateOpenReferralTaxonomyCommandHandler>>();
        var handler = new UpdateOpenReferralTaxonomyCommandHandler(dbContext, logger.Object);
        var command = new UpdateOpenReferralTaxonomyCommand("a3226044-5c89-4257-8b07-f29745a22e2c", default!);

        // Act
        Func<Task> act = () => handler.Handle(command, CancellationToken.None);

        //Assert
        var exception = await Assert.ThrowsAsync<Exception>(act);

    }

    [Fact]
    public async Task ThenHandle_ThrowsNotFoundException_WhenTaxonomyIdNotFound()
    {
        // Arrange
        var dbContext = GetApplicationDbContext();
        var logger = new Mock<ILogger<UpdateOpenReferralTaxonomyCommandHandler>>();
        var handler = new UpdateOpenReferralTaxonomyCommandHandler(dbContext, logger.Object);
        var command = new UpdateOpenReferralTaxonomyCommand("a3226044-5c89-4257-8b07-f29745a22e2c", default!);

        // Act
        Func<Task> act = () => handler.Handle(command, CancellationToken.None);

        //Assert
        var exception = await Assert.ThrowsAsync<NotFoundException>(act);

    }

    [Fact]
    public async Task ThenGetTaxonomies()
    {
        var mockApplicationDbContext = GetApplicationDbContext();
        var dbTaxonomy = new OpenReferralTaxonomy("a3226044-5c89-4257-8b07-f29745a22e2c", "Test 1 Taxonomy", "Test 1 Vocabulary", null);
        mockApplicationDbContext.OpenReferralTaxonomies.Add(dbTaxonomy);
        mockApplicationDbContext.SaveChanges();


        GetOpenReferralTaxonomiesCommand command = new(1, 1, "Test 1 Taxonomy");
        GetOpenReferralTaxonomiesCommandHandler handler = new(mockApplicationDbContext);

        //Act
        var result = await handler.Handle(command, new CancellationToken());

        //Assert
        result.Should().NotBeNull();
        result.Items[0].Id.Should().Be("a3226044-5c89-4257-8b07-f29745a22e2c");
        result.Items[0].Name.Should().Be("Test 1 Taxonomy");
        result.Items[0].Vocabulary.Should().Be("Test 1 Vocabulary");
    }

    [Fact]
    public async Task ThenGetTaxonomiesWithNullRequest()
    {
        var mockApplicationDbContext = GetApplicationDbContext();
        var dbTaxonomy = new OpenReferralTaxonomy("a3226044-5c89-4257-8b07-f29745a22e2c", "Test 1 Taxonomy", "Test 1 Vocabulary", null);
        mockApplicationDbContext.OpenReferralTaxonomies.Add(dbTaxonomy);
        mockApplicationDbContext.SaveChanges();
        GetOpenReferralTaxonomiesCommandHandler handler = new(mockApplicationDbContext);

        //Act
        var result = await handler.Handle(default!, new CancellationToken());

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
