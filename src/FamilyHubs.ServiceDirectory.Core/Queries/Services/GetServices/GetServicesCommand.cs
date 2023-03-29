﻿using AutoMapper;
using FamilyHubs.ServiceDirectory.Core.Helper;
using FamilyHubs.ServiceDirectory.Data.Entities;
using FamilyHubs.ServiceDirectory.Data.Repository;
using FamilyHubs.ServiceDirectory.Shared.Dto;
using FamilyHubs.ServiceDirectory.Shared.Enums;
using FamilyHubs.ServiceDirectory.Shared.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FamilyHubs.ServiceDirectory.Core.Queries.Services.GetServices;

public class GetServicesCommand : IRequest<PaginatedList<ServiceDto>>
{
    public GetServicesCommand(ServiceType? serviceType, ServiceStatusType? status, string? districtCode, int? minimumAge, int? maximumAge, int? givenAge, double? latitude, double? longitude, double? proximity, int? pageNumber, int? pageSize, string? text, string? serviceDeliveries, bool? isPaidFor, string? taxonomyIds, string? languages, bool? canFamilyChooseLocation, bool? isFamilyHub, int? maxFamilyHubs)
    {
        ServiceType = serviceType ?? ServiceType.NotSet;
        Status = status ?? ServiceStatusType.NotSet;
        DistrictCode = districtCode;
        MaximumAge = maximumAge;
        MinimumAge = minimumAge;
        GivenAge = givenAge;
        Latitude = latitude;
        Longitude = longitude;
        Meters = proximity;
        PageNumber = pageNumber ?? 1;
        PageSize = pageSize ?? 10;
        Text = text;
        ServiceDeliveries = serviceDeliveries;
        IsPaidFor = isPaidFor;
        TaxonomyIds = taxonomyIds;
        Languages = languages;
        CanFamilyChooseLocation = canFamilyChooseLocation;
        IsFamilyHub = isFamilyHub;
        MaxFamilyHubs = maxFamilyHubs;
    }

    public ServiceType ServiceType { get; }
    public ServiceStatusType Status { get; set; }
    public string? DistrictCode { get; }
    public int? MaximumAge { get; }
    public int? MinimumAge { get; }
    public int? GivenAge { get; }
    public double? Latitude { get; }
    public double? Longitude { get; }
    public double? Meters { get; }
    public int PageNumber { get; }
    public int PageSize { get; }
    public string? Text { get; }
    public string? ServiceDeliveries { get; }
    public bool? IsPaidFor { get; }
    public string? TaxonomyIds { get; }
    public string? Languages { get; }
    public bool? CanFamilyChooseLocation { get; }
    public bool? IsFamilyHub { get; }
    public int? MaxFamilyHubs { get; }
}

