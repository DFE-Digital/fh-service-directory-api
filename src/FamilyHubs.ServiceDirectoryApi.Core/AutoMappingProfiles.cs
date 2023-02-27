using AutoMapper;
using FamilyHubs.ServiceDirectory.Core.Entities;
using FamilyHubs.ServiceDirectory.Shared.Dto;

namespace FamilyHubs.ServiceDirectory.Core;

public class AutoMappingProfiles : Profile
{
    public AutoMappingProfiles()
    {
        CreateMap<AccessibilityForDisabilitiesDto, AccessibilityForDisabilities>();
        CreateMap<AdminAreaDto, AdminArea>();
        CreateMap<ContactDto, Contact>().ReverseMap();
        CreateMap<CostOptionDto, CostOption>();
        CreateMap<EligibilityDto, Eligibility>();
        CreateMap<FundingDto, Funding>();
        CreateMap<HolidayScheduleDto, HolidaySchedule>();
        CreateMap<LanguageDto, Language>();
        CreateMap<LinkContactDto, LinkContact>().ReverseMap();
        CreateMap<LinkTaxonomyDto, LinkTaxonomy>().ReverseMap();
        CreateMap<LocationDto, Location>();
        CreateMap<LocationDto, Location>().ReverseMap();
        CreateMap<OrganisationWithServicesDto, Organisation>();
        CreateMap<OrganisationTypeDto, OrganisationType>();
        CreateMap<PhysicalAddressDto, PhysicalAddress>();
        CreateMap<PhysicalAddressDto, PhysicalAddress>().ReverseMap();
        CreateMap<RegularScheduleDto, RegularSchedule>();
        CreateMap<RelatedOrganisationDto, RelatedOrganisation>();
        CreateMap<ServiceDto, Service>();
        CreateMap<ServiceAreaDto, ServiceArea>();
        CreateMap<ServiceAtLocationDto, ServiceAtLocation>();
        CreateMap<ServiceDeliveryDto, ServiceDelivery>();
        CreateMap<ServiceTaxonomyDto, ServiceTaxonomy>();
        CreateMap<ServiceTypeDto, ServiceType>();
        CreateMap<TaxonomyDto, Taxonomy>();
        CreateMap<UICacheDto, UiCache>();
    }
}
