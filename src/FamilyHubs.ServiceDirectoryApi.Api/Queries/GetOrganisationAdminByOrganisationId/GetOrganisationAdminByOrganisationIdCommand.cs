using Ardalis.GuardClauses;
using FamilyHubs.ServiceDirectory.Core.Entities;
using FamilyHubs.ServiceDirectory.Infrastructure.Persistence.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FamilyHubs.ServiceDirectory.Api.Queries.GetOrganisationAdminByOrganisationId;

public class GetOrganisationAdminByOrganisationIdCommand : IRequest<string>
{
    public GetOrganisationAdminByOrganisationIdCommand(string organisationId)
    {
        OrganisationId = organisationId;
    }

    public string OrganisationId { get; }
}

public class GetOrganisationAdminByOrganisationIdCommandHandler : IRequestHandler<GetOrganisationAdminByOrganisationIdCommand, string>
{
    private readonly ApplicationDbContext _context;

    public GetOrganisationAdminByOrganisationIdCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<string> Handle(GetOrganisationAdminByOrganisationIdCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.AdminAreas
           .FirstOrDefaultAsync(p => p.OrganisationId == request.OrganisationId, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Organisation), request.OrganisationId);
        }

        return entity.Code;
    }
}

