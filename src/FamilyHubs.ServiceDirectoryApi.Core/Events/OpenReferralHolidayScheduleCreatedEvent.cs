using FamilyHubs.SharedKernel;
using fh_service_directory_api.core.Entities;

namespace fh_service_directory_api.core.Events;

public class OpenReferralHolidayScheduleCreatedEvent : DomainEventBase
{
    public OpenReferralHolidayScheduleCreatedEvent(OpenReferralHoliday_Schedule item)
    {
        Item = item;
    }

    public OpenReferralHoliday_Schedule Item { get; }
}
