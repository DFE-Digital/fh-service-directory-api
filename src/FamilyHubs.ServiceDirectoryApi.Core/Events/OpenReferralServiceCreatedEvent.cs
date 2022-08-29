using FamilyHubs.ServiceDirectoryApi.Core.Entities.OpenReferralServices;
using FamilyHubs.SharedKernel;

namespace FamilyHubs.ServiceDirectoryApi.Core.Events;

public class OpenReferralServiceCreatedEvent : DomainEventBase
{
    public OpenReferralServiceCreatedEvent(IOpenReferralService item)
    {
        Item = item;
    }

    public IOpenReferralService Item { get; }
}
