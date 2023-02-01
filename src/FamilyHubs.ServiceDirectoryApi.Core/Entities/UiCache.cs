using FamilyHubs.SharedKernel;

namespace FamilyHubs.ServiceDirectory.Core.Entities;

public class UiCache : EntityBase<string>
{
    private UiCache() { }
    public UiCache(string id, string value)
    {
        Id = id;
        Value = value;
    }
    public string Value { get; set; } = default!;
}
