using AutoMapper;
using AutoMapper.QueryableExtensions;
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
    private readonly IMapper _mapper;

    public ListOrganisationCommandHandler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<OrganisationDto>> Handle(ListOrganisationCommand request, CancellationToken cancellationToken)
    {
        var organisations = await _context.Organisations
            .ProjectTo<OrganisationDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return organisations;
    }
}
