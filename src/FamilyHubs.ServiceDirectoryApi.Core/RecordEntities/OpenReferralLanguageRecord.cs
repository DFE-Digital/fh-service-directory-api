namespace fh_service_directory_api.core.RecordEntities;

public record OpenReferralLanguageRecord
{
    private OpenReferralLanguageRecord() { }
    public OpenReferralLanguageRecord(string id, string language)
    {
        Id = id;
        Language = language;
    }
    public string Id { get; set; } = default!;
    public string Language { get; init; } = default!;
}
