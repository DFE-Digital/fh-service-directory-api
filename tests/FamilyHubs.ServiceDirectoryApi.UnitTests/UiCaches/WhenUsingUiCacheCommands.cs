using Ardalis.GuardClauses;
using FamilyHubs.ServiceDirectory.Api.Commands.CreateUiCache;
using FamilyHubs.ServiceDirectory.Api.Commands.UpdateUiCache;
using FamilyHubs.ServiceDirectory.Api.Queries.GetUiCacheById;
using FamilyHubs.ServiceDirectory.Core.Entities;
using FamilyHubs.ServiceDirectory.Shared.Dto;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;

namespace FamilyHubs.ServiceDirectoryApi.UnitTests.UiCaches;

public class WhenUsingUiCacheCommands : BaseCreateDbUnitTest
{
       
    [Fact]
    public async Task ThenCreateUiCache()
    {
        //Arrange
        var mockApplicationDbContext = GetApplicationDbContext();
        var logger = new Mock<ILogger<CreateUiCacheCommandHandler>>();
        CreateUiCacheCommand command = new(new UICacheDto("6e23bc85-fff9-49f9-99e4-98160a9a2b56", TestViewModel.GetTestViewModel() ));
        CreateUiCacheCommandHandler handler = new(mockApplicationDbContext, logger.Object);

        //Act
        var result = await handler.Handle(command, new CancellationToken());

        //Assert
        result.Should().Be("6e23bc85-fff9-49f9-99e4-98160a9a2b56");
    }

    [Fact]
    public async Task ThenCreateUiCacheShouldThrowAnException()
    {
        //Arrange
        var mockApplicationDbContext = GetApplicationDbContext();
        var logger = new Mock<ILogger<CreateUiCacheCommandHandler>>();
        CreateUiCacheCommand command = new(new UICacheDto("6e23bc85-fff9-49f9-99e4-98160a9a2b56", default!));
        CreateUiCacheCommandHandler handler = new(mockApplicationDbContext, logger.Object);


        // Act
        //Assert
        await Assert.ThrowsAsync<DbUpdateException>(() => handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task ThenUpdateUiCache()
    {
        //Arrange
        const string id = "9ae3237d-73fd-46fc-afa3-f178250e0c09";
        var logger = new Mock<ILogger<UpdateUiCacheCommandHandler>>();
        var mockApplicationDbContext = GetApplicationDbContext();
        mockApplicationDbContext.UiCaches.Add(new UiCache(id, TestViewModel.GetTestViewModel()));
        await mockApplicationDbContext.SaveChangesAsync();

        var testViewModel = new TestViewModel
        {
            Id = new Guid("e5f0299a-a676-4d27-a4b9-1608c0d6d3db"),
            Name = "New Test View Model"
        };

        var newViewModel = JsonConvert.SerializeObject(testViewModel);

        UpdateUiCacheCommand command = new(id, new UICacheDto(id, newViewModel ));
        UpdateUiCacheCommandHandler handler = new(mockApplicationDbContext, logger.Object);

        //Act
        var result = await handler.Handle(command, new CancellationToken());

        //Assert
        result.Should().NotBeNull();
        result.Should().Be(id);
    }

    [Fact]
    public async Task ThenUpdateUiCacheShouldThrowANotFoundException()
    {
        //Arrange
        const string id = "9ae3237d-73fd-46fc-afa3-f178250e0c09";
        var logger = new Mock<ILogger<UpdateUiCacheCommandHandler>>();
        var mockApplicationDbContext = GetApplicationDbContext();

        UpdateUiCacheCommand command = new(id, new UICacheDto(id, default!));
        UpdateUiCacheCommandHandler handler = new(mockApplicationDbContext, logger.Object);


        // Act
        //Assert
        await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task ThenUpdateUiCacheShouldThrowAnException()
    {
        //Arrange
        const string id = "9ae3237d-73fd-46fc-afa3-f178250e0c09";
        var logger = new Mock<ILogger<UpdateUiCacheCommandHandler>>();
        var mockApplicationDbContext = GetApplicationDbContext();
        mockApplicationDbContext.UiCaches.Add(new UiCache(id, TestViewModel.GetTestViewModel()));
        await mockApplicationDbContext.SaveChangesAsync();

        UpdateUiCacheCommand command = new(id, default!); //new UICacheDto(id, newViewModel));
        UpdateUiCacheCommandHandler handler = new(mockApplicationDbContext, logger.Object);

        // Act
        //Assert
        await Assert.ThrowsAsync<NullReferenceException>(() => handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task ThenGetUiCacheById()
    {
        //Arrange
        const string id = "9ae3237d-73fd-46fc-afa3-f178250e0c09";
        var mockApplicationDbContext = GetApplicationDbContext();
        mockApplicationDbContext.UiCaches.Add(new UiCache(id, TestViewModel.GetTestViewModel()));
        await mockApplicationDbContext.SaveChangesAsync();
        var command = new GetUiCacheByIdCommand(id);
        GetUiCacheByIdCommandHandler handler = new(mockApplicationDbContext);

        //Act
        var result = await handler.Handle(command, new CancellationToken());

        //Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(id);
        result.Value.Should().Be("{\"Id\":\"e5f0299a-a676-4d27-a4b9-1608c0d6d3db\",\"Name\":\"Test View Model\"}");
    }
}

internal class TestViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;

    public static string GetTestViewModel()
    {
        var testViewModel = new TestViewModel
        {
            Id = new Guid("e5f0299a-a676-4d27-a4b9-1608c0d6d3db"),
            Name = "Test View Model"
        };

        return JsonConvert.SerializeObject(testViewModel);
    }
}
