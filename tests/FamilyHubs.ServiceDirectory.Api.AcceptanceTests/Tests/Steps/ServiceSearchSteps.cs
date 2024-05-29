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
 

  public void GivenIPostToTheServiceSearchEndpointWithParameters(string radiusValue, string postcodeEntry, string postCodeEndpointResponseEntry,string searchTriggerEventId,string serviceSearchTypeId)
  {
      DateTime time = DateTime.UtcNow;
      var radius = int.Parse(radiusValue);
      string postcode = postcodeEntry;
      var postcodeEndpointStatusCode = int.Parse(postCodeEndpointResponseEntry);
      var searchTriggerEventIdEntry = int.Parse(searchTriggerEventId);
      var serviceSearchTypeIdEntry = int.Parse(serviceSearchTypeId);
      Console.WriteLine("Postcode Entry is: {0}", postcode);
      Console.WriteLine("Radius Entry is: {0}", radius);
      Console.WriteLine("Postcode Entry is: {0}", postcodeEndpointStatusCode);
      Console.WriteLine("Trigger Entry is: {0}", searchTriggerEventId);
      Console.WriteLine("Service Type is: {0}", serviceSearchTypeId);
      request = new ServiceSearchRequest()
      {
          searchPostcode = postcode,
          searchRadiusMiles = radius,
          userId = 0,
          httpResponseCode = postcodeEndpointStatusCode,
          requestTimestamp = time,
          responseTimestamp = time,
          correlationId = "",
          searchTriggerEventId = searchTriggerEventIdEntry,
          serviceSearchTypeId = serviceSearchTypeIdEntry,
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

  public void Then500StatusCodeReturned()
  {
      statusCode.Should().Be(System.Net.HttpStatusCode.InternalServerError,
          $"Response from {lastResponse.RequestMessage.Method} {lastResponse.RequestMessage.RequestUri} {statusCode} was not as expected");
  }
 
  public void Then400StatusCodeReturned()
  {
      statusCode.Should().Be(System.Net.HttpStatusCode.BadRequest,
          $"Response from {lastResponse.RequestMessage.Method} {lastResponse.RequestMessage.RequestUri} {statusCode} was not as expected");
  }

  #endregion Then

  #endregion Step Definitions
}
