using FamilyHubs.SharedKernel;
using fh_service_directory_api.core.Entities;

namespace fh_service_directory_api.core.Events;


public class OpenReferralServiceAreaCreatedEvent : DomainEventBase
{
    public OpenReferralServiceAreaCreatedEvent(OpenReferralService_Area item)
    {
        Item = item;
    }

    public OpenReferralService_Area Item { get; }
}
