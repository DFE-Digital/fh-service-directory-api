using FamilyHubs.ServiceDirectory.Shared.Models.Api;
using FluentAssertions;
using FunctionalTests;
using System.Text;
using System.Text.Json;

namespace FamilyHubs.ServiceDirectoryApi.FunctionalTests;

[Collection("Sequential")]
public class WhenUsingUICacheApiUnitTests : BaseWhenUsingOpenReferralApiUnitTests
{
    [Fact]
    public async Task ThenTheUICacheIsCreated()
    {
        var command = new UICacheDto
        {
            Id = Guid.NewGuid().ToString(), 
            Value = GetTestViewModel()
        };

        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri(_client.BaseAddress + "api/uicaches"),
            Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(command), Encoding.UTF8, "application/json"),
        };

        using var response = await _client.SendAsync(request);

        response.EnsureSuccessStatusCode();

        var stringResult = await response.Content.ReadAsStringAsync();

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        stringResult.ToString().Should().Be(command.Id);
    }

    [Fact]
    public async Task ThenTheUICacheIsUpdated()
    {
        string id = await CreateUICache();

        var testViewModel = new TestViewModel
        {
            Id = new Guid("e5f0299a-a676-4d27-a4b9-1608c0d6d3db"),
            Name = "Name Changed View Model"
        };

        var value = Newtonsoft.Json.JsonConvert.SerializeObject(testViewModel);

        var command = new UICacheDto
        {
            Id = id,
            Value = value
        };

        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Put,
            RequestUri = new Uri(_client.BaseAddress + $"api/uicaches/{id}"),
            Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(command), Encoding.UTF8, "application/json"),
        };

        using var response = await _client.SendAsync(request);

        response.EnsureSuccessStatusCode();

        var stringResult = await response.Content.ReadAsStringAsync();

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        stringResult.ToString().Should().Be(command.Id);
    }

    [Fact]
    public async Task ThenGetUICacheById()
    {
        string id = await CreateUICache();

        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri(_client.BaseAddress + $"api/uicaches/{id}"),            
        };

        using var response = await _client.SendAsync(request);

        response.EnsureSuccessStatusCode();

        var retVal = await JsonSerializer.DeserializeAsync<UICacheDto>(await response.Content.ReadAsStreamAsync(), options: new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        retVal.Should().NotBeNull();
        ArgumentNullException.ThrowIfNull(retVal, nameof(retVal));
        retVal.Id.Should().Be(id);
    }

    private async Task<string> CreateUICache()
    {
        var command = new UICacheDto
        {
            Id = Guid.NewGuid().ToString(),
            Value = GetTestViewModel()
        };

        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri(_client.BaseAddress + "api/uicaches"),
            Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(command), Encoding.UTF8, "application/json"),
        };

        using var response = await _client.SendAsync(request);

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsStringAsync();
    }


    private static string GetTestViewModel()
    {
        var testViewModel = new TestViewModel
        {
            Id = new Guid("e5f0299a-a676-4d27-a4b9-1608c0d6d3db"),
            Name = "Test View Model"
        };

        return Newtonsoft.Json.JsonConvert.SerializeObject(testViewModel);
    }
}

internal class TestViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
}
