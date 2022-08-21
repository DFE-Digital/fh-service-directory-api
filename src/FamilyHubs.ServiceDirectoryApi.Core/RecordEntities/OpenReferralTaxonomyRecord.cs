namespace fh_service_directory_api.core.RecordEntities;

public record OpenReferralTaxonomyRecord
{
    private OpenReferralTaxonomyRecord() { }
    public OpenReferralTaxonomyRecord(string id, string name, string? vocabulary, string? parent)
    {
        Id = id;
        Name = name;
        Vocabulary = vocabulary;
        Parent = parent;
    }

    public string Id { get; init; } = default!;
    public string Name { get; init; } = default!;
    public string? Vocabulary { get; init; }
    public string? Parent { get; init; }
}
