using FamilyHubs.ServiceDirectory.Core.Entities;
using FamilyHubs.SharedKernel;

namespace FamilyHubs.ServiceDirectory.Core.Events;

public class ContactCreatedEvent : DomainEventBase
{
    public ContactCreatedEvent(Contact item)
    {
        Item = item;
    }

    public Contact Item { get; }
}
