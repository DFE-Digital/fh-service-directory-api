using FamilyHubs.ServiceDirectoryApi.Core.Entities.OpenReferralHolidaySchedules;
using FamilyHubs.ServiceDirectoryApi.Core.Entities.OpenReferralLocations;
using FamilyHubs.ServiceDirectoryApi.Core.Entities.OpenReferralRegularSchedules;
using FamilyHubs.SharedKernel;
using FamilyHubs.SharedKernel.Interfaces;

namespace FamilyHubs.ServiceDirectoryApi.Core.Entities.OpenReferralServiceAtLocations;

public class OpenReferralServiceAtLocation : EntityBase<string>, IOpenReferralServiceAtLocation, IAggregateRoot
{
    private OpenReferralServiceAtLocation() { }
    public OpenReferralServiceAtLocation(string id,
        OpenReferralLocation location,
        ICollection<OpenReferralHolidaySchedule>? holidayScheduleCollection,
        ICollection<OpenReferralRegularSchedule>? regular_schedule
    )
    {
        Id = id;
        Location = location;
        HolidayScheduleCollection = holidayScheduleCollection as ICollection<OpenReferralHolidaySchedule>;
        Regular_schedule = regular_schedule as ICollection<OpenReferralRegularSchedule>;
    }

    public OpenReferralLocation Location { get; init; } = default!;
    public virtual ICollection<OpenReferralHolidaySchedule>? HolidayScheduleCollection { get; init; }
    public virtual ICollection<OpenReferralRegularSchedule>? Regular_schedule { get; init; }
}
