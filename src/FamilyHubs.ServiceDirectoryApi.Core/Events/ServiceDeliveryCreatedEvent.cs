using FamilyHubs.ServiceDirectory.Core.Entities;
using FamilyHubs.SharedKernel;

namespace FamilyHubs.ServiceDirectory.Core.Events;

public class ServiceDeliveryCreatedEvent : DomainEventBase
{
    public ServiceDeliveryCreatedEvent(ServiceDelivery item)
    {
        Item = item;
    }

    public ServiceDelivery Item { get; }
}
