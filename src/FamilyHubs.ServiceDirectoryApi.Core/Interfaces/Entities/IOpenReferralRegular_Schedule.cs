namespace fh_service_directory_api.core.Interfaces.Entities
{
    public interface IOpenReferralRegular_Schedule : IEntityBase<string>
    {
        string? Byday { get; set; }
        string? Bymonthday { get; set; }
        DateTime? Closes_at { get; set; }
        string Description { get; set; }
        string? Dtstart { get; set; }
        string? Freq { get; set; }
        string? Interval { get; set; }
        DateTime? Opens_at { get; set; }
        DateTime? Valid_from { get; set; }
        DateTime? Valid_to { get; set; }
    }
}