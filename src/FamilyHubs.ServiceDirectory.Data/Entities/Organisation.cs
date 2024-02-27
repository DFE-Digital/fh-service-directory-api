using FamilyHubs.ServiceDirectory.Data.Entities.Base;
using FamilyHubs.ServiceDirectory.Shared.Enums;

namespace FamilyHubs.ServiceDirectory.Data.Entities;

public class Organisation : EntityBase<long>
{
    public required OrganisationType OrganisationType { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required string AdminAreaCode { get; set; }
    public long? AssociatedOrganisationId { get; set; }
    public string? Logo { get; set; }
    public string? Uri { get; set; }
    public string? Url { get; set; }
    public IList<Service> Services { get; set; } = new List<Service>();
    //todo: rename to Locations
    public IList<Location> Location { get; set; } = new List<Location>();
}