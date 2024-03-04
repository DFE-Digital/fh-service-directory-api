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
            //todo: can we just check if id is null or not
            var entity = await _context.Services
                .IgnoreAutoIncludes()
                .FirstOrDefaultAsync(x => x.Id == request.Service.Id, cancellationToken);

            if (entity is not null)
                throw new ArgumentException("Duplicate Id");

            var service = _mapper.Map<Service>(request.Service);

            //todo: poc, what we really want, is to do this generically
            // do we need to do this for all entities? need tests to check
            // e.g. what happens if e.g. existing location has new contacts?
            // if so, we'll probably 
            // could we use context.ChangeTracker.TrackGraph, temporary id's, or write our own generic object graph walker?
            List<Location> newLocs = new();
            foreach (var location in service.Locations)
            {
                //or IsKeySet
                if (location.Id != 0)
                {
                    var existingLocation = await _context.Locations.FindAsync(location.Id);
                    _mapper.Map(location, existingLocation);
                    newLocs.Add(existingLocation);
                }
                else
                {
                    newLocs.Add(location);
                }
            }

            service.Locations = newLocs;

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