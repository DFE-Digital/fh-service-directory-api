﻿using Ardalis.GuardClauses;
using AutoMapper;
using FamilyHubs.ServiceDirectory.Api.Commands.CreateTaxonomy;
using FamilyHubs.ServiceDirectory.Api.Commands.UpdateTaxonomy;
using FamilyHubs.ServiceDirectory.Api.Queries.GetTaxonomies;
using FamilyHubs.ServiceDirectory.Core;
using FamilyHubs.ServiceDirectory.Core.Entities;
using FamilyHubs.ServiceDirectory.Shared.Dto;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace FamilyHubs.ServiceDirectoryApi.UnitTests.Taxonomies;

public class WhenUsingTaxonomyCommands : BaseCreateDbUnitTest
{
    [Fact]
    public async Task ThenCreateTaxonomy()
    {
        //Arrange
        var myProfile = new AutoMappingProfiles();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
        IMapper mapper = new Mapper(configuration);
        var logger = new Mock<ILogger<CreateTaxonomyCommandHandler>>();
        var mockApplicationDbContext = GetApplicationDbContext();
        var testTaxonomy = GetTestTaxonomyDto();
        CreateTaxonomyCommand command = new(testTaxonomy);
        CreateTaxonomyCommandHandler handler = new(mockApplicationDbContext, mapper, logger.Object);

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
        var logger = new Logger<CreateTaxonomyCommandHandler>(new LoggerFactory());
        var handler = new CreateTaxonomyCommandHandler(context, mapper, logger);
        var command = new CreateTaxonomyCommand(default!);

        // Act
        Func<Task> act = () => handler.Handle(command, CancellationToken.None);
    
        //Assert
        var exception = await Assert.ThrowsAsync<ArgumentNullException>(act);
    }

    [Fact]
    public async Task ThenUpdateTaxonomy()
    {
        var mockApplicationDbContext = GetApplicationDbContext();
        var dbTaxonomy = new Taxonomy("a3226044-5c89-4257-8b07-f29745a22e2c", "Test 1 Taxonomy", "Test 1 Vocabulary", null);
        mockApplicationDbContext.Taxonomies.Add(dbTaxonomy);
        await mockApplicationDbContext.SaveChangesAsync();
        var testTaxonomy = GetTestTaxonomyDto();
        var logger = new Mock<ILogger<UpdateTaxonomyCommandHandler>>();

        UpdateTaxonomyCommand command = new("a3226044-5c89-4257-8b07-f29745a22e2c", testTaxonomy);
        UpdateTaxonomyCommandHandler handler = new(mockApplicationDbContext, logger.Object);

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
        var dbTaxonomy = new Taxonomy("a3226044-5c89-4257-8b07-f29745a22e2c", "Test 1 Taxonomy", "Test 1 Vocabulary", null);
        dbContext.Taxonomies.Add(dbTaxonomy);
        dbContext.SaveChanges();
        var logger = new Mock<ILogger<UpdateTaxonomyCommandHandler>>();
        var handler = new UpdateTaxonomyCommandHandler(dbContext, logger.Object);
        var command = new UpdateTaxonomyCommand("a3226044-5c89-4257-8b07-f29745a22e2c", default!);

        // Act
        //Assert
        var exception = await Assert.ThrowsAsync<NullReferenceException>(() => handler.Handle(command, CancellationToken.None));

    }

    [Fact]
    public async Task ThenHandle_ThrowsNotFoundException_WhenTaxonomyIdNotFound()
    {
        // Arrange
        var dbContext = GetApplicationDbContext();
        var logger = new Mock<ILogger<UpdateTaxonomyCommandHandler>>();
        var handler = new UpdateTaxonomyCommandHandler(dbContext, logger.Object);
        var command = new UpdateTaxonomyCommand("a3226044-5c89-4257-8b07-f29745a22e2c", default!);

        // Act
        Func<Task> act = () => handler.Handle(command, CancellationToken.None);

        //Assert
        var exception = await Assert.ThrowsAsync<NotFoundException>(act);

    }

    [Fact]
    public async Task ThenGetTaxonomies()
    {
        var mockApplicationDbContext = GetApplicationDbContext();
        var dbTaxonomy = new Taxonomy("a3226044-5c89-4257-8b07-f29745a22e2c", "Test 1 Taxonomy", "Test 1 Vocabulary", null);
        mockApplicationDbContext.Taxonomies.Add(dbTaxonomy);
        await mockApplicationDbContext.SaveChangesAsync();


        GetTaxonomiesCommand command = new(1, 1, null);
        GetTaxonomiesCommandHandler handler = new(mockApplicationDbContext);

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
        var dbTaxonomy = new Taxonomy("a3226044-5c89-4257-8b07-f29745a22e2c", "Test 1 Taxonomy", "Test 1 Vocabulary", null);
        mockApplicationDbContext.Taxonomies.Add(dbTaxonomy);
        mockApplicationDbContext.SaveChanges();
        GetTaxonomiesCommandHandler handler = new(mockApplicationDbContext);

        //Act
        var result = await handler.Handle(default!, new CancellationToken());

        //Assert
        result.Should().NotBeNull();
        result.Items[0].Id.Should().Be("a3226044-5c89-4257-8b07-f29745a22e2c");
        result.Items[0].Name.Should().Be("Test 1 Taxonomy");
        result.Items[0].Vocabulary.Should().Be("Test 1 Vocabulary");
    }

    private static TaxonomyDto GetTestTaxonomyDto()
    {
        return new TaxonomyDto("a3226044-5c89-4257-8b07-f29745a22e2c", "Test Taxonomy", "Test Vocabulary", null);
    }
}
