using FamilyHubs.ServiceDirectory.Core.Entities;
using FamilyHubs.SharedKernel;

namespace FamilyHubs.ServiceDirectory.Core.Events;


public class PhysicalAddressCreatedEvent : DomainEventBase
{
    public PhysicalAddressCreatedEvent(PhysicalAddress item)
    {
        Item = item;
    }

    public PhysicalAddress Item { get; }
}
