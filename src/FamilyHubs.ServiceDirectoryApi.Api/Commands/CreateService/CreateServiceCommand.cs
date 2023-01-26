using AutoMapper;
using FamilyHubs.ServiceDirectory.Core.Entities;
using FamilyHubs.ServiceDirectory.Core.Events;
using FamilyHubs.ServiceDirectory.Infrastructure.Persistence.Repository;
using FamilyHubs.ServiceDirectory.Shared.Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FamilyHubs.ServiceDirectory.Api.Commands.CreateService;

public class CreateServiceCommand : IRequest<string>
{
    public CreateServiceCommand(ServiceDto service)
    {
        Service = service;
    }

    public ServiceDto Service { get; init; }
}

public class CreateServiceCommandHandler : IRequestHandler<CreateServiceCommand, string>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateServiceCommandHandler> _logger;

    public CreateServiceCommandHandler(ApplicationDbContext context, IMapper mapper, ILogger<CreateServiceCommandHandler> logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<string> Handle(CreateServiceCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var entity = _mapper.Map<Service>(request.Service);
            ArgumentNullException.ThrowIfNull(entity, nameof(entity));

            var serviceType = _context.ServiceTypes.FirstOrDefault(x => x.Id == request.Service.ServiceType.Id);
            if (serviceType != null)
                entity.ServiceType = serviceType;

            foreach (var serviceTaxonomy in entity.ServiceTaxonomies)
            {
                if (serviceTaxonomy.Taxonomy != null)
                {
                    var taxonomy = _context.Taxonomies.FirstOrDefault(x => x.Id == serviceTaxonomy.Taxonomy.Id);
                    if (taxonomy != null)
                    {
                        serviceTaxonomy.Taxonomy = taxonomy;
                    }
                }
            }

            foreach (var serviceAtLocation in entity.ServiceAtLocations)
            {
                if (serviceAtLocation.RegularSchedules != null)
                {
                    foreach (var regularSchedules in serviceAtLocation.RegularSchedules)
                    {
                        regularSchedules.ServiceAtLocationId = serviceAtLocation.Id;
                    }
                }

                if (serviceAtLocation.HolidaySchedules != null)
                {
                    foreach (var holidaySchedules in serviceAtLocation.HolidaySchedules)
                    {
                        holidaySchedules.ServiceAtLocationId = serviceAtLocation.Id;
                    }
                }

                var existingLocation = await _context.Locations
                    .Include(l => l.PhysicalAddresses)
                    .Include(l => l.LinkTaxonomies)!
                    .ThenInclude(l => l.Taxonomy)
                    .Where(l => l.Name == serviceAtLocation.Location.Name && serviceAtLocation.Location.Name!="")
                    .FirstOrDefaultAsync(cancellationToken);

                if (existingLocation != null)
                {
                    if (serviceAtLocation.Location.PhysicalAddresses != null)
                    {
                        foreach (var newAddresses in serviceAtLocation.Location.PhysicalAddresses)
                        {
                            existingLocation.PhysicalAddresses ??= new List<PhysicalAddress>();
                            if (existingLocation.PhysicalAddresses.All(a => a.PostCode != newAddresses.PostCode))
                            {
                                existingLocation.PhysicalAddresses.Add(newAddresses);
                            }
                        }
                    }
                    serviceAtLocation.Location = existingLocation;
                }
                else if (serviceAtLocation.Location.LinkTaxonomies != null)
                {
                    foreach (var linkTaxonomy in serviceAtLocation.Location.LinkTaxonomies)
                    {
                        if (linkTaxonomy.Taxonomy != null)
                        {
                            var taxonomy = _context.Taxonomies.FirstOrDefault(x => x.Id == linkTaxonomy.Taxonomy.Id);
                            if (taxonomy != null)
                            {
                                linkTaxonomy.Taxonomy = taxonomy;
                            }
                        }
                    }
                }
            }

            entity.RegisterDomainEvent(new ServiceCreatedEvent(entity));

            _context.Services.Add(entity);

            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred creating taxonomy. {exceptionMessage}", ex.Message);
            throw new Exception(ex.Message, ex);
        }

        return request.Service.Id;
    }
}
