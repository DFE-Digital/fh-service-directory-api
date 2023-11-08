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
    public string OrderByColumn { get; }

    public ListLocationsCommand(int? pageNumber, string? orderByColumn, int? pageSize, bool? isAscending)
    {
        PageNumber = pageNumber ?? 1;
        OrderByColumn = orderByColumn ?? "Location";
        PageSize = pageSize ?? 10;
        IsAscending = isAscending ?? true;
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

        locationsQuery = OrderBy(request, locationsQuery);

        var locations = await locationsQuery
            .Skip(skip)
            .Take(request.PageSize)
            .ProjectTo<LocationDto>(_mapper.ConfigurationProvider)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        int totalCount = await _context.Locations.CountAsync(cancellationToken);

        return new PaginatedList<LocationDto>(locations, totalCount, request.PageNumber, request.PageSize);
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
