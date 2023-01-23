using Ardalis.GuardClauses;
using AutoMapper;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralOrganisations;
using fh_service_directory_api.api.Commands.UpdateOpenReferralService;
using fh_service_directory_api.core.Entities;
using fh_service_directory_api.core.Events;
using fh_service_directory_api.infrastructure.Persistence.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace fh_service_directory_api.api.Commands.UpdateOpenReferralOrganisation;

public class UpdateOpenReferralOrganisationCommand : IRequest<string>
{
    public UpdateOpenReferralOrganisationCommand(string id, OpenReferralOrganisationWithServicesDto openReferralOrganisation)
    {
        Id = id;
        OpenReferralOrganisation = openReferralOrganisation;
    }

    public OpenReferralOrganisationWithServicesDto OpenReferralOrganisation { get; init; }

    public string Id { get; set; }
}

public class UpdateOpenReferralOrganisationCommandHandler : IRequestHandler<UpdateOpenReferralOrganisationCommand, string>
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<UpdateOpenReferralOrganisationCommandHandler> _logger;
    private readonly ISender _mediator;
    private readonly IMapper _mapper;

    public UpdateOpenReferralOrganisationCommandHandler(ApplicationDbContext context, ILogger<UpdateOpenReferralOrganisationCommandHandler> logger, ISender mediator, IMapper mapper)
    {
        _context = context;
        _logger = logger;
        _mediator = mediator;
        _mapper = mapper;
    }

    public async Task<string> Handle(UpdateOpenReferralOrganisationCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));

        var entity = await _context.OpenReferralOrganisations
          .Include(x => x.OrganisationType)
          .Include(x => x.Services!)
          .SingleOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(OpenReferralOrganisation), request.Id);
        }

        try
        {

            var org = _mapper.Map<OpenReferralOrganisation>(request.OpenReferralOrganisation);
            var organisationType = _context.OrganisationTypes.FirstOrDefault(x => x.Id == request.OpenReferralOrganisation.OrganisationType.Id);
            if (organisationType != null)
            {
                org.OrganisationType = organisationType;
            }

            entity.Update(org);
            AddOrUpdateAdministrativeDistrict(request, entity);

            if (entity.Services != null && request.OpenReferralOrganisation.Services != null)
            {
                // Update and Insert children
                foreach (var childModel in request.OpenReferralOrganisation.Services)
                {
                    var existingChild = entity.Services
                        .SingleOrDefault(c => c.Id == childModel.Id);

                    if (existingChild != null)
                    {
                        UpdateOpenReferralServiceCommand updateOpenReferralServiceCommand = new(existingChild.Id, childModel);
                        await _mediator.Send(updateOpenReferralServiceCommand, cancellationToken);
                    }
                    else
                    {
                        var service = _mapper.Map<OpenReferralService>(childModel);

                        var serviceType = _context.ServiceTypes.FirstOrDefault(x => x.Id == childModel.ServiceType.Id);
                        if (serviceType != null)
                            service.ServiceType = serviceType;

                        if (childModel.Service_taxonomys != null)
                        {
                            for (var i = 0; i < childModel.Service_taxonomys.Count; i++)
                            {
                                if (childModel.Service_taxonomys.ElementAt(i).Taxonomy != null)
                                {
                                    var id = childModel.Service_taxonomys.ElementAt(i).Taxonomy?.Id ?? string.Empty;
                                    var tx = _context.OpenReferralTaxonomies.FirstOrDefault(x => x.Id == id);
                                    service.Service_taxonomys.ElementAt(i).Taxonomy = tx;
                                }
                            }
                        }

                        foreach (var serviceAtLocation in service.Service_at_locations)
                        {
                            var existingLocation = await _context.OpenReferralLocations
                                .Include(l => l.Physical_addresses)
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
                                            var taxonomy = _context.OpenReferralTaxonomies.FirstOrDefault(x => x.Id == linkTaxonomy.Taxonomy.Id);
                                            if (taxonomy != null)
                                            {
                                                linkTaxonomy.Taxonomy = taxonomy;
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        entity.RegisterDomainEvent(new OpenReferralServiceCreatedEvent(service));
                        _context.OpenReferralServices.Add(service);
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

    private void AddOrUpdateAdministrativeDistrict(UpdateOpenReferralOrganisationCommand request, OpenReferralOrganisation openReferralOrganisation)
    {
        if (!string.IsNullOrEmpty(request.OpenReferralOrganisation.AdminAreaCode))
        {
            var organisationAdminDistrict = _context.AdminAreas.FirstOrDefault(x => x.OpenReferralOrganisationId == openReferralOrganisation.Id);
            if (organisationAdminDistrict == null)
            {
                var entity = new AdminArea(
                    Guid.NewGuid().ToString(),
                    request.OpenReferralOrganisation.AdminAreaCode,
                    openReferralOrganisation.Id);

                _context.AdminAreas.Add(entity);
            }
            else
            {
                organisationAdminDistrict.Code = request.OpenReferralOrganisation.AdminAreaCode;
            }

        }
    }

}


