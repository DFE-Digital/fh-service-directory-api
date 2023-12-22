using FamilyHubs.ServiceDirectory.Data.Entities.Base;
using FamilyHubs.ServiceDirectory.Shared.Enums;

namespace FamilyHubs.ServiceDirectory.Data.Entities;

public class Service : OrganisationEntityBase<long>
{
    public required string ServiceOwnerReferenceId { get; set; }
    public required ServiceType ServiceType { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public ServiceStatusType Status { get; set; }
    public string? InterpretationServices { get; set; }
    public string? Fees { get; set; }
    public string? Accreditations { get; set; }
    public DeliverableType DeliverableType { get; set; }
    public DateTime? AssuredDate { get; set; }
    public AttendingType AttendingType { get; set; }
    public AttendingAccessType AttendingAccess { get; set; }
    public bool CanFamilyChooseDeliveryLocation { get; set; }
    public IList<ServiceDelivery> ServiceDeliveries { get; set; } = new List<ServiceDelivery>();
    public IList<Eligibility> Eligibilities { get; set; } = new List<Eligibility>();
    public IList<Funding> Fundings { get; set; } = new List<Funding>();
    public IList<CostOption> CostOptions { get; set; } = new List<CostOption>();
    public IList<Language> Languages { get; set; } = new List<Language>();
    public IList<ServiceArea> ServiceAreas { get; set; } = new List<ServiceArea>();
    public IList<Location> Locations { get; set; } = new List<Location>();
    public IList<Taxonomy> Taxonomies { get; set; } = new List<Taxonomy>();
    public IList<RegularSchedule> RegularSchedules { get; set; } = new List<RegularSchedule>();
    public IList<HolidaySchedule> HolidaySchedules { get; set; } = new List<HolidaySchedule>();
    public IList<Review> Reviews { get; set; } = new List<Review>();
    public IList<Contact> Contacts { get; set; } = new List<Contact>();
}
