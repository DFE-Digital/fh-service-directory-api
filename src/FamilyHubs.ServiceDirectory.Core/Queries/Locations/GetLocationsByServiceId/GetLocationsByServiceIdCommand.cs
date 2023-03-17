using Ardalis.GuardClauses;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using FamilyHubs.ServiceDirectory.Data.Entities;
using FamilyHubs.ServiceDirectory.Data.Repository;
using FamilyHubs.ServiceDirectory.Shared.Dto;
using FamilyHubs.ServiceDirectory.Shared.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FamilyHubs.ServiceDirectory.Core.Queries.Locations.GetLocationsByServiceId;

public class GetLocationsByServiceIdCommand : IRequest<List<LocationDto>>
{
    public required long ServiceId { get; set; }
}

public class GetLocationsByServiceIdCommandHandler : IRequestHandler<GetLocationsByServiceIdCommand, List<LocationDto>>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetLocationsByServiceIdCommandHandler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<LocationDto>> Handle(GetLocationsByServiceIdCommand request, CancellationToken cancellationToken)
    {
        var locations = await _context.Services

            .Include(x => x.Locations)
            .ThenInclude(x => x.Contacts)

            .Include(x => x.Locations)
            .ThenInclude(x => x.HolidaySchedules)

            .Include(x => x.Locations)
            .ThenInclude(x => x.RegularSchedules)

            .Where(s => s.Status != ServiceStatusType.Deleted)
            .Where(x => x.Id == request.ServiceId)

            .SelectMany(s => s.Locations)

            .AsSplitQuery()
            .AsNoTracking()
            .ProjectTo<LocationDto>(_mapper.ConfigurationProvider)

            .ToListAsync(cancellationToken);

        if (!locations.Any())
            throw new NotFoundException(nameof(Location), request.ServiceId.ToString());

        return locations;
    }
}

