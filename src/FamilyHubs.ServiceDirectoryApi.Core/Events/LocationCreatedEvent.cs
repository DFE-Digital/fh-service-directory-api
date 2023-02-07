using FamilyHubs.ServiceDirectory.Core.Entities;
using FamilyHubs.SharedKernel;

namespace FamilyHubs.ServiceDirectory.Core.Events;

public class LocationCreatedEvent : DomainEventBase
{
    public LocationCreatedEvent(Location item)
    {
        Item = item;
    }

    public Location Item { get; }
}
