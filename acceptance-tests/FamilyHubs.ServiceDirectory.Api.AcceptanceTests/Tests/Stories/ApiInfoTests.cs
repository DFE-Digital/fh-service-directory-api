using TestStack.BDDfy;
using Xunit;
using FamilyHubs.ServiceDirectory.Api.AcceptanceTests.Tests.Steps;

namespace FamilyHubs.ServiceDirectory.Api.AcceptanceTests.Tests.Stories;

[TestClass]
public class ApiInfoTests
{
    private readonly ApiInfoSteps steps;

    public ApiInfoTests()
    {
        steps = new ApiInfoSteps();
    }

    [TestMethod]
    public void Api_Info_Returned()
    {
        this.When(s => steps.ICheckTheApiInfo())
            .Then(s => steps.AnOkStatusCodeIsReturned())
            .BDDfy();
    }
}