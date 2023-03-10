using FamilyHubs.ServiceDirectory.Core.Entities;
using FluentAssertions;

namespace FamilyHubs.ServiceDirectoryApi.UnitTests.Misc;

public class WhenUsingFunding : BaseCreateDbUnitTest
{
    [Fact]
    public void ThenCreateFunding()
    {
        //Arrange
        var mockApplicationDbContext = GetApplicationDbContext();
        
        var funding = new Funding
        {
            ServiceId = 1,
            Source = "Source"
        };

        //Act
        mockApplicationDbContext.Fundings.Add(funding);
        mockApplicationDbContext.SaveChanges();
        var result = mockApplicationDbContext.Fundings.FirstOrDefault(x => x.Id == funding.Id);

        //Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(funding);
    }
}
