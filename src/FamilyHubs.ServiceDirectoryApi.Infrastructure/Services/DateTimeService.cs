using FamilyHubs.SharedKernel.Interfaces;

namespace FamilyHubs.ServiceDirectory.Infrastructure.Services;

public class DateTimeService : IDateTime
{
    public DateTime Now => DateTime.UtcNow;
}

