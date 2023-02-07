using FamilyHubs.ServiceDirectory.Core.Entities;
using FamilyHubs.SharedKernel;

namespace FamilyHubs.ServiceDirectory.Core.Events;

public class TaxonomyCreatedEvent : DomainEventBase
{
    public TaxonomyCreatedEvent(Taxonomy item)
    {
        Item = item;
    }

    public Taxonomy Item { get; }
}
