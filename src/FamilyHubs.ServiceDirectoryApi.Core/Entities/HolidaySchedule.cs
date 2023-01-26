using FamilyHubs.SharedKernel;
using FamilyHubs.SharedKernel.Interfaces;

namespace FamilyHubs.ServiceDirectory.Core.Entities;

public class HolidaySchedule : EntityBase<string>, IAggregateRoot
{
    private HolidaySchedule() { }
    public HolidaySchedule(
        string id, 
        bool closed, 
        DateTime? closesAt, 
        DateTime? startDate, 
        DateTime? endDate, 
        DateTime? opensAt
        )
    {
        Id = id;
        Closed = closed;
        ClosesAt = closesAt;
        StartDate = startDate;
        EndDate = endDate;
        OpensAt = opensAt;
    }
    public bool Closed { get; set; }
    public DateTime? ClosesAt { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public DateTime? OpensAt { get; set; }
    public string ServiceAtLocationId { get; set; } = default!;
}
