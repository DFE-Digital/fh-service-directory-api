﻿using AutoMapper;
using FamilyHubs.ServiceDirectory.Data.Entities;
using FamilyHubs.ServiceDirectory.Shared.Dto;

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
        CreateMap<ReviewDto, Review>().ReverseMap();
        CreateMap<ServiceDto, Service>().ReverseMap();
        CreateMap<ServiceAreaDto, ServiceArea>().ReverseMap();
        CreateMap<ServiceDeliveryDto, ServiceDelivery>().ReverseMap();
        CreateMap<OrganisationDto, Organisation>().ReverseMap();
        CreateMap<OrganisationWithServicesDto, Organisation>().ReverseMap();
        CreateMap<OrganisationExDto, Organisation>().ReverseMap();

        CreateMap<LocationDto, Location>().ReverseMap();
        CreateMap<Location, Location>();

        CreateMap<TaxonomyDto, Taxonomy>().ReverseMap();
        CreateMap<Taxonomy, Taxonomy>();

        CreateMap<ContactDto, Contact>().ReverseMap();
        CreateMap<Contact, Contact>();

        CreateMap<HolidayScheduleDto, HolidaySchedule>().ReverseMap();
        CreateMap<HolidaySchedule, HolidaySchedule>();

        CreateMap<RegularScheduleDto, RegularSchedule>().ReverseMap();
        CreateMap<RegularSchedule, RegularSchedule>();
    }
}
