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
        HolidayScheduleCollection = holidayScheduleCollection;
        Regular_schedule = regular_schedule;
    }

    public IOpenReferralLocation Location { get; init; } = default!;
    public virtual ICollection<IOpenReferralHoliday_Schedule>? HolidayScheduleCollection { get; init; }
    public virtual ICollection<IOpenReferralRegular_Schedule>? Regular_schedule { get; init; }
}
