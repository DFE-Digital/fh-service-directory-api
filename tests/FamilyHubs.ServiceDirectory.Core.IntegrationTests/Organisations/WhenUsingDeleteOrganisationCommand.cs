using FamilyHubs.ServiceDirectory.Core.Commands.Organisations.DeleteOrganisation;
using FamilyHubs.ServiceDirectory.Core.Exceptions;
using FamilyHubs.SharedKernel.Identity;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using System.Security.Claims;

namespace FamilyHubs.ServiceDirectory.Core.IntegrationTests.Organisations;

public class WhenUsingDeleteOrganisationCommand : DataIntegrationTestBase
{

    public readonly Mock<IHttpContextAccessor> _mockHttpContextAccessor;
    public readonly Mock<ILogger<DeleteOrganisationCommandHandler>> DeleteLogger = new Mock<ILogger<DeleteOrganisationCommandHandler>>();

    public WhenUsingDeleteOrganisationCommand()
    {
        _mockHttpContextAccessor = GetMockHttpContextAccessor(-1, RoleTypes.DfeAdmin);
    }

    [Fact]
    public async Task ThenDeleteOrganisation()
    {
        //Arrange
        await CreateOrganisation();

        var command = new DeleteOrganisationCommand(1);
        var handler = new DeleteOrganisationCommandHandler(TestDbContext, _mockHttpContextAccessor.Object, DeleteLogger.Object);

        //Act
        var results = await handler.Handle(command, new CancellationToken());

        //Assert
        results.Should().Be(true);

    }

    [Fact]
    public async Task ThenDeleteOrganisationThatDoesNotExist_ShouldThrowException()
    {
        //Arrange
        var command = new DeleteOrganisationCommand(Random.Shared.Next());
        var handler = new DeleteOrganisationCommandHandler(TestDbContext, _mockHttpContextAccessor.Object, DeleteLogger.Object);

        // Act 
        // Assert
        await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, new CancellationToken()));

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
