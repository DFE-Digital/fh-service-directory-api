using FamilyHubs.SharedKernel.Interfaces;

namespace FamilyHubs.ServiceDirectoryApi.Infrastructure.System;

public class DateTimeService : IDateTime
{
    public DateTime Now => DateTime.Now;
}
