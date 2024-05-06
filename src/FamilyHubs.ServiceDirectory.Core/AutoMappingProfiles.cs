using AutoMapper;
using FamilyHubs.ServiceDirectory.Data;
using FamilyHubs.ServiceDirectory.Data.Entities;
using FamilyHubs.ServiceDirectory.Data.Entities.ManyToMany;
using FamilyHubs.ServiceDirectory.Shared.CreateUpdateDto;
using FamilyHubs.ServiceDirectory.Shared.Dto;
using FamilyHubs.ServiceDirectory.Shared.Dto.Metrics;
using DoNotRemove = AutoMapper.EntityFrameworkCore.Extensions;

namespace FamilyHubs.ServiceDirectory.Core;

public class AutoMappingProfiles : Profile
{
    public AutoMappingProfiles()
    {
        CreateMap<AccessibilityForDisabilitiesDto, AccessibilityForDisabilities>().ReverseMap();
        CreateMap<CostOptionDto, CostOption>().ReverseMap();
        CreateMap<EligibilityDto, Eligibility>().ReverseMap();
        CreateMap<FundingDto, Funding>().ReverseMap();
        CreateMap<LanguageDto, Language>().ReverseMap();
        CreateMap<ServiceNameDto, Service>().ReverseMap();
        CreateMap<ServiceAreaDto, ServiceArea>().ReverseMap();
        CreateMap<ServiceDeliveryDto, ServiceDelivery>().ReverseMap();

        CreateMap<OrganisationDto, Organisation>().ReverseMap();
        CreateMap<OrganisationDetailsDto, Organisation>().ReverseMap();
        //todo: some of these maps are purely for unit testing purposes. we should separate them out into a test profile
        CreateMap<OrganisationDetailsDto, OrganisationDto>().ReverseMap();

        CreateMap<ServiceDto, Service>().ReverseMap();
        CreateMap<ServiceChangeDto, Service>().ReverseMap();
        CreateMap<ServiceChangeDto, ServiceDto>().ReverseMap();

        CreateMap<ServiceAtLocationChangeDto, ServiceAtLocation>().ReverseMap();

        CreateMap<LocationDto, Location>().ReverseMap();
        CreateMap<Location, Location>();

        CreateMap<TaxonomyDto, Taxonomy>().ReverseMap();
        CreateMap<Taxonomy, Taxonomy>();

        CreateMap<ContactDto, Contact>().ReverseMap();
        CreateMap<Contact, Contact>();

        CreateMap<ScheduleDto, Schedule>().ReverseMap();
        CreateMap<Schedule, Schedule>();

        CreateMap<ServiceSearchDto, ServiceSearch>().ReverseMap();
        // CreateMap<ServiceSearch, ServiceSearch>();
        CreateMap<ServiceSearchResult, ServiceSearchResultDto>().ReverseMap();
        // CreateMap<ServiceSearchResult, ServiceSearchResult>();
        CreateMap<Event, EventDto>().ReverseMap();
        // CreateMap<Event, Event>();
    }
}
