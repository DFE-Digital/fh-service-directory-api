using FamilyHubs.SharedKernel;
using FamilyHubs.SharedKernel.Interfaces;

namespace FamilyHubs.ServiceDirectory.Core.Entities;

public class Taxonomy : EntityBase<string>, IAggregateRoot
{
    private Taxonomy() { }
    public Taxonomy(string id, string name, string? vocabulary, string? parent
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
