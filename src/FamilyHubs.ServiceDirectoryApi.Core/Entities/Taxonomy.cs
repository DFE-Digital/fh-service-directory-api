using FamilyHubs.ServiceDirectory.Shared.Enums;
using FamilyHubs.SharedKernel;
using FamilyHubs.SharedKernel.Interfaces;

namespace FamilyHubs.ServiceDirectory.Core.Entities;

public class Taxonomy : EntityBase<string>, IAggregateRoot
{
    private Taxonomy() { }
    public Taxonomy(string id, string name, TaxonomyType taxonomyType, string? parent)
    {
        Id = id;
        Name = name;
        TaxonomyType = taxonomyType;
        Parent = parent;
    }

    public string Name { get; set; } = default!;
    public TaxonomyType TaxonomyType { get; set; }
    public string? Parent { get; set; }
}
