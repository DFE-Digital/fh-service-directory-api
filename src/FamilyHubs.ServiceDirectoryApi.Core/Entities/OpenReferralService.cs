using FamilyHubs.SharedKernel;
using FamilyHubs.SharedKernel.Interfaces;
using fh_service_directory_api.core.Interfaces.Entities;
using System.Collections.ObjectModel;

namespace FamilyHubs.ServiceDirectory.Shared.Entities;

public class OpenReferralService : EntityBase<string>, IOpenReferralService, IAggregateRoot
{
    private OpenReferralService() { }

    public OpenReferralService(string id, string parentId, string name, string? description, string? accreditations, DateTime? assured_date, string? attending_access, string? attending_type, string? deliverable_type, string? status, string? url, string? email, string? fees
        , ICollection<IOpenReferralServiceDelivery> serviceDelivery
        , ICollection<IOpenReferralEligibility> eligibilitys
        , ICollection<IOpenReferralFunding> fundings
        , ICollection<IOpenReferralHoliday_Schedule> holiday_schedules
        , ICollection<IOpenReferralLanguage> languages
        , ICollection<IOpenReferralRegular_Schedule> regular_schedules
        , ICollection<IOpenReferralReview> reviews
        , ICollection<IOpenReferralContact> contacts
        , ICollection<IOpenReferralCost_Option> cost_options
        , ICollection<IOpenReferralService_Area> service_areas
        , ICollection<IOpenReferralServiceAtLocation> service_at_locations
        , ICollection<IOpenReferralService_Taxonomy> service_taxonomys
        )
    {
        Id = id;
        OpenReferralOrganisationId = parentId;
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
        Eligibilitys = eligibilitys;
        Fundings = fundings;
        Holiday_schedules = holiday_schedules;
        Languages = languages;
        Regular_schedules = regular_schedules;
        Reviews = reviews;
        Contacts = contacts;
        Cost_options = cost_options;
        Service_areas = service_areas;
        Service_at_locations = service_at_locations;
        Service_taxonomys = service_taxonomys;
        ServiceDelivery = serviceDelivery;
    }

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
    public virtual ICollection<IOpenReferralServiceDelivery> ServiceDelivery { get; init; } = new Collection<IOpenReferralServiceDelivery>();
    public virtual ICollection<IOpenReferralEligibility> Eligibilitys { get; init; } = new Collection<IOpenReferralEligibility>();
    public virtual ICollection<IOpenReferralFunding> Fundings { get; init; } = new Collection<IOpenReferralFunding>();
    public virtual ICollection<IOpenReferralHoliday_Schedule> Holiday_schedules { get; init; } = new Collection<IOpenReferralHoliday_Schedule>();
    public virtual ICollection<IOpenReferralLanguage> Languages { get; init; } = new Collection<IOpenReferralLanguage>();
    public virtual ICollection<IOpenReferralRegular_Schedule> Regular_schedules { get; init; } = new Collection<IOpenReferralRegular_Schedule>();
    public virtual ICollection<IOpenReferralReview> Reviews { get; init; } = new Collection<IOpenReferralReview>();
    public virtual ICollection<IOpenReferralContact> Contacts { get; init; } = new Collection<IOpenReferralContact>();
    public virtual ICollection<IOpenReferralCost_Option> Cost_options { get; init; } = new Collection<IOpenReferralCost_Option>();
    public virtual ICollection<IOpenReferralService_Area> Service_areas { get; init; } = new Collection<IOpenReferralService_Area>();
    public virtual ICollection<IOpenReferralServiceAtLocation> Service_at_locations { get; init; } = new Collection<IOpenReferralServiceAtLocation>();
    public virtual ICollection<IOpenReferralService_Taxonomy> Service_taxonomys { get; init; } = new Collection<IOpenReferralService_Taxonomy>();
    
    public void Update(IOpenReferralService openReferralService)
    {
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
