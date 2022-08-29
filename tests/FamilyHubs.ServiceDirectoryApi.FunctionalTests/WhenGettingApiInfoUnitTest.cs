//using Microsoft.AspNetCore.Mvc.Testing;

//namespace FamilyHubs.ServiceDirectoryApi.FunctionalTests;

//[Collection("Sequential")]
//public class WhenGettingApiInfoUnitTest
//{
//    private readonly HttpClient _client;

//    public WhenGettingApiInfoUnitTest()
//    {
//        var webAppFactory = new WebApplicationFactory<oldProgram>();

//        _client = webAppFactory.CreateDefaultClient();
//        _client.BaseAddress = new Uri("https://localhost:7128/");
//    }


//    [Fact]
//    public async Task ThenReturnsVersionAndLastUpdateDate()
//    {
//        var response = await _client.GetAsync("api/info");
//        response.EnsureSuccessStatusCode();
//        var stringResponse = await response.Content.ReadAsStringAsync();

//        Assert.Contains("Version", stringResponse);
//        Assert.Contains("Last Updated", stringResponse);
//    }


//    //[Fact]
//    //public void Test1()
//    //{

//    //}
//}

///*
//public class WhenGettingApiInfoUnitTest : IClassFixture<CustomWebApplicationFactory<WebMarker>>
//{
//    private readonly HttpClient _client;

//    public WhenGettingApiInfoUnitTest(CustomWebApplicationFactory<WebMarker> factory)
//    {
//        _client = factory.CreateClient();
//        _client.BaseAddress = new Uri("https://localhost:7128/");
//    }

    
//    [Fact]
//    public async Task ThenReturnsVersionAndLastUpdateDate()
//    {
//        var response = await _client.GetAsync("info");
//        response.EnsureSuccessStatusCode();
//        var stringResponse = await response.Content.ReadAsStringAsync();

//        Assert.Contains("Version", stringResponse);
//        Assert.Contains("Last Updated", stringResponse);
//    }
    

//    //[Fact]
//    //public void Test1()
//    //{

//    //}
//}
//*/