using AutoMapper;
using AutoMapper.QueryableExtensions;
using FamilyHubs.ServiceDirectory.Data.Repository;
using FamilyHubs.ServiceDirectory.Shared.Dto;
using FamilyHubs.ServiceDirectory.Shared.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FamilyHubs.ServiceDirectory.Core.Queries.Locations.ListLocations;

public class ListLocationsCommand : IRequest<List<LocationDto>>
{
}

public class ListLocationCommandHandler : IRequestHandler<ListLocationsCommand, List<LocationDto>>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public ListLocationCommandHandler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<LocationDto>> Handle(ListLocationsCommand _, CancellationToken cancellationToken)
    {
        var locations = await _context.Services

            .Include(x => x.Locations)
            .ThenInclude(x => x.Contacts)

            .Include(x => x.Locations)
            .ThenInclude(x => x.HolidaySchedules)

            .Include(x => x.Locations)
            .ThenInclude(x => x.RegularSchedules)

            .Where(s => s.Status != ServiceStatusType.Deleted)

            .SelectMany(s => s.Locations)

            .AsSplitQuery()
            .AsNoTracking()

            .ProjectTo<LocationDto>(_mapper.ConfigurationProvider)

            .ToListAsync(cancellationToken);

        return locations;
    }
}
