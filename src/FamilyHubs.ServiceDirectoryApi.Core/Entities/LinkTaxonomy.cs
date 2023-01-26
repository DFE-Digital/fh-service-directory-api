using FamilyHubs.SharedKernel;
using FamilyHubs.SharedKernel.Interfaces;

namespace FamilyHubs.ServiceDirectory.Core.Entities;

public class LinkTaxonomy : EntityBase<string>, IAggregateRoot
{
    private LinkTaxonomy() { }
    public LinkTaxonomy(string id, string linkId, string linkType, Taxonomy? taxonomy)
    {
        Id = id;
        Taxonomy = taxonomy;
        LinkId = linkId;
        LinkType = linkType;
    }
    public Taxonomy? Taxonomy { get; set; }
    public string LinkId { get; set; } = default!;
    public string LinkType { get; set; } = default!;
}
