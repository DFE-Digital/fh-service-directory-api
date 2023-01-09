using FamilyHubs.SharedKernel;
using fh_service_directory_api.core.Entities;

namespace fh_service_directory_api.core.Events
{
    public class OpenReferralLinkContactCreatedEvent : DomainEventBase
    {
        public OpenReferralLinkContactCreatedEvent(OpenReferralLinkContact item)
        {
            Item = item;
        }

        public OpenReferralLinkContact Item { get; }

    }
}
