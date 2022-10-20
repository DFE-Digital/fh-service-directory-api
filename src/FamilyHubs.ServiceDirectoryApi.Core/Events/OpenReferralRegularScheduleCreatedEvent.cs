using FamilyHubs.SharedKernel;
using fh_service_directory_api.core.Entities;

namespace fh_service_directory_api.core.Events;

public class OpenReferralRegularScheduleCreatedEvent : DomainEventBase
{
    public OpenReferralRegularScheduleCreatedEvent(OpenReferralRegular_Schedule item)
    {
        Item = item;
    }

    public OpenReferralRegular_Schedule Item { get; }
}
