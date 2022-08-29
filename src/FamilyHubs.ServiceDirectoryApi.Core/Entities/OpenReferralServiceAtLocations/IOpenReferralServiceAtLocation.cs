using FamilyHubs.ServiceDirectoryApi.Core.Entities.OpenReferralLocations;
using FamilyHubs.ServiceDirectoryApi.Core.Entities.OpenReferralRegularSchedules;

namespace FamilyHubs.ServiceDirectoryApi.Core.Entities.OpenReferralServiceAtLocations;

public interface IOpenReferralServiceAtLocation : IEntityBase<string>
{
    ICollection<OpenReferralHolidaySchedules.OpenReferralHolidaySchedule>? HolidayScheduleCollection { get; init; }
    OpenReferralLocation Location { get; init; }
    ICollection<OpenReferralRegularSchedule>? Regular_schedule { get; init; }
}