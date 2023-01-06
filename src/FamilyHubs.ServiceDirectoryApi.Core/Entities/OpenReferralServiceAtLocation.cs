using FamilyHubs.SharedKernel;
using FamilyHubs.SharedKernel.Interfaces;
using fh_service_directory_api.core.Interfaces.Entities;

namespace fh_service_directory_api.core.Entities;

public class OpenReferralServiceAtLocation : EntityBase<string>, IOpenReferralServiceAtLocation, IAggregateRoot
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
        HolidayScheduleCollection = holidayScheduleCollection as ICollection<OpenReferralHoliday_Schedule>;
        Regular_schedule = regular_schedule as ICollection<OpenReferralRegular_Schedule>;
    }

    public OpenReferralLocation Location { get; set; } = default!;
    public virtual ICollection<OpenReferralRegular_Schedule>? Regular_schedule { get; set; }
    public virtual ICollection<OpenReferralHoliday_Schedule>? HolidayScheduleCollection { get; set; }
    public virtual ICollection<OpenReferralContactLink>? ContactLinks { get; set; }
    public string OpenReferralServiceId { get; set; } = default!;

}
