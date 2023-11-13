using AutoMapper;
using AutoMapper.QueryableExtensions;
using FamilyHubs.ServiceDirectory.Data.Entities;
using FamilyHubs.ServiceDirectory.Data.Repository;
using FamilyHubs.ServiceDirectory.Shared.Dto;
using FamilyHubs.ServiceDirectory.Shared.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FamilyHubs.ServiceDirectory.Core.Queries.Locations.ListLocations;

public class ListLocationsCommand : IRequest<PaginatedList<LocationDto>>
{
    public int PageNumber { get; }
    public int PageSize { get; }
    public bool IsAscending { get; }
    public string? SearchName { get; }
    public bool IsFamilyHub { get; }
    public bool IsNonFamilyHub { get; }
    public string OrderByColumn { get; }

    public ListLocationsCommand(int? pageNumber, string? orderByColumn, int? pageSize, bool? isAscending, string? searchName, bool? isFamilyHub, bool? isNonFamilyHub)
    {
        PageNumber = pageNumber ?? 1;
        OrderByColumn = orderByColumn ?? "Location";
        PageSize = pageSize ?? 10;
        IsAscending = isAscending ?? true;
        SearchName = searchName;
        IsFamilyHub = isFamilyHub ?? false;
        IsNonFamilyHub = isNonFamilyHub ?? false;
    }
}

public class ListLocationCommandHandler : IRequestHandler<ListLocationsCommand, PaginatedList<LocationDto>>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public ListLocationCommandHandler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<LocationDto>> Handle(ListLocationsCommand request, CancellationToken cancellationToken)
    {
        int skip = (request.PageNumber - 1) * request.PageSize;

        IQueryable<Location> locationsQuery = _context.Locations;

        locationsQuery = Search(request, locationsQuery);
        locationsQuery = OrderBy(request, locationsQuery);

        var locations = await locationsQuery
            .Skip(skip)
            .Take(request.PageSize)
            .ProjectTo<LocationDto>(_mapper.ConfigurationProvider)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        int totalCount = await GetTotalCount(request, cancellationToken);

        return new PaginatedList<LocationDto>(locations, totalCount, request.PageNumber, request.PageSize);
    }

    private async Task<int> GetTotalCount(ListLocationsCommand request, CancellationToken cancellationToken)
    {
        IQueryable<Location> locationQuery = _context.Locations;
        locationQuery = Search(request, locationQuery);

        var count = await locationQuery.CountAsync(cancellationToken);
        return count;
    }

    private IQueryable<Location> Search(ListLocationsCommand request, IQueryable<Location> locationsQuery)
    {
        if (request.SearchName != null && request.SearchName != string.Empty)
        {
            locationsQuery = locationsQuery.Where(x => (x.Name != null && x.Name.Contains(request.SearchName))
                || x.Address1.Contains(request.SearchName)
                || (x.Address2 != null && x.Address2.Contains(request.SearchName))
                || x.City.Contains(request.SearchName)
                || x.PostCode.Contains(request.SearchName));
        }
        if (request.IsFamilyHub)
        {
            locationsQuery = locationsQuery.Where(x => x.LocationType == Shared.Enums.LocationType.FamilyHub);
        }
        if (request.IsNonFamilyHub)
        {
            locationsQuery = locationsQuery.Where(x => x.LocationType != Shared.Enums.LocationType.FamilyHub);
        }

        return locationsQuery;
    }

    private IQueryable<Location> OrderBy(ListLocationsCommand request, IQueryable<Location> locationsQuery)
    {
        switch (request.OrderByColumn)
        {
            case "Location":
                {
                    if (request.IsAscending)
                    {
                        locationsQuery = locationsQuery.OrderBy(x => x.Name).ThenBy(x => x.Address1)
                            .ThenBy(x => x.Address2).ThenBy(x => x.City).ThenBy(x => x.PostCode);
                    }
                    else
                    {
                        locationsQuery = locationsQuery.OrderByDescending(x => x.Name).ThenByDescending(x => x.Address1).ThenByDescending(x => x.Address2)
                            .ThenByDescending(x => x.City).ThenByDescending(x => x.PostCode);
                    }
                    break;
                }
            case "LocationType":
                {
                    if (request.IsAscending)
                    {
                        locationsQuery = locationsQuery.OrderBy(x => x.LocationType).ThenBy(x => x.Name).ThenBy(x => x.Address1).ThenBy(x => x.Address2)
                            .ThenBy(x => x.City).ThenBy(x => x.PostCode);
                    }
                    else
                    {
                        locationsQuery = locationsQuery.OrderByDescending(x => x.LocationType).ThenBy(x => x.Name).ThenByDescending(x => x.Address1).ThenByDescending(x => x.Address2)
                            .ThenByDescending(x => x.City).ThenByDescending(x => x.PostCode);
                    }
                    break;
                }
        }

        return locationsQuery;
    }
}
