using FamilyHubs.ServiceDirectory.Core.Entities;
using FamilyHubs.ServiceDirectory.Shared.Dto;

namespace FamilyHubs.ServiceDirectory.Api.Helper;

public class DtoHelper
{
    public static List<ServiceDto> GetServicesDto(IEnumerable<Service> dbservices)
    {
        var dtoServices = dbservices.Select(service =>
            new ServiceDto(
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
            service.ServiceDeliveries.Select(serviceDelivery => new ServiceDeliveryDto(serviceDelivery.Id, serviceDelivery.Name)).ToList(),
            service.Eligibilities.Select(eligibility => new EligibilityDto(eligibility.Id, eligibility.EligibilityDescription, eligibility.MaximumAge, eligibility.MinimumAge)).ToList(),
            service.CostOptions.Select(costOption => new CostOptionDto(costOption.Id, costOption.AmountDescription, costOption.Amount, costOption.LinkId, costOption.Option, costOption.ValidFrom, costOption.ValidTo)).ToList(),
            service.Languages.Select(language => new LanguageDto(language.Id, language.Name)).ToList(),
            service.ServiceAreas.Select(serviceArea => new ServiceAreaDto(serviceArea.Id, serviceArea.ServiceAreaDescription, serviceArea.Extent, serviceArea.Uri)).ToList(),
            service.ServiceAtLocations.Select(serviceAtLocation => new ServiceAtLocationDto(serviceAtLocation.Id, new LocationDto(serviceAtLocation.Location.Id, serviceAtLocation.Location.Name, serviceAtLocation.Location.Description, serviceAtLocation.Location.Latitude, serviceAtLocation.Location.Longitude, GetAddresses(serviceAtLocation.Location), GetLinkTaxonomies(serviceAtLocation.Location), GetContacts(serviceAtLocation.LinkContacts ?? new List<LinkContact>())), GetRegularSchedules(serviceAtLocation.RegularSchedules), GetHolidaySchedules(serviceAtLocation.HolidaySchedules), GetContacts(serviceAtLocation.LinkContacts ?? new List<LinkContact>()))).ToList(),
            service.ServiceTaxonomies.Select(serviceTaxonomy => new ServiceTaxonomyDto(serviceTaxonomy.Id, serviceTaxonomy.Taxonomy != null ? new TaxonomyDto(serviceTaxonomy.Taxonomy.Id, serviceTaxonomy.Taxonomy.Name, serviceTaxonomy.Taxonomy.Vocabulary, serviceTaxonomy.Taxonomy.Parent) : null)).ToList(),
            service.RegularSchedules.Select(regularSchedule => new RegularScheduleDto(regularSchedule.Id, regularSchedule.Description, regularSchedule.OpensAt, regularSchedule.ClosesAt, regularSchedule.ByDay, regularSchedule.ByMonthDay, regularSchedule.DtStart, regularSchedule.Freq, regularSchedule.Interval, regularSchedule.ValidFrom, regularSchedule.ValidTo)).ToList(),
            service.HolidaySchedules.Select(holidaySchedule => new HolidayScheduleDto(holidaySchedule.Id, holidaySchedule.Closed, holidaySchedule.ClosesAt, holidaySchedule.StartDate, holidaySchedule.EndDate, holidaySchedule.OpensAt)).ToList(),
            GetContacts(service.LinkContacts)
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
            service.CostOptions.Select(x => new CostOptionDto(x.Id, x.AmountDescription, x.Amount, x.LinkId, x.Option, x.ValidFrom, x.ValidTo)).ToList(),
            service.Languages.Select(x => new LanguageDto(x.Id, x.Name)).ToList(),
            service.ServiceAreas.Select(x => new ServiceAreaDto(x.Id, x.ServiceAreaDescription, x.Extent, x.Uri)).ToList(),
            service.ServiceAtLocations.Select(x => new ServiceAtLocationDto(x.Id, new LocationDto(x.Location.Id, x.Location.Name, x.Location.Description, x.Location.Latitude, x.Location.Longitude, GetAddresses(x.Location), GetLinkTaxonomies(x.Location), GetContacts(x.Location.LinkContacts ?? new List<LinkContact>())), GetRegularSchedules(x.RegularSchedules), GetHolidaySchedules(x.HolidaySchedules), GetContacts(x.LinkContacts ?? new List<LinkContact>()))).ToList(),
            service.ServiceTaxonomies.Select(x => new ServiceTaxonomyDto(x.Id, x.Taxonomy != null ? new TaxonomyDto(x.Taxonomy.Id, x.Taxonomy.Name, x.Taxonomy.Vocabulary, x.Taxonomy.Parent) : null)).ToList(),
            service.RegularSchedules.Select(x => new RegularScheduleDto(x.Id, x.Description, x.OpensAt, x.ClosesAt, x.ByDay, x.ByMonthDay, x.DtStart, x.Freq, x.Interval, x.ValidFrom, x.ValidTo)).ToList(),
            service.HolidaySchedules.Select(x => new HolidayScheduleDto(x.Id, x.Closed, x.ClosesAt, x.StartDate, x.EndDate, x.OpensAt)).ToList(),
            GetContacts(service.LinkContacts)
            );

        return dtoService;

    }

    private static List<LinkContactDto> GetContacts(ICollection<LinkContact> contacts)
    {
        var items = contacts.Select(x => new LinkContactDto(x.Id, x.LinkType, x.LinkId, 
            x.Contact == null ? new ContactDto() : new ContactDto(x.Contact.Id, x.Contact.Title, x.Contact.Name, x.Contact.Telephone, x.Contact.TextPhone, x.Contact.Url, x.Contact.Email))).ToList();

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
