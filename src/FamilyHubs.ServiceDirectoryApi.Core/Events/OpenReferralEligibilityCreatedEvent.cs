using FamilyHubs.SharedKernel;
using fh_service_directory_api.core.Entities;

namespace fh_service_directory_api.core.Events;

public class OpenReferralEligibilityCreatedEvent : DomainEventBase
{
    public OpenReferralEligibilityCreatedEvent(OpenReferralEligibility item)
    {
        Item = item;
    }

    public OpenReferralEligibility Item { get; }
}