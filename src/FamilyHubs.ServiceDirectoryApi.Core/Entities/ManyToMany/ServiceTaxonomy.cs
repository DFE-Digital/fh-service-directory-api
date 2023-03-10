namespace FamilyHubs.ServiceDirectory.Core.Entities.ManyToMany;

public class ServiceTaxonomy
{
    public required long ServiceId { get; set; }
    public required long TaxonomyId { get; set; }
}
