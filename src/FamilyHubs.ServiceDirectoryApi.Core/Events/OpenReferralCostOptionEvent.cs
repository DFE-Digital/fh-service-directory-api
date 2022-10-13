using FamilyHubs.SharedKernel;
using fh_service_directory_api.core.Entities;

namespace fh_service_directory_api.core.Events;

public class OpenReferralCostOptionEvent : DomainEventBase
{
    public OpenReferralCostOptionEvent(OpenReferralCost_Option item)
    {
        Item = item;
    }

    public OpenReferralCost_Option Item { get; }
}