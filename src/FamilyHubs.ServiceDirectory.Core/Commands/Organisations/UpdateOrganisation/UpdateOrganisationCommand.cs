using Ardalis.GuardClauses;
using AutoMapper;
using FamilyHubs.ServiceDirectory.Core.Helper;
using FamilyHubs.ServiceDirectory.Data.Entities;
using FamilyHubs.ServiceDirectory.Data.Repository;
using FamilyHubs.ServiceDirectory.Shared.Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FamilyHubs.ServiceDirectory.Core.Commands.Organisations.UpdateOrganisation;

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
    private readonly ISender _sender;
    private readonly IMapper _mapper;

    public UpdateOrganisationCommandHandler(ApplicationDbContext context, IMapper mapper, ISender sender,
        ILogger<UpdateOrganisationCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
        _mapper = mapper;
        _sender = sender;
    }

    public async Task<long> Handle(UpdateOrganisationCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var entity = await _context.Organisations
            .Include(o => o.Services)
            .ThenInclude(s => s.Taxonomies)
            .Include(o => o.Services)
            .ThenInclude(s => s.Locations)
            .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        if (entity is null)
            throw new NotFoundException(nameof(Organisation), request.Id.ToString());

        try
        {
            entity = _mapper.Map(request.Organisation, entity);
            
            foreach (var service in entity.Services)
            {
                service.AttachExistingManyToMany(_context, _mapper);
            }

            _context.Organisations.Update(entity);

            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Organisation {Name} saved to DB", request.Organisation.Name);

            _logger.LogInformation("Account {Name} sending an event grid message", request.Organisation.Name);
            SendEventGridMessageCommand sendEventGridMessageCommand = new(request.Organisation);
            _ = _sender.Send(sendEventGridMessageCommand, cancellationToken);
            _logger.LogInformation("Account {Name} completed the event grid message", request.Organisation.Name);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred updating organisation with Name:{name}.", request.Organisation.Name);
            throw;
        }

        return entity.Id;
    }
}