using FamilyHubs.ServiceDirectory.Infrastructure.Persistence.Repository;
using FamilyHubs.ServiceDirectory.Shared.Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FamilyHubs.ServiceDirectory.Api.Queries.GetOrganisationTypes;

public class GetOrganisationTypesCommand : IRequest<List<OrganisationTypeDto>>
{
}

public class GetOrganisationTypesCommandHandler : IRequestHandler<GetOrganisationTypesCommand, List<OrganisationTypeDto>>
{
    private readonly ApplicationDbContext _context;

    public GetOrganisationTypesCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<OrganisationTypeDto>> Handle(GetOrganisationTypesCommand request, CancellationToken cancellationToken)
    {
        var orgainsationTypes = await _context.OrganisationTypes.Select(x => new OrganisationTypeDto(x.Id, x.Name, x.Description)).AsNoTracking().ToListAsync(cancellationToken);

        return orgainsationTypes;
    }
}

