using FamilyHubs.SharedKernel;
using FamilyHubs.SharedKernel.Interfaces;

namespace FamilyHubs.ServiceDirectory.Core.Entities;

public class ServiceAtLocation : EntityBase<string>, IAggregateRoot
{
    private ServiceAtLocation() { }
    public ServiceAtLocation(string id,
        Location location,
        ICollection<RegularSchedule>? regularSchedules,
        ICollection<HolidaySchedule>? holidaySchedules,
        ICollection<LinkContact>? linkContacts
        )
    {
        Id = id;
        Location = location;
        HolidaySchedules = holidaySchedules;
        RegularSchedules = regularSchedules;
        LinkContacts = linkContacts;
    }

    public Location Location { get; set; } = default!;
    public ICollection<RegularSchedule>? RegularSchedules { get; set; }
    public ICollection<HolidaySchedule>? HolidaySchedules { get; set; }
    public ICollection<LinkContact>? LinkContacts { get; set; }
    public string ServiceId { get; set; } = default!;
}
