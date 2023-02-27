using FamilyHubs.ServiceDirectory.Core.Entities.Abstract;
using FamilyHubs.SharedKernel;
using FamilyHubs.SharedKernel.Interfaces;

namespace FamilyHubs.ServiceDirectory.Core.Entities;

public class Location : EntityBase<string>, IAggregateRoot
{
    private Location() { }
    public Location(string id, string name, string? description, double latitude, double longitude,
        ICollection<LinkTaxonomy>? linkTaxonomies,
        ICollection<PhysicalAddress>? physicalAddresses,
        ICollection<AccessibilityForDisabilities>? accessibilityForDisabilities,
        ICollection<LinkContact>? linkContacts
        )
    {
        Id = id;
        Name = name;
        Description = description;
        Latitude = latitude;
        Longitude = longitude;
        LinkTaxonomies = linkTaxonomies;
        PhysicalAddresses = physicalAddresses;
        AccessibilityForDisabilities = accessibilityForDisabilities;
        LinkContacts = linkContacts;
    }
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public ICollection<LinkTaxonomy>? LinkTaxonomies { get; set; }
    public ICollection<PhysicalAddress>? PhysicalAddresses { get; set; }
    public ICollection<AccessibilityForDisabilities>? AccessibilityForDisabilities { get; set; }
    public ICollection<LinkContact>? LinkContacts { get; set; }
}

public class LocationQuery : IPaginationQuery
{
    public string? Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? PostCode { get; set; }
    public int? PageNumber { get; set; }
    public int? PageSize { get; set; }

}