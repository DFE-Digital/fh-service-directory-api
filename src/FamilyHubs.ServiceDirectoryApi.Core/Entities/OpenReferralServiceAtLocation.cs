using FamilyHubs.SharedKernel;
using FamilyHubs.SharedKernel.Interfaces;
using fh_service_directory_api.core.Interfaces.Entities;

namespace FamilyHubs.ServiceDirectory.Shared.Entities;

public class OpenReferralServiceAtLocation : EntityBase<string>, IOpenReferralServiceAtLocation, IAggregateRoot
{
    private OpenReferralServiceAtLocation() { }
    public OpenReferralServiceAtLocation(string id,
        OpenReferralLocation location,
        ICollection<IOpenReferralHoliday_Schedule>? holidayScheduleCollection, ICollection<IOpenReferralRegular_Schedule>? regular_schedule
        )
    {
        Id = id;
        Location = location;
        HolidayScheduleCollection = holidayScheduleCollection as ICollection<OpenReferralHoliday_Schedule>;
        Regular_schedule = regular_schedule as ICollection<OpenReferralRegular_Schedule>;
    }

    public OpenReferralLocation Location { get; init; } = default!;
    public virtual ICollection<OpenReferralHoliday_Schedule>? HolidayScheduleCollection { get; init; }
    public virtual ICollection<OpenReferralRegular_Schedule>? Regular_schedule { get; init; }
}
