using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralTaxonomys;
using fh_service_directory_api.core.Entities;

namespace fh_service_directory_api.core.Interfaces.Entities
{
    public interface IOpenReferralLinkTaxonomy : IEntityBase<string>
    {
        OpenReferralTaxonomy? Taxonomy { get; set; }
        string LinkId { get; set; }
        string LinkType { get; set; }
    }
}