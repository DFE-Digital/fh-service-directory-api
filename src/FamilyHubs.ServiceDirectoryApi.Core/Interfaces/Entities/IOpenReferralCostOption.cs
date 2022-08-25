namespace fh_service_directory_api.core.Interfaces.Entities
{
    public interface IOpenReferralCostOption
    {
        decimal Amount { get; init; }
        string Amount_description { get; init; }
        string? LinkId { get; init; }
        string? Option { get; init; }
        DateTime? Valid_from { get; init; }
        DateTime? Valid_to { get; init; }
    }
}