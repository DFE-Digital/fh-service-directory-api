using fh_service_directory_api.core.Interfaces.Infrastructure;
using fh_service_directory_api.core.RecordEntities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace fh_service_directory_api.core.Commands.ListOrganisation;

public class ListOpenReferralOrganisationCommand : IRequest<List<OpenReferralOrganisationRecord>>
{
    public ListOpenReferralOrganisationCommand()
    {

    }
}

public class ListOpenReferralOrganisationCommandHandler : IRequestHandler<ListOpenReferralOrganisationCommand, List<OpenReferralOrganisationRecord>>
{
    private readonly IApplicationDbContext _context;

    public ListOpenReferralOrganisationCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<OpenReferralOrganisationRecord>> Handle(ListOpenReferralOrganisationCommand request, CancellationToken cancellationToken)
    {
        var organisations = await _context.OpenReferralOrganisations.Select(org => new OpenReferralOrganisationRecord(
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
