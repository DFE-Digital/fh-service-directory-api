using System.Collections.ObjectModel;
using FamilyHubs.SharedKernel;
using FamilyHubs.SharedKernel.Interfaces;

namespace FamilyHubs.ServiceDirectory.Core.Entities;

public class Service : EntityBase<string>, IAggregateRoot
{
    public Service() { }

    public Service(
        string id, 
        ServiceType serviceType, 
        string organisationId, 
        string name, 
        string? description, 
        string? accreditations, 
        DateTime? assuredDate, 
        string? attendingAccess, 
        string? attendingType, 
        string? deliverableType, 
        string? status, 
        string? fees, 
        bool canFamilyChooseDeliveryLocation, 
        ICollection<ServiceDelivery> serviceDeliveries, 
        ICollection<Eligibility> eligibilities, 
        ICollection<Funding> fundings, 
        ICollection<HolidaySchedule> holidaySchedules, 
        ICollection<Language> languages, 
        ICollection<RegularSchedule> regularSchedules, 
        ICollection<Review> reviews, 
        ICollection<Contact> contacts, 
        ICollection<CostOption> costOptions, 
        ICollection<ServiceArea> serviceAreas, 
        ICollection<ServiceAtLocation> serviceAtLocations, 
        ICollection<ServiceTaxonomy> serviceTaxonomies
        )
    {
        Id = id;
        ServiceType = serviceType;
        OrganisationId = organisationId;   
        Name = name;
        Description = description;
        Accreditations = accreditations;
        AssuredDate = assuredDate;
        AttendingAccess = attendingAccess;
        AttendingType = attendingType;
        DeliverableType = deliverableType;
        Status = status;
        Fees = fees;
        CanFamilyChooseDeliveryLocation = canFamilyChooseDeliveryLocation;
        Eligibilities = eligibilities;
        Fundings = fundings;
        HolidaySchedules = holidaySchedules;
        Languages = languages;
        RegularSchedules = regularSchedules;
        Reviews = reviews;
        Contacts = contacts;
        CostOptions = costOptions;
        ServiceAreas = serviceAreas;
        ServiceAtLocations = serviceAtLocations;
        ServiceTaxonomies = serviceTaxonomies;
        ServiceDeliveries = serviceDeliveries;
    }

    public ServiceType ServiceType { get; set; } = default!;
    public string OrganisationId { get; set; } = default!;  
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public string? Accreditations { get; set; }
    public DateTime? AssuredDate { get; set; }
    public string? AttendingAccess { get; set; }
    public string? AttendingType { get; set; }
    public string? DeliverableType { get; set; }
    public string? Status { get; set; }
    public string? Fees { get; set; }
    public bool CanFamilyChooseDeliveryLocation { get; set; }
    public virtual ICollection<ServiceDelivery> ServiceDeliveries { get; set; } = new Collection<ServiceDelivery>();
    public virtual ICollection<Eligibility> Eligibilities { get; set; } = new Collection<Eligibility>();
    public virtual ICollection<Funding> Fundings { get; set; } = new Collection<Funding>();
    public virtual ICollection<HolidaySchedule> HolidaySchedules { get; set; } = new Collection<HolidaySchedule>();
    public virtual ICollection<Language> Languages { get; set; } = new Collection<Language>();
    public virtual ICollection<RegularSchedule> RegularSchedules { get; set; } = new Collection<RegularSchedule>();
    public virtual ICollection<Review> Reviews { get; set; } = new Collection<Review>();
    public virtual ICollection<Contact> Contacts { get; set; } = new Collection<Contact>();
    public virtual ICollection<CostOption> CostOptions { get; set; } = new Collection<CostOption>();
    public virtual ICollection<ServiceArea> ServiceAreas { get; set; } = new Collection<ServiceArea>();
    public virtual ICollection<ServiceAtLocation> ServiceAtLocations { get; set; } = new Collection<ServiceAtLocation>();
    public virtual ICollection<ServiceTaxonomy> ServiceTaxonomies { get; set; } = new Collection<ServiceTaxonomy>();

    public void Update(Service service)
    {
        Id = service.Id;
        ServiceType = service.ServiceType;
        Name = service.Name;
        Description = service.Description;
        Accreditations = service.Accreditations;
        AssuredDate = service.AssuredDate;
        AttendingAccess = service.AttendingAccess;
        AttendingType = service.AttendingType;
        DeliverableType = service.DeliverableType;
        Status = service.Status;
        Fees = service.Fees;
    }
}
