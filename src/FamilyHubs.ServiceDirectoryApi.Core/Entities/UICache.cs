using FamilyHubs.SharedKernel;

namespace FamilyHubs.ServiceDirectory.Core.Entities;

public class UICache : EntityBase<string>
{
    private UICache() { }
    public UICache(string id, string value)
    {
        Id = id;
        Value = value;
    }
    public string Value { get; set; } = default!;
}
