using FamilyHubs.SharedKernel;
using FamilyHubs.SharedKernel.Interfaces;

namespace fh_service_directory_api.core.Entities;

public class OpenReferralHoliday_Schedule : EntityBase<string>, IAggregateRoot
{
    private OpenReferralHoliday_Schedule() { }
    public OpenReferralHoliday_Schedule(string id, bool closed, DateTime? closes_at, DateTime? start_date, DateTime? end_date, DateTime? opens_at
        )
    {
        Id = id;
        Closed = closed;
        Closes_at = closes_at;
        Start_date = start_date;
        End_date = end_date;
        Opens_at = opens_at;
    }
    public bool Closed { get; set; }
    public DateTime? Closes_at { get; set; }
    public DateTime? Start_date { get; set; }
    public DateTime? End_date { get; set; }
    public DateTime? Opens_at { get; set; }
    public string OpenReferralServiceAtLocationId { get; set; } = default!;
    
}
