using FamilyHubs.SharedKernel;
using FamilyHubs.SharedKernel.Interfaces;

namespace FamilyHubs.ServiceDirectory.Core.Entities;

public class Parent : EntityBase<string>, IAggregateRoot
{
    private Parent() { }
    public Parent(string id, string name, string? vocabulary, ICollection<ServiceTaxonomy>? serviceTaxonomies, ICollection<LinkTaxonomy>? linkTaxonomies)
    {
        Id = id;
        Name = name;
        Vocabulary = vocabulary;
        ServiceTaxonomies = serviceTaxonomies;
        LinkTaxonomies = linkTaxonomies;
    }
    public string Name { get; init; } = default!;
    public string? Vocabulary { get; init; }
    public virtual ICollection<ServiceTaxonomy>? ServiceTaxonomies { get; init; }
    public virtual ICollection<LinkTaxonomy>? LinkTaxonomies { get; init; }
}
