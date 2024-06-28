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
    [InlineData("15", "E1 2EN", "200", "1", "2", HttpStatusCode.OK)]
    public void Service_Search_Metrics_Endpoint_Returns_A_200_When_Initial_Postcode_Search_Is_Done_As_A_Find_User(
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

    [Theory]
    [InlineData("15", "E1 2EN", "200", "1", "1", HttpStatusCode.OK)]
    public void
        Service_Search_Metrics_Endpoint_Returns_A_200_When_An_Initial_Postcode_Search_Is_Done_As_A_Connect_User(
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

    [Theory]
    [InlineData("15", "E1 2EN", "200", "2", "2", HttpStatusCode.OK)]
    public void
        Service_Search_Metrics_Endpoint_Returns_A_200_When_A_Subsequent_Filter_Postcode_Search_Is_Done_As_A_Find_User(
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

    [Theory]
    [InlineData("15", "E1 2EN", "200", "2", "1", HttpStatusCode.OK)]
    public void
        Service_Search_Metrics_Endpoint_Returns_A_200_When_A_Subsequent_Filter_Postcode_Search_Is_Done_As_A_Connect_User(
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

    [Theory]
    [InlineData("15", "E1 2EN", "300", "1", "2", HttpStatusCode.OK)]
    public void
        Service_Search_Metrics_Endpoint_Returns_A_200_For_An_Initial_Postcode_Search_As_A_Find_User_After_the_Service_Search_API_Responds_With_A_Non_200_Response_Code(
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

    [Theory]
    [InlineData("15", "E1 2EN", "500", "1", "1", HttpStatusCode.OK)]
    public void
        Service_Search_Metrics_Endpoint_Returns_A_200_For_An_Initial_Postcode_Search_As_A_Connect_User_After_Service_Search_API_Responds_With_A_Non_200_Response_Code(
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

    [Theory]
    [InlineData("15", "E1 2EN", "422", "2", "2", HttpStatusCode.OK)]
    public void
        Service_Search_Metrics_Endpoint_Returns_A_200_For_A_Subsequent_Postcode_Search_As_A_Find_User_After_the_Service_Search_API_Responds_With_A_Non_200_Response_Code(
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

    [Theory]
    [InlineData("15", "E1 2EN", "400", "2", "1", HttpStatusCode.OK)]
    public void
        Service_Search_Metrics_Endpoint_Returns_A_200_For_Subsequent_Postcode_Search_As_A_Connect_User_After_the_Service_Search_API_Responds_With_A_Non_200_Response_Code(
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
    [InlineData("15", "E1 2EN", "200", "3", "1", HttpStatusCode.InternalServerError)]
    public void ErrorCode_When_Wrong_SearchTriggerEventType_Is_Sent(string radius, string postcode, string statusCode,
        string searchTriggerEventId, string serviceSearchTypeId, HttpStatusCode expectedStatusCode)
    {
        this.Given(s =>
                _steps.GivenIHaveASearchServiceRequest(radius, postcode, statusCode, searchTriggerEventId,
                    serviceSearchTypeId))
            .When(s => _steps.WhenISendARequest())
            .Then(s => _steps.ThenExpectedStatusCodeReturned(expectedStatusCode))
            .BDDfy();
    }

    [Theory]
    [InlineData("15", "E1 2EN", "200", "1", "3", HttpStatusCode.InternalServerError)]
    public void ErrorCode_Message_When_Wrong_ServiceSearchTypeId_Is_Posted(string radius, string postcode,
        string statusCode, string searchTriggerEventId, string serviceSearchTypeId, HttpStatusCode expectedStatusCode)
    {
        this.Given(s =>
                _steps.GivenIHaveASearchServiceRequest(radius, postcode, statusCode, searchTriggerEventId,
                    serviceSearchTypeId))
            .When(s => _steps.WhenISendARequest())
            .Then(s => _steps.ThenExpectedStatusCodeReturned(expectedStatusCode))
            .BDDfy();
    }

    [Theory]
    [InlineData("500", "E1 2EN", "200", "1", "1", HttpStatusCode.BadRequest)]
    public void ErrorCode_Message_When_Wrong_SearchRadiusMiles_Is_Posted(string radius, string postcode,
        string statusCode, string searchTriggerEventId, string serviceSearchTypeId, HttpStatusCode expectedStatusCode)
    {
        this.Given(s =>
                _steps.GivenIHaveASearchServiceRequest(radius, postcode, statusCode, searchTriggerEventId,
                    serviceSearchTypeId))
            .When(s => _steps.WhenISendARequest())
            .Then(s => _steps.ThenExpectedStatusCodeReturned(expectedStatusCode))
            .BDDfy();
    }
}