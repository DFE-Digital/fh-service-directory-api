using FamilyHubs.ServiceDirectory.Data.Entities.Base;
using FamilyHubs.ServiceDirectory.Shared.Enums;

namespace FamilyHubs.ServiceDirectory.Data.Entities;

public class Location : EntityBase<long>
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
    public IList<AccessibilityForDisabilities> AccessibilityForDisabilities { get; set; } = new List<AccessibilityForDisabilities>();
    public IList<Schedule> Schedules { get; set; } = new List<Schedule>();
    public IList<Contact> Contacts { get; set; } = new List<Contact>();
}
