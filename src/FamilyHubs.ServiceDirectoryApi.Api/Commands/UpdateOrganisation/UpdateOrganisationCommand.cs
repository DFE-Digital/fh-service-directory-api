using Ardalis.GuardClauses;
using AutoMapper;
using FamilyHubs.ServiceDirectory.Api.Commands.UpdateService;
using FamilyHubs.ServiceDirectory.Core.Entities;
using FamilyHubs.ServiceDirectory.Core.Events;
using FamilyHubs.ServiceDirectory.Infrastructure.Persistence.Repository;
using FamilyHubs.ServiceDirectory.Shared.Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FamilyHubs.ServiceDirectory.Api.Commands.UpdateOrganisation;

public class UpdateOrganisationCommand : IRequest<string>
{
    public UpdateOrganisationCommand(string id, OrganisationWithServicesDto organisation)
    {
        Id = id;
        Organisation = organisation;
    }

    public OrganisationWithServicesDto Organisation { get; init; }

    public string Id { get; set; }
}

public class UpdateOrganisationCommandHandler : IRequestHandler<UpdateOrganisationCommand, string>
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<UpdateOrganisationCommandHandler> _logger;
    private readonly ISender _mediator;
    private readonly IMapper _mapper;

    public UpdateOrganisationCommandHandler(ApplicationDbContext context, ILogger<UpdateOrganisationCommandHandler> logger, ISender mediator, IMapper mapper)
    {
        _context = context;
        _logger = logger;
        _mediator = mediator;
        _mapper = mapper;
    }

    public async Task<string> Handle(UpdateOrganisationCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));

        var entity = await _context.Organisations
          .Include(x => x.OrganisationType)
          .Include(x => x.Services!)
          .SingleOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Organisation), request.Id);
        }

        try
        {

            var org = _mapper.Map<Organisation>(request.Organisation);
            var organisationType = _context.OrganisationTypes.FirstOrDefault(x => x.Id == request.Organisation.OrganisationType.Id);
            if (organisationType != null)
            {
                org.OrganisationType = organisationType;
            }

            entity.Update(org);
            AddOrUpdateAdministrativeDistrict(request, entity);

            if (entity.Services != null && request.Organisation.Services != null)
            {
                // Update and Insert children
                foreach (var childModel in request.Organisation.Services)
                {
                    var existingChild = entity.Services
                        .SingleOrDefault(c => c.Id == childModel.Id);

                    if (existingChild != null)
                    {
                        UpdateServiceCommand updateServiceCommand = new(existingChild.Id, childModel);
                        await _mediator.Send(updateServiceCommand, cancellationToken);
                    }
                    else
                    {
                        var service = _mapper.Map<Service>(childModel);

                        var serviceType = _context.ServiceTypes.FirstOrDefault(x => x.Id == childModel.ServiceType.Id);
                        if (serviceType != null)
                            service.ServiceType = serviceType;

                        if (childModel.ServiceTaxonomies != null)
                        {
                            for (var i = 0; i < childModel.ServiceTaxonomies.Count; i++)
                            {
                                if (childModel.ServiceTaxonomies.ElementAt(i).Taxonomy != null)
                                {
                                    var id = childModel.ServiceTaxonomies.ElementAt(i).Taxonomy?.Id ?? string.Empty;
                                    var tx = _context.Taxonomies.FirstOrDefault(x => x.Id == id);
                                    service.ServiceTaxonomies.ElementAt(i).Taxonomy = tx;
                                }
                            }
                        }

                        foreach (var serviceAtLocation in service.ServiceAtLocations)
                        {
                            var existingLocation = await _context.Locations
                                .Include(l => l.PhysicalAddresses)
                                .Include(l => l.LinkTaxonomies)!
                                .ThenInclude(l => l.Taxonomy)
                                .Where(l => l.Name == serviceAtLocation.Location.Name)
                                .FirstOrDefaultAsync(cancellationToken);

                            if (existingLocation != null)
                            {
                                serviceAtLocation.Location = existingLocation;
                            }
                            else
                            {
                                if (serviceAtLocation.Location.LinkTaxonomies != null)
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
                        }

                        entity.RegisterDomainEvent(new ServiceCreatedEvent(service));
                        _context.Services.Add(service);
                    }
                }
            }

            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred updating organisation. {exceptionMessage}", ex.Message);
            throw new Exception(ex.Message, ex);
        }

        return entity.Id;
    }

    private void AddOrUpdateAdministrativeDistrict(UpdateOrganisationCommand request, Organisation organisation)
    {
        if (!string.IsNullOrEmpty(request.Organisation.AdminAreaCode))
        {
            var organisationAdminDistrict = _context.AdminAreas.FirstOrDefault(x => x.OrganisationId == organisation.Id);
            if (organisationAdminDistrict == null)
            {
                var entity = new AdminArea(
                    Guid.NewGuid().ToString(),
                    request.Organisation.AdminAreaCode,
                    organisation.Id);

                _context.AdminAreas.Add(entity);
            }
            else
            {
                organisationAdminDistrict.Code = request.Organisation.AdminAreaCode;
            }

        }
    }

}


