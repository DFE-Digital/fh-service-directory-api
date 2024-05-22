using Newtonsoft.Json;
using FluentAssertions;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FamilyHubs.ServiceDirectory.Api.AcceptanceTests.Builders;
using FamilyHubs.ServiceDirectory.Api.AcceptanceTests.Configuration;
using FamilyHubs.ServiceDirectory.Api.AcceptanceTests.Models;
using FamilyHubs.ServiceDirectory.Api.AcceptanceTests.Builders.Http;

namespace FamilyHubs.ServiceDirectory.Api.AcceptanceTests.Tests.Steps;

/// <summary>
/// These are the steps required for testing the Postcodes endpoints
/// </summary>
public class ServiceSearchSteps
{
    readonly ConfigModel config;    
    private readonly string baseUrl;
    private ServiceSearchRequest request;
    private HttpResponseMessage lastResponse;
    private HttpStatusCode statusCode;
    private const string serviceSearchPath = "api/metrics/service-search";
    public ServiceSearchSteps()
    {
        config = ConfigAccessor.GetApplicationConfiguration();
        baseUrl = config.BaseUrl;
    }

    #region Step Definitions

    #region Given

    public void GivenIHaveAPostcodeToSearchWithinFind()
    {
        DateTime time = DateTime.UtcNow;

        request = new ServiceSearchRequest()
        {
            searchPostcode = "E1 2EN",
            searchRadiusMiles = 15,
            userId = 0,
            httpResponseCode = 200,
            requestTimestamp = time,
            responseTimestamp = time,
            correlationId = "",
            searchTriggerEventId = 1,
            serviceSearchTypeId = 1,
            serviceSearchResults = new List<ServiceSearchResults>()
        };
    }

    #endregion Given

    #region When

    public async Task<HttpStatusCode> WhenISendARequest()
    {
        lastResponse = await HttpRequestFactory.Post(baseUrl, serviceSearchPath, request);
        statusCode = lastResponse.StatusCode;  
        
        return statusCode; 
    }

    #endregion When

    #region Then

    public void Then200StatusCodeReturned()
    {
        statusCode.Should().Be(System.Net.HttpStatusCode.OK,
            $"Response from {lastResponse.RequestMessage.Method} {lastResponse.RequestMessage.RequestUri} {statusCode} was not as expected");
    }

    #endregion Then

    #endregion Step Definitions
}