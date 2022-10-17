using fh_service_directory_api.core.Entities;

namespace fh_service_directory_api.core.Interfaces.Entities
{
    public interface IOpenReferralEligibility : IEntityBase<string>
    {
        string Eligibility { get; set; }
        string? LinkId { get; set; }
        int Maximum_age { get; set; }
        int Minimum_age { get; set; }
        ICollection<OpenReferralTaxonomy>? Taxonomys { get; set; }
    }
}