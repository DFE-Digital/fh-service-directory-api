using FamilyHubs.ServiceDirectory.Shared.Enums;
using FamilyHubs.SharedKernel;
using FamilyHubs.SharedKernel.Interfaces;

namespace FamilyHubs.ServiceDirectory.Core.Entities;

public class Location : EntityBase<long>, IAggregateRoot
{
    public required LocationType LocationType { get; set; }
    public required string? Name { get; set; }
    public string? Description { get; set; }
    public required double Latitude { get; set; }
    public required double Longitude { get; set; }
    public required string Address1 { get; set; }
    public string? Address2 { get; set; }
    public required string City { get; set; }
    public required string PostCode { get; set; }
    public required string StateProvince { get; set; }
    public required string Country { get; set; }
    public ICollection<AccessibilityForDisabilities> AccessibilityForDisabilities { get; set; } = new List<AccessibilityForDisabilities>();
    public ICollection<RegularSchedule> RegularSchedules { get; set; } = new List<RegularSchedule>();
    public ICollection<HolidaySchedule> HolidaySchedules { get; set; } = new List<HolidaySchedule>();
    public ICollection<Contact> Contacts { get; set; } = new List<Contact>();
}
