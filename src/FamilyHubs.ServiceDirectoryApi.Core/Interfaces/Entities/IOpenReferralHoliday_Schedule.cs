namespace fh_service_directory_api.core.Interfaces.Entities
{
    public interface IOpenReferralHoliday_Schedule : IEntityBase<string>
    {
        bool Closed { get; set; }
        DateTime? Closes_at { get; set; }
        DateTime? End_date { get; set; }
        DateTime? Opens_at { get; set; }
        DateTime? Start_date { get; set; }
    }
}