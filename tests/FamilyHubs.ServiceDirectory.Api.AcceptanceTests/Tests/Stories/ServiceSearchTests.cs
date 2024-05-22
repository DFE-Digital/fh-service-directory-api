using TestStack.BDDfy;
using Xunit;
using FamilyHubs.ServiceDirectory.Api.AcceptanceTests.Tests.Steps;

namespace FamilyHubs.ServiceDirectory.Api.AcceptanceTests.Tests.Stories;

// Define the story/feature being tested
[Story(
    AsA = "user of the metrics api",
    IWant = "to be able to update usage information for postcode searches and filters",
    SoThat = "I can record how search functionality is being used")]

[TestClass]
public class ServiceSearchTests
{
    private readonly ServiceSearchSteps steps;

    public ServiceSearchTests()
    {
        //Get instances of the steps required for the test
        steps = new ServiceSearchSteps();
    }

    //Add all tests that make up the story to this class.
    [TestMethod]
    public void Users_Can_Update_Metrics_For_Postcode_Search()
    {
        this.Given(s => steps.GivenIHaveAPostcodeToSearchWithinFind())
            .When(s => steps.WhenISendARequest())
            .Then(s => steps.Then200StatusCodeReturned())
            .BDDfy();
    }
}