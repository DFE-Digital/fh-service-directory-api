using FamilyHubs.ServiceDirectory.Data.Entities.Base;
using FamilyHubs.ServiceDirectory.Shared.Enums;

namespace FamilyHubs.ServiceDirectory.Data.Entities;

public class Schedule : ServiceLocationSharedEntityBase
{    
    public DateTime? OpensAt { get; set; }
    public DateTime? ClosesAt { get; set; }
    public DateTime? ValidFrom { get; set; }
    public DateTime? ValidTo { get; set; }
    public string? DtStart { get; set; }
    public FrequencyType? Freq { get; set; }
    public int? Interval { get; set; }
    /// <summary>
    /// The BYDAY rule part specifies a COMMA-separated list of days of the week; SU indicates Sunday; MO indicates Monday; TU indicates Tuesday; WE indicates Wednesday; TH indicates Thursday; FR indicates Friday; and SA indicates Saturday.
    /// Each BYDAY value can also be preceded by a positive(+n) or negative(-n) integer.If present, this indicates the nth occurrence of a specific day within the MONTHLY or YEARLY "RRULE". For example, within a MONTHLY rule, +1MO(or simply 1MO) represents the first Monday within the month, whereas -1MO represents the last Monday of the month.The numeric value in a BYDAY rule part with the FREQ rule part set to YEARLY corresponds to an offset within the month when the BYMONTH rule part is present, and corresponds to an offset within the year when the BYWEEKNO or BYMONTH rule parts are present.If an integer modifier is not present, it means all days of this type within the specified frequency.For example, within a MONTHLY rule, MO represents all Mondays within the month.The BYDAY rule part MUST NOT be specified with a numeric value when the FREQ rule part is not set to MONTHLY or YEARLY. Furthermore, the BYDAY rule part MUST NOT be specified with a numeric value with the FREQ rule part set to YEARLY when the BYWEEKNO rule part is specified.
    /// </summary>
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
