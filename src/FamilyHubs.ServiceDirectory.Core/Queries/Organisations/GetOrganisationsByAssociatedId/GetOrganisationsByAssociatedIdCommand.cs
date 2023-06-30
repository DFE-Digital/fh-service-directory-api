using AutoMapper;
using FamilyHubs.ServiceDirectory.Data.Repository;
using FamilyHubs.ServiceDirectory.Shared.Dto;
using FamilyHubs.ServiceDirectory.Shared.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FamilyHubs.ServiceDirectory.Core.Queries.Organisations.GetOrganisationsByAssociatedId;


public class GetOrganisationsByAssociatedIdCommand : IRequest<List<OrganisationDto>>
{
    public GetOrganisationsByAssociatedIdCommand(long organisationId)
    {
        OrganisationId = organisationId;
    }

    public long OrganisationId { get; }
}

public class GetOrganisationsByAssociatedIdCommandHandler : IRequestHandler<GetOrganisationsByAssociatedIdCommand, List<OrganisationDto>>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetOrganisationsByAssociatedIdCommandHandler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<OrganisationDto>> Handle(GetOrganisationsByAssociatedIdCommand request, CancellationToken cancellationToken)
    {
        var organisationsQuery = _context.Organisations
            .AsNoTracking()
            .Where(x => x.Id == request.OrganisationId || x.AssociatedOrganisationId == request.OrganisationId);

        var organisations = await organisationsQuery.ToListAsync(cancellationToken);

        var mappedOrganisations = _mapper.Map<List<OrganisationDto>>(organisations);

        return mappedOrganisations;
    }

}
