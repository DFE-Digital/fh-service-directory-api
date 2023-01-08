using FamilyHubs.SharedKernel;
using FamilyHubs.SharedKernel.Interfaces;
using fh_service_directory_api.core.Interfaces.Entities;

namespace fh_service_directory_api.core.Entities;

public class OpenReferralLocation : EntityBase<string>, IOpenReferralLocation, IAggregateRoot
{
    private OpenReferralLocation() { }
    public OpenReferralLocation(string id, string name, string? description, double latitude, double longitude
        , ICollection<OpenReferralPhysical_Address>? physical_addresses, ICollection<Accessibility_For_Disabilities>? accessibility_for_disabilities
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
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public virtual ICollection<OpenReferralLinkTaxonomy>? LinkTaxonomies { get; set; }
    public virtual ICollection<OpenReferralLinkContact>? LinkContacts { get; set; }
    public virtual ICollection<OpenReferralPhysical_Address>? Physical_addresses { get; set; }
    public virtual ICollection<Accessibility_For_Disabilities>? Accessibility_for_disabilities { get; set; }
}