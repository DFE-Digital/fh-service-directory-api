﻿using AutoMapper;
using FamilyHubs.ServiceDirectory.Core.Entities;
using FamilyHubs.ServiceDirectory.Infrastructure.Persistence.Repository;
using FamilyHubs.ServiceDirectory.Shared.Dto;
using FamilyHubs.ServiceDirectory.Shared.Enums;
using FamilyHubs.SharedKernel;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FamilyHubs.ServiceDirectory.Api.Queries.GetServices;

public class GetServicesCommand : IRequest<PaginatedList<ServiceDto>>
{
    public GetServicesCommand(ServiceType serviceType, ServiceStatusType status, string? districtCode, int? minimumAge, int? maximumAge, int? givenAge, double? latitude, double? longitude, double? proximity, int? pageNumber, int? pageSize, string? text, string? serviceDeliveries, bool? isPaidFor, string? taxonomyIds, string? languages, bool? canFamilyChooseLocation, bool? isFamilyHub, int? maxFamilyHubs)
    {
        ServiceType = serviceType;
        Status = status;
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
            .Include(x => x.ServiceDeliveries)
            .Include(x => x.Eligibilities)
            .Include(x => x.CostOptions)
            .Include(x => x.Fundings)
            .Include(x => x.Languages)
            .Include(x => x.ServiceAreas)
            .Include(x => x.RegularSchedules)
            .Include(x => x.HolidaySchedules)
            .Include(x => x.Contacts)
            .Include(x => x.Taxonomies)

            .Where(x => x.Status == request.Status && x.Status != ServiceStatusType.Deleted);

        if (request.DistrictCode != null)
        {
            var organisationIds = await _context.Organisations.Where(x => x.AdminAreaCode == request.DistrictCode).Select(x => x.Id).ToListAsync(cancellationToken);
            services = services.Where(x => organisationIds.Contains(x.OrganisationId));
        }

        if (request.ServiceType != ServiceType.NotSet)
        {
            services = services.Where(x => x.ServiceType == request.ServiceType);
        }

        if (!string.IsNullOrEmpty(request.TaxonomyIds))
        {
            var parts = request.TaxonomyIds.Split(',').Select(long.Parse);
            services = services.Where(x => x.Taxonomies.Any(st => parts.Any(taxonomyId => taxonomyId  == st.Id)));
        }

        if (request.CanFamilyChooseLocation is true)
        {
            services = services.Where(x => x.CanFamilyChooseDeliveryLocation);
        }

        if (!string.IsNullOrEmpty(request.Languages))
        {
            var parts = request.Languages.Split(',');
            services = services.Where(x => x.Languages.Any(language => parts.Any(languageName => languageName == language.Name)));
        }

        if (request.MaximumAge != null)
            services = services.Where(x => x.Eligibilities.Any(eligibility => eligibility.MaximumAge <= request.MaximumAge.Value));

        if (request.MinimumAge != null)
            services = services.Where(x => x.Eligibilities.Any(eligibility => eligibility.MinimumAge >= request.MinimumAge.Value));

        if (request.GivenAge != null)
            services = services.Where(x => x.Eligibilities.Any(eligibility => eligibility.MinimumAge <= request.GivenAge.Value && eligibility.MaximumAge >= request.GivenAge.Value));

        if (!string.IsNullOrEmpty(request.Text))
            services = services.Where(x => x.Name.Contains(request.Text) || x.Description != null && x.Description.Contains(request.Text));

        var dbServices = await services.ToListAsync(cancellationToken);

        // ServiceDeliveries
        if (!string.IsNullOrEmpty(request.ServiceDeliveries))
        {
            var servicesFilteredByDelMethod = new List<Service>();
            var parts = request.ServiceDeliveries.Split(',');

            foreach (var part in parts)
                if (Enum.TryParse(part, true, out ServiceDeliveryType serviceDelivery))
                    servicesFilteredByDelMethod.AddRange(dbServices.Where(x => x.ServiceDeliveries.Any(delivery => delivery.Name == serviceDelivery)).ToList());

            dbServices = servicesFilteredByDelMethod;
        }

        if (request.IsPaidFor != null)
        {
            dbServices = dbServices.Where(x => IsPaidFor(x) == request.IsPaidFor).ToList();
        }

        var serviceIds = dbServices.Select(x => x.Id).ToList();
        if (serviceIds.Any())
        {
            dbServices = await _context.Services.Where(x => serviceIds.Any(y => y == x.Id))
                .Include(x => x.Locations)
                .ThenInclude(x => x.RegularSchedules)

                .Include(x => x.Locations)
                .ThenInclude(x => x.HolidaySchedules)

                .Include(x => x.Locations)
                .ThenInclude(x => x.Contacts)
                
                .ToListAsync(cancellationToken);
        }

        if (request.IsFamilyHub != null)
        {
            dbServices = dbServices
                .Where(s => s.Locations.Any(lt => lt.LocationType == LocationType.FamilyHub) == request.IsFamilyHub)
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
                service.Distance = Core.Helper.GetDistance(
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

        if (request.IsFamilyHub is null && request.MaxFamilyHubs != null)
        {
            // MaxFamilyHubs is really a flag to only include the nearest max family hubs at the start of the results set (when not filtering by IsFamilyHub)
            services = (services.Where(IsFamilyHub).Take(request.MaxFamilyHubs.Value)
                .Concat(services.Where(s => !IsFamilyHub(s)))).ToList();
        }

        return services;
    }

    private bool IsPaidFor(Service service)
    {
        if (!service.CostOptions.Any())
            return false;

        return !(service.CostOptions.Any(co => co.Amount == decimal.Zero || string.Equals(co.Option, "Free", StringComparison.OrdinalIgnoreCase)));
    }

    private bool IsFamilyHub(ServiceDto service)
    {
        return service.Locations.Any(lt => lt.LocationType == LocationType.FamilyHub);
    }
}
