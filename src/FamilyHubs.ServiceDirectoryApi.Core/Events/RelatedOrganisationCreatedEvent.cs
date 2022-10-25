using FamilyHubs.SharedKernel;
using fh_service_directory_api.core.Entities;

namespace fh_service_directory_api.core.Events;

public class RelatedOrganisationCreatedEvent : DomainEventBase
{
    public RelatedOrganisationCreatedEvent(RelatedOrganisation item)
    {
        Item = item;
    }

    public RelatedOrganisation Item { get; }
}