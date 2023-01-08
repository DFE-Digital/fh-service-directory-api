using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralContacts;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralCostOptions;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralEligibilitys;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralHolidaySchedule;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralLanguages;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralLocations;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralPhones;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralPhysicalAddresses;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralRegularSchedule;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralServiceAreas;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralServiceAtLocations;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralServiceDeliverysEx;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralServices;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralServiceTaxonomys;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralTaxonomys;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.ServiceType;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralLinkTaxonomies;
using fh_service_directory_api.core.Entities;
using System.Collections.ObjectModel;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralLinkContacts;

namespace fh_service_directory_api.api.Helper;

public class OpenReferralDtoHelper
{
    public static List<OpenReferralServiceDto> GetOpenReferralServicesDto(IEnumerable<OpenReferralService> dbservices)
    {
        var dtoServices = dbservices.Select(x => new OpenReferralServiceDto(
            x.Id,
            new ServiceTypeDto(x.ServiceType.Id, x.ServiceType.Name, x.ServiceType.Description),
            x.OpenReferralOrganisationId,
            x.Name,
            x.Description,
            x.Accreditations,
            x.Assured_date,
            x.Attending_access,
            x.Attending_type,
            x.Deliverable_type,
            x.Status,
            x.Url,
            x.Email,
            x.Fees,
            x.CanFamilyChooseDeliveryLocation,
            x.ServiceDelivery.Select(x => new OpenReferralServiceDeliveryExDto(x.Id, x.ServiceDelivery)).ToList(),
            x.Eligibilities.Select(x => new OpenReferralEligibilityDto(x.Id, x.Eligibility, x.Maximum_age, x.Minimum_age)).ToList(),
            //GetContacts(x.Contacts),
            x.Cost_options.Select(x => new OpenReferralCostOptionDto(x.Id, x.Amount_description, x.Amount, x.LinkId, x.Option, x.Valid_from, x.Valid_to)).ToList(),
            x.Languages.Select(x => new OpenReferralLanguageDto(x.Id, x.Language)).ToList(),
            x.Service_areas.Select(x => new OpenReferralServiceAreaDto(x.Id, x.Service_area, x.Extent, x.Uri)).ToList(),
            x.Service_at_locations.Select(x => new OpenReferralServiceAtLocationDto(x.Id, new OpenReferralLocationDto(x.Location.Id, x.Location.Name, x.Location.Description, x.Location.Latitude, x.Location.Longitude, GetAddresses(x.Location), GetLinkTaxonomies(x.Location)), GetRegularSchedules(x.Regular_schedule), GetHolidaySchedules(x.HolidayScheduleCollection), GetLinkContacts(x.Location.LinkContacts))).ToList(),
            x.Service_taxonomys.Select(x => new OpenReferralServiceTaxonomyDto(x.Id, x.Taxonomy != null ? new OpenReferralTaxonomyDto(x.Taxonomy.Id, x.Taxonomy.Name, x.Taxonomy.Vocabulary, x.Taxonomy.Parent) : null)).ToList(),
            x.Regular_schedules.Select(x => new OpenReferralRegularScheduleDto(x.Id, x.Description, x.Opens_at, x.Closes_at, x.Byday, x.Bymonthday, x.Dtstart, x.Freq, x.Interval, x.Valid_from, x.Valid_to)).ToList(),
            x.Holiday_schedules.Select(x => new OpenReferralHolidayScheduleDto(x.Id, x.Closed, x.Closes_at, x.Start_date, x.End_date, x.Opens_at)).ToList(),
            GetLinkContacts(x.Link_Contacts)
            )).ToList();

        return dtoServices;

    }

    public static ICollection<OpenReferralRegularScheduleDto> GetRegularSchedules(ICollection<OpenReferralRegular_Schedule>? schedules)
    {
        if (schedules != null)
        {
            return schedules.Select(x => new OpenReferralRegularScheduleDto(x.Id, x.Description, x.Opens_at, x.Closes_at, x.Byday, x.Bymonthday, x.Dtstart, x.Freq, x.Interval, x.Valid_from, x.Valid_to)).ToList();
        }

        return new List<OpenReferralRegularScheduleDto>();

    }

    public static ICollection<OpenReferralHolidayScheduleDto> GetHolidaySchedules(ICollection<OpenReferralHoliday_Schedule>? schedules)
    {
        if (schedules != null)
        {
            return schedules.Select(x => new OpenReferralHolidayScheduleDto(x.Id, x.Closed, x.Closes_at, x.Start_date, x.End_date, x.Opens_at)).ToList();
        }
        List<OpenReferralHolidayScheduleDto> holidaySchedules = new();

        return holidaySchedules;
    }

