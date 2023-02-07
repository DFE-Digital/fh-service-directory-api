using FamilyHubs.ServiceDirectory.Core.Entities;
using FamilyHubs.SharedKernel;

namespace FamilyHubs.ServiceDirectory.Core.Events;

public class OrganisationCreatedEvent : DomainEventBase
{
    public OrganisationCreatedEvent(Organisation item)
    {
        Item = item;
    }

    public Organisation Item { get; }
}
