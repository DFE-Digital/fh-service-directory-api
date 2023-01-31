using FluentAssertions;
using FamilyHubs.ServiceDirectory.Core.Entities;

namespace FamilyHubs.ServiceDirectoryApi.UnitTests.Misc;

public class WhenUsingFunding : BaseCreateDbUnitTest
{
    [Fact]
    public void ThenCreateFunding()
    {
        //Arrange
        var mockApplicationDbContext = GetApplicationDbContext();
        var funding = new Funding("96c0cfa6-0057-403c-9f4b-c9c111b742ec", "source");

        //Act
        mockApplicationDbContext.Fundings.Add(funding);
        mockApplicationDbContext.SaveChanges();
        var result = mockApplicationDbContext.Fundings.FirstOrDefault(x => x.Id == "96c0cfa6-0057-403c-9f4b-c9c111b742ec");

        //Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(funding);
    }
}
