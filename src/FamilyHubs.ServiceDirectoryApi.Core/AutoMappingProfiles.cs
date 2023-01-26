using AutoMapper;
using FamilyHubs.ServiceDirectory.Core.Entities;
using FamilyHubs.ServiceDirectory.Shared.Dto;

namespace FamilyHubs.ServiceDirectory.Core;

public class AutoMappingProfiles : Profile
{
    public AutoMappingProfiles()
    {
        CreateMap<ContactDto, Contact>();
        CreateMap<CostOptionDto, CostOption>();
        CreateMap<EligibilityDto, Eligibility>();
        CreateMap<LanguageDto, Language>();
        CreateMap<LocationDto, Location>();
        CreateMap<OrganisationWithServicesDto, Organisation>();
        CreateMap<PhysicalAddressDto, PhysicalAddress>();
        CreateMap<ServiceAreaDto, ServiceArea>();
        CreateMap<ServiceTaxonomyDto, ServiceTaxonomy>();
        CreateMap<ServiceAtLocationDto, ServiceAtLocation>();
        CreateMap<ServiceDeliveryDto, ServiceDelivery>();
        CreateMap<ServiceDto, Service>();
        CreateMap<LinkTaxonomyDto, LinkTaxonomy>();
        CreateMap<TaxonomyDto, Taxonomy>();
        CreateMap<OrganisationTypeDto, OrganisationType>();
        CreateMap<ServiceTypeDto, ServiceType>();
        CreateMap<RegularScheduleDto, RegularSchedule>();
        CreateMap<HolidayScheduleDto, HolidaySchedule>();
    }
}
