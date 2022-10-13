namespace fh_service_directory_api.core.Interfaces.Entities
{
    public interface IOpenReferralCost_Option : IEntityBase<string>
    {
        decimal Amount { get; set; }
        string Amount_description { get; set; }
        string? LinkId { get; set; }
        string? Option { get; set; }
        DateTime? Valid_from { get; set; }
        DateTime? Valid_to { get; set; }
    }
}