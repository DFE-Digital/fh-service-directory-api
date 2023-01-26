using Ardalis.GuardClauses;
using AutoMapper;
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

    public string OrganisationId { get; init; } = default!;
}

public class GetOrganisationAdminByOrganisationIdCommandHandler : IRequestHandler<GetOrganisationAdminByOrganisationIdCommand, string>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetOrganisationAdminByOrganisationIdCommandHandler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
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

