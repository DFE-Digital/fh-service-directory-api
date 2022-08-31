using FamilyHubs.SharedKernel;
using fh_service_directory_api.core.Entities;
using fh_service_directory_api.core.Interfaces.Events;

namespace fh_service_directory_api.core.Events;

public class OpenReferralTaxonomyCreatedEvent : DomainEventBase, IOpenReferralTaxonomyCreatedEvent
{
    public OpenReferralTaxonomyCreatedEvent(OpenReferralTaxonomy item)
    {
        Item = item;
    }

    public OpenReferralTaxonomy Item { get; }
}
