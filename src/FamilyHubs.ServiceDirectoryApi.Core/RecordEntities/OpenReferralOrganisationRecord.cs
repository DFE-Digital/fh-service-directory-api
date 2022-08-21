namespace fh_service_directory_api.core.RecordEntities;

public record OpenReferralOrganisationRecord
{
    protected OpenReferralOrganisationRecord()
    {

    }
    public OpenReferralOrganisationRecord(string id, string? name, string? description, string? logo, string? uri, string? url)
    {
        Id = id;
        Name = name;
        Description = description;
        Logo = logo;
        Uri = uri;
        Url = url;

    }

    public string Id { get; init; } = default!;
    public string? Name { get; init; }
    public string? Description { get; init; }
    public string? Logo { get; init; }
    public string? Uri { get; init; }
    public string? Url { get; init; }

}

public record OpenReferralOrganisationWithServicesRecord : OpenReferralOrganisationRecord
{
    private OpenReferralOrganisationWithServicesRecord()
    {

    }
    public OpenReferralOrganisationWithServicesRecord(string id, string? name, string? description, string? logo, string? uri, string? url, ICollection<OpenReferralServiceRecord>? services)
    {
        Id = id;
        Name = name;
        Description = description;
        Logo = logo;
        Uri = uri;
        Url = url;
        Services = services;
    }

    public virtual ICollection<OpenReferralServiceRecord>? Services { get; set; } = default!;

}

