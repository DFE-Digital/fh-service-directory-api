using AutoMapper;
using FamilyHubs.ServiceDirectory.Api.Commands.CreateService;
using FamilyHubs.ServiceDirectory.Api.Commands.UpdateService;
using FamilyHubs.ServiceDirectory.Core.Entities;
using FamilyHubs.ServiceDirectory.Infrastructure.Persistence.Repository;
using FamilyHubs.ServiceDirectory.Shared.Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FamilyHubs.ServiceDirectory.Api.Commands.CreateOrganisation;

public class CreateOrganisationCommand : IRequest<string>
{
    public CreateOrganisationCommand(OrganisationWithServicesDto organisation)
    {
        Organisation = organisation;
    }

    public OrganisationWithServicesDto Organisation { get; }
}

public class CreateOrganisationCommandHandler : IRequestHandler<CreateOrganisationCommand, string>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ISender _mediator;
    private readonly ILogger<CreateOrganisationCommandHandler> _logger;

    public CreateOrganisationCommandHandler(ApplicationDbContext context, IMapper mapper, ISender mediator, ILogger<CreateOrganisationCommandHandler> logger)
    {
        _context = context;
        _mapper = mapper;
        _mediator = mediator;
        _logger = logger;
    }

    public async Task<string> Handle(CreateOrganisationCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var entity = _mapper.Map<Organisation>(request.Organisation);

            ArgumentNullException.ThrowIfNull(entity);

            if (_context.Organisations.FirstOrDefault(x => x.Id == request.Organisation.Id) is not null)
            {
                throw new ArgumentException("Duplicate Id");
            }

            var organisationType = _context.OrganisationTypes.FirstOrDefault(x => x.Id == request.Organisation.OrganisationType.Id);
            if (organisationType is not null)
            {
                entity.OrganisationType = organisationType;
            }

            entity.Services?.Clear();

            AddAdministrativeDistrict(request, entity);

            AddRelatedOrganisation(request, entity);

            _context.Organisations.Add(entity);

            await _context.SaveChangesAsync(cancellationToken);
            
            //Organisation needs to be saved first before handling service otherwise RDBMS will throw foreign key violation exception
            //NOTE: this error only occurs in RDBMS, EF core InMemory provider does not enforces FK constrains
            //TODO: Use SQLLite DB provider for tests so that this kind of errors are caught at dev stage 
            if (request.Organisation.Services is not null)
            {
                // Update and Insert children
                foreach (var childModel in request.Organisation.Services)
                {
                    var existing = await _context.Services.AnyAsync(c => c.Id == childModel.Id, cancellationToken);

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
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred creating organisation. {exceptionMessage}", ex.Message);
            throw;
        }

        return request.Organisation.Id;
    }

    private void AddAdministrativeDistrict(CreateOrganisationCommand request, Organisation organisation)
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
        }
    }

    private void AddRelatedOrganisation(CreateOrganisationCommand request, Organisation organisation)
    {
        if (string.IsNullOrEmpty(request.Organisation.AdminAreaCode) || string.Compare(request.Organisation.OrganisationType.Name, "LA", StringComparison.OrdinalIgnoreCase) == 0)
            return;

        var result = (from adminArea in _context.AdminAreas
                      join org in _context.Organisations
                           on adminArea.OrganisationId equals org.Id
                      where adminArea.Code == request.Organisation.AdminAreaCode
                      && org.OrganisationType.Name == "LA"
                      select org).FirstOrDefault();

        if (result == null)
        {
            _logger.LogError($"Unable to find Local Authority for: {request.Organisation.AdminAreaCode}");
            return;
        }

        var entity = new RelatedOrganisation(Guid.NewGuid().ToString(), result.Id, organisation.Id);

        _context.RelatedOrganisations.Add(entity);
    }
}
