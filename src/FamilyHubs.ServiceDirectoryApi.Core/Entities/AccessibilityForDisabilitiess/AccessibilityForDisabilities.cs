using FamilyHubs.SharedKernel;
using FamilyHubs.SharedKernel.Interfaces;

namespace FamilyHubs.ServiceDirectoryApi.Core.Entities.AccessibilityForDisabilitiess;

public class AccessibilityForDisabilities : EntityBase<string>, IAggregateRoot, IAccessibilityForDisabilities
{
    private AccessibilityForDisabilities() { }
    public AccessibilityForDisabilities(string id, string accessibility)
    {
        Id = id;
        Accessibility = accessibility;
    }
    public string Accessibility { get; init; } = default!;

}
