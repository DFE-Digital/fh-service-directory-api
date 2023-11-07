using AutoMapper;
using AutoMapper.QueryableExtensions;
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
    public bool? IsAscending { get; }
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
        var locations = await _context.Locations
            .AsNoTracking()
            .ProjectTo<LocationDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        locations = OrderBy(request, locations);

        var pageList = locations.Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize).ToList();

        return new PaginatedList<LocationDto>(pageList, locations.Count, request.PageNumber, request.PageSize);
    }

    private static List<LocationDto> OrderBy(ListLocationsCommand request, List<LocationDto> locations)
    {
        switch (request.OrderByColumn)
        {
            case "Location":
                {
                    if (request.IsAscending ?? false)
                    {
                        locations = locations.OrderBy(x => x.Name).ThenBy(x => x.Address1).ThenBy(x => x.Address2)
                            .ThenBy(x => x.City).ThenBy(x => x.PostCode).ToList();
                    }
                    else
                    {
                        locations = locations.OrderByDescending(x => x.Name).ThenByDescending(x => x.Address1).ThenByDescending(x => x.Address2)
                            .ThenByDescending(x => x.City).ThenByDescending(x => x.PostCode).ToList();
                    }
                    break;
                }
            case "LocationType":
                {
                    if (request.IsAscending ?? false)
                    {
                        locations = locations.OrderBy(x => x.LocationType).ThenBy(x => x.Name).ThenBy(x => x.Address1).ThenBy(x => x.Address2)
                            .ThenBy(x => x.City).ThenBy(x => x.PostCode).ToList();
                    }
                    else
                    {
                        locations = locations.OrderByDescending(x => x.LocationType).ThenBy(x => x.Name).ThenByDescending(x => x.Address1).ThenByDescending(x => x.Address2)
                            .ThenByDescending(x => x.City).ThenByDescending(x => x.PostCode).ToList();
                    }
                    break;
                }
        }


        return locations;
    }
}
