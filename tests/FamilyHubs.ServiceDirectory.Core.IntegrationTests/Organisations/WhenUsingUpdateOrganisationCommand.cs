using FamilyHubs.ServiceDirectory.Core.Commands.Organisations.UpdateOrganisation;
using FamilyHubs.ServiceDirectory.Core.Exceptions;
using FamilyHubs.SharedKernel.Identity;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using System.Security.Claims;

namespace FamilyHubs.ServiceDirectory.Core.IntegrationTests.Organisations;

public class WhenUsingUpdateOrganisationCommand : DataIntegrationTestBase
{
    public readonly Mock<IHttpContextAccessor> MockHttpContextAccessor;
    public readonly Mock<ILogger<UpdateOrganisationCommandHandler>> UpdateLogger = new();

    public WhenUsingUpdateOrganisationCommand()
    {
        MockHttpContextAccessor = GetMockHttpContextAccessor(-1, RoleTypes.DfeAdmin);
    }

    [Fact]
    public async Task ThenUpdateOrganisationOnly()
    {
        //Arrange
        await CreateOrganisationDetails();
        var service = TestOrganisation.Services.ElementAt(0);
        TestOrganisation.Name = "Unit Test Update TestOrganisation Name";
        TestOrganisation.Description = "Unit Test Update TestOrganisation Name";

        var updateCommand = new UpdateOrganisationCommand(TestOrganisation.Id, TestOrganisation);
        var updateHandler = new UpdateOrganisationCommandHandler(MockHttpContextAccessor.Object, TestDbContext, Mapper, UpdateLogger.Object);

        //Act
        var result = await updateHandler.Handle(updateCommand, new CancellationToken());

        //Assert
        result.Should().NotBe(0);
        result.Should().Be(TestOrganisation.Id);

        var actualOrganisation = TestDbContext.Organisations.SingleOrDefault(s => s.Name == TestOrganisation.Name);
        actualOrganisation.Should().NotBeNull();
        actualOrganisation!.Description.Should().Be(TestOrganisation.Description);

        var actualService = TestDbContext.Services.SingleOrDefault(s => s.Name == service.Name);
        actualService.Should().NotBeNull();
        actualService!.Description.Should().Be(service.Description);
    }

    [Fact]
    public async Task ThenUpdateOrganisation_ThrowsForbiddenException()
    {
        //Arrange
        await CreateOrganisationDetails();
        var mockHttpContextAccessor = GetMockHttpContextAccessor(50, RoleTypes.LaManager);

        var updateCommand = new UpdateOrganisationCommand(TestOrganisation.Id, TestOrganisation);
        var updateHandler = new UpdateOrganisationCommandHandler(mockHttpContextAccessor.Object, TestDbContext, Mapper, UpdateLogger.Object);

        //Act / Assert
        await Assert.ThrowsAsync<ForbiddenException>(async () => await updateHandler.Handle(updateCommand, new CancellationToken()));
    }

    private Mock<IHttpContextAccessor> GetMockHttpContextAccessor(long organisationId, string userRole)
    {
        var mockUser = new Mock<ClaimsPrincipal>();
        var claims = new List<Claim>();
        claims.Add(new Claim(FamilyHubsClaimTypes.OrganisationId, organisationId.ToString()));
        claims.Add(new Claim(FamilyHubsClaimTypes.Role, userRole));

        mockUser.SetupGet(x => x.Claims).Returns(claims);

        var mockHttpContext = new Mock<HttpContext>();
        mockHttpContext.SetupGet(x => x.User).Returns(mockUser.Object);

        var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
        mockHttpContextAccessor.SetupGet(x => x.HttpContext).Returns(mockHttpContext.Object);

        return mockHttpContextAccessor;
    }
}