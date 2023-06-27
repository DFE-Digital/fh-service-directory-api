using FamilyHubs.ServiceDirectory.Data.Repository;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Text;

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

    /// <summary>
    /// Creates HttpRequestMessage
    /// </summary>
    /// <param name="role">If left blank request will not have bearer Token</param>
    public HttpRequestMessage CreatePostRequest(string path, object content, string role = "")
    {
        var request = CreateHttpRequestMessage(HttpMethod.Post, path, role);
        request.Content = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");
        return request;
    }

    /// <summary>
    /// Creates HttpRequestMessage
    /// </summary>
    /// <param name="role">If left blank request will not have bearer Token</param>
    public HttpRequestMessage CreatePutRequest(string path, object content, string role = "")
    {
        var request = CreateHttpRequestMessage(HttpMethod.Put, path, role);
        request.Content = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");
        return request;
    }

    /// <summary>
    /// Creates HttpRequestMessage
    /// </summary>
    /// <param name="role">If left blank request will not have bearer Token</param>
    public HttpRequestMessage CreateDeleteRequest(string path, object content, string role = "")
    {
        var request = CreateHttpRequestMessage(HttpMethod.Delete, path, role);
        request.Content = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");
        return request;
    }

    /// <summary>
    /// Creates HttpRequestMessage
    /// </summary>
    /// <param name="role">If left blank request will not have bearer Token</param>
    public HttpRequestMessage CreateGetRequest(string path, string role = "")
    {
        var request = CreateHttpRequestMessage(HttpMethod.Get, path, role);
        return request;
    }

    private HttpRequestMessage CreateHttpRequestMessage(HttpMethod verb, string path, string role = "")
    {
        var request = new HttpRequestMessage
        {
            Method = verb,
            RequestUri = new Uri($"{Client.BaseAddress}{path}"),
        };

        if (!string.IsNullOrEmpty(role))
        {
            request.Headers.Add("Authorization", $"Bearer {TestDataProvider.CreateBearerToken(role)}");
        }

        return request;
    }
}
