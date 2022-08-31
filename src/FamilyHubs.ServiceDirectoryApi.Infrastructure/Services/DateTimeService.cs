using FamilyHubs.SharedKernel.Interfaces;

namespace fh_service_directory_api.infrastructure.Services;

public class DateTimeService : IDateTime
{
    public DateTime Now => DateTime.Now;
}

