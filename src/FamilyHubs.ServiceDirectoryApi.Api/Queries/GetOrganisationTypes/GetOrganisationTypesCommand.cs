using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralOrganisations;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OrganisationType;
using fh_service_directory_api.infrastructure.Persistence.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace fh_service_directory_api.api.Queries.GetOrganisationTypes;

public class GetOrganisationTypesCommand : IRequest<List<OrganisationTypeDto>>
{
    public GetOrganisationTypesCommand()
    {

    }
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
        var orgainsationTypes = await _context.OrganisationTypes.Select(x => new OrganisationTypeDto(x.Id, x.Name, x.Description)).AsNoTracking().ToListAsync();

        return orgainsationTypes;
    }
}

