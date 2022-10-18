using FamilyHubs.SharedKernel;
using FamilyHubs.SharedKernel.Interfaces;
using fh_service_directory_api.core.Interfaces.Entities;

namespace fh_service_directory_api.core.Entities;

public class OpenReferralRegular_Schedule : EntityBase<string>, IOpenReferralRegular_Schedule, IAggregateRoot
{
    private OpenReferralRegular_Schedule() { }
    public OpenReferralRegular_Schedule(string id, string description, DateTime? opens_at, DateTime? closes_at, string? byday, string? bymonthday, string? dtstart, string? freq, string? interval, DateTime? valid_from, DateTime? valid_to
        )
    {
        Id = id;
        Description = description;
        Opens_at = opens_at;
        Closes_at = closes_at;
        Byday = byday;
        Bymonthday = bymonthday;
        Dtstart = dtstart;
        Freq = freq;
        Interval = interval;
        Valid_from = valid_from;
        Valid_to = valid_to;
    }
    public string Description { get; set; } = default!;
    public DateTime? Opens_at { get; set; }
    public DateTime? Closes_at { get; set; }
    public string? Byday { get; set; }
    public string? Bymonthday { get; set; }
    public string? Dtstart { get; set; }
    public string? Freq { get; set; }
    public string? Interval { get; set; }
    public DateTime? Valid_from { get; set; }
    public DateTime? Valid_to { get; set; }
    //public string OpenReferralServiceAtLocationId { get; set; } = default!;
    //public string OpenReferralServiceId { get; set; } = default!;
}
