﻿using AutoFixture;
using FamilyHubs.ServiceDirectory.Core.Entities;
using FamilyHubs.ServiceDirectory.Infrastructure.Services;

namespace FamilyHubs.ServiceDirectoryApi.UnitTests.InfrastructureProject.Services
{

    public class LocationServiceTests : BaseCreateDbUnitTest
    {
        private readonly Fixture _fixture;

        public LocationServiceTests()
        {
            _fixture = new Fixture();
        }

        [Fact]
        public async Task GetLocations_NoParameters_ReturnsResults()
        {
            //  Arrange
            var applicationDbContext = GetApplicationDbContext();
            var locations = _fixture.Create<List<Location>>();
            foreach (var location in locations) 
            {
                applicationDbContext.Locations.Add(location);
            }
            await applicationDbContext.SaveChangesAsync();

            var locationService = new LocationService(applicationDbContext);
            var query = new LocationQuery();

            //  Act
            var results = await locationService.GetLocations(query);

            //  Assert
            Assert.NotNull(results);
            Assert.Equal(locations.Count, results.Count);
        }

        [Fact]
        public async Task GetLocations_HasParameters_ReturnsResults()
        {
            //  Arrange
            var applicationDbContext = GetApplicationDbContext();
            var locations = _fixture.Create<List<Location>>();
            foreach (var location in locations)
            {
                applicationDbContext.Locations.Add(location);
            }
            await applicationDbContext.SaveChangesAsync();

            var locationService = new LocationService(applicationDbContext);
            var query = new LocationQuery 
            {
                Id = locations[0]!.Id,
                Name = locations[0]!.Name,
                Description= locations[0]!.Description,
                PostCode = locations[0]!.PhysicalAddresses!.FirstOrDefault()!.PostCode
            };

            //  Act
            var results = await locationService.GetLocations(query);

            //  Assert
            Assert.NotNull(results);
            Assert.Single(results);
            Assert.Equal(query.Id, results[0].Id);
            Assert.Equal(query.Name, results[0].Name);
            Assert.Equal(query.Description, results[0].Description);
        }
    }
}
