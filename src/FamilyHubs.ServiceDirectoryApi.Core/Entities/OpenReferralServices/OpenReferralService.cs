using FamilyHubs.ServiceDirectoryApi.Core.Entities.OpenReferralContacts;
using FamilyHubs.ServiceDirectoryApi.Core.Entities.OpenReferralCostOptions;
using FamilyHubs.ServiceDirectoryApi.Core.Entities.OpenReferralEligibilitys;
using FamilyHubs.ServiceDirectoryApi.Core.Entities.OpenReferralFundings;
using FamilyHubs.ServiceDirectoryApi.Core.Entities.OpenReferralHolidaySchedules;
using FamilyHubs.ServiceDirectoryApi.Core.Entities.OpenReferralLanguages;
using FamilyHubs.ServiceDirectoryApi.Core.Entities.OpenReferralRegularSchedules;
using FamilyHubs.ServiceDirectoryApi.Core.Entities.OpenReferralReviews;
using FamilyHubs.ServiceDirectoryApi.Core.Entities.OpenReferralServiceAreas;
using FamilyHubs.ServiceDirectoryApi.Core.Entities.OpenReferralServiceAtLocations;
using FamilyHubs.ServiceDirectoryApi.Core.Entities.OpenReferralServiceDeliveries;
using FamilyHubs.ServiceDirectoryApi.Core.Entities.OpenReferralServiceTaxonomies;
using FamilyHubs.SharedKernel;
using FamilyHubs.SharedKernel.Interfaces;
using System.Collections.ObjectModel;

namespace FamilyHubs.ServiceDirectoryApi.Core.Entities.OpenReferralServices;

public class OpenReferralService : EntityBase<string>, IOpenReferralService, IAggregateRoot
{
    private OpenReferralService() { }

    public OpenReferralService(
        string id,
        string parentId,
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
        ICollection<OpenReferralServiceDelivery> serviceDelivery,
        ICollection<OpenReferralEligibility> eligibilitys,
        ICollection<OpenReferralFunding> fundings,
        ICollection<OpenReferralHolidaySchedule> holiday_schedules,
        ICollection<OpenReferralLanguage> languages,
        ICollection<OpenReferralRegularSchedule> regular_schedules,
        ICollection<OpenReferralReview> reviews,
        ICollection<OpenReferralContact> contacts,
        ICollection<OpenReferralCostOption> cost_options,
        ICollection<OpenReferralServiceArea> service_areas,
        ICollection<OpenReferralServiceAtLocation> service_at_locations,
        ICollection<OpenReferralServiceTaxonomy> service_taxonomys
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
        Eligibilitys = eligibilitys ?? new Collection<OpenReferralEligibility>();
        Fundings = fundings ?? new Collection<OpenReferralFunding>();
        Holiday_schedules = holiday_schedules ?? new Collection<OpenReferralHolidaySchedule>();
        Languages = languages ?? new Collection<OpenReferralLanguage>();
        Regular_schedules = regular_schedules ?? new Collection<OpenReferralRegularSchedule>();
        Reviews = reviews ?? new Collection<OpenReferralReview>();
        Contacts = contacts ?? new Collection<OpenReferralContact>();
        Cost_options = cost_options ?? new Collection<OpenReferralCostOption>();
        Service_areas = service_areas ?? new Collection<OpenReferralServiceArea>();
        Service_at_locations = service_at_locations ?? new Collection<OpenReferralServiceAtLocation>();
        Service_taxonomys = service_taxonomys ?? new Collection<OpenReferralServiceTaxonomy>();
        ServiceDelivery = serviceDelivery ?? new Collection<OpenReferralServiceDelivery>();
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
    public virtual ICollection<OpenReferralServiceDelivery> ServiceDelivery { get; init; } = new Collection<OpenReferralServiceDelivery>();
    public virtual ICollection<OpenReferralEligibility> Eligibilitys { get; init; } = new Collection<OpenReferralEligibility>();
    public virtual ICollection<OpenReferralFunding> Fundings { get; init; } = new Collection<OpenReferralFunding>();
    public virtual ICollection<OpenReferralHolidaySchedule> Holiday_schedules { get; init; } = new Collection<OpenReferralHolidaySchedule>();
    public virtual ICollection<OpenReferralLanguage> Languages { get; init; } = new Collection<OpenReferralLanguage>();
    public virtual ICollection<OpenReferralRegularSchedule> Regular_schedules { get; init; } = new Collection<OpenReferralRegularSchedule>();
    public virtual ICollection<OpenReferralReview> Reviews { get; init; } = new Collection<OpenReferralReview>();
    public virtual ICollection<OpenReferralContact> Contacts { get; init; } = new Collection<OpenReferralContact>();
    public virtual ICollection<OpenReferralCostOption> Cost_options { get; init; } = new Collection<OpenReferralCostOption>();
    public virtual ICollection<OpenReferralServiceArea> Service_areas { get; init; } = new Collection<OpenReferralServiceArea>();
    public virtual ICollection<OpenReferralServiceAtLocation> Service_at_locations { get; init; } = new Collection<OpenReferralServiceAtLocation>();
    public virtual ICollection<OpenReferralServiceTaxonomy> Service_taxonomys { get; init; } = new Collection<OpenReferralServiceTaxonomy>();

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
