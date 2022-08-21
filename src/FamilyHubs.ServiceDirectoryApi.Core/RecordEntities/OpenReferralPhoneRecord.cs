namespace fh_service_directory_api.core.RecordEntities;

public record OpenReferralPhoneRecord
{
    private OpenReferralPhoneRecord() { }
    public OpenReferralPhoneRecord(string id, string number)
    {
        Id = id;
        Number = number;
    }

    public string Id { get; init; } = default!;
    public string Number { get; init; } = default!;
}
