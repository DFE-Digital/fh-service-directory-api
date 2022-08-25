using FamilyHubs.SharedKernel;
using FamilyHubs.SharedKernel.Interfaces;
using fh_service_directory_api.core.Interfaces.Entities;

namespace FamilyHubs.ServiceDirectory.Shared.Entities;

public class AccessibilityForDisabilities : EntityBase<string>, IAccessibilityForDisabilities, IAggregateRoot
{
    private AccessibilityForDisabilities() { }
    public AccessibilityForDisabilities(string id, string accessibility)
    {
        Id = id;
        Accessibility = accessibility;
    }
    public string Accessibility { get; init; } = default!;

}
