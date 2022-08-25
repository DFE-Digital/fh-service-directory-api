namespace fh_service_directory_api.core.Interfaces.Entities
{
    public interface IOpenReferralServiceAtLocation : IEntityBase<string>
    {
        ICollection<IOpenReferralHoliday_Schedule>? HolidayScheduleCollection { get; init; }
        IOpenReferralLocation Location { get; init; }
        ICollection<IOpenReferralRegular_Schedule>? Regular_schedule { get; init; }
    }
}