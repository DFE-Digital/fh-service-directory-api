using Ardalis.GuardClauses;
using AutoMapper;
using fh_service_directory_api.core.Entities;
using fh_service_directory_api.infrastructure.Persistence.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace fh_service_directory_api.api.Queries.GetOrganisationAdminByOrganisationId;

public class GetOrganisationAdminByOrganisationIdCommand : IRequest<string>
{
    public GetOrganisationAdminByOrganisationIdCommand(string openReferralOrganisationId)
    {
        OpenReferralOrganisationId = openReferralOrganisationId;
    }

    public string OpenReferralOrganisationId { get; init; } = default!;
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
        var entity = await _context.OrganisationAdminDistricts
           .FirstOrDefaultAsync(p => p.OpenReferralOrganisationId == request.OpenReferralOrganisationId, cancellationToken: cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(OpenReferralOrganisation), request.OpenReferralOrganisationId);
        }

        return entity.Code;
    }
}

