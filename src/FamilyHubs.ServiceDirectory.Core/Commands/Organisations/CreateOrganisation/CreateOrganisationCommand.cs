using AutoMapper;
using FamilyHubs.ServiceDirectory.Core.Helper;
using FamilyHubs.ServiceDirectory.Data.Entities;
using FamilyHubs.ServiceDirectory.Data.Repository;
using FamilyHubs.ServiceDirectory.Shared.Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FamilyHubs.ServiceDirectory.Core.Commands.Organisations.CreateOrganisation;

public class CreateOrganisationCommand : IRequest<long>
{
    public CreateOrganisationCommand(OrganisationWithServicesDto organisation)
    {
        Organisation = organisation;
    }

    public OrganisationWithServicesDto Organisation { get; }
}

public class CreateOrganisationCommandHandler : IRequestHandler<CreateOrganisationCommand, long>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateOrganisationCommandHandler> _logger;

    public CreateOrganisationCommandHandler(ApplicationDbContext context, IMapper mapper,
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
            var entity = await _context.Organisations
                .IgnoreAutoIncludes()
                .FirstOrDefaultAsync(x => x.Id == request.Organisation.Id, cancellationToken);

            if (entity is not null)
                throw new ArgumentException("Duplicate Id");

            if (request.Organisation.AssociatedOrganisationId is not null)
            {
                var associatedOrganisation =
                    _context.Organisations.SingleOrDefault(o => o.Id == request.Organisation.AssociatedOrganisationId);
                if (associatedOrganisation is null)
                    throw new InvalidOperationException("Invalid Associated Organisation ID");
                request.Organisation.AdminAreaCode = associatedOrganisation.AdminAreaCode;
            }

            var organisation = _mapper.Map<Organisation>(request.Organisation);

            foreach (var service in organisation.Services)
            {
                service.AttachExistingManyToMany(_context, _mapper);
            }

            _context.Organisations.Add(organisation);

            await _context.SaveChangesAsync(cancellationToken);

            return organisation.Id;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred creating organisation. {exceptionMessage}", ex.Message);
            throw;
        }
    }
}