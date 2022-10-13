using FamilyHubs.SharedKernel;
using fh_service_directory_api.core.Entities;

namespace fh_service_directory_api.core.Events;

public class OpenReferralServiceDeliveryEvent : DomainEventBase
{
    public OpenReferralServiceDeliveryEvent(OpenReferralServiceDelivery item)
    {
        Item = item;
    }

    public OpenReferralServiceDelivery Item { get; }
}
