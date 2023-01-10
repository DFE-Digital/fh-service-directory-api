using FamilyHubs.SharedKernel;
using fh_service_directory_api.core.Entities;

namespace fh_service_directory_api.core.Events;

public class OpenReferralLinkTaxonomyCreatedEvent : DomainEventBase
{
    public OpenReferralLinkTaxonomyCreatedEvent(OpenReferralLinkTaxonomy item)
    {
        Item = item;
    }

    public OpenReferralLinkTaxonomy Item { get; }
}
