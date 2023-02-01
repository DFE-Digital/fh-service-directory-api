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
    public string Name { get; set; } = default!;
    public string? Vocabulary { get; set; }
    public ICollection<ServiceTaxonomy>? ServiceTaxonomies { get; set; }
    public ICollection<LinkTaxonomy>? LinkTaxonomies { get; set; }
}
