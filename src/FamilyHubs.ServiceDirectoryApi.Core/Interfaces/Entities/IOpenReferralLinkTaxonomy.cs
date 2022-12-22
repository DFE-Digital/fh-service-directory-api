namespace fh_service_directory_api.core.Interfaces.Entities
{
    public interface IOpenReferralLinkTaxonomy : IEntityBase<string>
    {
        string Link_id { get; init; }
        string Link_type { get; init; }
    }
}