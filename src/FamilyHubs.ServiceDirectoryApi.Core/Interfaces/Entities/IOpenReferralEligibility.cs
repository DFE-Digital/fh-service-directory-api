using fh_service_directory_api.core.Entities;

namespace fh_service_directory_api.core.Interfaces.Entities
{
    public interface IOpenReferralEligibility
    {
        string Eligibility { get; init; }
        string? LinkId { get; init; }
        int Maximum_age { get; init; }
        int Minimum_age { get; init; }
        ICollection<OpenReferralTaxonomy>? Taxonomys { get; init; }
    }
}