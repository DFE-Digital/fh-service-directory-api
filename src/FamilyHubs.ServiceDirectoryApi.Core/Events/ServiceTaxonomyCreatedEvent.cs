using FamilyHubs.ServiceDirectory.Core.Entities;
using FamilyHubs.SharedKernel;

namespace FamilyHubs.ServiceDirectory.Core.Events;

public class ServiceTaxonomyCreatedEvent : DomainEventBase
{
    public ServiceTaxonomyCreatedEvent(ServiceTaxonomy item)
    {
        Item = item;
    }

    public ServiceTaxonomy Item { get; }
}
