using FamilyHubs.ServiceDirectory.Core.Entities;
using FamilyHubs.SharedKernel;

namespace FamilyHubs.ServiceDirectory.Core.Events;

public class CostOptionCreatedEvent : DomainEventBase
{
    public CostOptionCreatedEvent(CostOption item)
    {
        Item = item;
    }

    public CostOption Item { get; }
}