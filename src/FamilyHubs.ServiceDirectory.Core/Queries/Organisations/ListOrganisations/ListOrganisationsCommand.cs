using AutoMapper;
using AutoMapper.QueryableExtensions;
using FamilyHubs.ServiceDirectory.Data.Repository;
using FamilyHubs.ServiceDirectory.Shared.Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FamilyHubs.ServiceDirectory.Core.Queries.Organisations.ListOrganisations;

public class ListOrganisationsCommand : IRequest<List<OrganisationDto>>
{
}

public class ListOrganisationCommandHandler : IRequestHandler<ListOrganisationsCommand, List<OrganisationDto>>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public ListOrganisationCommandHandler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<OrganisationDto>> Handle(ListOrganisationsCommand _, CancellationToken cancellationToken)
    {
        var organisations = await _context.Organisations

            .IgnoreAutoIncludes()

            .AsNoTracking()

            .ProjectTo<OrganisationDto>(_mapper.ConfigurationProvider)

            .ToListAsync(cancellationToken);

        return organisations;
    }
}
