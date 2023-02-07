using FamilyHubs.ServiceDirectory.Core.Entities;
using FamilyHubs.SharedKernel;

namespace FamilyHubs.ServiceDirectory.Core.Events;

public class ServiceCreatedEvent : DomainEventBase
{
    public ServiceCreatedEvent(Service item)
    {
        Item = item;
    }

    public Service Item { get; }
}
