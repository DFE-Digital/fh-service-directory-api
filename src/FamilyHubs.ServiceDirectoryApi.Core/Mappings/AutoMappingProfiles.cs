using AutoMapper;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralContacts;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralCostOptions;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralEligibilitys;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralLanguages;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralLocations;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralOrganisations;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralPhones;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralPhysicalAddresses;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralServiceAreas;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralServiceAtLocations;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralServiceDeliverysEx;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralServices;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralServiceTaxonomys;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralTaxonomys;
using FamilyHubs.ServiceDirectoryApi.Core.Entities.OpenReferralContacts;
using FamilyHubs.ServiceDirectoryApi.Core.Entities.OpenReferralCostOptions;
using FamilyHubs.ServiceDirectoryApi.Core.Entities.OpenReferralEligibilitys;
using FamilyHubs.ServiceDirectoryApi.Core.Entities.OpenReferralLanguages;
using FamilyHubs.ServiceDirectoryApi.Core.Entities.OpenReferralLocations;
using FamilyHubs.ServiceDirectoryApi.Core.Entities.OpenReferralOrganisations;
using FamilyHubs.ServiceDirectoryApi.Core.Entities.OpenReferralPhones;
using FamilyHubs.ServiceDirectoryApi.Core.Entities.OpenReferralPhysicalAddresses;
using FamilyHubs.ServiceDirectoryApi.Core.Entities.OpenReferralServiceAreas;
using FamilyHubs.ServiceDirectoryApi.Core.Entities.OpenReferralServiceAtLocations;
using FamilyHubs.ServiceDirectoryApi.Core.Entities.OpenReferralServiceDeliveries;
using FamilyHubs.ServiceDirectoryApi.Core.Entities.OpenReferralServices;
using FamilyHubs.ServiceDirectoryApi.Core.Entities.OpenReferralServiceTaxonomies;
using FamilyHubs.ServiceDirectoryApi.Core.Entities.OpenReferralTaxonomies;

namespace FamilyHubs.ServiceDirectoryApi.Core.Mappings;

public class AutoMappingProfiles : Profile
{
    public AutoMappingProfiles()
    {
        CreateMap<OpenReferralContactDto, OpenReferralContact>();
        CreateMap<OpenReferralCostOptionDto, OpenReferralCostOption>();
        CreateMap<OpenReferralEligibilityDto, OpenReferralEligibility>();
        CreateMap<OpenReferralLanguageDto, OpenReferralLanguage>();
        CreateMap<OpenReferralLocationDto, OpenReferralLocation>();
        CreateMap<OpenReferralOrganisationWithServicesDto, OpenReferralOrganisation>();
        CreateMap<OpenReferralPhoneDto, OpenReferralPhone>();
        CreateMap<OpenReferralPhysicalAddressDto, OpenReferralPhysicalAddress>();
        CreateMap<OpenReferralServiceAreaDto, OpenReferralServiceArea>();
        CreateMap<OpenReferralServiceTaxonomyDto, OpenReferralServiceTaxonomy>();
        CreateMap<OpenReferralServiceAtLocationDto, OpenReferralServiceAtLocation>();
        CreateMap<OpenReferralServiceDeliveryExDto, OpenReferralServiceDelivery>();
        CreateMap<OpenReferralServiceDto, OpenReferralService>();
        CreateMap<OpenReferralTaxonomyDto, OpenReferralTaxonomy>();
    }
}
