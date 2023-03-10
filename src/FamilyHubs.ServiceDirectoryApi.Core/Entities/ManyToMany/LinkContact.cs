namespace FamilyHubs.ServiceDirectory.Core.Entities.ManyToMany;
public enum LinkContactType
{
    NotSet       = 0,
    Service      = 1,
    Location     = 2,
    Organisation = 3
}
public class LinkContact
{
    public long ContactId { get; set; }
    public long? LinkId { get; set; }
    public LinkTaxonomyType LinkType { get; set; }
}