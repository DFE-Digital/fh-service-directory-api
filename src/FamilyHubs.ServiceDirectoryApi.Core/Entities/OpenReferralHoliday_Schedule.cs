﻿using FamilyHubs.SharedKernel;
using FamilyHubs.SharedKernel.Interfaces;
using fh_service_directory_api.core.Interfaces.Entities;

namespace FamilyHubs.ServiceDirectory.Shared.Entities;

public class OpenReferralHoliday_Schedule : EntityBase<string>, IOpenReferralHoliday_Schedule, IAggregateRoot
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
    public bool Closed { get; init; }
    public DateTime? Closes_at { get; init; }
    public DateTime? Start_date { get; init; }
    public DateTime? End_date { get; init; }
    public DateTime? Opens_at { get; init; }
}
