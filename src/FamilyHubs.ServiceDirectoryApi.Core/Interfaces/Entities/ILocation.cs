using fh_service_directory_api.core.Concretions.Entities;

namespace fh_service_directory_api.core.Interfaces.Entities
{
    public interface ILocation
    {
        ICollection<IAccessibilityForDisabilities>? Accessibility_for_disabilities { get; init; }

        string? Description { get; init; }

        double Latitude { get; init; }

        double Longitude { get; init; }

        string Name { get; init; }

        ICollection<IPhysicalAddress>? PhysicalAddresses { get; init; }
    }
}