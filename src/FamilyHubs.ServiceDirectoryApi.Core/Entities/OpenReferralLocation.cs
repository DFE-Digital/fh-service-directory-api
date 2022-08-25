using FamilyHubs.SharedKernel;
using FamilyHubs.SharedKernel.Interfaces;
using fh_service_directory_api.core.Interfaces.Entities;

namespace FamilyHubs.ServiceDirectory.Shared.Entities;

public class OpenReferralLocation : EntityBase<string>, IOpenReferralLocation, IAggregateRoot
{
    private OpenReferralLocation() { }
    public OpenReferralLocation(string id, string name, string? description, double latitude, double longitude
        , ICollection<IOpenReferralPhysical_Address>? physical_addresses, ICollection<IAccessibility_For_Disabilities>? accessibility_for_disabilities
        )
    {
        Id = id;
        Name = name;
        Description = description;
        Latitude = latitude;
        Longitude = longitude;
        Physical_addresses = physical_addresses as ICollection<OpenReferralPhysical_Address>;
        Accessibility_for_disabilities = accessibility_for_disabilities as ICollection<Accessibility_For_Disabilities>;
    }
    public string Name { get; init; } = default!;
    public string? Description { get; init; }
    public double Latitude { get; init; }
    public double Longitude { get; init; }
    public virtual ICollection<OpenReferralPhysical_Address>? Physical_addresses { get; init; }
    public virtual ICollection<Accessibility_For_Disabilities>? Accessibility_for_disabilities { get; init; }   
}