using FamilyHubs.SharedKernel;
using fh_service_directory_api.core.Entities;

namespace fh_service_directory_api.core.Events;

public class OpenReferralEligibilityEvent : DomainEventBase
{
    public OpenReferralEligibilityEvent(OpenReferralEligibility item)
    {
        Item = item;
    }

    public OpenReferralEligibility Item { get; }
}