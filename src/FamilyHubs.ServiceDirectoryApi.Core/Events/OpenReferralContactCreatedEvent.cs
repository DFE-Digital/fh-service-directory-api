using FamilyHubs.SharedKernel;
using fh_service_directory_api.core.Entities;

namespace fh_service_directory_api.core.Events;

public class OpenReferralContactCreatedEvent : DomainEventBase
{
    public OpenReferralContactCreatedEvent(OpenReferralContact item)
    {
        Item = item;
    }

    public OpenReferralContact Item { get; }
}
