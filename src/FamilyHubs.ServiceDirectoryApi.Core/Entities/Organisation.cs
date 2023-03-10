using FamilyHubs.ServiceDirectory.Shared.Enums;
using FamilyHubs.SharedKernel;
using FamilyHubs.SharedKernel.Interfaces;

namespace FamilyHubs.ServiceDirectory.Core.Entities;

public class Organisation : EntityBase<long>, IAggregateRoot
{
    public required OrganisationType OrganisationType { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required string AdminAreaCode { get; set; }
    public long? AssociatedOrganisationId { get; set; }
    public string? Logo { get; set; }
    public string? Uri { get; set; }
    public string? Url { get; set; }
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
    public ICollection<Service> Services { get; set; } = new List<Service>();
    public ICollection<Contact> Contacts { get; set; } = new List<Contact>();
    public ICollection<Taxonomy> Taxonomies { get; set; } = new List<Taxonomy>();
}