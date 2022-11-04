using FamilyHubs.SharedKernel;
using fh_service_directory_api.core.Entities;

namespace fh_service_directory_api.core.Events;

public class OpenReferralPhoneCreatedEvent : DomainEventBase
{
    public OpenReferralPhoneCreatedEvent(OpenReferralPhone item)
    {
        Item = item;
    }

    public OpenReferralPhone Item { get; }
}
