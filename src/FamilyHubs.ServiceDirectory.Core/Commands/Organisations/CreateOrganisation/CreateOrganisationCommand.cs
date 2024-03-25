using AutoMapper;
using FamilyHubs.ServiceDirectory.Core.Exceptions;
using FamilyHubs.ServiceDirectory.Data.Entities;
using FamilyHubs.ServiceDirectory.Data.Repository;
using FamilyHubs.ServiceDirectory.Shared.Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FamilyHubs.ServiceDirectory.Core.Commands.Organisations.CreateOrganisation;

public class CreateOrganisationCommand : IRequest<long>
{
    public CreateOrganisationCommand(OrganisationDto organisation)
    {
        Organisation = organisation;
    }

    public OrganisationDto Organisation { get; }
}

public class CreateOrganisationCommandHandler : IRequestHandler<CreateOrganisationCommand, long>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateOrganisationCommandHandler> _logger;

    public CreateOrganisationCommandHandler(
        ApplicationDbContext context,
        IMapper mapper,
        ILogger<CreateOrganisationCommandHandler> logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<long> Handle(CreateOrganisationCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (request.Organisation.Id != 0)
            {
                throw new AlreadyExistsException("Organisation Id must be 0 to create an organisation");

            }
            await ThrowIfOrganisationNameExists(request, cancellationToken);

            if (request.Organisation.AssociatedOrganisationId is not null)
            {
                var associatedOrganisation =
                    _context.Organisations.SingleOrDefault(o => o.Id == request.Organisation.AssociatedOrganisationId);
                if (associatedOrganisation is null)
                    throw new InvalidOperationException("Invalid Associated Organisation ID");
                request.Organisation.AdminAreaCode = associatedOrganisation.AdminAreaCode;
            }

            var organisation = _mapper.Map<Organisation>(request.Organisation);

            _context.Organisations.Add(organisation);

            await _context.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Organisation {Name} saved to DB", request.Organisation.Name);

            return organisation.Id;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred creating organisation with Name:{name}.", request.Organisation.Name);
            throw;
        }
    }

    private async Task ThrowIfOrganisationNameExists(CreateOrganisationCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Organisations
                .IgnoreAutoIncludes()
                .FirstOrDefaultAsync(x => x.Name == request.Organisation.Name && x.AssociatedOrganisationId == request.Organisation.AssociatedOrganisationId, cancellationToken);

        if (entity is not null)
            throw new AlreadyExistsException("Cannot create an organisation with a name that matches an existing organisation"); 
    }
}