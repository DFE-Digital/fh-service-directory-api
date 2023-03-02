using AutoFixture;
using FamilyHubs.ServiceDirectory.Core.Entities;
using FamilyHubs.ServiceDirectory.Infrastructure.Services;
using IdGen;
using Microsoft.Extensions.Logging;
using Moq;

namespace FamilyHubs.ServiceDirectoryApi.UnitTests.InfrastructureProject.Services
{

    public class ContactServiceTests : BaseCreateDbUnitTest
    {
        private readonly Fixture _fixture;
        private readonly ILogger<ContactService> _logger;
        private readonly Mock<IIdGenerator<long>> _mockIdGenerator;

        public ContactServiceTests()
        {
            _fixture = new Fixture();
            _logger = Mock.Of<ILogger<ContactService>>();
            _mockIdGenerator = new Mock<IIdGenerator<long>>();
            _mockIdGenerator.Setup(m => m.CreateId()).Returns(DateTime.Now.Ticks);
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

            var contactService = new ContactService(applicationDbContext, _mockIdGenerator.Object, _logger);
            var query = new ContactQuery();

            //  Act
            var results = await contactService.GetContacts(query);

            //  Assert
            Assert.NotNull(results);
            Assert.Equal(contacts.Count, results.Count);
        }

        [Fact]
        public async Task GetContacts_HasParameters_ReturnsResults()
        {
            //  Arrange
            var applicationDbContext = GetApplicationDbContext();
            var contacts = _fixture.Create<List<Contact>>();
            foreach (var contact in contacts)
            {
                applicationDbContext.Contacts.Add(contact);
            }
            await applicationDbContext.SaveChangesAsync();

            var contactService = new ContactService(applicationDbContext, _mockIdGenerator.Object, _logger);
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

        [Fact]
        public async Task UpsertContact_AddedToDbSet()
        {
            //  Arrange
            var applicationDbContext = GetApplicationDbContext();
            var contact = _fixture.Create<Contact>();

            var contactService = new ContactService(applicationDbContext, _mockIdGenerator.Object, _logger);

            //  Act
            var result = await contactService.Upsert(contact);

            //  Assert
            var addedContact = applicationDbContext.Contacts.Where(c => c.Id == contact.Id).FirstOrDefault();
            Assert.NotNull(addedContact);
            Assert.Equal(addedContact.Name, addedContact.Name);
        }
    }
}
