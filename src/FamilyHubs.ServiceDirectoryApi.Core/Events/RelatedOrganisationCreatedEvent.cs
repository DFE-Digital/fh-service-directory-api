using FamilyHubs.ServiceDirectory.Core.Entities;
using FamilyHubs.SharedKernel;

namespace FamilyHubs.ServiceDirectory.Core.Events;

public class RelatedOrganisationCreatedEvent : DomainEventBase
{
    public RelatedOrganisationCreatedEvent(RelatedOrganisation item)
    {
        Item = item;
    }

    public RelatedOrganisation Item { get; }
}