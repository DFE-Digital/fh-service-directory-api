namespace FamilyHubs.ServiceDirectoryApi.Core.Entities.OpenReferralHolidaySchedules;

public interface IOpenReferralHolidaySchedule : IEntityBase<string>
{
    bool Closed { get; init; }
    DateTime? Closes_at { get; init; }
    DateTime? End_date { get; init; }
    DateTime? Opens_at { get; init; }
    DateTime? Start_date { get; init; }
}