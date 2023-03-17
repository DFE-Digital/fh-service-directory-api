using Ardalis.GuardClauses;
using FamilyHubs.ServiceDirectory.Data.Entities;
using FamilyHubs.ServiceDirectory.Data.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FamilyHubs.ServiceDirectory.Core.Queries.Organisations.GetOrganisationAdminAreaById;

public class GetOrganisationAdminAreaByIdCommand : IRequest<string>
{
    public required long OrganisationId { get; set; }
}

public class GetOrganisationAdminAreaByIdCommandHandler : IRequestHandler<GetOrganisationAdminAreaByIdCommand, string>
{
    private readonly ApplicationDbContext _context;

    public GetOrganisationAdminAreaByIdCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<string> Handle(GetOrganisationAdminAreaByIdCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Organisations
            .IgnoreAutoIncludes()
            .Select(o => new { o.Id, o.AdminAreaCode })
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == request.OrganisationId, cancellationToken);

        if (entity is null)
            throw new NotFoundException(nameof(Organisation), request.OrganisationId.ToString());

        return entity.AdminAreaCode;
    }
}

