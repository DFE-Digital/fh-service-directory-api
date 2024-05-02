using AutoMapper;
using FamilyHubs.ServiceDirectory.Core.Helper;
using FamilyHubs.ServiceDirectory.Data.Entities;
using FamilyHubs.ServiceDirectory.Data.Entities.ManyToMany;
using FamilyHubs.ServiceDirectory.Shared.CreateUpdateDto;
using FamilyHubs.ServiceDirectory.Shared.Dto;
using NetTopologySuite.Geometries;
using DoNotRemove = AutoMapper.EntityFrameworkCore.Extensions;
using Location = FamilyHubs.ServiceDirectory.Data.Entities.Location;

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

        CreateMap<ServiceAtLocationDto, ServiceAtLocation>().ReverseMap();
        CreateMap<ServiceAtLocationChangeDto, ServiceAtLocation>().ReverseMap();
        CreateMap<ServiceAtLocationChangeDto, ServiceAtLocationDto>().ReverseMap();

        CreateMap<LocationDto, Location>()
            .ForMember(dest => dest.GeoPoint, opt => opt.MapFrom<GeoPoint.Resolver>())
            .ReverseMap();
        CreateMap<Location, Location>();

        CreateMap<TaxonomyDto, Taxonomy>().ReverseMap();
        CreateMap<Taxonomy, Taxonomy>();

        CreateMap<ContactDto, Contact>().ReverseMap();
        CreateMap<Contact, Contact>();

        CreateMap<ScheduleDto, Schedule>().ReverseMap();
        CreateMap<Schedule, Schedule>();
    }
}
