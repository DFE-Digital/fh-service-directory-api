namespace fh_service_directory_api.core.Interfaces.Entities
{
    public interface IOpenReferralLocation
    {
        ICollection<IAccessibility_For_Disabilities>? Accessibility_for_disabilities { get; init; }
        string? Description { get; init; }
        double Latitude { get; init; }
        double Longitude { get; init; }
        string Name { get; init; }
        ICollection<IOpenReferralPhysical_Address>? Physical_addresses { get; init; }
    }
}