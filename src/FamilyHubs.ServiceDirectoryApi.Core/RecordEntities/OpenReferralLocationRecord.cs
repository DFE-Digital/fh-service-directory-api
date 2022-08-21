namespace fh_service_directory_api.core.RecordEntities;

public record OpenReferralLocationRecord
{
    private OpenReferralLocationRecord() { }
    public OpenReferralLocationRecord(string id, string name, string? description, double latitude, double longitude
        , ICollection<OpenReferralPhysical_AddressRecord>? physical_addresses
        // ICollection<Accessibility_For_Disabilities>? accessibility_for_disabilities
        //, ICollection<OpenReferralServiceAtLocation>? service_at_locations
        )
    {
        Id = id;
        Name = name;
        Description = description;
        Latitude = latitude;
        Longitude = longitude;
        Physical_addresses = physical_addresses;
        //May be added latter
        //Accessibility_for_disabilities = accessibility_for_disabilities;
        //Service_at_locations = service_at_locations;
    }
    public string Id { get; init; } = default!;
    public string Name { get; init; } = default!;
    public string? Description { get; init; }
    public double Latitude { get; init; }
    public double Longitude { get; init; }
    public ICollection<OpenReferralPhysical_AddressRecord>? Physical_addresses { get; init; }
    //public ICollection<Accessibility_For_Disabilities>? Accessibility_for_disabilities { get; init; }
    //public ICollection<OpenReferralServiceAtLocation>? Service_at_locations { get; init; }
}

