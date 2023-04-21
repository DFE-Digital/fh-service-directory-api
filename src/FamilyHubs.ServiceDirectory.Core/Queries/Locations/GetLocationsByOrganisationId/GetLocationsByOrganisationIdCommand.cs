using Ardalis.GuardClauses;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using FamilyHubs.ServiceDirectory.Data.Entities;
using FamilyHubs.ServiceDirectory.Data.Repository;
using FamilyHubs.ServiceDirectory.Shared.Dto;
using FamilyHubs.ServiceDirectory.Shared.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FamilyHubs.ServiceDirectory.Core.Queries.Locations.GetLocationsByOrganisationId;

public class GetLocationsByOrganisationIdCommand : IRequest<List<LocationDto>>
{
    public required long OrganisationId { get; set; }
}

public class GetLocationsByOrganisationIdCommandHandler : IRequestHandler<GetLocationsByOrganisationIdCommand, List<LocationDto>>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetLocationsByOrganisationIdCommandHandler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<LocationDto>> Handle(GetLocationsByOrganisationIdCommand request, CancellationToken cancellationToken)
    {
        var locations = await _context.Services

            .Include(x => x.Locations)
            .ThenInclude(x => x.Contacts)

            .Include(x => x.Locations)
            .ThenInclude(x => x.HolidaySchedules)

            .Include(x => x.Locations)
            .ThenInclude(x => x.RegularSchedules)

            .Where(s => s.Status != ServiceStatusType.Deleted)
            .Where(x => x.OrganisationId == request.OrganisationId)

            .SelectMany(s => s.Locations)

            .AsSplitQuery()
            .AsNoTracking()
            .ProjectTo<LocationDto>(_mapper.ConfigurationProvider)

            .ToListAsync(cancellationToken);

        if (!locations.Any())
            throw new NotFoundException(nameof(Location), request.OrganisationId.ToString());

        return locations;
    }
}