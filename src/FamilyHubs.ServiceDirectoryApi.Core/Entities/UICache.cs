using FamilyHubs.SharedKernel;

namespace fh_service_directory_api.core.Entities;

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
