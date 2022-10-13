namespace fh_service_directory_api.core.Interfaces.Entities
{
    public interface IOpenReferralPhysical_Address : IEntityBase<string>
    {
        string Address_1 { get; set; }
        string? City { get; set; }
        string? Country { get; set; }
        string Postal_code { get; set; }
        string? State_province { get; set; }
    }
}