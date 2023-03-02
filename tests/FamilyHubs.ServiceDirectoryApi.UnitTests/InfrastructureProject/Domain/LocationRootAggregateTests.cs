using AutoFixture;
using FamilyHubs.ServiceDirectory.Core.Entities;
using FamilyHubs.ServiceDirectory.Infrastructure.Domain;
using FamilyHubs.ServiceDirectory.Infrastructure.Persistence.Repository;
using FamilyHubs.ServiceDirectory.Infrastructure.Services;
using IdGen;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyHubs.ServiceDirectoryApi.UnitTests.InfrastructureProject.Domain
{
    public class LocationRootAggregateTests : BaseCreateDbUnitTest
    {
        private readonly Fixture _fixture;
        private readonly IContactService _contactService;
        private readonly ILocationService _locationService;
        private readonly ApplicationDbContext _applicationDbContext;

        public LocationRootAggregateTests()
        {
            _fixture = new Fixture();

            _applicationDbContext = GetApplicationDbContext();
            var contacts = _fixture.Create<List<Contact>>();
            foreach (var contact in contacts)
            {
                _applicationDbContext.Contacts.Add(contact);
            }
            _applicationDbContext.SaveChanges();
            var mockedIdGenerator = GetMockedIdGenerator();

            //  It is intentional to use concrete types and not mocks here as the tests have little value individually
            _contactService = new ContactService(Mock.Of<ILogger<ContactService>>(), mockedIdGenerator, _applicationDbContext);
            _locationService= new LocationService(Mock.Of<ILogger<LocationService>>(), mockedIdGenerator, _applicationDbContext);
        }

        [Fact]
        public async Task Upsert_AddsNewRecord_DoesNotThrow()
        {
            //  Arrange
            var location = GenerateTestLocation();
            var locationRootAggregate = new LocationRootAggregate(_locationService, _contactService);

            //  Act
            var exception = await Record.ExceptionAsync(() => 
            { 
                return locationRootAggregate.Upsert(location);
            });

            //  Assert
            Assert.Null(exception);

        }

        [Fact]
        public async Task Upsert_AddsNewRecord_NewItemInDb()
        {
            //  Arrange
            var location = GenerateTestLocation();
            var locationRootAggregate = new LocationRootAggregate(_locationService, _contactService);

            //  Act
            var result = await locationRootAggregate.Upsert(location);

            //  Assert
            Assert.NotNull(result);
            var dbRecord = _applicationDbContext.Locations.Where(x => x.Id == result!.Id).FirstOrDefault();
            Assert.NotNull(dbRecord);
            Assert.Equal(location.Name, dbRecord.Name);
            Assert.Equal(location.Latitude, dbRecord.Latitude);
            Assert.Equal(location.Longitude, dbRecord.Longitude);
            Assert.Equal(location.LinkContacts!.Count, dbRecord.LinkContacts!.Count);
            
        }

        private Location GenerateTestLocation()
        {
            var location = _fixture.Create<Location>();
            location.Id = string.Empty;

            foreach(var linkContact in location.LinkContacts)
            {
                linkContact.Id = string.Empty;
                linkContact.LinkId= string.Empty;
                linkContact.LinkType = string.Empty;
                linkContact.Contact!.Id = string.Empty;
            }
            //var linkContacts = new List<LinkContact>();

            //var contactOne = new Contact(string.Empty, "Test1", "12345678", string.Empty, string.Empty, string.Empty, string.Empty);
            //var contactTwo = new Contact(string.Empty, "Test2", "12345679", string.Empty, string.Empty, string.Empty, string.Empty);
            //var contactThree = new Contact(string.Empty, "Test3", "12345680", string.Empty, string.Empty, string.Empty, string.Empty);

            //linkContacts.Add(new LinkContact(string.Empty, string.Empty, string.Empty, contactOne));
            //linkContacts.Add(new LinkContact(string.Empty, string.Empty, string.Empty, contactTwo));
            //linkContacts.Add(new LinkContact(string.Empty, string.Empty, string.Empty, contactThree));

            //location.LinkContacts = linkContacts;

            return location;
        }

    }
}
