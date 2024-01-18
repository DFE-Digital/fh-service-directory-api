using Ardalis.GuardClauses;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using FamilyHubs.ServiceDirectory.Data.Entities;
using FamilyHubs.ServiceDirectory.Data.Repository;
using FamilyHubs.ServiceDirectory.Shared.Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FamilyHubs.ServiceDirectory.Core.Queries.Services.GetServiceById;

public class GetServiceByIdCommand : IRequest<ServiceDto>
{
    public required long Id { get; set; }
    public required bool IsSimple { get; set; }
}

public class GetServiceByIdCommandHandler : IRequestHandler<GetServiceByIdCommand, ServiceDto>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetServiceByIdCommandHandler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ServiceDto> Handle(GetServiceByIdCommand request, CancellationToken cancellationToken)
    {
        ServiceDto? entity = default!;
        if (request.IsSimple)
        {
            entity = await _context.Services
           .Include(x => x.Taxonomies)
           .Include(x => x.Locations)
           .ThenInclude(x => x.Contacts)
           .AsSplitQuery()
           .AsNoTracking()

           .ProjectTo<ServiceDto>(_mapper.ConfigurationProvider)

           .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);
        }
        else
        {
            entity = await _context.Services
           .Include(x => x.Taxonomies)

           .Include(x => x.Locations)
           .ThenInclude(x => x.Contacts)

           .Include(x => x.Locations)
           .ThenInclude(x => x.Schedules)

           .AsSplitQuery()
           .AsNoTracking()

           .ProjectTo<ServiceDto>(_mapper.ConfigurationProvider)

           .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        }


        if (entity is null)
            throw new NotFoundException(nameof(Service), request.Id.ToString());

        return entity;
    }
}

