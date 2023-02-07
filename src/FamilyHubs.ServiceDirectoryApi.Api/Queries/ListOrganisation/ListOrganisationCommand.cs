using FamilyHubs.ServiceDirectory.Infrastructure.Persistence.Repository;
using FamilyHubs.ServiceDirectory.Shared.Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FamilyHubs.ServiceDirectory.Api.Queries.ListOrganisation;

public class ListOrganisationCommand : IRequest<List<OrganisationDto>>
{
}

public class ListOrganisationCommandHandler : IRequestHandler<ListOrganisationCommand, List<OrganisationDto>>
{
    private readonly ApplicationDbContext _context;

    public ListOrganisationCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<OrganisationDto>> Handle(ListOrganisationCommand request, CancellationToken cancellationToken)
    {
        var organisations = await _context.Organisations
                            .Join(_context.AdminAreas, org => org.Id, oad => oad.OrganisationId,
                            (org, oad) => new { org, oad }).Select(
                                x => new OrganisationDto(
                                x.org.Id,
                                new OrganisationTypeDto(x.org.OrganisationType.Id, x.org.OrganisationType.Name, x.org.OrganisationType.Description),
                                x.org.Name,
                                x.org.Description,
                                x.org.Logo,
                                x.org.Uri,
                                x.org.Url)
                                {
                                    AdminAreaCode = x.oad.Code
                                }
                            ).ToListAsync(cancellationToken);

        return organisations;
    }
}
