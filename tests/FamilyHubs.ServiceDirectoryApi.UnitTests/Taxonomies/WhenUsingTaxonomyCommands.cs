using Ardalis.GuardClauses;
using AutoMapper;
using FamilyHubs.ServiceDirectory.Api.Commands.CreateTaxonomy;
using FamilyHubs.ServiceDirectory.Api.Commands.UpdateTaxonomy;
using FamilyHubs.ServiceDirectory.Api.Queries.GetTaxonomies;
using FamilyHubs.ServiceDirectory.Core;
using FamilyHubs.ServiceDirectory.Core.Entities;
using FamilyHubs.ServiceDirectory.Shared.Dto;
using FamilyHubs.ServiceDirectory.Shared.Enums;
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
        var mapper = new Mapper(configuration);
        var logger = new Mock<ILogger<CreateTaxonomyCommandHandler>>();
        var mockApplicationDbContext = GetApplicationDbContext();
        var testTaxonomy = GetTestTaxonomyDto();
        var command = new CreateTaxonomyCommand(testTaxonomy);
        var handler = new CreateTaxonomyCommandHandler(mockApplicationDbContext, mapper, logger.Object);

        //Act
        var result = await handler.Handle(command, new CancellationToken());

        //Assert
        result.Should().NotBe(0);
        result.Should().Be(testTaxonomy.Id);
    }

    [Fact]
    public async Task ThenHandle_ShouldThrowArgumentNullException_WhenEntityIsNull()
    {
        // Arrange
        var context = GetApplicationDbContext();
        var myProfile = new AutoMappingProfiles();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
        var mapper = new Mapper(configuration);
        var logger = new Logger<CreateTaxonomyCommandHandler>(new LoggerFactory());
        var handler = new CreateTaxonomyCommandHandler(context, mapper, logger);
        var command = new CreateTaxonomyCommand(default!);

        // Act
        //Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task ThenUpdateTaxonomy()
    {
        var mockApplicationDbContext = GetApplicationDbContext();
        var dbTaxonomy = new Taxonomy
        {
            Name = "Test 1 Taxonomy",
            TaxonomyType = TaxonomyType.ServiceCategory,
            ParentId = null
        };
        mockApplicationDbContext.Taxonomies.Add(dbTaxonomy);
        await mockApplicationDbContext.SaveChangesAsync();
        var testTaxonomy = GetTestTaxonomyDto();
        var logger = new Mock<ILogger<UpdateTaxonomyCommandHandler>>();

        var command = new UpdateTaxonomyCommand(dbTaxonomy.Id, testTaxonomy);
        var handler = new UpdateTaxonomyCommandHandler(mockApplicationDbContext, logger.Object);

        //Act
        var result = await handler.Handle(command, new CancellationToken());

        //Assert
        result.Should().NotBe(0);
        result.Should().Be(testTaxonomy.Id);
    }

    [Fact]
    public async Task ThenHandle_ThrowsException_WhenTaxonomyNotFound()
    {
        // Arrange
        var dbContext = GetApplicationDbContext();
        var dbTaxonomy = new Taxonomy
        {
            Name = "Test 1 Taxonomy",
            TaxonomyType = TaxonomyType.ServiceCategory,
            ParentId = null
        };
        dbContext.Taxonomies.Add(dbTaxonomy);
        await dbContext.SaveChangesAsync();
        var logger = new Mock<ILogger<UpdateTaxonomyCommandHandler>>();
        var handler = new UpdateTaxonomyCommandHandler(dbContext, logger.Object);
        var command = new UpdateTaxonomyCommand(dbTaxonomy.Id, default!);

        // Act
        //Assert
        await Assert.ThrowsAsync<NullReferenceException>(() => handler.Handle(command, CancellationToken.None));

    }

    [Fact]
    public async Task ThenHandle_ThrowsNotFoundException_WhenTaxonomyIdNotFound()
    {
        // Arrange
        var dbContext = GetApplicationDbContext();
        var logger = new Mock<ILogger<UpdateTaxonomyCommandHandler>>();
        var handler = new UpdateTaxonomyCommandHandler(dbContext, logger.Object);
        var command = new UpdateTaxonomyCommand(Random.Shared.Next(), default!);

        // Act
        //Assert
        await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, CancellationToken.None));

    }

    [Fact]
    public async Task ThenGetTaxonomies()
    {
        var mockApplicationDbContext = GetApplicationDbContext();
        var myProfile = new AutoMappingProfiles();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
        var mapper = new Mapper(configuration);
        var dbTaxonomy = new Taxonomy
        {
            Name = "Test 1 Taxonomy",
            TaxonomyType = TaxonomyType.ServiceCategory,
            ParentId = null
        };
        mockApplicationDbContext.Taxonomies.Add(dbTaxonomy);
        await mockApplicationDbContext.SaveChangesAsync();


        var command = new GetTaxonomiesCommand(TaxonomyType.NotSet, 1, null, null);
        var handler = new GetTaxonomiesCommandHandler(mockApplicationDbContext, mapper);

        //Act
        var result = await handler.Handle(command, new CancellationToken());

        //Assert
        result.Should().NotBeNull();
        result.Items[0].Name.Should().Be("Test 1 Taxonomy");
        result.Items[0].TaxonomyType.Should().Be(TaxonomyType.ServiceCategory);
    }

    [Fact]
    public async Task ThenGetTaxonomiesWithNullRequest()
    {
        var mockApplicationDbContext = GetApplicationDbContext();
        var myProfile = new AutoMappingProfiles();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
        var mapper = new Mapper(configuration);
        var dbTaxonomy = new Taxonomy
        {
            Name = "Test 1 Taxonomy",
            TaxonomyType = TaxonomyType.ServiceCategory,
            ParentId = null
        };
        mockApplicationDbContext.Taxonomies.Add(dbTaxonomy);
        await mockApplicationDbContext.SaveChangesAsync();
        var handler = new GetTaxonomiesCommandHandler(mockApplicationDbContext, mapper);

        //Act
        var result = await handler.Handle(new GetTaxonomiesCommand(TaxonomyType.NotSet, null, null, null), new CancellationToken());

        //Assert
        result.Should().NotBeNull();
        result.Items[0].Name.Should().Be("Test 1 Taxonomy");
        result.Items[0].TaxonomyType.Should().Be(TaxonomyType.ServiceCategory);
    }

    private static TaxonomyDto GetTestTaxonomyDto()
    {
        return new TaxonomyDto
        {
            Name = "Test Taxonomy",
            TaxonomyType = TaxonomyType.ServiceCategory,
            ParentId = null
        };
    }
}
