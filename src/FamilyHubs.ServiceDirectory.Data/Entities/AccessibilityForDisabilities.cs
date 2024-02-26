using FamilyHubs.ServiceDirectory.Data.Entities.Base;

namespace FamilyHubs.ServiceDirectory.Data.Entities;

public class AccessibilityForDisabilities : EntityBase<long>
{
    public long? LocationId { get; set; }
    public string? Accessibility { get; set; }
}
