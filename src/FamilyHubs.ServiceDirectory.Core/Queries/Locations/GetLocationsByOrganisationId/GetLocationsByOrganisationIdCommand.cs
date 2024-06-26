﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using FamilyHubs.ServiceDirectory.Core.Helper;
using FamilyHubs.ServiceDirectory.Data.Entities;
using FamilyHubs.ServiceDirectory.Data.Repository;
using FamilyHubs.ServiceDirectory.Shared.Dto;
using FamilyHubs.ServiceDirectory.Shared.Enums;
using FamilyHubs.ServiceDirectory.Shared.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FamilyHubs.ServiceDirectory.Core.Queries.Locations.GetLocationsByOrganisationId;

public class GetLocationsByOrganisationIdCommand : IRequest<PaginatedList<LocationDto>>
{
    public long OrganisationId { get; }
    public int PageNumber { get; }
    public int PageSize { get; }
    public bool IsAscending { get; }
    public string OrderByColumn { get; }
    public string? SearchName { get; }
    public bool IsFamilyHub { get; }

    public GetLocationsByOrganisationIdCommand(long organisationId, int? pageNumber, int? pageSize, bool? isAscending, string? orderByColumn, string? searchName, bool? isFamilyHub)
    {
        OrganisationId = organisationId;
        PageNumber = pageNumber ?? 1;
        PageSize = pageSize ?? 10;
        IsAscending = isAscending ?? true;
        OrderByColumn = orderByColumn ?? "Location";
        SearchName = searchName;
        IsFamilyHub = isFamilyHub ?? false;
    }
}

public class GetLocationsByOrganisationIdCommandHandler : IRequestHandler<GetLocationsByOrganisationIdCommand, PaginatedList<LocationDto>>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetLocationsByOrganisationIdCommandHandler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<LocationDto>> Handle(GetLocationsByOrganisationIdCommand request, CancellationToken cancellationToken)
    {
        var skip = (request.PageNumber - 1) * request.PageSize;

        var locationsQuery = _context.Locations
            .Where(l => l.OrganisationId == request.OrganisationId);

        locationsQuery = Search(request, locationsQuery);
        locationsQuery = OrderBy(request, locationsQuery);

        var locations = await locationsQuery
            .Skip(skip)
            .Take(request.PageSize)
            .AsNoTracking()
            .ProjectTo<LocationDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        if (!locations.Any())
            return new PaginatedList<LocationDto>();

        var totalCount = await GetTotalCount(request, cancellationToken);

        return new PaginatedList<LocationDto>(locations, totalCount, request.PageNumber, request.PageSize);
    }

    private IQueryable<Location> Search(GetLocationsByOrganisationIdCommand request, IQueryable<Location> locationsQuery)
    {

        if (!string.IsNullOrEmpty(request.SearchName))
        {
            locationsQuery = locationsQuery.Where(x => (x.Name != null && x.Name.Contains(request.SearchName))
                || x.Address1.Contains(request.SearchName)
                || (x.Address2 != null && x.Address2.Contains(request.SearchName))
                || x.City.Contains(request.SearchName)
                || x.PostCode.Contains(request.SearchName)
                //allow to search by the the full phrase 
                || ((x.Name != null && x.Name != "" ? x.Name + ", " : "")
                    + (x.Address1 != "" ? x.Address1 + ", " : "")
                    + (x.Address2 != null && x.Address2 != "" ? x.Address2 + ", " : "")
                    + (x.City != "" ? x.City + ", " : "")
                    + (x.PostCode != "" ? x.PostCode : "")
                    ).Contains(request.SearchName));
        }

        if (request.IsFamilyHub)
        {
            locationsQuery = locationsQuery.Where(x => x.LocationTypeCategory == LocationTypeCategory.FamilyHub);
        }

        return locationsQuery;
    }


    private async Task<int> GetTotalCount(GetLocationsByOrganisationIdCommand request, CancellationToken cancellationToken)
    {
        var countQuery = _context.Services
            .Include(x => x.Locations)
            .Where(s => s.Status != ServiceStatusType.Deleted && s.OrganisationId == request.OrganisationId)
            .SelectMany(s => s.Locations);

        countQuery = Search(request, countQuery);

        var count = await countQuery
            .CountAsync(cancellationToken);

        return count;
    }

    private IQueryable<Location> OrderBy(GetLocationsByOrganisationIdCommand request, IQueryable<Location> locationsQuery)
    {
        switch (request.OrderByColumn)
        {
            case "Location":
                locationsQuery = locationsQuery
                    .OrderBy(x => x.Name, request.IsAscending)
                    .ThenBy(x => x.Address1, request.IsAscending)
                    .ThenBy(x => x.Address2, request.IsAscending)
                    .ThenBy(x => x.City, request.IsAscending)
                    .ThenBy(x => x.PostCode, request.IsAscending);
                break;
        }

        return locationsQuery;
    }
}