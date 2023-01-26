using FamilyHubs.ServiceDirectory.Core.Entities;
using FamilyHubs.SharedKernel;

namespace FamilyHubs.ServiceDirectory.Core.Events;

public class ServiceAtLocationCreatedEvent : DomainEventBase
{
    public ServiceAtLocationCreatedEvent(ServiceAtLocation item)
    {
        Item = item;
    }

    public ServiceAtLocation Item { get; }
}
