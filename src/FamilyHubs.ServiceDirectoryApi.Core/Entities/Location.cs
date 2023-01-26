using FamilyHubs.SharedKernel;
using FamilyHubs.SharedKernel.Interfaces;

namespace FamilyHubs.ServiceDirectory.Core.Entities;

public class Location : EntityBase<string>, IAggregateRoot
{
    private Location() { }
    public Location(string id, string name, string? description, double latitude, double longitude,
        ICollection<LinkTaxonomy>? linkTaxonomies,
        ICollection<PhysicalAddress>? physicalAddresses,
        ICollection<AccessibilityForDisabilities>? accessibilityForDisabilities
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
    }
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public virtual ICollection<LinkTaxonomy>? LinkTaxonomies { get; set; }
    public virtual ICollection<PhysicalAddress>? PhysicalAddresses { get; set; }
    public virtual ICollection<AccessibilityForDisabilities>? AccessibilityForDisabilities { get; set; }
}