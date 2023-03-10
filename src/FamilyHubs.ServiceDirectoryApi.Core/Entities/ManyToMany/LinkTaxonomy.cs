namespace FamilyHubs.ServiceDirectory.Core.Entities.ManyToMany;

public enum LinkTaxonomyType
{
    NotSet        = 0,
    Organisation  = 1,
    Eligibility   = 2,
    CostOption    = 3,
    ServiceArea   = 4, 
}

public class LinkTaxonomy
{
    public long TaxonomyId { get; set; }
    public long? LinkId { get; set; }
    public LinkTaxonomyType LinkType { get; set; }
}