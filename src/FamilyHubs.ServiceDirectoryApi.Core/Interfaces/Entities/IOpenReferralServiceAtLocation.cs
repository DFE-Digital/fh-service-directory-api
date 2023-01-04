using fh_service_directory_api.core.Entities;

namespace fh_service_directory_api.core.Interfaces.Entities
{
    public interface IOpenReferralServiceAtLocation : IEntityBase<string>
    {
        ICollection<OpenReferralHoliday_Schedule>? HolidayScheduleCollection { get; set; }
        OpenReferralLocation Location { get; set; }
        ICollection<OpenReferralRegular_Schedule>? Regular_schedule { get; set; }
        ICollection<OpenReferralContactLink>? ContactLinks { get; set; }
    }
}