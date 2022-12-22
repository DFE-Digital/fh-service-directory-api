using fh_service_directory_api.core.Entities;

namespace fh_service_directory_api.core.Interfaces.Entities
{
    public interface IOpenReferralLocation : IEntityBase<string>
    {
        string? LinkId { get; set; }
        string? Description { get; set; }
        double Latitude { get; set; }
        double Longitude { get; set; }
        string Name { get; set; }
        ICollection<Accessibility_For_Disabilities>? Accessibility_for_disabilities { get; set; }
        ICollection<OpenReferralPhysical_Address>? Physical_addresses { get; set; }
    }
}