using FamilyHubs.ServiceDirectory.Shared.Enums;
using FamilyHubs.SharedKernel;
using FamilyHubs.SharedKernel.Interfaces;

namespace FamilyHubs.ServiceDirectory.Core.Entities;

public class RegularSchedule : EntityBase<long>, IAggregateRoot
{
    public DayOfWeek Weekday { get; set; }
    public DateTime? OpensAt { get; set; }
    public DateTime? ClosesAt { get; set; }
    public DateTime? ValidFrom { get; set; }
    public DateTime? ValidTo { get; set; }
    public string? DtStart { get; set; }
    public FrequencyType Freq { get; set; }
    public string? Interval { get; set; }
    public string? ByDay { get; set; }
    public string? ByMonthDay { get; set; }
    public string? Description { get; set; }
    public long? ServiceId { get; set; }
    public long? LocationId { get; set; }
}
