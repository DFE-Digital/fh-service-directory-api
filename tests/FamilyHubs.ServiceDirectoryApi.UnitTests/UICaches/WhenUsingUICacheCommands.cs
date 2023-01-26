using FamilyHubs.ServiceDirectory.Api.Commands.CreateUICache;
using FamilyHubs.ServiceDirectory.Api.Commands.UpdateUICache;
using FamilyHubs.ServiceDirectory.Api.Queries.GetUICacheById;
using FamilyHubs.ServiceDirectory.Core.Entities;
using FamilyHubs.ServiceDirectory.Shared.Dto;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;

namespace FamilyHubs.ServiceDirectoryApi.UnitTests.UICaches;

public class WhenUsingUICacheCommands : BaseCreateDbUnitTest
{
       
    [Fact]
    public async Task ThenCreateUICache()
    {
        //Arange
        var mockApplicationDbContext = GetApplicationDbContext();
        var logger = new Mock<ILogger<CreateUICacheCommandHandler>>();
        CreateUICacheCommand command = new(new UICacheDto("6e23bc85-fff9-49f9-99e4-98160a9a2b56", TestViewModel.GetTestViewModel() ));
        CreateUICacheCommandHandler handler = new(mockApplicationDbContext, logger.Object);

        //Act
        var result = await handler.Handle(command, new CancellationToken());

        //Assert
        result.Should().Be("6e23bc85-fff9-49f9-99e4-98160a9a2b56");
    }

    [Fact]
    public async Task ThenUpdateUICache()
    {
        //Arange
        const string id = "9ae3237d-73fd-46fc-afa3-f178250e0c09";
        var logger = new Mock<ILogger<UpdateUICacheCommandHandler>>();
        var mockApplicationDbContext = GetApplicationDbContext();
        mockApplicationDbContext.UICaches.Add(new UICache(id, TestViewModel.GetTestViewModel()));
        await mockApplicationDbContext.SaveChangesAsync();

        var testViewModel = new TestViewModel
        {
            Id = new Guid("e5f0299a-a676-4d27-a4b9-1608c0d6d3db"),
            Name = "New Test View Model"
        };

        var newViewModel = JsonConvert.SerializeObject(testViewModel);

        UpdateUICacheCommand command = new(id, new UICacheDto(id, newViewModel ));
        UpdateUICacheCommandHandler handler = new(mockApplicationDbContext, logger.Object);

        //Act
        var result = await handler.Handle(command, new CancellationToken());

        //Assert
        result.Should().NotBeNull();
        result.Should().Be(id);
    }

    [Fact]
    public async Task ThenGetUICacheById()
    {
        //Arange
        const string id = "9ae3237d-73fd-46fc-afa3-f178250e0c09";
        var mockApplicationDbContext = GetApplicationDbContext();
        mockApplicationDbContext.UICaches.Add(new UICache(id, TestViewModel.GetTestViewModel()));
        await mockApplicationDbContext.SaveChangesAsync();
        var command = new GetUICacheByIdCommand(id);
        GetUICacheByIdCommandHandler handler = new(mockApplicationDbContext);

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