public class GetServicesCommandHandler : IRequestHandler<GetServicesCommand, PaginatedList<ServiceDto>>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetServicesCommandHandler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<ServiceDto>> Handle(GetServicesCommand request, CancellationToken cancellationToken)
    {
        if (request.Status == ServiceStatusType.NotSet)
            request.Status = ServiceStatusType.Active;

        var dbServices = await GetServices(request, cancellationToken);

        var filteredServices = _mapper.Map<List<ServiceDto>>(dbServices);

        filteredServices = SortServicesDto(request, filteredServices);

        var pageList = filteredServices.Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize).ToList();

        var result = new PaginatedList<ServiceDto>(pageList, filteredServices.Count, request.PageNumber, request.PageSize);

        return result;
    }

    private async Task<List<Service>> GetServices(GetServicesCommand request, CancellationToken cancellationToken)
    {
        var services = _context.Services
            .Include(x => x.Taxonomies)

            .Include(x => x.Locations)
            .ThenInclude(x => x.Contacts)

            .Include(x => x.Locations)
            .ThenInclude(x => x.HolidaySchedules)

            .Include(x => x.Locations)
            .ThenInclude(x => x.RegularSchedules)

            .AsNoTracking()
            .AsSplitQuery()

            .Where(x => x.Status == request.Status && x.Status != ServiceStatusType.Deleted);

        if (request.DistrictCode != null)
        {
            var organisationIds = await _context.Organisations.Where(x => x.AdminAreaCode == request.DistrictCode).Select(x => x.Id).ToListAsync(cancellationToken);
            services = services.Where(x => organisationIds.Contains(x.OrganisationId));
        }

        if (request.ServiceType != ServiceType.NotSet)
            services = services.Where(x => x.ServiceType == request.ServiceType);

        if (!string.IsNullOrEmpty(request.TaxonomyIds))
        {
            var parts = request.TaxonomyIds.Split(',').Select(long.Parse);
            services = services.Where(x => x.Taxonomies.Any(st => parts.Any(taxonomyId => taxonomyId == st.Id)));
        }

        if (request.CanFamilyChooseLocation is true) services = services.Where(x => x.CanFamilyChooseDeliveryLocation);

        if (!string.IsNullOrEmpty(request.Languages))
        {
            var parts = request.Languages.Split(',');
            services = services.Where(x => x.Languages.Any(language => parts.Any(languageName => languageName == language.Name)));
        }

        if (request.MaximumAge is not null)
            services = services.Where(x => x.Eligibilities.Any(eligibility => eligibility.MaximumAge <= request.MaximumAge.Value));

        if (request.MinimumAge is not null)
            services = services.Where(x => x.Eligibilities.Any(eligibility => eligibility.MinimumAge >= request.MinimumAge.Value));

        if (request.GivenAge is not null)
            services = services.Where(x => x.Eligibilities.Any(eligibility => eligibility.MinimumAge <= request.GivenAge.Value && eligibility.MaximumAge >= request.GivenAge.Value));

        if (!string.IsNullOrEmpty(request.Text))
            services = services.Where(x => x.Name.Contains(request.Text) || x.Description != null && x.Description.Contains(request.Text));

        if (request.IsFamilyHub is not null)
            services = services.Where(s => s.Locations.Any(lt => lt.LocationType == LocationType.FamilyHub) == request.IsFamilyHub);

        if (request.IsPaidFor is not null)
            services = services.Where(s => s.CostOptions.Any(co => co.Amount == decimal.Zero || co.Option == null || co.Option.ToLower() == "free".ToLower()) != request.IsPaidFor.Value);

        if (!string.IsNullOrEmpty(request.ServiceDeliveries))
        {
            var parts = request.ServiceDeliveries.Split(',').Select(sd => Enum.Parse<ServiceDeliveryType>(sd, true)).ToList();
            services = services.Where(s => s.ServiceDeliveries.Any(co => parts.Contains(co.Name)));
        }

        var dbServices = await services.ToListAsync(cancellationToken);

        if (request.IsFamilyHub is null && request.MaxFamilyHubs is not null)
        {
            // MaxFamilyHubs is really a flag to only include the nearest max family hubs at the start of the results set (when not filtering by IsFamilyHub)
            dbServices = dbServices
                .Where(s => s.Locations.Any(lt => lt.LocationType == LocationType.FamilyHub))
                .Take(request.MaxFamilyHubs.Value)
                .Concat(dbServices.Where(s => s.Locations.Any(lt => lt.LocationType == LocationType.FamilyHub) == false))
                .ToList();
        }

        return dbServices;
    }

    private List<ServiceDto> SortServicesDto(GetServicesCommand request, List<ServiceDto> services)
    {
        if (request.Latitude is not null && request.Longitude is not null)
        {
            foreach (var service in services)
            {
                service.Distance = HelperUtility.GetDistance(
                request.Latitude,
                    request.Longitude,
                    service.Locations.FirstOrDefault()?.Latitude,
                    service.Locations.FirstOrDefault()?.Longitude);
            }

            if (request.Meters is not null)
            {
                services = services.Where(x => x.Distance < request.Meters).ToList();
            }
            services = services.OrderBy(x => x.Distance).ToList();
        }

        return services;
    }
}
