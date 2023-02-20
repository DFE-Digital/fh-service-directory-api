using FamilyHubs.ServiceDirectory.Api.Data;
using FamilyHubs.ServiceDirectory.Infrastructure.Persistence.Repository;
using FluentAssertions;
using IdGen;
using Moq;
using Xunit;

namespace FamilyHubs.ServiceDirectoryApi.IntegrationTests.Persistence;

public class WhenUsingDatabaseContextFactory
{
    [Fact]
    public void ThenApplicationDbContextIsReturned()
    {
        //Arrange
        var mockIdGenerator = new Mock<IIdGenerator<long>>();
        var databaseContextFactory = new DatabaseContextFactory(mockIdGenerator.Object);
        string[] args = new string[0];

        //Act
        var result = databaseContextFactory.CreateDbContext(args);

        //Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<ApplicationDbContext>();

    }
}
