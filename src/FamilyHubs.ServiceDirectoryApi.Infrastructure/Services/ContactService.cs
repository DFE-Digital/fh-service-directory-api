using FamilyHubs.ServiceDirectory.Core.Entities;
using FamilyHubs.ServiceDirectory.Infrastructure.Persistence.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;

namespace FamilyHubs.ServiceDirectory.Infrastructure.Services
{
    public interface IContactService
    {
        Task<List<Contact>> GetContacts(ContactQuery queryValues);
        Task<Contact> UpsertContact(Contact contact);
    }

    public class ContactService : BaseRepositoryService, IContactService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ContactService> _logger;

        public ContactService(ApplicationDbContext context, ILogger<ContactService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<Contact>> GetContacts(ContactQuery queryValues)
        {
            var contacts = _context.Contacts.AsQueryable();

            if (!string.IsNullOrEmpty(queryValues.Id))
                contacts = contacts.Where(x => x.Id == queryValues.Id);

            if (!string.IsNullOrEmpty(queryValues.Title))
                contacts = contacts.Where(x => x.Title == queryValues.Title);

            if (!string.IsNullOrEmpty(queryValues.Name))
                contacts = contacts.Where(x => x.Name == queryValues.Name);

            if (!string.IsNullOrEmpty(queryValues.Telephone))
                contacts = contacts.Where(x => x.Telephone == queryValues.Telephone);

            if (!string.IsNullOrEmpty(queryValues.TextPhone))
                contacts = contacts.Where(x => x.TextPhone == queryValues.TextPhone);

            if (!string.IsNullOrEmpty(queryValues.Url))
                contacts = contacts.Where(x => x.Url == queryValues.Url);

            if (!string.IsNullOrEmpty(queryValues.Email))
                contacts = contacts.Where(x => x.Email == queryValues.Email);

            return await contacts.ToListAsync();
        }

        public async Task<Contact> UpsertContact(Contact contact)
        {
            _logger.LogDebug("UpsertContact - Begin");

            if (!string.IsNullOrEmpty(contact.Id))
            {
                var existingContact = _context.Contacts.FirstOrDefault(x => x.Id == contact.Id);
                if (existingContact != null)
                {
                    return await UpdateContact(existingContact, contact);
                }
            }

            return await AddContact(contact);
        }

        private async Task<Contact> AddContact(Contact contact)
        {
            _logger.LogDebug("AddContact - Begin");

            _context.Contacts.Add(contact);
            await _context.SaveChangesAsync();

            _logger.LogInformation("AddContact - Success for : {id}", contact.Id);
            return contact;
        }

        private async Task<Contact> UpdateContact(Contact existingContact, Contact modifiedContact)
        {
            _logger.LogDebug("UpdateContact - Begin");

            existingContact.Title = modifiedContact.Title;
            existingContact.Name = modifiedContact.Name;
            existingContact.TextPhone = modifiedContact.TextPhone;
            existingContact.Telephone = modifiedContact.Telephone;
            existingContact.Url = modifiedContact.Url;
            existingContact.Email = modifiedContact.Email;

            await _context.SaveChangesAsync();
            _logger.LogInformation("UpdateContact - Success for : {id}", existingContact.Id);
            return existingContact;
        }
    }
}
