using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralOrganisations;
using fh_service_directory_api.core.Interfaces.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace fh_service_directory_api.core.Queries.ListOrganisation;

public class ListOpenReferralOrganisationCommand : IRequest<List<OpenReferralOrganisationDto>>
{
    public ListOpenReferralOrganisationCommand()
    {

    }
}

public class ListOpenReferralOrganisationCommandHandler : IRequestHandler<ListOpenReferralOrganisationCommand, List<OpenReferralOrganisationDto>>
{
    private readonly IApplicationDbContext _context;

    public ListOpenReferralOrganisationCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<OpenReferralOrganisationDto>> Handle(ListOpenReferralOrganisationCommand request, CancellationToken cancellationToken)
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
