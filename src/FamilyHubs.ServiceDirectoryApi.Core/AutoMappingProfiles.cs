using AutoMapper;
using FamilyHubs.ServiceDirectory.Core.Entities;
using FamilyHubs.ServiceDirectory.Shared.Dto;

namespace FamilyHubs.ServiceDirectory.Core;

public class AutoMappingProfiles : Profile
{
    public AutoMappingProfiles()
    {
        CreateMap<AccessibilityForDisabilitiesDto, AccessibilityForDisabilities>().ReverseMap();
        CreateMap<ContactDto, Contact>().ReverseMap();
        CreateMap<CostOptionDto, CostOption>().ReverseMap();
        CreateMap<EligibilityDto, Eligibility>().ReverseMap();
        CreateMap<FundingDto, Funding>().ReverseMap();
        CreateMap<HolidayScheduleDto, HolidaySchedule>().ReverseMap();
        CreateMap<LanguageDto, Language>().ReverseMap();
        CreateMap<LocationDto, Location>().ReverseMap();
        CreateMap<OrganisationWithServicesDto, Organisation>().ReverseMap();
        CreateMap<RegularScheduleDto, RegularSchedule>().ReverseMap();
        CreateMap<ReviewDto, Review>().ReverseMap();
        CreateMap<ServiceDto, Service>().ReverseMap();
        CreateMap<ServiceAreaDto, ServiceArea>().ReverseMap();
        CreateMap<ServiceDeliveryDto, ServiceDelivery>().ReverseMap();
        CreateMap<TaxonomyDto, Taxonomy>().ReverseMap();
        CreateMap<Organisation, OrganisationDto>().ReverseMap();
    }
}
