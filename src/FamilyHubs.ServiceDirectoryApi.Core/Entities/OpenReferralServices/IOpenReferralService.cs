using FamilyHubs.ServiceDirectoryApi.Core.Entities.OpenReferralContacts;
using FamilyHubs.ServiceDirectoryApi.Core.Entities.OpenReferralEligibilitys;
using FamilyHubs.ServiceDirectoryApi.Core.Entities.OpenReferralFundings;
using FamilyHubs.ServiceDirectoryApi.Core.Entities.OpenReferralLanguages;
using FamilyHubs.ServiceDirectoryApi.Core.Entities.OpenReferralRegularSchedules;
using FamilyHubs.ServiceDirectoryApi.Core.Entities.OpenReferralReviews;
using FamilyHubs.ServiceDirectoryApi.Core.Entities.OpenReferralServiceAreas;
using FamilyHubs.ServiceDirectoryApi.Core.Entities.OpenReferralServiceAtLocations;
using FamilyHubs.ServiceDirectoryApi.Core.Entities.OpenReferralServiceDeliveries;

namespace FamilyHubs.ServiceDirectoryApi.Core.Entities.OpenReferralServices;

public interface IOpenReferralService : IEntityBase<string>
{
    string? Accreditations { get; }
    DateTime? Assured_date { get; }
    string? Attending_access { get; }
    string? Attending_type { get; }
    ICollection<OpenReferralContact> Contacts { get; init; }
    ICollection<OpenReferralCostOptions.OpenReferralCostOption> Cost_options { get; init; }
    string? Deliverable_type { get; }
    string? Description { get; }
    ICollection<OpenReferralEligibility> Eligibilitys { get; init; }
    string? Email { get; }
    string? Fees { get; }
    ICollection<OpenReferralFunding> Fundings { get; init; }
    ICollection<OpenReferralHolidaySchedules.OpenReferralHolidaySchedule> Holiday_schedules { get; init; }
    ICollection<OpenReferralLanguage> Languages { get; init; }
    string Name { get; }
    string OpenReferralOrganisationId { get; set; }
    ICollection<OpenReferralRegularSchedule> Regular_schedules { get; init; }
    ICollection<OpenReferralReview> Reviews { get; init; }
    ICollection<OpenReferralServiceArea> Service_areas { get; init; }
    ICollection<OpenReferralServiceAtLocation> Service_at_locations { get; init; }
    ICollection<OpenReferralServiceTaxonomies.OpenReferralServiceTaxonomy> Service_taxonomys { get; init; }
    ICollection<OpenReferralServiceDelivery> ServiceDelivery { get; init; }
    string? Status { get; }
    string? Url { get; }

    void Update(IOpenReferralService openReferralService);
}