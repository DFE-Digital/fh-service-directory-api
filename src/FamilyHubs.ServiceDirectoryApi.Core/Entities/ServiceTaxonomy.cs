using FamilyHubs.SharedKernel;
using FamilyHubs.SharedKernel.Interfaces;

namespace FamilyHubs.ServiceDirectory.Core.Entities;

public class ServiceTaxonomy : EntityBase<string>, IAggregateRoot
{
    private ServiceTaxonomy() { }
    public ServiceTaxonomy(
        string id, 
        string? linkId, 
        Taxonomy? taxonomy)
    {
        Id = id;
        LinkId = linkId;
        Taxonomy = taxonomy;
    }
    public string? LinkId { get; set; }
    public Taxonomy? Taxonomy { get; set; }
    public string ServiceId { get; set; } = default!;
}
