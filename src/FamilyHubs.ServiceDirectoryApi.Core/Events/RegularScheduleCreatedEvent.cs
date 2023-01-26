using FamilyHubs.ServiceDirectory.Core.Entities;
using FamilyHubs.SharedKernel;

namespace FamilyHubs.ServiceDirectory.Core.Events;

public class RegularScheduleCreatedEvent : DomainEventBase
{
    public RegularScheduleCreatedEvent(RegularSchedule item)
    {
        Item = item;
    }

    public RegularSchedule Item { get; }
}
