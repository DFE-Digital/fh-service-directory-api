using FamilyHubs.ServiceDirectory.Core.Exceptions;
using FamilyHubs.ServiceDirectory.Data.Entities;
using FamilyHubs.ServiceDirectory.Data.Repository;
using FamilyHubs.SharedKernel.Identity;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace FamilyHubs.ServiceDirectory.Core.Commands.Organisations.DeleteOrganisation;

public class DeleteOrganisationCommand : IRequest<bool>
{
    public DeleteOrganisationCommand(long id)
    {
        Id = id;
    }

    public long Id { get; }
}

public class DeleteOrganisationCommandHandler : IRequestHandler<DeleteOrganisationCommand, bool>
{
    private readonly ApplicationDbContext _context;    
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<DeleteOrganisationCommandHandler> _logger;

    public DeleteOrganisationCommandHandler(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor,
        ILogger<DeleteOrganisationCommandHandler> logger)
    {
        _context = context;        
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    public async Task<bool> Handle(DeleteOrganisationCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var entity = _context.Organisations.SingleOrDefault(o => o.Id == request.Id);
            if (entity is null)
                throw new NotFoundException("Organisation Not Found");

            ThrowIfForbidden(request, entity);

            _context.Organisations.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred deleting organisation with Id :{id}.", request.Id);
            throw;
        }
    }

    private void ThrowIfForbidden(DeleteOrganisationCommand request, Organisation organisation)
    {
        var user = _httpContextAccessor?.HttpContext?.GetFamilyHubsUser();
        if (user == null)
        {
            _logger.LogError("No user retrieved from HttpContext");
            throw new ForbiddenException("No user detected"); // This should be impossible as Authorization is applied to the endpoint
        }

        if (user.Role == RoleTypes.DfeAdmin || user.Role == RoleTypes.ServiceAccount)
        {
            return;
        }

        var userOrganisationId = long.Parse(user.OrganisationId);
        if (userOrganisationId == request.Id || userOrganisationId == organisation.AssociatedOrganisationId)
        {
            return;
        }

        throw new ForbiddenException("This user cannot update this organisation");
    }
}