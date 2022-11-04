using FamilyHubs.SharedKernel;
using fh_service_directory_api.core.Entities;

namespace fh_service_directory_api.core.Events;

public class OpenReferralLocationCreatedEvent : DomainEventBase
{
    public OpenReferralLocationCreatedEvent(OpenReferralLocation item)
    {
        Item = item;
    }

    public OpenReferralLocation Item { get; }
}
