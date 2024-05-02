using FamilyHubs.ServiceDirectory.Data.Repository;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace FamilyHubs.ServiceDirectory.Api.FunctionalTests;

#pragma warning disable S3881
public abstract class BaseWhenUsingApiUnitTests : IDisposable
{
    protected readonly HttpClient? Client;
    private readonly CustomWebApplicationFactory? _webAppFactory;
    private readonly bool _initSuccessful;
    public static string BearerTokenSigningKey;

    protected BaseWhenUsingApiUnitTests()
    {
        try
        {
            var configuration = new ConfigurationBuilder()
                .AddUserSecrets<Program>()
                .Build();

            _webAppFactory = new CustomWebApplicationFactory();
            _webAppFactory.SetupTestDatabaseAndSeedData();

            Client = _webAppFactory.CreateDefaultClient();
            Client.BaseAddress = new Uri("https://localhost:7128/");

            _initSuccessful = true;

            BearerTokenSigningKey = IsRunningLocally(configuration)
                ? configuration["GovUkOidcConfiguration:BearerTokenSigningKey"] : "StubPrivateKey123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        }
        catch
        {
            _initSuccessful = false;
        }
    }

    private bool IsRunningLocally(IConfiguration? configuration)
    {
        if (configuration == null)
        {
            return false;
        }

        try
        {
            string localMachineName = configuration["LocalSettings:MachineName"] ?? string.Empty;

            if (!string.IsNullOrEmpty(localMachineName))
            {
                return Environment.MachineName.Equals(localMachineName, StringComparison.OrdinalIgnoreCase);
            }
        }
        catch
        {
            return false;
        }

// Fallback to a default check if User Secrets file or machine name is not specified
// For example, you can add additional checks or default behavior here
        return false;
    }

    public void Dispose()
    {
        if (!_initSuccessful)
        {
            return;
        }

        if (_webAppFactory != null)
        {
            using var scope = _webAppFactory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            context.Database.EnsureDeleted();
        }

        if (Client != null)
        {
            Client.Dispose();
        }

        if (_webAppFactory != null)
        {
            _webAppFactory.Dispose();
        }

        GC.SuppressFinalize(this);
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
            RequestUri = new Uri($"{Client?.BaseAddress}{path}"),
        };

        if (!string.IsNullOrEmpty(role))
        {
            request.Headers.Add("Authorization", $"Bearer {TestDataProvider.CreateBearerToken(role, BearerTokenSigningKey)}");
        }

        return request;
    }
}

#pragma warning restore S3881
