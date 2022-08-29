using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralOrganisations;
using FamilyHubs.ServiceDirectoryApi.Infrastructure.Persistence.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FamilyHubs.ServiceDirectoryApi.Api.Queries.ListOpenReferralOrganisation;

public class ListOpenReferralOrganisationQuery : IRequest<List<OpenReferralOrganisationDto>>
{
    public ListOpenReferralOrganisationQuery()
    {

    }
}

public class ListOpenReferralOrganisationCommandHandler : IRequestHandler<ListOpenReferralOrganisationQuery, List<OpenReferralOrganisationDto>>
{
    private readonly ServiceDirectoryDbContext _context;

    public ListOpenReferralOrganisationCommandHandler(ServiceDirectoryDbContext context)
    {
        _context = context;
    }

    public async Task<List<OpenReferralOrganisationDto>> Handle(ListOpenReferralOrganisationQuery request, CancellationToken cancellationToken)
    {
        var organisations = await _context.OpenReferralOrganisations.Select(org => new OpenReferralOrganisationDto(
            org.Id,
            org.Name,
            org.Description,
            org.Logo,
            org.Uri,
            org.Url
            )).ToListAsync();
        return organisations;
    }
}
