using AutoMapper;
using AutoMapper.QueryableExtensions;
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

    public GetLocationsByOrganisationIdCommand(long organisationId, int? pageNumber, int? pageSize, bool? isAscending, string? orderByColumn, string? searchName)
    {
        OrganisationId = organisationId;
        PageNumber = pageNumber ?? 1;
        PageSize = pageSize ?? 10;
        IsAscending = isAscending ?? true;
        OrderByColumn = orderByColumn ?? "Location";
        SearchName = searchName;
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
        int skip = (request.PageNumber - 1) * request.PageSize;

        IQueryable<Location> locationsQuery = _context.Services
            .Where(s => s.Status != ServiceStatusType.Deleted && s.OrganisationId == request.OrganisationId)
            .Include(x => x.Locations)
            .SelectMany(s => s.Locations);

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

        if (request.SearchName != null && request.SearchName != string.Empty)
        {
            locationsQuery = locationsQuery.Where(x => (x.Name != null && x.Name.Contains(request.SearchName))
                || x.Address1.Contains(request.SearchName)
                || (x.Address2 != null && x.Address2.Contains(request.SearchName))
                || x.City.Contains(request.SearchName)
                || x.PostCode.Contains(request.SearchName));
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
                {
                    if (request.IsAscending)
                    {
                        locationsQuery = locationsQuery.OrderBy(x => x.Name).ThenBy(x => x.Address1).ThenBy(x => x.Address2)
                            .ThenBy(x => x.City).ThenBy(x => x.PostCode);
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