namespace fh_service_directory_api.core.Interfaces.Entities
{
    public interface IOpenReferralParent : IEntityBase<string>
    {
        ICollection<IOpenReferralLinktaxonomycollection>? LinkTaxonomyCollection { get; init; }
        string Name { get; init; }
        ICollection<IOpenReferralService_Taxonomy>? ServiceTaxonomyCollection { get; init; }
        string? Vocabulary { get; init; }
    }
}