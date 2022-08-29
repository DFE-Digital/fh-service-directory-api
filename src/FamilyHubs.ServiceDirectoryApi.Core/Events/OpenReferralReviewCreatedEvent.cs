using FamilyHubs.ServiceDirectoryApi.Core.Entities.OpenReferralReviews;
using FamilyHubs.SharedKernel;

namespace FamilyHubs.ServiceDirectoryApi.Core.Events;

public class OpenReferralReviewCreatedEvent : DomainEventBase
{
    public OpenReferralReviewCreatedEvent(IOpenReferralReview item)
    {
        Item = item;
    }

    public IOpenReferralReview Item { get; }
}