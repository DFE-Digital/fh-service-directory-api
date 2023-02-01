using System.Net;
using System.Text;
using System.Text.Json;
using FamilyHubs.ServiceDirectory.Shared.Dto;
using FluentAssertions;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace FamilyHubs.ServiceDirectoryApi.FunctionalTests;

[Collection("Sequential")]
public class WhenUsingUICacheApiUnitTests : BaseWhenUsingApiUnitTests
{
#if DEBUG
    [Fact]
#else
    [Fact(Skip = "This test should be run locally")]
#endif
    public async Task ThenTheUICacheIsCreated()
    {
        var command = new UICacheDto(Guid.NewGuid().ToString(), GetTestViewModel());

        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri(_client.BaseAddress + "api/uicaches"),
            Content = new StringContent(JsonConvert.SerializeObject(command), Encoding.UTF8, "application/json"),
        };

        //request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue($"Bearer", $"{new JwtSecurityTokenHandler().WriteToken(_token)}");

        using var response = await _client.SendAsync(request);

        response.EnsureSuccessStatusCode();

        var stringResult = await response.Content.ReadAsStringAsync();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        stringResult.Should().Be(command.Id);
    }

#if DEBUG
    [Fact]
#else
    [Fact(Skip = "This test should be run locally")]
#endif
    public async Task ThenTheUICacheIsUpdated()
    {
        var id = await CreateUICache();

        var testViewModel = new TestViewModel
        {
            Id = new Guid("e5f0299a-a676-4d27-a4b9-1608c0d6d3db"),
            Name = "Name Changed View Model"
        };

        var value = JsonConvert.SerializeObject(testViewModel);

        var command = new UICacheDto(id, value);
        
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Put,
            RequestUri = new Uri(_client.BaseAddress + $"api/uicaches/{id}"),
            Content = new StringContent(JsonConvert.SerializeObject(command), Encoding.UTF8, "application/json"),
        };

        //request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue($"Bearer", $"{new JwtSecurityTokenHandler().WriteToken(_token)}");

        using var response = await _client.SendAsync(request);

        response.EnsureSuccessStatusCode();

        var stringResult = await response.Content.ReadAsStringAsync();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        stringResult.Should().Be(command.Id);
    }

#if DEBUG
    [Fact]
#else
    [Fact(Skip = "This test should be run locally")]
#endif
    public async Task ThenGetUICacheById()
    {
        var id = await CreateUICache();

        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri(_client.BaseAddress + $"api/uicaches/{id}"),            
        };

        //request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue($"Bearer", $"{new JwtSecurityTokenHandler().WriteToken(_token)}");

        using var response = await _client.SendAsync(request);

        response.EnsureSuccessStatusCode();

        var retVal = await JsonSerializer.DeserializeAsync<UICacheDto>(await response.Content.ReadAsStreamAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        retVal.Should().NotBeNull();
        ArgumentNullException.ThrowIfNull(retVal);
        retVal.Id.Should().Be(id);
    }

    private async Task<string> CreateUICache()
    {
        var command = new UICacheDto(Guid.NewGuid().ToString(), GetTestViewModel());

        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri(_client.BaseAddress + "api/uicaches"),
            Content = new StringContent(JsonConvert.SerializeObject(command), Encoding.UTF8, "application/json"),
        };

        //request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue($"Bearer", $"{new JwtSecurityTokenHandler().WriteToken(_token)}");

        var response = await _client.SendAsync(request);

        var contentString = await response.Content.ReadAsStringAsync();

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

        return JsonConvert.SerializeObject(testViewModel);
    }
}

internal class TestViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
}
