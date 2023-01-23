using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralOrganisations;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OrganisationType;
using fh_service_directory_api.infrastructure.Persistence.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace fh_service_directory_api.api.Queries.ListOrganisation;

public class ListOpenReferralOrganisationCommand : IRequest<List<OpenReferralOrganisationDto>>
{
}

public class ListOpenReferralOrganisationCommandHandler : IRequestHandler<ListOpenReferralOrganisationCommand, List<OpenReferralOrganisationDto>>
{
    private readonly ApplicationDbContext _context;

    public ListOpenReferralOrganisationCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<OpenReferralOrganisationDto>> Handle(ListOpenReferralOrganisationCommand request, CancellationToken cancellationToken)
    {
        var organisations = await _context.OpenReferralOrganisations
                            .Join(_context.AdminAreas, org => org.Id, oad => oad.OpenReferralOrganisationId,
                            (org, oad) => new { org, oad }).Select(
                                x => new OpenReferralOrganisationDto(
                                x.org.Id,
                                new OrganisationTypeDto(x.org.OrganisationType.Id, x.org.OrganisationType.Name, x.org.OrganisationType.Description),
                                x.org.Name,
                                x.org.Description,
                                x.org.Logo,
                                x.org.Uri,
                                x.org.Url
                                )
                                {
                                    AdminAreaCode = x.oad.Code
                                }).ToListAsync(cancellationToken);

        return organisations;
    }
}
