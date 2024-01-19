using Ardalis.GuardClauses;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using FamilyHubs.ServiceDirectory.Data.Entities;
using FamilyHubs.ServiceDirectory.Data.Repository;
using FamilyHubs.ServiceDirectory.Shared.Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FamilyHubs.ServiceDirectory.Core.Queries.Services.GetServiceByOwnerReferenceIdCommand;

public class GetServiceByOwnerReferenceIdCommand : IRequest<ServiceDto>
{
    public required string OwnerReferenceId { get; set; }
}

public class GetServiceByOwnerReferenceIdCommandHandler : IRequestHandler<GetServiceByOwnerReferenceIdCommand, ServiceDto>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetServiceByOwnerReferenceIdCommandHandler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ServiceDto> Handle(GetServiceByOwnerReferenceIdCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Services
            .Include(x => x.Taxonomies)

            .Include(x => x.Locations)
            .ThenInclude(x => x.Contacts)

            .Include(x => x.Locations)
            .ThenInclude(x => x.Schedules)

            .AsSplitQuery()
            .AsNoTracking()

            .ProjectTo<ServiceDto>(_mapper.ConfigurationProvider)

            .FirstOrDefaultAsync(p => p.ServiceOwnerReferenceId == request.OwnerReferenceId, cancellationToken);

        if (entity is null)
            throw new NotFoundException(nameof(Service), request.OwnerReferenceId);

        return entity;
    }
}

