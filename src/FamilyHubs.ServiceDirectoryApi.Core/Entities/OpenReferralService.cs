using FamilyHubs.SharedKernel;
using FamilyHubs.SharedKernel.Interfaces;
using fh_service_directory_api.core.Interfaces.Entities;
using System.Collections.ObjectModel;

namespace fh_service_directory_api.core.Entities;

public class OpenReferralService : EntityBase<string>, IOpenReferralService, IAggregateRoot
{
    public OpenReferralService() { }

    public OpenReferralService(
        string id,
        ServiceType serviceType,
        string openReferralOrganisationId,
        string name,
        string? description,
        string? accreditations,
        DateTime? assured_date,
        string? attending_access,
        string? attending_type,
        string? deliverable_type,
        string? status,
        string? url,
        string? email,
        string? fees,
        bool canFamilyChooseDeliveryLocation
        , ICollection<OpenReferralServiceDelivery> serviceDelivery
        , ICollection<OpenReferralEligibility> eligibilitys
        , ICollection<OpenReferralFunding> fundings
        , ICollection<OpenReferralHoliday_Schedule> holiday_schedules
        , ICollection<OpenReferralLanguage> languages
        , ICollection<OpenReferralRegular_Schedule> regular_schedules
        , ICollection<OpenReferralReview> reviews
//        , ICollection<OpenReferralContact> contacts
        , ICollection<OpenReferralCost_Option> cost_options
        , ICollection<OpenReferralService_Area> service_areas
        , ICollection<OpenReferralServiceAtLocation> service_at_locations
        , ICollection<OpenReferralService_Taxonomy> service_taxonomys
        , ICollection<OpenReferralLinkContact> link_Contact
    )
    {
        Id = id;
        ServiceType = serviceType;
        OpenReferralOrganisationId = openReferralOrganisationId;   
        Name = name;
        Description = description;
        Accreditations = accreditations;
        Assured_date = assured_date;
        Attending_access = attending_access;
        Attending_type = attending_type;
        Deliverable_type = deliverable_type;
        Status = status;
        Url = url;
        Email = email;
        Fees = fees;
        CanFamilyChooseDeliveryLocation = canFamilyChooseDeliveryLocation;
        Eligibilities = eligibilitys ?? new Collection<OpenReferralEligibility>();
        Fundings = fundings ?? new Collection<OpenReferralFunding>();
        Holiday_schedules = holiday_schedules ?? new Collection<OpenReferralHoliday_Schedule>();
        Languages = languages ?? new Collection<OpenReferralLanguage>();
        Regular_schedules = regular_schedules ?? new Collection<OpenReferralRegular_Schedule>();
        Reviews = reviews ?? new Collection<OpenReferralReview>();
//        Contacts = contacts ?? new Collection<OpenReferralContact>();
        Cost_options = cost_options ?? new Collection<OpenReferralCost_Option>();
        Service_areas = service_areas ?? new Collection<OpenReferralService_Area>();
        Service_at_locations = service_at_locations ?? new Collection<OpenReferralServiceAtLocation>();
        Service_taxonomys = service_taxonomys ?? new Collection<OpenReferralService_Taxonomy>();
        ServiceDelivery = serviceDelivery ?? new Collection<OpenReferralServiceDelivery>();
        Link_Contacts = link_Contact ?? new Collection<OpenReferralLinkContact>();
    }

    public ServiceType ServiceType { get; set; } = default!;
    public string OpenReferralOrganisationId { get; set; } = default!;  
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public string? Accreditations { get; set; }
    public DateTime? Assured_date { get; set; }
    public string? Attending_access { get; set; }
    public string? Attending_type { get; set; }
    public string? Deliverable_type { get; set; }
    public string? Status { get; set; }
    public string? Url { get; set; }
    public string? Email { get; set; }
    public string? Fees { get; set; }
    public bool CanFamilyChooseDeliveryLocation { get; set; }
    public virtual ICollection<OpenReferralServiceDelivery> ServiceDelivery { get; set; } = new Collection<OpenReferralServiceDelivery>();
    public virtual ICollection<OpenReferralEligibility> Eligibilities { get; set; } = new Collection<OpenReferralEligibility>();
    public virtual ICollection<OpenReferralFunding> Fundings { get; set; } = new Collection<OpenReferralFunding>();
    public virtual ICollection<OpenReferralHoliday_Schedule> Holiday_schedules { get; set; } = new Collection<OpenReferralHoliday_Schedule>();
    public virtual ICollection<OpenReferralLanguage> Languages { get; set; } = new Collection<OpenReferralLanguage>();
    public virtual ICollection<OpenReferralRegular_Schedule> Regular_schedules { get; set; } = new Collection<OpenReferralRegular_Schedule>();
    public virtual ICollection<OpenReferralReview> Reviews { get; set; } = new Collection<OpenReferralReview>();
    public virtual ICollection<OpenReferralContact> Contacts { get; set; } = new Collection<OpenReferralContact>();
    public virtual ICollection<OpenReferralCost_Option> Cost_options { get; set; } = new Collection<OpenReferralCost_Option>();
    public virtual ICollection<OpenReferralService_Area> Service_areas { get; set; } = new Collection<OpenReferralService_Area>();
    public virtual ICollection<OpenReferralServiceAtLocation> Service_at_locations { get; set; } = new Collection<OpenReferralServiceAtLocation>();
    public virtual ICollection<OpenReferralService_Taxonomy> Service_taxonomys { get; set; } = new Collection<OpenReferralService_Taxonomy>();
    public virtual ICollection<OpenReferralLinkContact> Link_Contacts { get; set; } = new Collection<OpenReferralLinkContact>();

    public void Update(OpenReferralService openReferralService)
    {
        Id = openReferralService.Id;
        ServiceType = openReferralService.ServiceType;
        Name = openReferralService.Name;
        Description = openReferralService.Description;
        Accreditations = openReferralService.Accreditations;
        Assured_date = openReferralService.Assured_date;
        Attending_access = openReferralService.Attending_access;
        Attending_type = openReferralService.Attending_type;
        Deliverable_type = openReferralService.Deliverable_type;
        Status = openReferralService.Status;
        Url = openReferralService.Url;
        Email = openReferralService.Email;
        Fees = openReferralService.Fees;
    }
}
