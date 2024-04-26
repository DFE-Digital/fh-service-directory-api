using AutoMapper;
using AutoMapper.QueryableExtensions;
using FamilyHubs.ServiceDirectory.Core.Helper;
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
    public string OrderByColumn { get; }

    public ListLocationsCommand(int? pageNumber, string? orderByColumn, int? pageSize, bool? isAscending, string? searchName, bool? isFamilyHub)
    {
        PageNumber = pageNumber ?? 1;
        OrderByColumn = orderByColumn ?? "Location";
        PageSize = pageSize ?? 10;
        IsAscending = isAscending ?? true;
        SearchName = searchName;
        IsFamilyHub = isFamilyHub ?? false;
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
        if (!string.IsNullOrEmpty(request.SearchName))
        {
            locationsQuery = locationsQuery.Where(
                x => (x.Name != null && x.Name.Contains(request.SearchName))
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
            locationsQuery = locationsQuery.Where(x => x.LocationTypeCategory == Shared.Enums.LocationTypeCategory.FamilyHub);
        }

        return locationsQuery;
    }

    private IQueryable<Location> OrderBy(ListLocationsCommand request, IQueryable<Location> locationsQuery)
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
