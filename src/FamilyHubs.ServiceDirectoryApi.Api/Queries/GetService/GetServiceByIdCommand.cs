using Ardalis.GuardClauses;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using FamilyHubs.ServiceDirectory.Core.Entities;
using FamilyHubs.ServiceDirectory.Infrastructure.Persistence.Repository;
using FamilyHubs.ServiceDirectory.Shared.Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FamilyHubs.ServiceDirectory.Api.Queries.GetService;

public class GetServiceByIdCommand : IRequest<ServiceDto>
{
    public GetServiceByIdCommand(long id)
    {
        Id = id;
    }

    public long Id { get; }
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
        var entity = await _context.Services
            .Include(x => x.ServiceDeliveries)
            .Include(x => x.Eligibilities)
            .Include(x => x.CostOptions)
            .Include(x => x.Fundings)
            .Include(x => x.Languages)
            .Include(x => x.ServiceAreas)
            .Include(x => x.RegularSchedules)
            .Include(x => x.HolidaySchedules)
            .Include(x => x.Contacts)
            .Include(x => x.Taxonomies)
            .Include(x => x.Locations)
            .ThenInclude(x => x.Contacts)
            .Include(x => x.Locations)
            .ThenInclude(x => x.RegularSchedules)
            .Include(x => x.Locations)
            .ThenInclude(x => x.HolidaySchedules)
            .AsSplitQuery()
            .ProjectTo<ServiceDto>(_mapper.ConfigurationProvider)
            
            .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);
        
        if (entity is null)
            throw new NotFoundException(nameof(Service), request.Id.ToString());
        
        return entity;
    }
}

