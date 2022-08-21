namespace fh_service_directory_api.core.RecordEntities;

public record OpenReferralServiceRecord
{
    private OpenReferralServiceRecord()
    {

    }
    public OpenReferralServiceRecord(string id, string name, string? description, string? accreditations, DateTime? assured_date, string? attending_access, string? attending_type, string? deliverable_type, string? status, string? url, string? email, string? fees
        , ICollection<OpenReferralServiceDeliveryRecord>? serviceDelivery
        , ICollection<OpenReferralEligibilityRecord>? eligibilities
        , ICollection<OpenReferralContactRecord>? contacts
        , ICollection<OpenReferralCost_OptionRecord> cost_options
        , ICollection<OpenReferralLanguageRecord>? languages
        , ICollection<OpenReferralService_AreaRecord>? service_areas
        , ICollection<OpenReferralServiceAtLocationRecord>? service_at_locations
        , ICollection<OpenReferralService_TaxonomyRecord>? service_taxonomys
        )
    {
        Id = id;
        Name = name;
        Description = description;
        Accreditations = accreditations;
        Assured_date = assured_date;
        Attending_access = attending_access;
        Attending_type = attending_type;
        Deliverable_type = deliverable_type;
        Status = status;
        Url = url;
        Email = email;
        Fees = fees;
        ServiceDelivery = serviceDelivery;
        Eligibilities = eligibilities;
        Contacts = contacts;
        Cost_options = cost_options;
        Languages = languages;
        Service_areas = service_areas;
        Service_at_locations = service_at_locations;
        Service_taxonomys = service_taxonomys;
    }

    public string Id { get; init; } = default!;
    public string Name { get; init; } = default!;
    public string? Description { get; init; }
    public string? Accreditations { get; init; }
    public DateTime? Assured_date { get; init; }
    public string? Attending_access { get; init; }
    public string? Attending_type { get; init; }
    public string? Deliverable_type { get; init; }
    public string? Status { get; init; }
    public string? Url { get; init; }
    public string? Email { get; init; }
    public string? Fees { get; init; }
    public ICollection<OpenReferralServiceDeliveryRecord>? ServiceDelivery { get; init; }
    public ICollection<OpenReferralEligibilityRecord>? Eligibilities { get; init; }
    public ICollection<OpenReferralContactRecord>? Contacts { get; init; }
    public ICollection<OpenReferralCost_OptionRecord>? Cost_options { get; init; }
    public ICollection<OpenReferralLanguageRecord>? Languages { get; init; }
    public ICollection<OpenReferralService_AreaRecord>? Service_areas { get; init; }
    public ICollection<OpenReferralServiceAtLocationRecord>? Service_at_locations { get; init; }
    public ICollection<OpenReferralService_TaxonomyRecord>? Service_taxonomys { get; init; }

}
