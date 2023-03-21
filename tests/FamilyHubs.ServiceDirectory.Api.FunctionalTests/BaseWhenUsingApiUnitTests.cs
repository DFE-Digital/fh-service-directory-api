using FamilyHubs.ServiceDirectory.Data.Repository;
using FluentAssertions.Common;
using Microsoft.Extensions.DependencyInjection;

namespace FamilyHubs.ServiceDirectory.Api.FunctionalTests;

public abstract class BaseWhenUsingApiUnitTests : IDisposable
{
    protected readonly HttpClient Client;
    private readonly CustomWebApplicationFactory _webAppFactory;

    protected BaseWhenUsingApiUnitTests()
    {
        _webAppFactory = new CustomWebApplicationFactory();
        _webAppFactory.SetupTestDatabaseAndSeedData();

        Client = _webAppFactory.CreateDefaultClient();
        Client.BaseAddress = new Uri("https://localhost:7128/");
    }

    public void Dispose()
    {
        using var scope = _webAppFactory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        context.Database.EnsureDeleted();

        Client.Dispose();
        _webAppFactory.Dispose();
    }
}
