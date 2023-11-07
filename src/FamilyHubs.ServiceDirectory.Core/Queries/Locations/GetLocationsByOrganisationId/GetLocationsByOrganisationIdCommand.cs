using Ardalis.GuardClauses;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using FamilyHubs.ServiceDirectory.Data.Entities;
using FamilyHubs.ServiceDirectory.Data.Repository;
using FamilyHubs.ServiceDirectory.Shared.Dto;
using FamilyHubs.ServiceDirectory.Shared.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FamilyHubs.ServiceDirectory.Core.Queries.Locations.GetLocationsByOrganisationId;

public class GetLocationsByOrganisationIdCommand : IRequest<PaginatedList<LocationDto>>
{
    public long OrganisationId { get; }
    public int PageNumber { get; }
    public int PageSize { get; }
    public bool? IsAscending { get; }
    public string OrderByColumn { get; }

    public GetLocationsByOrganisationIdCommand(long organisationId, int? pageNumber, int? pageSize, bool? isAscending, string? orderByColumn)
    {
        OrganisationId = organisationId;
        PageNumber = pageNumber ?? 1;
        PageSize = pageSize ?? 10;
        IsAscending = isAscending ?? true;
        OrderByColumn = orderByColumn ?? "Location";
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
        var locations = await _context.Services

            .Include(x => x.Locations)
            .ThenInclude(x => x.Contacts)

            .Include(x => x.Locations)
            .ThenInclude(x => x.HolidaySchedules)

            .Include(x => x.Locations)
            .ThenInclude(x => x.RegularSchedules)

            //.Where(s => s.Status != ServiceStatusType.Deleted)
            .Where(x => x.OrganisationId == request.OrganisationId)

            .SelectMany(s => s.Locations)

            .AsSplitQuery()
            .AsNoTracking()
            .ProjectTo<LocationDto>(_mapper.ConfigurationProvider)

            .ToListAsync(cancellationToken);

        if (!locations.Any())
            throw new NotFoundException(nameof(Location), request.OrganisationId.ToString());

        locations = OrderBy(request, locations);
        var pageList = locations.Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize).ToList();

        return new PaginatedList<LocationDto>(pageList, locations.Count, request.PageNumber, request.PageSize);
    }

    private static List<LocationDto> OrderBy(GetLocationsByOrganisationIdCommand request, List<LocationDto> locations)
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