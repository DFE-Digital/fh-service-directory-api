using FamilyHubs.SharedKernel;
using FamilyHubs.SharedKernel.Interfaces;

namespace FamilyHubs.ServiceDirectory.Core.Entities;

public class AccessibilityForDisabilities : EntityBase<string>, IAggregateRoot
{
    private AccessibilityForDisabilities() { }
    public AccessibilityForDisabilities(
        string id, 
        string accessibility)
    {
        Id = id;
        Accessibility = accessibility;
    }
    public string Accessibility { get; set; } = default!;
    public string LocationId { get; set; } = default!;
}
