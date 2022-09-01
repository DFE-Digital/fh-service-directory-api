using FamilyHubs.ServiceDirectory.Shared.Models.Api;
using fh_service_directory_api.api.Commands.CreateUICache;
using fh_service_directory_api.api.Commands.UpdateUICache;
using fh_service_directory_api.api.Queries.GetUICacheById;
using fh_service_directory_api.core.Entities;
using FluentAssertions;

namespace FamilyHubs.ServiceDirectoryApi.UnitTests.UICaches;

public class WhenUsingUICacheCommands : BaseCreateDbUnitTest
{
       
    [Fact]
    public async Task ThenCreateUICache()
    {
        //Arange
        var mockApplicationDbContext = GetApplicationDbContext();
        CreateUICacheCommand command = new(new UICacheDto { Id = "6e23bc85-fff9-49f9-99e4-98160a9a2b56", Value = TestViewModel.GetTestViewModel() });
        CreateUICacheCommandHandler handler = new(mockApplicationDbContext);

        //Act
        var result = await handler.Handle(command, new System.Threading.CancellationToken());

        //Assert
        result.Should().Be("6e23bc85-fff9-49f9-99e4-98160a9a2b56");
    }

    [Fact]
    public async Task ThenUpdateUICache()
    {
        //Arange
        const string id = "9ae3237d-73fd-46fc-afa3-f178250e0c09";
        var mockApplicationDbContext = GetApplicationDbContext();
        mockApplicationDbContext.UICaches.Add(new UICache(id, TestViewModel.GetTestViewModel()));
        mockApplicationDbContext.SaveChanges();

        var testViewModel = new TestViewModel
        {
            Id = new Guid("e5f0299a-a676-4d27-a4b9-1608c0d6d3db"),
            Name = "New Test View Model"
        };

        var newViewModel = Newtonsoft.Json.JsonConvert.SerializeObject(testViewModel);

        UpdateUICacheCommand command = new(id, new UICacheDto { Id = id, Value = newViewModel });
        UpdateUICacheCommandHandler handler = new(mockApplicationDbContext);

        //Act
        var result = await handler.Handle(command, new System.Threading.CancellationToken());

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
        mockApplicationDbContext.SaveChanges();
        var command = new GetUICacheByIdCommand(id);
        GetUICacheByIdCommandHandler handler = new(mockApplicationDbContext);

        //Act
        UICacheDto result = await handler.Handle(command, new System.Threading.CancellationToken());

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

        return Newtonsoft.Json.JsonConvert.SerializeObject(testViewModel);
    }
}
