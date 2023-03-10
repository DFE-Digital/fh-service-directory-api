using Ardalis.GuardClauses;
using FamilyHubs.ServiceDirectory.Core.Entities;
using FamilyHubs.ServiceDirectory.Infrastructure.Persistence.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FamilyHubs.ServiceDirectory.Api.Queries.GetOrganisationAdminByOrganisationId;

public class GetOrganisationAdminByOrganisationIdCommand : IRequest<string>
{
    public GetOrganisationAdminByOrganisationIdCommand(long organisationId)
    {
        OrganisationId = organisationId;
    }

    public long OrganisationId { get; }
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
        var entity = await _context.Organisations
           .FirstOrDefaultAsync(p => p.Id == request.OrganisationId, cancellationToken);

        if (entity is null)
            throw new NotFoundException(nameof(Organisation), request.OrganisationId.ToString());

        return entity.AdminAreaCode;
    }
}

