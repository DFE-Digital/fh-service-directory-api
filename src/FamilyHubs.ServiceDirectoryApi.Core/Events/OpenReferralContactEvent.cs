using FamilyHubs.SharedKernel;
using fh_service_directory_api.core.Entities;

namespace fh_service_directory_api.core.Events;

public class OpenReferralContactEvent : DomainEventBase
{
    public OpenReferralContactEvent(OpenReferralContact item)
    {
        Item = item;
    }

    public OpenReferralContact Item { get; }
}
