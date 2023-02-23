using FamilyHubs.SharedKernel;
using FamilyHubs.SharedKernel.Interfaces;

namespace FamilyHubs.ServiceDirectory.Core.Entities;

public class RegularSchedule : EntityBase<string>, IAggregateRoot
{
    private RegularSchedule() { }
    public RegularSchedule(
        string id, 
        string description, 
        DateTime? opensAt, 
        DateTime? closesAt, 
        string? byDay, 
        string? byMonthDay, 
        string? dtStart, 
        string? freq, 
        string? interval, 
        DateTime? validFrom,
        DateTime? validTo
        )
    {
        Id = id;
        Description = description;
        OpensAt = opensAt;
        ClosesAt = closesAt;
        ByDay = byDay;
        ByMonthDay = byMonthDay;
        DtStart = dtStart;
        Freq = freq;
        Interval = interval;
        ValidFrom = validFrom;
        ValidTo = validTo;
    }
    public string Description { get; set; } = default!;
    public DateTime? OpensAt { get; set; }
    public DateTime? ClosesAt { get; set; }
    public string? ByDay { get; set; }
    public string? ByMonthDay { get; set; }
    public string? DtStart { get; set; }
    public string? Freq { get; set; }
    public string? Interval { get; set; }
    public DateTime? ValidFrom { get; set; }
    public DateTime? ValidTo { get; set; }
    public string ServiceAtLocationId { get; set; } = default!;
    public string ServiceId { get; set; } = default!;
}
