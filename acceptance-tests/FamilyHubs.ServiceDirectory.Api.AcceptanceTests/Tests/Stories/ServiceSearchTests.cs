using System.Net;
using TestStack.BDDfy;
using FamilyHubs.ServiceDirectory.Api.AcceptanceTests.Tests.Steps;
using Xunit;

namespace FamilyHubs.ServiceDirectory.Api.AcceptanceTests.Tests.Stories;

// Define the story/feature being tested
[Story(
    AsA = "user of the metrics api",
    IWant = "to be able to update usage information for postcode searches and filters",
    SoThat = "I can record how search functionality is being used")]
[TestClass]
public class ServiceSearchTests
{
    private readonly ServiceSearchSteps _steps;

    public ServiceSearchTests()
    {
        //Get instances of the steps required for the test
        _steps = new ServiceSearchSteps();
    }

    //Add all tests that make up the story to this class.
    [Theory]
    [InlineData("20", "E1 2EN", "200", "1", "2", HttpStatusCode.OK)] // As a Find user
    [InlineData("5", "E1 2EN", "401", "2", "2", HttpStatusCode.OK)] // As a Find user subsequent search
    [InlineData("20", "E1 2EN", "500", "1", "1", HttpStatusCode.OK)] // As a Connect user
    [InlineData("0", "E1 2EN", "400", "2", "1", HttpStatusCode.OK)] // As a Connect user subsequent search
    public void Service_Search_Metrics_Endpoint_Returns_A_Status_Code_Ok(
        string radius, string postcode, string statusCode, string searchTriggerEventId, string serviceSearchTypeId,
        HttpStatusCode expectedStatusCode)
    {
        this.Given(s =>
                _steps.GivenIHaveASearchServiceRequest(radius, postcode, statusCode, searchTriggerEventId,
                    serviceSearchTypeId))
            .When(s => _steps.WhenISendARequest())
            .Then(s => _steps.ThenExpectedStatusCodeReturned(expectedStatusCode))
            .BDDfy();
    }
    

    
    //Negative Scenarios
    [Theory]
    [InlineData("15", "E1 2EN", "200", "3", "1", HttpStatusCode.InternalServerError)] // Invalid Search Trigger Event Type
    [InlineData("15", "E1 2EN", "200", "1", "3", HttpStatusCode.InternalServerError)] // Invalid ServiceSearchTypeId
    public void Service_Search_Metrics_Endpoint_Returns_Status_Code_Internal_Server_Error(string radius, string postcode, string statusCode,
        string searchTriggerEventId, string serviceSearchTypeId, HttpStatusCode expectedStatusCode)
    {
        this.Given(s =>
                _steps.GivenIHaveASearchServiceRequest(radius, postcode, statusCode, searchTriggerEventId,
                    serviceSearchTypeId))
            .When(s => _steps.WhenISendARequest())
            .Then(s => _steps.ThenExpectedStatusCodeReturned(expectedStatusCode))
            .BDDfy();
    }
}