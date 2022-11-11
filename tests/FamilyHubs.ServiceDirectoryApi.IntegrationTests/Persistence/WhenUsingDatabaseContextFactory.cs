using fh_service_directory_api.api.Data;
using fh_service_directory_api.infrastructure.Persistence.Repository;
using FluentAssertions;
using System;
using Xunit;

namespace FamilyHubs.ServiceDirectoryApi.IntegrationTests.Persistence;

public class WhenUsingDatabaseContextFactory
{
    [Fact]
    public void ThenApplicationDbContextIsReturned()
    {
        //Arrange
        DatabaseContextFactory databaseContextFactory = new DatabaseContextFactory();
        string[] args = new string[0];

        //Act
        var result = databaseContextFactory.CreateDbContext(args);

        //Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<ApplicationDbContext>();

    }
}
