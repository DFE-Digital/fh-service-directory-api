using TestStack.BDDfy;
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
   private readonly ServiceSearchSteps _steps;

   public ServiceSearchTests()
   {
       //Get instances of the steps required for the test
       _steps = new ServiceSearchSteps();
   }

   //Add all tests that make up the story to this class.
   [TestMethod]
   public void Initial_Postcode_Search_As_A_Find_User()
   {
       this.Given(s => _steps.GivenIHaveASearchServiceRequest("15","E1 2EN","200","1","2"))
           .When(s => _steps.WhenISendARequest())
           .Then(s => _steps.Then200StatusCodeReturned())
           .BDDfy();
   }
   [TestMethod]
   public void Initial_Postcode_Search_As_A_Connect_User()
   {
       this.Given(s => _steps.GivenIHaveASearchServiceRequest("15","E1 2EN","200","1","1"))
           .When(s => _steps.WhenISendARequest())
           .Then(s => _steps.Then200StatusCodeReturned())
           .BDDfy();
   }
   [TestMethod]
   public void Subsequent_Filter_Postcode_Search_As_A_Find_User()
   {
       this.Given(s => _steps.GivenIHaveASearchServiceRequest("15","E1 2EN","200","2","2"))
           .When(s => _steps.WhenISendARequest())
           .Then(s => _steps.Then200StatusCodeReturned())
           .BDDfy();
   }
   [TestMethod]
   public void Subsequent_Filter_Postcode_Search_As_A_Connect_User()
   {
       this.Given(s => _steps.GivenIHaveASearchServiceRequest("15","E1 2EN","200","2","1"))
           .When(s => _steps.WhenISendARequest())
           .Then(s => _steps.Then200StatusCodeReturned())
           .BDDfy();
   }
   [TestMethod]
   public void Initial_Postcode_Search_As_A_Find_User_When_ServiceSearchAPI_Responds_With_A_Non_200_ResponseCode()
   {
       this.Given(s => _steps.GivenIHaveASearchServiceRequest("15","E1 2EN","300","1","2"))
           .When(s => _steps.WhenISendARequest())
           .Then(s => _steps.Then200StatusCodeReturned())
           .BDDfy();
   }
   [TestMethod]
   public void Initial_Postcode_Search_As_A_Connect_User_When_ServiceSearchAPI_Responds_With_A_Non_200_ResponseCode()
   {
       this.Given(s => _steps.GivenIHaveASearchServiceRequest("15","E1 2EN","500","1","1"))
           .When(s => _steps.WhenISendARequest())
           .Then(s => _steps.Then200StatusCodeReturned())
           .BDDfy();
   }
   [TestMethod]
   public void Subsequent_Filter_Postcode_Search_As_A_Find_User_When_ServiceSearchAPI_Responds_With_A_Non_200_ResponseCode()
   {
       this.Given(s => _steps.GivenIHaveASearchServiceRequest("15","E1 2EN","422","2","2"))
           .When(s => _steps.WhenISendARequest())
           .Then(s => _steps.Then200StatusCodeReturned())
           .BDDfy();
   }
   [TestMethod]
   public void Subsequent_Filter_Postcode_Search_As_A_Connect_User_When_ServiceSearchAPI_Responds_With_A_Non_200_ResponseCode()
   {
       this.Given(s => _steps.GivenIHaveASearchServiceRequest("15","E1 2EN","400","2","1"))
           .When(s => _steps.WhenISendARequest())
           .Then(s => _steps.Then200StatusCodeReturned())
           .BDDfy();
   }
   //Negative Scenarios
   [TestMethod]
   public void ErrorCode_When_Wrong_SearchTriggerEventType_Is_Sent()
   {
       this.Given(s => _steps.GivenIHaveASearchServiceRequest("15","E1 2EN","200","3","1"))
           .When(s => _steps.WhenISendARequest())
           .Then(s => _steps.Then500StatusCodeReturned())
           .BDDfy();
   }

   [TestMethod]
   public void ErrorCode_Message_When_Wrong_ServiceSearchTypeId_Is_Posted()
   {
       this.Given(s => _steps.GivenIHaveASearchServiceRequest("15","E1 2EN","200","1","3"))
           .When(s => _steps.WhenISendARequest())
           .Then(s => _steps.Then500StatusCodeReturned())
           .BDDfy();
   }
  
   [TestMethod]
   public void ErrorCode_Message_When_Wrong_SearchRadiusMiles_Is_Posted()
   {
       this.Given(s => _steps.GivenIHaveASearchServiceRequest("500","E1 2EN","200","1","1"))
           .When(s => _steps.WhenISendARequest())
           .Then(s => _steps.Then400StatusCodeReturned())
           .BDDfy();
   }
}