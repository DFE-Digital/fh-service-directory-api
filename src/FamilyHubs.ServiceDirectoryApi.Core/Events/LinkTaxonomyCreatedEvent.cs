using FamilyHubs.ServiceDirectory.Core.Entities;
using FamilyHubs.SharedKernel;

namespace FamilyHubs.ServiceDirectory.Core.Events;

public class LinkTaxonomyCreatedEvent : DomainEventBase
{
    public LinkTaxonomyCreatedEvent(LinkTaxonomy item)
    {
        Item = item;
    }

    public LinkTaxonomy Item { get; }
}
