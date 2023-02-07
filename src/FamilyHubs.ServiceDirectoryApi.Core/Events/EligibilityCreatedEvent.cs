using FamilyHubs.ServiceDirectory.Core.Entities;
using FamilyHubs.SharedKernel;

namespace FamilyHubs.ServiceDirectory.Core.Events;

public class EligibilityCreatedEvent : DomainEventBase
{
    public EligibilityCreatedEvent(Eligibility item)
    {
        Item = item;
    }

    public Eligibility Item { get; }
}