using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralContacts;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralCostOptions;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralEligibilitys;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralLanguages;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralLocations;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralPhones;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralPhysicalAddresses;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralServiceAreas;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralServiceAtLocations;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralServiceDeliverysEx;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralServices;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralServiceTaxonomys;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralTaxonomys;
using fh_service_directory_api.core.Entities;
using System.Collections.ObjectModel;

namespace fh_service_directory_api.api.Helper;

public class OpenReferralDtoHelper
{
    public static List<OpenReferralServiceDto> GetOpenReferralServicesDto(IEnumerable<OpenReferralService> dbservices)
    {
        var dtoServices = dbservices.Select(x => new OpenReferralServiceDto(
            x.Id,
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
            new List<IOpenReferralServiceDeliveryExDto>(x.ServiceDelivery.Select(x => new OpenReferralServiceDeliveryExDto(x.Id, x.ServiceDelivery)).ToList()),
            new List<IOpenReferralEligibilityDto>(x.Eligibilitys.Select(x => new OpenReferralEligibilityDto(x.Id, x.Eligibility, x.Maximum_age, x.Minimum_age)).ToList()),
            GetContacts(x.Contacts),
            new List<IOpenReferralCostOptionDto>(x.Cost_options.Select(x => new OpenReferralCostOptionDto(x.Id, x.Amount_description, x.Amount, x.LinkId, x.Option, x.Valid_from, x.Valid_to)).ToList()),
            new List<IOpenReferralLanguageDto>(x.Languages.Select(x => new OpenReferralLanguageDto(x.Id, x.Language)).ToList()),
            new List<IOpenReferralServiceAreaDto>(x.Service_areas.Select(x => new OpenReferralServiceAreaDto(x.Id, x.Service_area, x.Extent, x.Uri)).ToList()),
            new List<IOpenReferralServiceAtLocationDto>(x.Service_at_locations.Select(x => new OpenReferralServiceAtLocationDto(x.Id, new OpenReferralLocationDto(x.Location.Id, x.Location.Name, x.Location.Description, x.Location.Latitude, x.Location.Longitude, GetAddresses(x.Location)))).ToList()),
            new List<IOpenReferralServiceTaxonomyDto>(x.Service_taxonomys.Select(x => new OpenReferralServiceTaxonomyDto(x.Id, x.Taxonomy != null ? new OpenReferralTaxonomyDto(x.Taxonomy.Id, x.Taxonomy.Name, x.Taxonomy.Vocabulary, x.Taxonomy.Parent) : null)).ToList())
            )).ToList();

        return dtoServices;

    }

    public static OpenReferralServiceDto GetOpenReferralServiceDto(OpenReferralService service)
    {
        var dtoService = new OpenReferralServiceDto(
            service.Id,
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
            new List<IOpenReferralServiceDeliveryExDto>(service.ServiceDelivery.Select(x => new OpenReferralServiceDeliveryExDto(x.Id, x.ServiceDelivery)).ToList()),
            new List<IOpenReferralEligibilityDto>(service.Eligibilitys.Select(x => new OpenReferralEligibilityDto(x.Id, x.Eligibility, x.Maximum_age, x.Minimum_age)).ToList()),
            GetContacts(service.Contacts),
            new List<IOpenReferralCostOptionDto>(service.Cost_options.Select(x => new OpenReferralCostOptionDto(x.Id, x.Amount_description, x.Amount, x.LinkId, x.Option, x.Valid_from, x.Valid_to)).ToList()),
            new List<IOpenReferralLanguageDto>(service.Languages.Select(x => new OpenReferralLanguageDto(x.Id, x.Language)).ToList()),
            new List<IOpenReferralServiceAreaDto>(service.Service_areas.Select(x => new OpenReferralServiceAreaDto(x.Id, x.Service_area, x.Extent, x.Uri)).ToList()),
            new List<IOpenReferralServiceAtLocationDto>(service.Service_at_locations.Select(x => new OpenReferralServiceAtLocationDto(x.Id, new OpenReferralLocationDto(x.Location.Id, x.Location.Name, x.Location.Description, x.Location.Latitude, x.Location.Longitude, GetAddresses(x.Location) ))).ToList()),
            new List<IOpenReferralServiceTaxonomyDto>(service.Service_taxonomys.Select(x => new OpenReferralServiceTaxonomyDto(x.Id, x.Taxonomy != null ? new OpenReferralTaxonomyDto(x.Taxonomy.Id, x.Taxonomy.Name, x.Taxonomy.Vocabulary, x.Taxonomy.Parent) : null)).ToList())
            );

        return dtoService;

    }

    private static List<IOpenReferralContactDto> GetContacts(ICollection<OpenReferralContact> contacts)
    {
        var items = contacts.Select(x => new OpenReferralContactDto(x.Id, x.Title, x.Name,
            GetPhones(x.Phones ?? new Collection<OpenReferralPhone>())
            )).ToList();

        if (items != null)
            return new List<IOpenReferralContactDto>(items);

        return new List<IOpenReferralContactDto>();
    }

    private static List<IOpenReferralPhoneDto> GetPhones(ICollection<OpenReferralPhone> phones)
    {
        var selectedPhones = phones?.Select(x => new OpenReferralPhoneDto(x.Id, x.Number)).ToList();
        if (selectedPhones != null)
        {
            List<IOpenReferralPhoneDto> list = new(selectedPhones);
            return list;
        }

        return new List<IOpenReferralPhoneDto>();
    }

    private static List<IOpenReferralPhysicalAddressDto> GetAddresses(OpenReferralLocation location)
    {
        if (location != null && location.Physical_addresses != null)
        {
            var addressList = location?.Physical_addresses?.Select(x => new OpenReferralPhysicalAddressDto(x.Id, x.Address_1, x.City, x.Postal_code, x.Country, x.State_province)).ToList();
            if (addressList != null)
                return new List<IOpenReferralPhysicalAddressDto>(addressList);
        }

        return new List<IOpenReferralPhysicalAddressDto>();
    }
}
