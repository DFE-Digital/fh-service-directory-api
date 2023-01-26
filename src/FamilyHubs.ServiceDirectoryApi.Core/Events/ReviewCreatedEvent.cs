using FamilyHubs.ServiceDirectory.Core.Entities;
using FamilyHubs.SharedKernel;

namespace FamilyHubs.ServiceDirectory.Core.Events;

public class ReviewCreatedEvent : DomainEventBase
{
    public ReviewCreatedEvent(Review item)
    {
        Item = item;
    }

    public Review Item { get; }
}