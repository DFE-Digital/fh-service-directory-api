using FamilyHubs.ServiceDirectory.Data.Entities.Base;
using FamilyHubs.ServiceDirectory.Data.Entities.ManyToMany;
using Enums = FamilyHubs.ServiceDirectory.Shared.Enums;

namespace FamilyHubs.ServiceDirectory.Data.Entities;

public class Service : EntityBase<long>
{
    public long OrganisationId { get; set; }
    public required Enums.ServiceType ServiceType { get; set; }
    public required string Name { get; set; }
    public string? Summary { get; set; }
    public string? Description { get; set; }
    public Enums.ServiceStatusType Status { get; set; }
    public string? InterpretationServices { get; set; }
    public string? Fees { get; set; }
    public string? Accreditations { get; set; }
    public Enums.DeliverableType DeliverableType { get; set; }
    public DateTime? AssuredDate { get; set; }
    public bool CanFamilyChooseDeliveryLocation { get; set; }
    public IList<Location> Locations { get; set; } = new List<Location>();
    public IList<ServiceAtLocation> ServiceAtLocations { get; set; } = new List<ServiceAtLocation>();
    public IList<ServiceDelivery> ServiceDeliveries { get; set; } = new List<ServiceDelivery>();
    public IList<Eligibility> Eligibilities { get; set; } = new List<Eligibility>();
    public IList<Funding> Fundings { get; set; } = new List<Funding>();
    public IList<CostOption> CostOptions { get; set; } = new List<CostOption>();
    public IList<Language> Languages { get; set; } = new List<Language>();
    public IList<ServiceArea> ServiceAreas { get; set; } = new List<ServiceArea>();
    public IList<Taxonomy> Taxonomies { get; set; } = new List<Taxonomy>();
    public IList<Schedule> Schedules { get; set; } = new List<Schedule>();
    public IList<Contact> Contacts { get; set; } = new List<Contact>();
    public IList<ServiceSearchResult> ServiceSearchResults { get; } = new List<ServiceSearchResult>();
}
