namespace fh_service_directory_api.core.Interfaces.Entities
{
    public interface IServiceAtLocation
    {
        ICollection<IHolidaySchedule>? HolidayScheduleCollection { get; init; }
        ILocation Location { get; init; }
        ICollection<IRegularSchedule>? Regular_schedule { get; init; }
    }
}