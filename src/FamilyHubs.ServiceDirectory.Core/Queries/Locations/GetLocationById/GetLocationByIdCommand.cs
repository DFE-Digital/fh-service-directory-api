using Ardalis.GuardClauses;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using FamilyHubs.ServiceDirectory.Data.Entities;
using FamilyHubs.ServiceDirectory.Data.Repository;
using FamilyHubs.ServiceDirectory.Shared.Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FamilyHubs.ServiceDirectory.Core.Queries.Locations.GetLocationById;

public class GetLocationByIdCommand : IRequest<LocationDto>
{
    public required long Id { get; set; }
}

public class GetLocationByIdCommandHandler : IRequestHandler<GetLocationByIdCommand, LocationDto>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetLocationByIdCommandHandler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<LocationDto> Handle(GetLocationByIdCommand request, CancellationToken cancellationToken)
    {
        var location = await _context.Locations
            .Include(x => x.Contacts)
            .Include(x => x.Schedules)

            .AsSplitQuery()
            .AsNoTracking()

            .ProjectTo<LocationDto>(_mapper.ConfigurationProvider)

            .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        if (location is null)
            throw new NotFoundException(nameof(Service), request.Id.ToString());

        return location;
    }
}

