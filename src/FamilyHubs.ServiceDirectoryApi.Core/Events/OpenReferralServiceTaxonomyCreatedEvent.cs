using FamilyHubs.SharedKernel;
using fh_service_directory_api.core.Entities;

namespace fh_service_directory_api.core.Events;

public class OpenReferralServiceTaxonomyCreatedEvent : DomainEventBase
{
    public OpenReferralServiceTaxonomyCreatedEvent(OpenReferralService_Taxonomy item)
    {
        Item = item;
    }

    public OpenReferralService_Taxonomy Item { get; }
}
