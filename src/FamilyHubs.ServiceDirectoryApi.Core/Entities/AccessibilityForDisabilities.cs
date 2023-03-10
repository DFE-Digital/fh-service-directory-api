using FamilyHubs.SharedKernel;
using FamilyHubs.SharedKernel.Interfaces;

namespace FamilyHubs.ServiceDirectory.Core.Entities;

public class AccessibilityForDisabilities : EntityBase<long>, IAggregateRoot
{
    public string? Accessibility { get; set; }
    public required long LocationId { get; set; }
}
