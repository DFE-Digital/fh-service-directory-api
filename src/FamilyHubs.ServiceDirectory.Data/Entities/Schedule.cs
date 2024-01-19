using FamilyHubs.ServiceDirectory.Data.Entities.Base;
using FamilyHubs.ServiceDirectory.Shared.Enums;
using System;

namespace FamilyHubs.ServiceDirectory.Data.Entities;

public class Schedule : ServiceLocationSharedEntityBase
{    
    public DateTime? OpensAt { get; set; }
    public DateTime? ClosesAt { get; set; }
    public DateTime? ValidFrom { get; set; }
    public DateTime? ValidTo { get; set; }
    public string? DtStart { get; set; }
    public FrequencyType Freq { get; set; }
    public int? Interval { get; set; }
    public string? ByDay { get; set; }
    public string? ByMonthDay { get; set; }
    public string? Description { get; set; }

    public int? Timezone { get; set; }
    public string? Until { get; set; }
    public int? Count { get; set; }
    public string? WkSt { get; set; } 
    public string? ByWeekNo { get;set; }
    public string? ByYearDay { get; set; }
    public string? ScheduleLink { get; set; }
    public string? AttendingType { get; set; }
    public string? Notes { get; set; }
}
