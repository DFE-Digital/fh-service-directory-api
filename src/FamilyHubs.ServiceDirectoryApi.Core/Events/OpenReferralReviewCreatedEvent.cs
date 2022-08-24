using FamilyHubs.ServiceDirectory.Shared.Interfaces.Entities;
using FamilyHubs.SharedKernel;

namespace fh_service_directory_api.core.Events;

public class OpenReferralReviewCreatedEvent : DomainEventBase
{
    public OpenReferralReviewCreatedEvent(IOpenReferralReview item)
    {
        Item = item;
    }

    public IOpenReferralReview Item { get; }
}