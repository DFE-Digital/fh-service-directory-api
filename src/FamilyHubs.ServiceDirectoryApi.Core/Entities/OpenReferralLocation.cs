using FamilyHubs.ServiceDirectory.Shared.Interfaces.Entities;
using FamilyHubs.SharedKernel;
using FamilyHubs.SharedKernel.Interfaces;

namespace FamilyHubs.ServiceDirectory.Shared.Entities;

public class OpenReferralLocation : EntityBase<string>, IOpenReferralLocation, IAggregateRoot
{
    private OpenReferralLocation() { }
    public OpenReferralLocation(string id, string name, string? description, double latitude, double longitude
        , ICollection<OpenReferralPhysical_Address>? physical_addresses, ICollection<Accessibility_For_Disabilities>? accessibility_for_disabilities
        //, ICollection<OpenReferralServiceAtLocation>? service_at_locations
        )
    {
        Id = id;
        Name = name;
        Description = description;
        Latitude = latitude;
        Longitude = longitude;
        Physical_addresses = physical_addresses;
        Accessibility_for_disabilities = accessibility_for_disabilities;
        //Service_at_locations = service_at_locations;
    }
    public string Name { get; init; } = default!;
    public string? Description { get; init; }
    public double Latitude { get; init; }
    public double Longitude { get; init; }
    public virtual ICollection<OpenReferralPhysical_Address>? Physical_addresses { get; init; }
    public virtual ICollection<Accessibility_For_Disabilities>? Accessibility_for_disabilities { get; init; }
    //public virtual ICollection<OpenReferralServiceAtLocation>? Service_at_locations { get; init; }
}
