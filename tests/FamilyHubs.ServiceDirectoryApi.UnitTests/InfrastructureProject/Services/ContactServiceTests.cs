using AutoFixture;
using FamilyHubs.ServiceDirectory.Core.Entities;
using FamilyHubs.ServiceDirectory.Infrastructure.Services;

namespace FamilyHubs.ServiceDirectoryApi.UnitTests.InfrastructureProject.Services
{

    public class ContactServiceTests : BaseCreateDbUnitTest
    {
        private readonly Fixture _fixture;

        public ContactServiceTests()
        {
            _fixture = new Fixture();
        }

        [Fact]
        public async Task GetContacts_NoParameters_ReturnsResults()
        {
            //  Arrange
            var applicationDbContext = GetApplicationDbContext();
            var contacts = _fixture.Create<List<Contact>>();
            foreach (var contact in contacts) 
            {
                applicationDbContext.Contacts.Add(contact);
            }
            await applicationDbContext.SaveChangesAsync();

            var contactService = new ContactService(applicationDbContext);
            var query = new ContactQuery();

            //  Act
            var results = await contactService.GetContacts(query);

            //  Assert
            Assert.NotNull(results);
            Assert.Equal(contacts.Count, results.Count);
        }

        [Fact]
        public async Task GetLocations_HasParameters_ReturnsResults()
        {
            //  Arrange
            var applicationDbContext = GetApplicationDbContext();
            var contacts = _fixture.Create<List<Contact>>();
            foreach (var contact in contacts)
            {
                applicationDbContext.Contacts.Add(contact);
            }
            await applicationDbContext.SaveChangesAsync();

            var contactService = new ContactService(applicationDbContext);
            var query = new ContactQuery 
            {
                Id = contacts[0]!.Id,
                Title = contacts[0]!.Title,
                Name = contacts[0]!.Name,
                Telephone = contacts[0]!.Telephone,
                TextPhone = contacts[0]!.TextPhone,
                Url = contacts[0]!.Url,
                Email = contacts[0]!.Email,
            };

            //  Act
            var results = await contactService.GetContacts(query);

            //  Assert
            Assert.NotNull(results);
            Assert.Single(results);
            Assert.Equal(query.Id, results[0].Id);
            Assert.Equal(query.Name, results[0].Name);
            Assert.Equal(query.Telephone, results[0].Telephone);
        }
    }
}
