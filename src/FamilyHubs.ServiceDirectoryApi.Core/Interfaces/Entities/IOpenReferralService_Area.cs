namespace fh_service_directory_api.core.Interfaces.Entities
{
    public interface IOpenReferralService_Area : IEntityBase<string>
    {
        string? Extent { get; set; }
        string? LinkId { get; set; }
        string Service_area { get; set; }
        string? Uri { get; set; }
    }
}