    public static OpenReferralServiceDto GetOpenReferralServiceDto(OpenReferralService service)
    {
        var dtoService = new OpenReferralServiceDto(
            service.Id,
            new ServiceTypeDto(service.ServiceType.Id, service.ServiceType.Name, service.ServiceType.Description),
            service.OpenReferralOrganisationId,
            service.Name,
            service.Description,
            service.Accreditations,
            service.Assured_date,
            service.Attending_access,
            service.Attending_type,
            service.Deliverable_type,
            service.Status,
            service.Url,
            service.Email,
            service.Fees,
            service.CanFamilyChooseDeliveryLocation,
            service.ServiceDelivery.Select(x => new OpenReferralServiceDeliveryExDto(x.Id, x.ServiceDelivery)).ToList(),
            service.Eligibilities.Select(x => new OpenReferralEligibilityDto(x.Id, x.Eligibility, x.Maximum_age, x.Minimum_age)).ToList(),
            //GetContacts(service.Contacts),
            service.Cost_options.Select(x => new OpenReferralCostOptionDto(x.Id, x.Amount_description, x.Amount, x.LinkId, x.Option, x.Valid_from, x.Valid_to)).ToList(),
            service.Languages.Select(x => new OpenReferralLanguageDto(x.Id, x.Language)).ToList(),
            service.Service_areas.Select(x => new OpenReferralServiceAreaDto(x.Id, x.Service_area, x.Extent, x.Uri)).ToList(),
            service.Service_at_locations.Select(x => new OpenReferralServiceAtLocationDto(x.Id, new OpenReferralLocationDto(x.Location.Id, x.Location.Name, x.Location.Description, x.Location.Latitude, x.Location.Longitude, GetAddresses(x.Location), GetLinkTaxonomies(x.Location)), GetRegularSchedules(x.Regular_schedule), GetHolidaySchedules(x.HolidayScheduleCollection), GetLinkContacts(x.Location.LinkContacts))).ToList(),
            service.Service_taxonomys.Select(x => new OpenReferralServiceTaxonomyDto(x.Id, x.Taxonomy != null ? new OpenReferralTaxonomyDto(x.Taxonomy.Id, x.Taxonomy.Name, x.Taxonomy.Vocabulary, x.Taxonomy.Parent) : null)).ToList(),
            service.Regular_schedules.Select(x => new OpenReferralRegularScheduleDto(x.Id, x.Description, x.Opens_at, x.Closes_at, x.Byday, x.Bymonthday, x.Dtstart, x.Freq, x.Interval, x.Valid_from, x.Valid_to)).ToList(),
            service.Holiday_schedules.Select(x => new OpenReferralHolidayScheduleDto(x.Id, x.Closed, x.Closes_at, x.Start_date, x.End_date, x.Opens_at)).ToList(),
            GetLinkContacts(service.Link_Contacts)
        );

        return dtoService;

    }

    private static List<OpenReferralLinkContactDto> GetLinkContacts(ICollection<OpenReferralLinkContact> linkContacts)
    {
        var items = linkContacts.Select(x => new OpenReferralLinkContactDto(x.Id, x.LinkId, x.LinkType, GetContacts(x.Contact))).ToList();

        if (items != null)
            return items;

        return new List<OpenReferralLinkContactDto>();
    }

    private static OpenReferralContactDto GetContacts(OpenReferralContact contact)
    {
        var item = new OpenReferralContactDto(contact.Id, contact.Title, contact.Name, GetPhones(contact.Phones));

        if (item != null)
            return item;

        return new OpenReferralContactDto();
    }

    private static List<OpenReferralPhoneDto> GetPhones(ICollection<OpenReferralPhone> phones)
    {
        var selectedPhones = phones?.Select(x => new OpenReferralPhoneDto(x.Id, x.Number)).ToList();
        if (selectedPhones != null)
        {
            return selectedPhones;
        }

        return new List<OpenReferralPhoneDto>();
    }

    private static List<OpenReferralPhysicalAddressDto> GetAddresses(OpenReferralLocation location)
    {
        if (location != null && location.Physical_addresses != null)
        {
            var addressList = location?.Physical_addresses?.Select(x => new OpenReferralPhysicalAddressDto(x.Id, x.Address_1, x.City, x.Postal_code, x.Country, x.State_province)).ToList();
            if (addressList != null)
                return addressList;
        }

        return new List<OpenReferralPhysicalAddressDto>();
    }
    private static List<OpenReferralLinkTaxonomyDto> GetLinkTaxonomies(OpenReferralLocation location)
    {
        if (location != null && location.LinkTaxonomies != null)
        {
            var linkTaxonomies = location?.LinkTaxonomies?.Select(x => new OpenReferralLinkTaxonomyDto(x.Id, x.LinkType, x.LinkId, new OpenReferralTaxonomyDto(x.Taxonomy!.Id, x.Taxonomy.Name, x.Taxonomy.Vocabulary, x.Taxonomy.Parent))).ToList();
            if (linkTaxonomies != null)
                return linkTaxonomies;
        }

        return new List<OpenReferralLinkTaxonomyDto>();
    }
}
