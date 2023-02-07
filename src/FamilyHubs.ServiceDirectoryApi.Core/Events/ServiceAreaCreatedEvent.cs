using FamilyHubs.ServiceDirectory.Core.Entities;
using FamilyHubs.SharedKernel;

namespace FamilyHubs.ServiceDirectory.Core.Events;


public class ServiceAreaCreatedEvent : DomainEventBase
{
    public ServiceAreaCreatedEvent(ServiceArea item)
    {
        Item = item;
    }

    public ServiceArea Item { get; }
}
