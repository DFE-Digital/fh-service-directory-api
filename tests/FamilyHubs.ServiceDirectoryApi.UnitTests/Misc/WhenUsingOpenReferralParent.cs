using FamilyHubs.ServiceDirectory.Core.Entities;
using FluentAssertions;

namespace FamilyHubs.ServiceDirectoryApi.UnitTests.Misc;

public class WhenUsingParent : BaseCreateDbUnitTest
{
    [Fact]
    public void ThenCreateParent()
    {
        //Arrange
        var mockApplicationDbContext = GetApplicationDbContext();
        var parent = new Parent(
            "c5d202c6-2390-494c-bd96-6ef423425b13", 
            "Name",
            "vocabulary",
            new List<ServiceTaxonomy>(),
            new List<LinkTaxonomy>()
            );

        //Act
        mockApplicationDbContext.Parents.Add(parent);
        mockApplicationDbContext.SaveChanges();
        var result = mockApplicationDbContext.Parents.FirstOrDefault(x => x.Id == "c5d202c6-2390-494c-bd96-6ef423425b13");

        //Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(parent);
    }
}
