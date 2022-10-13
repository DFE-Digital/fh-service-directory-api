using FamilyHubs.SharedKernel;
using fh_service_directory_api.core.Entities;

namespace fh_service_directory_api.core.Events;

public class OpenReferralServiceAtLocationEvent : DomainEventBase
{
    public OpenReferralServiceAtLocationEvent(OpenReferralServiceAtLocation item)
    {
        Item = item;
    }

    public OpenReferralServiceAtLocation Item { get; }
}
