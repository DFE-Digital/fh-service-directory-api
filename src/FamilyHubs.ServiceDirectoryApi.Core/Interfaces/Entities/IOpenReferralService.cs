using fh_service_directory_api.core.Entities;

namespace fh_service_directory_api.core.Interfaces.Entities
{
    public interface IOpenReferralService : IEntityBase<string>
    {
        string? Accreditations { get; }
        DateTime? Assured_date { get; }
        string? Attending_access { get; }
        string? Attending_type { get; }
        ICollection<OpenReferralContact> Contacts { get; set; }
        ICollection<OpenReferralCost_Option> Cost_options { get; set; }
        string? Deliverable_type { get; }
        string? Description { get; }
        ICollection<OpenReferralEligibility> Eligibilities { get; set; }
        string? Email { get; }
        string? Fees { get; }
        ICollection<OpenReferralFunding> Fundings { get; set; }
        ICollection<OpenReferralHoliday_Schedule> Holiday_schedules { get; set; }
        ICollection<OpenReferralLanguage> Languages { get; set; }
        string Name { get; }
        string OpenReferralOrganisationId { get; set; }
        ICollection<OpenReferralRegular_Schedule> Regular_schedules { get; set; }
        ICollection<OpenReferralReview> Reviews { get; set; }
        ICollection<OpenReferralService_Area> Service_areas { get; set; }
        ICollection<OpenReferralServiceAtLocation> Service_at_locations { get; set; }
        ICollection<OpenReferralService_Taxonomy> Service_taxonomys { get; set; }
        ICollection<OpenReferralServiceDelivery> ServiceDelivery { get; set; }
        string? Status { get; }
        string? Url { get; }
        ServiceType ServiceType { get; set; }

        void Update(OpenReferralService openReferralService);
    }
}