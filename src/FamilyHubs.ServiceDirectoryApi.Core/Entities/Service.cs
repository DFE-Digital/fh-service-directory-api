using FamilyHubs.ServiceDirectory.Shared.Enums;
using FamilyHubs.SharedKernel;
using FamilyHubs.SharedKernel.Interfaces;

namespace FamilyHubs.ServiceDirectory.Core.Entities;

public class Service : EntityBase<long>, IAggregateRoot
{
    public required string ServiceOwnerReferenceId { get; set; }
    public required ServiceType ServiceType { get; set; }
    public long OrganisationId { get; set; }  
    public required string Name { get; set; }
    public string? Description { get; set; }
    public ServiceStatusType Status { get; set; }
    public string? Fees { get; set; }
    public string? Accreditations { get; set; }
    public DeliverableType DeliverableType { get; set; }
    public DateTime? AssuredDate { get; set; }
    public AttendingType AttendingType { get; set; }
    public AttendingAccessType AttendingAccess { get; set; }
    public bool CanFamilyChooseDeliveryLocation { get; set; }
    public ICollection<ServiceDelivery> ServiceDeliveries { get; set; } = new List<ServiceDelivery>();
    public ICollection<Eligibility> Eligibilities { get; set; } = new List<Eligibility>();
    public ICollection<Funding> Fundings { get; set; } = new List<Funding>();
    public ICollection<CostOption> CostOptions { get; set; } = new List<CostOption>();
    public ICollection<Language> Languages { get; set; } = new List<Language>();
    public ICollection<ServiceArea> ServiceAreas { get; set; } = new List<ServiceArea>();
    public ICollection<Location> Locations { get; set; } = new List<Location>();
    public ICollection<Taxonomy> Taxonomies { get; set; } = new List<Taxonomy>();
    public ICollection<RegularSchedule> RegularSchedules { get; set; } = new List<RegularSchedule>();
    public ICollection<HolidaySchedule> HolidaySchedules { get; set; } = new List<HolidaySchedule>();
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
    public ICollection<Contact> Contacts { get; set; } = new List<Contact>();
}
