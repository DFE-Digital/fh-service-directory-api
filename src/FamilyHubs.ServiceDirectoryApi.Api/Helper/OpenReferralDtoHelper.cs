using FamilyHubs.ServiceDirectory.Core.Entities;
using FamilyHubs.ServiceDirectory.Shared.Dto;

namespace FamilyHubs.ServiceDirectory.Api.Helper;

public class DtoHelper
{
    public static List<ServiceDto> GetServicesDto(IEnumerable<Service> dbservices)
    {
        var dtoServices = dbservices.Select(x => new ServiceDto(
            x.Id,
            new ServiceTypeDto(x.ServiceType.Id, x.ServiceType.Name, x.ServiceType.Description),
            x.OrganisationId,
            x.Name,
            x.Description,
            x.Accreditations,
            x.AssuredDate,
            x.AttendingAccess,
            x.AttendingType,
            x.DeliverableType,
            x.Status,
            x.Fees,
            x.CanFamilyChooseDeliveryLocation,
            x.ServiceDeliveries.Select(x => new ServiceDeliveryDto(x.Id, x.Name)).ToList(),
            x.Eligibilities.Select(x => new EligibilityDto(x.Id, x.EligibilityDescription, x.MaximumAge, x.MinimumAge)).ToList(),
            GetContacts(x.Contacts),
            x.CostOptions.Select(x => new CostOptionDto(x.Id, x.AmountDescription, x.Amount, x.LinkId, x.Option, x.ValidFrom, x.ValidTo)).ToList(),
            x.Languages.Select(x => new LanguageDto(x.Id, x.Name)).ToList(),
            x.ServiceAreas.Select(x => new ServiceAreaDto(x.Id, x.ServiceAreaDescription, x.Extent, x.Uri)).ToList(),
            x.ServiceAtLocations.Select(x => new ServiceAtLocationDto(x.Id, new LocationDto(x.Location.Id, x.Location.Name, x.Location.Description, x.Location.Latitude, x.Location.Longitude, GetAddresses(x.Location), GetLinkTaxonomies(x.Location)), GetRegularSchedules(x.RegularSchedules), GetHolidaySchedules(x.HolidaySchedules))).ToList(),
            x.ServiceTaxonomies.Select(x => new ServiceTaxonomyDto(x.Id, x.Taxonomy != null ? new TaxonomyDto(x.Taxonomy.Id, x.Taxonomy.Name, x.Taxonomy.Vocabulary, x.Taxonomy.Parent) : null)).ToList(),
            x.RegularSchedules.Select(x => new RegularScheduleDto(x.Id, x.Description, x.OpensAt, x.ClosesAt, x.ByDay, x.ByMonthDay, x.DtStart, x.Freq, x.Interval, x.ValidFrom, x.ValidTo)).ToList(),
            x.HolidaySchedules.Select(x => new HolidayScheduleDto(x.Id, x.Closed, x.ClosesAt, x.StartDate, x.EndDate, x.OpensAt)).ToList()
            )).ToList();

        return dtoServices;

    }

    public static ICollection<RegularScheduleDto> GetRegularSchedules(ICollection<RegularSchedule>? schedules)
    {
        return schedules != null ? schedules.Select(x => new RegularScheduleDto(x.Id, x.Description, x.OpensAt, x.ClosesAt, x.ByDay, x.ByMonthDay, x.DtStart, x.Freq, x.Interval, x.ValidFrom, x.ValidTo)).ToList() : new List<RegularScheduleDto>();
    }

    public static ICollection<HolidayScheduleDto> GetHolidaySchedules(ICollection<HolidaySchedule>? schedules)
    {
        if (schedules != null)
        {
            return schedules.Select(x => new HolidayScheduleDto(x.Id, x.Closed, x.ClosesAt, x.StartDate, x.EndDate, x.OpensAt)).ToList();
        }
        List<HolidayScheduleDto> holidaySchedules = new();

        return holidaySchedules;
    }

    public static ServiceDto GetServiceDto(Service service)
    {
        var dtoService = new ServiceDto(
            service.Id,
            new ServiceTypeDto(service.ServiceType.Id, service.ServiceType.Name, service.ServiceType.Description),
            service.OrganisationId,
            service.Name,
            service.Description,
            service.Accreditations,
            service.AssuredDate,
            service.AttendingAccess,
            service.AttendingType,
            service.DeliverableType,
            service.Status,
            service.Fees,
            service.CanFamilyChooseDeliveryLocation,
            service.ServiceDeliveries.Select(x => new ServiceDeliveryDto(x.Id, x.Name)).ToList(),
            service.Eligibilities.Select(x => new EligibilityDto(x.Id, x.EligibilityDescription, x.MaximumAge, x.MinimumAge)).ToList(),
            GetContacts(service.Contacts),
            service.CostOptions.Select(x => new CostOptionDto(x.Id, x.AmountDescription, x.Amount, x.LinkId, x.Option, x.ValidFrom, x.ValidTo)).ToList(),
            service.Languages.Select(x => new LanguageDto(x.Id, x.Name)).ToList(),
            service.ServiceAreas.Select(x => new ServiceAreaDto(x.Id, x.ServiceAreaDescription, x.Extent, x.Uri)).ToList(),
            service.ServiceAtLocations.Select(x => new ServiceAtLocationDto(x.Id, new LocationDto(x.Location.Id, x.Location.Name, x.Location.Description, x.Location.Latitude, x.Location.Longitude, GetAddresses(x.Location), GetLinkTaxonomies(x.Location)), GetRegularSchedules(x.RegularSchedules), GetHolidaySchedules(x.HolidaySchedules))).ToList(),
            service.ServiceTaxonomies.Select(x => new ServiceTaxonomyDto(x.Id, x.Taxonomy != null ? new TaxonomyDto(x.Taxonomy.Id, x.Taxonomy.Name, x.Taxonomy.Vocabulary, x.Taxonomy.Parent) : null)).ToList(),
            service.RegularSchedules.Select(x => new RegularScheduleDto(x.Id, x.Description, x.OpensAt, x.ClosesAt, x.ByDay, x.ByMonthDay, x.DtStart, x.Freq, x.Interval, x.ValidFrom, x.ValidTo)).ToList(),
            service.HolidaySchedules.Select(x => new HolidayScheduleDto(x.Id, x.Closed, x.ClosesAt, x.StartDate, x.EndDate, x.OpensAt)).ToList()
            );

        return dtoService;

    }

    private static List<ContactDto> GetContacts(ICollection<Contact> contacts)
    {
        var items = contacts.Select(x => new ContactDto(x.Id, x.Title, x.Name, x.Telephone, x.TextPhone, x.Url, x.Email)).ToList();

        return items;
    }

    private static List<PhysicalAddressDto> GetAddresses(Location location)
    {
        var addressList = location.PhysicalAddresses?.Select(x => new PhysicalAddressDto(x.Id, x.Address1, x.City, x.PostCode, x.Country, x.StateProvince)).ToList();
        
        return addressList ?? new List<PhysicalAddressDto>();
    }
    private static List<LinkTaxonomyDto> GetLinkTaxonomies(Location location)
    {
        var linkTaxonomies = location.LinkTaxonomies?.Select(x => new LinkTaxonomyDto(x.Id, x.LinkType, x.LinkId, new TaxonomyDto(x.Taxonomy!.Id, x.Taxonomy.Name, x.Taxonomy.Vocabulary, x.Taxonomy.Parent))).ToList();
        
        return linkTaxonomies ?? new List<LinkTaxonomyDto>();
    }
}
