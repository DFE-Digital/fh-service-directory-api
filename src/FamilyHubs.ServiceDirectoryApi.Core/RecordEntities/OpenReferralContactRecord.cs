namespace fh_service_directory_api.core.RecordEntities;

public record OpenReferralContactRecord
{
    private OpenReferralContactRecord() { }
    public OpenReferralContactRecord(string id, string title, string name,
        ICollection<OpenReferralPhoneRecord>? phones)
    {
        Id = id;
        Title = title;
        Name = name;
        Phones = phones;
    }
    public string Id { get; set; } = default!;
    public string Title { get; init; } = default!;
    public string Name { get; init; } = default!;
    public virtual ICollection<OpenReferralPhoneRecord>? Phones { get; init; }
}
