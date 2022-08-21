namespace fh_service_directory_api.core.RecordEntities;

public record OpenReferralServiceAtLocationRecord
{
    private OpenReferralServiceAtLocationRecord() { }
    public OpenReferralServiceAtLocationRecord(string id,
        OpenReferralLocationRecord location
        //ICollection<OpenReferralHoliday_Schedule>? holidayScheduleCollection, ICollection<OpenReferralRegular_Schedule>? regular_schedule
        )
    {
        Id = id;
        Location = location;
        //HolidayScheduleCollection = holidayScheduleCollection;
        //Regular_schedule = regular_schedule;
    }

    public string Id { get; set; } = default!;
    public OpenReferralLocationRecord Location { get; init; } = default!;
    //public ICollection<OpenReferralHoliday_Schedule>? HolidayScheduleCollection { get; init; }
    //public ICollection<OpenReferralRegular_Schedule>? Regular_schedule { get; init; }
}

