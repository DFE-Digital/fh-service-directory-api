using FamilyHubs.ServiceDirectory.Core.Entities;
using FamilyHubs.SharedKernel;

namespace FamilyHubs.ServiceDirectory.Core.Events;

public class HolidayScheduleCreatedEvent : DomainEventBase
{
    public HolidayScheduleCreatedEvent(HolidaySchedule item)
    {
        Item = item;
    }

    public HolidaySchedule Item { get; }
}
