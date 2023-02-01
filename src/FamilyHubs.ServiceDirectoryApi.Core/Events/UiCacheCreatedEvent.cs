using FamilyHubs.ServiceDirectory.Core.Entities;
using FamilyHubs.SharedKernel;

namespace FamilyHubs.ServiceDirectory.Core.Events;


public class UiCacheCreatedEvent : DomainEventBase
{
    public UiCacheCreatedEvent(UiCache item)
    {
        Item = item;
    }

    public UiCache Item { get; }
}

