using FamilyHubs.SharedKernel;
using FamilyHubs.SharedKernel.Interfaces;

namespace FamilyHubs.ServiceDirectory.Core.Entities;

public class ServiceAtLocation : EntityBase<string>, IAggregateRoot
{
    private ServiceAtLocation() { }
    public ServiceAtLocation(string id,
        Location location,
        ICollection<RegularSchedule>? regularSchedules,
        ICollection<HolidaySchedule>? holidaySchedules
        )
    {
        Id = id;
        Location = location;
        HolidaySchedules = holidaySchedules;
        RegularSchedules = regularSchedules;
    }

    public Location Location { get; set; } = default!;
    public virtual ICollection<RegularSchedule>? RegularSchedules { get; set; }
    public virtual ICollection<HolidaySchedule>? HolidaySchedules { get; set; }
    public string ServiceId { get; set; } = default!;
}
