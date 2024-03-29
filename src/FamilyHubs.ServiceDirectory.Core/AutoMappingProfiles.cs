﻿using AutoMapper;
using FamilyHubs.ServiceDirectory.Data.Entities;
using FamilyHubs.ServiceDirectory.Shared.Dto;

using DoNotRemove = AutoMapper.EntityFrameworkCore.Extensions;

namespace FamilyHubs.ServiceDirectory.Core;

public class AutoMappingProfiles : Profile
{
    public AutoMappingProfiles()
    {
        //todo: is entity and dto's are identical, why bother having dto's?
        CreateMap<AccessibilityForDisabilitiesDto, AccessibilityForDisabilities>().ReverseMap();
        CreateMap<CostOptionDto, CostOption>().ReverseMap();
        CreateMap<EligibilityDto, Eligibility>().ReverseMap();
        CreateMap<FundingDto, Funding>().ReverseMap();
        CreateMap<LanguageDto, Language>().ReverseMap();
        CreateMap<ServiceDto, Service>().ReverseMap();
        CreateMap<ServiceNameDto, Service>().ReverseMap();
        CreateMap<ServiceAreaDto, ServiceArea>().ReverseMap();
        CreateMap<ServiceDeliveryDto, ServiceDelivery>().ReverseMap();
        CreateMap<OrganisationDto, Organisation>().ReverseMap();
        CreateMap<OrganisationDetailsDto, Organisation>().ReverseMap();

        CreateMap<LocationDto, Location>().ReverseMap();
        CreateMap<Location, Location>();

        CreateMap<TaxonomyDto, Taxonomy>().ReverseMap();
        CreateMap<Taxonomy, Taxonomy>();

        CreateMap<ContactDto, Contact>().ReverseMap();
        CreateMap<Contact, Contact>();

        CreateMap<ScheduleDto, Schedule>().ReverseMap();
        CreateMap<Schedule, Schedule>();
    }
}
