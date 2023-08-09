using Ardalis.GuardClauses;
using AutoMapper;
using FamilyHubs.ServiceDirectory.Core.Exceptions;
using FamilyHubs.ServiceDirectory.Core.Helper;
using FamilyHubs.ServiceDirectory.Data.Entities;
using FamilyHubs.ServiceDirectory.Data.Repository;
using FamilyHubs.ServiceDirectory.Shared.Dto;
using FamilyHubs.SharedKernel;
using FamilyHubs.SharedKernel.Identity;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NotFoundException = Ardalis.GuardClauses.NotFoundException;

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
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ApplicationDbContext _context;
    private readonly ILogger<UpdateOrganisationCommandHandler> _logger;
    private readonly ISender _sender;
    private readonly IMapper _mapper;

    public UpdateOrganisationCommandHandler(
        IHttpContextAccessor httpContextAccessor,
        ApplicationDbContext context, 
        IMapper mapper,
        ISender sender,
        ILogger<UpdateOrganisationCommandHandler> logger)
    {
        _httpContextAccessor = httpContextAccessor;
        _context = context;
        _logger = logger;
        _mapper = mapper;
        _sender = sender;
    }

    public async Task<long> Handle(UpdateOrganisationCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        ThrowIfForbidden(request);

        var entity = await _context.Organisations
                .Include(o => o.Services)
                .ThenInclude(s => s.Taxonomies)
                .Include(o => o.Services)
                .ThenInclude(s => s.Locations)
                .AsSplitQuery()
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

            _logger.LogInformation("Organisation {Name} sending an event grid message", request.Organisation.Name);
            request.Organisation.Id = entity.Id;
            var eventData = new[]
            {
                new
                {
                    Id = Guid.NewGuid(),
                    EventType = "OrganisationDto",
                    Subject = "Organisation",
                    EventTime = DateTime.UtcNow,
                    Data = request.Organisation
                }
            };
            SendEventGridMessageCommand sendEventGridMessageCommand = new(eventData);
            _ = await _sender.Send(sendEventGridMessageCommand, cancellationToken);
            _logger.LogInformation("Organisation {Name} completed the event grid message", request.Organisation.Name);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred updating organisation with Name:{name}.", request.Organisation.Name);
            throw;
        }

        return entity.Id;
    }

    private void ThrowIfForbidden(UpdateOrganisationCommand request)
    {
        var user = _httpContextAccessor?.HttpContext?.GetFamilyHubsUser();
        if(user == null)
        {
            _logger.LogError("No user retrieved from HttpContext");
            throw new ForbiddenException("No user detected"); // This should be impossible as Authorization is applied to the endpoint
        }

        if(user.Role == RoleTypes.DfeAdmin || user.Role == RoleTypes.ServiceAccount) 
        {
            return;
        }

        var userOrganisationId = long.Parse(user.OrganisationId);
        if(userOrganisationId == request.Organisation.Id || userOrganisationId == request.Organisation.AssociatedOrganisationId)
        {
            return;
        }

        throw new ForbiddenException("This user cannot update this organisation");
    }
}