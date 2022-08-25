using FamilyHubs.SharedKernel;
using fh_service_directory_api.core.Interfaces.Entities;

namespace fh_service_directory_api.core.Events;

public class OpenReferralServiceCreatedEvent : DomainEventBase
{
    public OpenReferralServiceCreatedEvent(IOpenReferralService item)
    {
        Item = item;
    }

    public IOpenReferralService Item { get; }
}
