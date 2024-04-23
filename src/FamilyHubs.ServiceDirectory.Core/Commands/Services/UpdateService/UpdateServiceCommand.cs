using Ardalis.GuardClauses;
using AutoMapper;
using FamilyHubs.ServiceDirectory.Core.Helper;
using FamilyHubs.ServiceDirectory.Data.Entities;
using FamilyHubs.ServiceDirectory.Data.Repository;
using FamilyHubs.ServiceDirectory.Shared.CreateUpdateDto;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FamilyHubs.ServiceDirectory.Core.Commands.Services.UpdateService;

//todo: when adding/deleting locations (service at locations), ef complains about the schedule being referenced by the location
//todo: when editing a service (to add a location), ended up with a duplicate service with a different id
//todo: set whether ef logs in the config

public class UpdateServiceCommand : IRequest<long>
{
    public UpdateServiceCommand(long id, ServiceChangeDto service)
    {
        Id = id;
        Service = service;
    }

    public ServiceChangeDto Service { get; }

    public long Id { get; }
}

public class UpdateServiceCommandHandler : IRequestHandler<UpdateServiceCommand, long>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public UpdateServiceCommandHandler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    //todo: check updated/modified by and modified date (handled by AuditableEntitySaveChangesInterceptor)
    //todo: still getting the reference issue on delete
    //todo: creating a new service, instead of updating the existing one
    //todo: the schedule off service at location is going missing

    public async Task<long> Handle(UpdateServiceCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        //Many to Many needs to be included otherwise EF core does not know how to perform merge on navigation tables
        var service = await _context.Services
            .Include(s => s.Taxonomies)
            .Include(s => s.Locations)
            .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        if (service is null)
            throw new NotFoundException(nameof(Service), request.Id.ToString());

        service = _mapper.Map(request.Service, service);

        foreach (var serviceAtLocation in service.ServiceAtLocations)
        {
            serviceAtLocation.ServiceId = service.Id;
        }

        service.Taxonomies = await request.Service.TaxonomyIds.GetEntities(_context.Taxonomies);

        _context.Services.Update(service);

        await _context.SaveChangesAsync(cancellationToken);

        // ensure that schedules (which can be referenced by location, service and serviceatlocations) are deleted when they're no longer referenced
        // we need to do this, as we can't specify cascade delete on the ServiceAtLocation schedules relationship as it would cause a cyclic reference
        var schedulesToRemove = _context.Schedules
            .Where(s => s.ServiceId == null && s.LocationId == null && s.ServiceAtLocationId == null)
            .ToList();

        _context.Schedules.RemoveRange(schedulesToRemove);
        await _context.SaveChangesAsync(cancellationToken);

        return service.Id;
    }
}