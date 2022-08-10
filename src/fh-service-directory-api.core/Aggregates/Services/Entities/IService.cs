using fh_service_directory_api.core.Interfaces.Entities;
using fh_service_directory_api.core.Interfaces.Entities.Aggregates;

namespace fh_service_directory_api.core.Aggregates.Services.Entities
{
    public interface IService
    {
        string? Accreditations { get; init; }

        DateTime? Assured_date { get; init; }

        string? Attending_access { get; init; }

        string? Attending_type { get; init; }

        ICollection<IContact>? Contacts { get; init; }

        ICollection<ICostOption>? Cost_options { get; init; }

        string? Deliverable_type { get; init; }

        string? Description { get; init; }

        ICollection<IEligibility>? Eligibilitys { get; init; }

        string? Email { get; init; }

        string? Fees { get; init; }

        ICollection<IFunding>? Fundings { get; init; }

        ICollection<IHolidaySchedule>? Holiday_schedules { get; init; }

        ICollection<ILanguage>? Languages { get; init; }

        string Name { get; init; }

        ICollection<IRegularSchedule>? Regular_schedules { get; init; }

        ICollection<IReview>? Reviews { get; init; }

        ICollection<IServiceArea>? Service_areas { get; init; }

        ICollection<IServiceAtLocation>? Service_at_locations { get; init; }

        ICollection<IServiceTaxonomy>? Service_taxonomys { get; init; }

        string? Status { get; init; }

        string? Url { get; init; }
    }
}