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
   public void Initial_Postcode_Search_as_a_Find_User()
   {
       this.Given(s => steps.GivenIPostToTheServiceSearchEndpointWithParameters("15","E1 2EN","200","1","2"))
           .When(s => steps.WhenISendARequest())
           .Then(s => steps.Then200StatusCodeReturned())
           .BDDfy();
   }
  
   [TestMethod]
   public void Initial_Postcode_Search_as_a_Connect_User()
   {
       this.Given(s => steps.GivenIPostToTheServiceSearchEndpointWithParameters("15","E1 2EN","200","1","1"))
           .When(s => steps.WhenISendARequest())
           .Then(s => steps.Then200StatusCodeReturned())
           .BDDfy();
   }
   [TestMethod]
   public void Subsequent_Filter_Postcode_Search_as_a_Find_User()
   {
       this.Given(s => steps.GivenIPostToTheServiceSearchEndpointWithParameters("15","E1 2EN","200","2","2"))
           .When(s => steps.WhenISendARequest())
           .Then(s => steps.Then200StatusCodeReturned())
           .BDDfy();
   }
   [TestMethod]
   public void Subsequent_Filter_Postcode_Search_as_a_Connect_User()
   {
       this.Given(s => steps.GivenIPostToTheServiceSearchEndpointWithParameters("15","E1 2EN","200","2","1"))
           .When(s => steps.WhenISendARequest())
           .Then(s => steps.Then200StatusCodeReturned())
           .BDDfy();
   }
  
  
   //Negative Scenarios
   [TestMethod]
   public void No_metrics_are_sent_when_a_non_200_message_is_received_from_postcode_IO()
   {
       this.Given(s => steps.GivenIPostToTheServiceSearchEndpointWithParameters("15","E1 2EN","400","1","1"))
           .When(s => steps.WhenISendARequest())
           .Then(s => steps.Then500StatusCodeReturned())
           .BDDfy();
   }

   [TestMethod]
   public void ErrorCode_when_wrong_searchTriggerEventType_is_sent()
   {
       this.Given(s => steps.GivenIPostToTheServiceSearchEndpointWithParameters("15","E1 2EN","200","3","1"))
           .When(s => steps.WhenISendARequest())
           .Then(s => steps.Then500StatusCodeReturned())
           .BDDfy();
   }

   [TestMethod]
   public void ErrorCode_message_when_wrong_serviceSearchTypeId_is_posted()
   {
       this.Given(s => steps.GivenIPostToTheServiceSearchEndpointWithParameters("15","E1 2EN","200","1","3"))
           .When(s => steps.WhenISendARequest())
           .Then(s => steps.Then500StatusCodeReturned())
           .BDDfy();
   }
  
   [TestMethod]
   public void ErrorCode_message_when_wrong_searchRadiusMiles_is_posted()
   {
       this.Given(s => steps.GivenIPostToTheServiceSearchEndpointWithParameters("500","E1 2EN","200","1","1"))
           .When(s => steps.WhenISendARequest())
           .Then(s => steps.Then400StatusCodeReturned())
           .BDDfy();
   }
}