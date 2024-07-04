using FluentAssertions;
using System.Net;
using FamilyHubs.ServiceDirectory.Api.AcceptanceTests.Configuration;
using FamilyHubs.ServiceDirectory.Api.AcceptanceTests.Models;
using FamilyHubs.ServiceDirectory.Api.AcceptanceTests.Builders.Http;

namespace FamilyHubs.ServiceDirectory.Api.AcceptanceTests.Tests.Steps;

/// <summary>
/// These are the steps required for testing the Postcodes endpoints
/// </summary>
public class ServiceSearchSteps
{
  private readonly string _baseUrl;
  private ServiceSearchRequest _request;
  private HttpResponseMessage _lastResponse;
  private HttpStatusCode _statusCode;
  private const string serviceSearchPath = "api/metrics/service-search";
  public ServiceSearchSteps()
  {
      _baseUrl = ConfigAccessor.GetApplicationConfiguration().BaseUrl;
  }
  private static string ResponseNotExpectedMessage(HttpMethod method, System.Uri requestUri, HttpStatusCode statusCode)
  {
      return $"Response from {method} {requestUri} {statusCode} was not as expected";
  }
  #region Step Definitions

  #region Given
 

  public void GivenIHaveASearchServiceRequest(string radiusValue, string postcodeEntry, string postCodeEndpointResponseEntry,string searchTriggerEventId,string serviceSearchTypeId)
  {
      DateTime time = DateTime.UtcNow;
      int radius = int.Parse(radiusValue);
      int postcodeEndpointStatusCode = int.Parse(postCodeEndpointResponseEntry);
      int searchTriggerEventIdEntry = int.Parse(searchTriggerEventId);
      int serviceSearchTypeIdEntry = int.Parse(serviceSearchTypeId);
      _request = new ServiceSearchRequest()
      {
          searchPostcode = postcodeEntry,
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
      _lastResponse = await HttpRequestFactory.Post(_baseUrl, serviceSearchPath, _request);
      _statusCode = _lastResponse.StatusCode;
    
      return _statusCode;
  }

  #endregion When

  #region Then
  
  public void ThenExpectedStatusCodeReturned(HttpStatusCode expectedStatusCode)
  {
      _statusCode.Should().Be(expectedStatusCode,
          ResponseNotExpectedMessage(
              _lastResponse.RequestMessage.Method,
              _lastResponse.RequestMessage.RequestUri,
              _statusCode));
  }

  #endregion Then

  #endregion Step Definitions
}
