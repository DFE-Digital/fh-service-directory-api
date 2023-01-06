using FamilyHubs.SharedKernel;
using fh_service_directory_api.core.Entities;

namespace fh_service_directory_api.core.Events
{
    public class OpenReferralContactLinkCreatedEvent : DomainEventBase
    {
        public OpenReferralContactLinkCreatedEvent(OpenReferralContactLink item)
        {
            Item = item;
        }

        public OpenReferralContactLink Item { get; }

    }
}
