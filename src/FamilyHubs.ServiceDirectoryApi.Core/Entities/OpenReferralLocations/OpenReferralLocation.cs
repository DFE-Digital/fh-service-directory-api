using FamilyHubs.ServiceDirectoryApi.Core.Entities.AccessibilityForDisabilitiess;
using FamilyHubs.ServiceDirectoryApi.Core.Entities.OpenReferralPhysicalAddresses;
using FamilyHubs.SharedKernel;
using FamilyHubs.SharedKernel.Interfaces;

namespace FamilyHubs.ServiceDirectoryApi.Core.Entities.OpenReferralLocations;

public class OpenReferralLocation : EntityBase<string>, IOpenReferralLocation, IAggregateRoot
{
    private OpenReferralLocation() { }
    public OpenReferralLocation(
        string id,
        string name,
        string? description,
        double latitude,
        double longitude,
        ICollection<OpenReferralPhysicalAddress>? physical_addresses,
        ICollection<AccessibilityForDisabilities>? accessibility_for_disabilities
        )
    {
        Id = id;
        Name = name;
        Description = description;
        Latitude = latitude;
        Longitude = longitude;
        Physical_addresses = physical_addresses;
        Accessibility_for_disabilities = accessibility_for_disabilities;
    }
    public string Name { get; init; } = default!;
    public string? Description { get; init; }
    public double Latitude { get; init; }
    public double Longitude { get; init; }
    public virtual ICollection<OpenReferralPhysicalAddress>? Physical_addresses { get; init; }
    public virtual ICollection<AccessibilityForDisabilities>? Accessibility_for_disabilities { get; init; }
}