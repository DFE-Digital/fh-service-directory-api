using FamilyHubs.ServiceDirectory.Shared.Interfaces.Entities;
using FamilyHubs.SharedKernel;

namespace fh_service_directory_api.core.Events;

public class OpenReferralServiceCreatedEvent : DomainEventBase
{
    public OpenReferralServiceCreatedEvent(IOpenReferralService item)
    {
        Item = item;
    }

    public IOpenReferralService Item { get; }
}
