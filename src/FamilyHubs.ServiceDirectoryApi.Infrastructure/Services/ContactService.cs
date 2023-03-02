using Ardalis.Specification;
using FamilyHubs.ServiceDirectory.Core.Entities;
using FamilyHubs.ServiceDirectory.Infrastructure.Persistence.Repository;
using IdGen;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FamilyHubs.ServiceDirectory.Infrastructure.Services
{
    public interface IContactService
    {
        Task<List<Contact>> GetContacts(ContactQuery queryValues);
        Task<Contact> Upsert(Contact contact);
        Task<Contact?> GetById(string id);
        Task HydrateLinkContact(LinkContact linkContact, string linkId, string linkType);
    }

    public class ContactService : BaseRepositoryService<Contact, ContactService>, IContactService
    {
        public ContactService(ILogger<ContactService> logger, IIdGenerator<long> idGenerator, ApplicationDbContext context) 
            :base(logger, idGenerator, context, context.Contacts)
        {

        }

        public async Task<List<Contact>> GetContacts(ContactQuery queryValues)
        {
            var contacts = DbContext.Contacts.AsQueryable();

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

        public async Task HydrateLinkContact(LinkContact linkContact, string linkId, string linkType)
        {
            var contact = await GetContact(linkContact.Contact!);
            if (contact == null)
            {
                contact = await Upsert(linkContact.Contact!);
            }

            linkContact.Contact = contact;
            linkContact.LinkType = linkType;
            linkContact.LinkId = linkId;
            if (string.IsNullOrEmpty(linkContact.Id))
            {
                linkContact.Id = GetNewId();
            }
        }

        protected override void UpdateEntityValues(Contact existing, Contact modified)
        {
            existing.Title = modified.Title;
            existing.Name = modified.Name;
            existing.TextPhone = modified.TextPhone;
            existing.Telephone = modified.Telephone;
            existing.Url = modified.Url;
            existing.Email = modified.Email;
        }

        private async Task<Contact?> GetContact(Contact contact)
        {
            if (!string.IsNullOrEmpty(contact.Id))
            {
                return await GetById(contact.Id);
            }

            return await DbContext.Contacts.Where(x =>
                x.Title == contact.Title &&
                x.Name == contact.Name &&
                x.Telephone == contact.Telephone &&
                x.TextPhone == contact.TextPhone &&
                x.Url == contact.Url
            ).FirstOrDefaultAsync();
        }
    }
}
