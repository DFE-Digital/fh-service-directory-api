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

public class UpdateOrganisationCommand : IRequest<long>
{
    public UpdateOrganisationCommand(long id, OrganisationWithServicesDto organisation)
    {
        Id = id;
        Organisation = organisation;
    }

    public OrganisationWithServicesDto Organisation { get; }

    public long Id { get; }
}

public class UpdateOrganisationCommandHandler : IRequestHandler<UpdateOrganisationCommand, long>
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

    public async Task<long> Handle(UpdateOrganisationCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var entity = await _context.Organisations
          .Include(x => x.Services)
          .SingleOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        if (entity is null)
            throw new NotFoundException(nameof(Organisation), request.Id.ToString());

        try
        {
            var org = _mapper.Map<Organisation>(request.Organisation);
            
            _context.Update(org);

            if (request.Organisation.Services.Any())
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
}