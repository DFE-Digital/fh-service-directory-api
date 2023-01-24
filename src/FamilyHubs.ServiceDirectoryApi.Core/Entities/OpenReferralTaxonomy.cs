using FamilyHubs.SharedKernel;
using FamilyHubs.SharedKernel.Interfaces;
using fh_service_directory_api.core.Interfaces.Entities;

namespace fh_service_directory_api.core.Entities;

public class OpenReferralTaxonomy : EntityBase<string>, IOpenReferralTaxonomy, IAggregateRoot
{
    private OpenReferralTaxonomy() { }
    public OpenReferralTaxonomy(string id, string name, string? vocabulary, string? parent
        )
    {
        Id = id;
        Name = name;
        Vocabulary = vocabulary;
        Parent = parent;
    }

    public string Name { get; set; } = default!;
    public string? Vocabulary { get; set; }
    public string? Parent { get; set; }
}
