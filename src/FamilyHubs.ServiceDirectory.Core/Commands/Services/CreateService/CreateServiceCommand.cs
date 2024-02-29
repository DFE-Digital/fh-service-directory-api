using AutoMapper;
using FamilyHubs.ServiceDirectory.Core.Helper;
using FamilyHubs.ServiceDirectory.Data.Entities;
using FamilyHubs.ServiceDirectory.Data.Repository;
using FamilyHubs.ServiceDirectory.Shared.Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FamilyHubs.ServiceDirectory.Core.Commands.Services.CreateService;

public class CreateServiceCommand : IRequest<long>
{
    public CreateServiceCommand(ServiceDto service)
    {
        Service = service;
    }

    public ServiceDto Service { get; }
}

public class CreateServiceCommandHandler : IRequestHandler<CreateServiceCommand, long>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateServiceCommandHandler> _logger;

    public CreateServiceCommandHandler(ApplicationDbContext context, IMapper mapper,
        ILogger<CreateServiceCommandHandler> logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<long> Handle(CreateServiceCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var entity = await _context.Services
                .IgnoreAutoIncludes()
                .FirstOrDefaultAsync(x => x.Id == request.Service.Id, cancellationToken);

            if (entity is not null)
                throw new ArgumentException("Duplicate Id");

            var service = _mapper.Map<Service>(request.Service);

            //todo: need to do the same here as per update organisation, i.e. attach all existing entities
            // can probably share common code

            List<Location> newLocs = new();
            foreach (var location in service.Locations)
            {
                //or IsKeySet
                if (location.Id != 0)
                {
                    var existingLocation = _context.Locations.Find(location.Id);
                    _mapper.Map(location, existingLocation);
                    newLocs.Add(existingLocation);
                }
                else
                {
                    newLocs.Add(location);
                }
            }
            service.Locations = newLocs;

            //foreach (var location in service.Locations)
            //{
            //    var existingLocation = _context.Locations.Local
            //        .FirstOrDefault(entry => entry.Id.Equals(location.Id));

            //    if (existingLocation != null)
            //    {
            //        // The entity is already being tracked
            //        //todo: if this works, need up update the existing with the new values
            //        _context.Entry(existingLocation).State = EntityState.Unchanged;
            //    }
            //    else
            //    {
            //        // The entity is not being tracked, so attach it
            //        _context.Locations.Attach(location);
            //    }
            //}

            //var distinctExistingLocations = service.Locations
            //    .Where(l => l.Id != 0)
            //    .GroupBy(l => l.Id)
            //    .Select(g => g.First()) // todo: throw if more than one and they aren't the same
            //    .ToArray();

            //foreach (var location in distinctExistingLocations)
            //{
            //    location.AttachExisting(_context, _context.Locations, _mapper);
            //}

            service.AttachExistingManyToMany(_context, _mapper);

            _context.Services.Add(service);

            await _context.SaveChangesAsync(cancellationToken);

            return service.Id;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred creating Service with Id:{ServiceRef}.", request.Service.ServiceOwnerReferenceId);
            throw;
        }
    }
}