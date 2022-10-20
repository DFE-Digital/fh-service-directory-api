using FamilyHubs.SharedKernel;
using fh_service_directory_api.core.Entities;

namespace fh_service_directory_api.core.Events;

public class OpenReferralServiceAtLocationCreatedEvent : DomainEventBase
{
    public OpenReferralServiceAtLocationCreatedEvent(OpenReferralServiceAtLocation item)
    {
        Item = item;
    }

    public OpenReferralServiceAtLocation Item { get; }
}
