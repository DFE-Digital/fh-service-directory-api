using Ardalis.GuardClauses;
using AutoMapper;
using FamilyHubs.ServiceDirectory.Api.Commands.CreateService;
using FamilyHubs.ServiceDirectory.Api.Commands.UpdateService;
using FamilyHubs.ServiceDirectory.Core.Entities;
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

    public OrganisationWithServicesDto Organisation { get; }

    public string Id { get; }
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
        ArgumentNullException.ThrowIfNull(request);

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
                    var existing = entity.Services.Any(c => c.Id == childModel.Id);

                    if (existing)
                    {
                        var updateServiceCommand = new UpdateServiceCommand(childModel.Id, childModel);
                        await _mediator.Send(updateServiceCommand, cancellationToken);
                    }
                    else
                    {
                        var createServiceCommand = new CreateServiceCommand(childModel);
                        await _mediator.Send(createServiceCommand, cancellationToken);
                    }
                }
            }

            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred updating organisation. {exceptionMessage}", ex.Message);
            throw;
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