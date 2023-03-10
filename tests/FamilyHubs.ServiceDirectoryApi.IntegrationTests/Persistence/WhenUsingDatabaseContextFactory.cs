using FamilyHubs.ServiceDirectory.Api.Data;
using FamilyHubs.ServiceDirectory.Infrastructure.Persistence.Repository;
using FluentAssertions;
using Xunit;

namespace FamilyHubs.ServiceDirectoryApi.IntegrationTests.Persistence;

public class WhenUsingDatabaseContextFactory
{
    [Fact]
    public void ThenApplicationDbContextIsReturned()
    {
        //Arrange
        var databaseContextFactory = new DatabaseContextFactory();
        string[] args = new string[0];

        //Act
        var result = databaseContextFactory.CreateDbContext(args);

        //Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<ApplicationDbContext>();
    }
}
