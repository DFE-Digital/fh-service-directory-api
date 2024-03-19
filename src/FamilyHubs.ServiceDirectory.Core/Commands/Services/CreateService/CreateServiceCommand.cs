using Ardalis.GuardClauses;
using AutoMapper;
using Azure.Core;
using FamilyHubs.ServiceDirectory.Core.Helper;
using FamilyHubs.ServiceDirectory.Data.Entities;
using FamilyHubs.ServiceDirectory.Data.Entities.ManyToMany;
using FamilyHubs.ServiceDirectory.Data.Repository;
using FamilyHubs.ServiceDirectory.Shared.CreateUpdateDto;
using MediatR;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Logging;

namespace FamilyHubs.ServiceDirectory.Core.Commands.Services.CreateService;

public class CreateServiceCommand : IRequest<long>
{
    public CreateServiceCommand(ServiceChangeDto service)
    {
        Service = service;
    }

    public ServiceChangeDto Service { get; }
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
            var service = _mapper.Map<Service>(request.Service);

            // service in serviceatlocation needs to point to new service. how do we do that? set Service object?
            //UpdateServiceAtLocations(service);
            //service.Locations = await request.Service.ServiceAtLocations.GetEntities(_context.Locations);
            service.Taxonomies = await request.Service.TaxonomyIds.GetEntities(_context.Taxonomies);

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

    //private async Task AddServiceAtLocations(ServiceChangeDto serviceChange, Service service)
    private void UpdateServiceAtLocations(Service service)
    {
        foreach (var serviceAtLocation in service.ServiceAtLocations)
        {
            serviceAtLocation.ServiceId = service.Id;
        }
        //foreach (var serviceAtLocation in serviceChange.ServiceAtLocations)
        //{
        //    // need to do this?
        //    var location = await _context.Locations.FindAsync(serviceAtLocation.LocationId);
        //    if (location == null)
        //    {
        //        throw new NotFoundException(serviceAtLocation.LocationId.ToString(), nameof(Location));
        //    }

        //    var schedules = _mapper.Map<IList<Schedule>>(serviceAtLocation.Schedules);

        //    service.ServiceAtLocations.Add(new ServiceAtLocation
        //    {
        //        Location = location,
        //        Service = service,
        //        Description = serviceAtLocation.Description,
        //        Schedules = schedules
        //    });
        //}
    }
}
