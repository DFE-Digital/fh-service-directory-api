﻿using FamilyHubs.ServiceDirectory.Core.Entities;
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
            service.Fundings.Select(funding => new FundingDto(funding.Id, funding.Source)).ToList(),
            service.CostOptions.Select(costOption => new CostOptionDto(costOption.Id, costOption.AmountDescription, costOption.Amount, costOption.LinkId, costOption.Option, costOption.ValidFrom, costOption.ValidTo)).ToList(),
            service.Languages.Select(language => new LanguageDto(language.Id, language.Name)).ToList(),
            service.ServiceAreas.Select(serviceArea => new ServiceAreaDto(serviceArea.Id, serviceArea.ServiceAreaDescription, serviceArea.Extent, serviceArea.Uri)).ToList(),
            service.ServiceAtLocations.Select(serviceAtLocation => new ServiceAtLocationDto(serviceAtLocation.Id, GetLocation(serviceAtLocation), GetRegularSchedules(serviceAtLocation.RegularSchedules), GetHolidaySchedules(serviceAtLocation.HolidaySchedules), GetContacts(serviceAtLocation.LinkContacts ?? new List<LinkContact>()))).ToList(),
            service.ServiceTaxonomies.Select(serviceTaxonomy => new ServiceTaxonomyDto(serviceTaxonomy.Id, serviceTaxonomy.Taxonomy != null ? new TaxonomyDto(serviceTaxonomy.Taxonomy.Id, serviceTaxonomy.Taxonomy.Name, serviceTaxonomy.Taxonomy.TaxonomyType, serviceTaxonomy.Taxonomy.Parent) : null)).ToList(),
            service.RegularSchedules.Select(regularSchedule => new RegularScheduleDto(regularSchedule.Id, regularSchedule.Description, regularSchedule.OpensAt, regularSchedule.ClosesAt, regularSchedule.ByDay, regularSchedule.ByMonthDay, regularSchedule.DtStart, regularSchedule.Freq, regularSchedule.Interval, regularSchedule.ValidFrom, regularSchedule.ValidTo)).ToList(),
            service.HolidaySchedules.Select(holidaySchedule => new HolidayScheduleDto(holidaySchedule.Id, holidaySchedule.Closed, holidaySchedule.ClosesAt, holidaySchedule.StartDate, holidaySchedule.EndDate, holidaySchedule.OpensAt)).ToList(),
            GetContacts(service.LinkContacts)
            )).ToList();

        return dtoServices;

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
            service.ServiceDeliveries.Select(serviceDelivery => new ServiceDeliveryDto(serviceDelivery.Id, serviceDelivery.Name)).ToList(),
            service.Eligibilities.Select(eligibility => new EligibilityDto(eligibility.Id, eligibility.EligibilityDescription, eligibility.MaximumAge, eligibility.MinimumAge)).ToList(),
            service.Fundings.Select(funding => new FundingDto(funding.Id, funding.Source)).ToList(),
            service.CostOptions.Select(costOption => new CostOptionDto(costOption.Id, costOption.AmountDescription, costOption.Amount, costOption.LinkId, costOption.Option, costOption.ValidFrom, costOption.ValidTo)).ToList(),
            service.Languages.Select(language => new LanguageDto(language.Id, language.Name)).ToList(),
            service.ServiceAreas.Select(serviceArea => new ServiceAreaDto(serviceArea.Id, serviceArea.ServiceAreaDescription, serviceArea.Extent, serviceArea.Uri)).ToList(),
            service.ServiceAtLocations.Select(serviceAtLocation => new ServiceAtLocationDto(serviceAtLocation.Id, GetLocation(serviceAtLocation), GetRegularSchedules(serviceAtLocation.RegularSchedules), GetHolidaySchedules(serviceAtLocation.HolidaySchedules), GetContacts(serviceAtLocation.LinkContacts ?? new List<LinkContact>()))).ToList(),
            service.ServiceTaxonomies.Select(serviceTaxonomy => new ServiceTaxonomyDto(serviceTaxonomy.Id, serviceTaxonomy.Taxonomy != null ? new TaxonomyDto(serviceTaxonomy.Taxonomy.Id, serviceTaxonomy.Taxonomy.Name, serviceTaxonomy.Taxonomy.TaxonomyType, serviceTaxonomy.Taxonomy.Parent) : null)).ToList(),
            service.RegularSchedules.Select(regularSchedule => new RegularScheduleDto(regularSchedule.Id, regularSchedule.Description, regularSchedule.OpensAt, regularSchedule.ClosesAt, regularSchedule.ByDay, regularSchedule.ByMonthDay, regularSchedule.DtStart, regularSchedule.Freq, regularSchedule.Interval, regularSchedule.ValidFrom, regularSchedule.ValidTo)).ToList(),
            service.HolidaySchedules.Select(holidaySchedule => new HolidayScheduleDto(holidaySchedule.Id, holidaySchedule.Closed, holidaySchedule.ClosesAt, holidaySchedule.StartDate, holidaySchedule.EndDate, holidaySchedule.OpensAt)).ToList(),
            GetContacts(service.LinkContacts)
            );

        return dtoService;

    }
    private static LocationDto GetLocation(ServiceAtLocation serviceAtLocation)
    {
        return new LocationDto(
            serviceAtLocation.Location.Id, 
            serviceAtLocation.Location.Name, 
            serviceAtLocation.Location.Description, 
            serviceAtLocation.Location.Latitude, 
            serviceAtLocation.Location.Longitude, 
            GetAddresses(serviceAtLocation.Location), 
            GetLinkTaxonomies(serviceAtLocation.Location), 
            GetContacts(serviceAtLocation.Location.LinkContacts ?? new List<LinkContact>()));
    }
    private static ICollection<RegularScheduleDto> GetRegularSchedules(ICollection<RegularSchedule>? schedules)
    {
        return schedules != null ? schedules.Select(x => new RegularScheduleDto(x.Id, x.Description, x.OpensAt, x.ClosesAt, x.ByDay, x.ByMonthDay, x.DtStart, x.Freq, x.Interval, x.ValidFrom, x.ValidTo)).ToList() : new List<RegularScheduleDto>();
    }
    private static ICollection<HolidayScheduleDto> GetHolidaySchedules(ICollection<HolidaySchedule>? schedules)
    {
        if (schedules != null)
        {
            return schedules.Select(x => new HolidayScheduleDto(x.Id, x.Closed, x.ClosesAt, x.StartDate, x.EndDate, x.OpensAt)).ToList();
        }
        List<HolidayScheduleDto> holidaySchedules = new List<HolidayScheduleDto>();

        return holidaySchedules;
    }
    private static List<LinkContactDto> GetContacts(ICollection<LinkContact> contacts)
    {
        var items = contacts.Select(x => new LinkContactDto(x.Id, x.LinkId, x.LinkType, 
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
        var linkTaxonomies = location.LinkTaxonomies?.Select(x => new LinkTaxonomyDto(x.Id, x.LinkType, x.LinkId, new TaxonomyDto(x.Taxonomy!.Id, x.Taxonomy.Name, x.Taxonomy.TaxonomyType, x.Taxonomy.Parent))).ToList();

        return linkTaxonomies ?? new List<LinkTaxonomyDto>();
    }
}
