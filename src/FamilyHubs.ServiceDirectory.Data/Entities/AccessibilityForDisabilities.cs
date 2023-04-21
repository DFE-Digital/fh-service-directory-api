using FamilyHubs.ServiceDirectory.Data.Entities.Base;

namespace FamilyHubs.ServiceDirectory.Data.Entities;

public class AccessibilityForDisabilities : LocationEntityBase<long>
{
    public string? Accessibility { get; set; }
}
