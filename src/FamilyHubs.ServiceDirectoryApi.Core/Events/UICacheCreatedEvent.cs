using FamilyHubs.ServiceDirectory.Core.Entities;
using FamilyHubs.SharedKernel;

namespace FamilyHubs.ServiceDirectory.Core.Events;


public class UICacheCreatedEvent : DomainEventBase
{
    public UICacheCreatedEvent(UICache item)
    {
        Item = item;
    }

    public UICache Item { get; }
}

