using FamilyHubs.SharedKernel.Interfaces;
using FamilyHubs.SharedKernel;

namespace FamilyHubs.ServiceDirectory.Core.Entities;

public class LinkContact : EntityBase<string>, IAggregateRoot
{
    public LinkContact() { }

    public LinkContact(string id, string linkId, string linkType, Contact? contact)
    {
        Id = id;
        Contact = contact;
        LinkId = linkId;
        LinkType = linkType;
    }
    public Contact? Contact { get; set; }
    public string LinkId { get; set; } = default!;
    public string LinkType { get; set; } = default!;
}