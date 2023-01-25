using FamilyHubs.SharedKernel;
using FamilyHubs.SharedKernel.Interfaces;

namespace fh_service_directory_api.core.Entities;

public class OpenReferralServiceAtLocation : EntityBase<string>, IAggregateRoot
{
    private OpenReferralServiceAtLocation() { }
    public OpenReferralServiceAtLocation(string id,
        OpenReferralLocation location,
        ICollection<OpenReferralRegular_Schedule>? regular_schedule,
        ICollection<OpenReferralHoliday_Schedule>? holidayScheduleCollection
        )
    {
        Id = id;
        Location = location;
        HolidayScheduleCollection = holidayScheduleCollection;
        Regular_schedule = regular_schedule;
    }

    public OpenReferralLocation Location { get; set; } = default!;
    public virtual ICollection<OpenReferralRegular_Schedule>? Regular_schedule { get; set; }
    public virtual ICollection<OpenReferralHoliday_Schedule>? HolidayScheduleCollection { get; set; }
    public string OpenReferralServiceId { get; set; } = default!;

}
