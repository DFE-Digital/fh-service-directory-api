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
        CreateMap<ContactDto, Contact>();
        CreateMap<CostOptionDto, CostOption>();
        CreateMap<EligibilityDto, Eligibility>();
        CreateMap<FundingDto, Funding>();
        CreateMap<HolidayScheduleDto, HolidaySchedule>();
        CreateMap<LanguageDto, Language>();
        CreateMap<LinkContactDto, LinkContact>();
        CreateMap<LinkTaxonomyDto, LinkTaxonomy>();
        CreateMap<LocationDto, Location>();
        CreateMap<OrganisationWithServicesDto, Organisation>();
        CreateMap<OrganisationTypeDto, OrganisationType>();
        CreateMap<PhysicalAddressDto, PhysicalAddress>();
        CreateMap<RegularScheduleDto, RegularSchedule>();
        CreateMap<RelatedOrganisationDto, RelatedOrganisation>();
        CreateMap<ServiceDto, Service>();
        CreateMap<ServiceAreaDto, ServiceArea>();
        CreateMap<ServiceAtLocationDto, ServiceAtLocation>();
        CreateMap<ServiceDeliveryDto, ServiceDelivery>();
        CreateMap<ServiceTaxonomyDto, ServiceTaxonomy>();
        CreateMap<ServiceTypeDto, ServiceType>();
        CreateMap<TaxonomyDto, Taxonomy>();
        CreateMap<UICacheDto, UICache>();
    }
}
