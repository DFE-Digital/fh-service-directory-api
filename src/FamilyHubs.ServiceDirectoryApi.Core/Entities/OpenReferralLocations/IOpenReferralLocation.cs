using FamilyHubs.ServiceDirectoryApi.Core.Entities.AccessibilityForDisabilitiess;
using FamilyHubs.ServiceDirectoryApi.Core.Entities.OpenReferralPhysicalAddresses;

namespace FamilyHubs.ServiceDirectoryApi.Core.Entities.OpenReferralLocations
{
    public interface IOpenReferralLocation : IEntityBase<string>
    {
        ICollection<AccessibilityForDisabilities>? Accessibility_for_disabilities { get; init; }
        string? Description { get; init; }
        double Latitude { get; init; }
        double Longitude { get; init; }
        string Name { get; init; }
        ICollection<OpenReferralPhysicalAddress>? Physical_addresses { get; init; }
    }
}