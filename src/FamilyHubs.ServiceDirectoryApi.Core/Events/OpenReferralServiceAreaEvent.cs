using FamilyHubs.SharedKernel;
using fh_service_directory_api.core.Entities;

namespace fh_service_directory_api.core.Events;


public class OpenReferralServiceAreaEvent : DomainEventBase
{
    public OpenReferralServiceAreaEvent(OpenReferralService_Area item)
    {
        Item = item;
    }

    public OpenReferralService_Area Item { get; }
}
