namespace fh_service_directory_api.core.Interfaces.Entities
{
    public interface IOpenReferralTaxonomy : IEntityBase<string>
    {
        ICollection<IOpenReferralLinktaxonomycollection>? LinkTaxonomyCollection { get; init; }
        string Name { get; init; }
        string? Parent { get; init; }
        string? Vocabulary { get; init; }
    }
}