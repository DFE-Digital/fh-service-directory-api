using FamilyHubs.SharedKernel;
using FamilyHubs.SharedKernel.Interfaces;

namespace FamilyHubs.ServiceDirectory.Core.Entities;

public class HolidaySchedule : EntityBase<long>, IAggregateRoot
{
    public bool Closed { get; set; }
    public DateTime? OpensAt { get; set; }
    public DateTime? ClosesAt { get; set; }
    public required DateTime StartDate { get; set; }
    public required DateTime EndDate { get; set; }
    public long? ServiceId { get; set; }
    public long? LocationId { get; set; }
}
