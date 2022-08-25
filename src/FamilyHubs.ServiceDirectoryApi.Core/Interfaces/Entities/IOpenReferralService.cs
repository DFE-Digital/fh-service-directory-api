namespace fh_service_directory_api.core.Interfaces.Entities
{
    public interface IOpenReferralService : IEntityBase<string>
    {
        string? Accreditations { get; }
        DateTime? Assured_date { get; }
        string? Attending_access { get; }
        string? Attending_type { get; }
        ICollection<IOpenReferralContact> Contacts { get; init; }
        ICollection<IOpenReferralCost_Option> Cost_options { get; init; }
        string? Deliverable_type { get; }
        string? Description { get; }
        ICollection<IOpenReferralEligibility> Eligibilitys { get; init; }
        string? Email { get; }
        string? Fees { get; }
        ICollection<IOpenReferralFunding> Fundings { get; init; }
        ICollection<IOpenReferralHoliday_Schedule> Holiday_schedules { get; init; }
        ICollection<IOpenReferralLanguage> Languages { get; init; }
        string Name { get; }
        string OpenReferralOrganisationId { get; set; }
        ICollection<IOpenReferralRegular_Schedule> Regular_schedules { get; init; }
        ICollection<IOpenReferralReview> Reviews { get; init; }
        ICollection<IOpenReferralService_Area> Service_areas { get; init; }
        ICollection<IOpenReferralServiceAtLocation> Service_at_locations { get; init; }
        ICollection<IOpenReferralService_Taxonomy> Service_taxonomys { get; init; }
        ICollection<IOpenReferralServiceDelivery> ServiceDelivery { get; init; }
        string? Status { get; }
        string? Url { get; }

        void Update(IOpenReferralService openReferralService);
    }
}