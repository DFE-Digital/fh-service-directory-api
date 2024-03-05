using Ardalis.GuardClauses;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using FamilyHubs.ServiceDirectory.Data.Entities;
using FamilyHubs.ServiceDirectory.Data.Repository;
using FamilyHubs.ServiceDirectory.Shared.Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FamilyHubs.ServiceDirectory.Core.Queries.Organisations.GetOrganisationById;


public class GetOrganisationByIdCommand : IRequest<OrganisationDetailsDto>
{
    public required long Id { get; set; }
}

public class GetOrganisationByIdHandler : IRequestHandler<GetOrganisationByIdCommand, OrganisationDetailsDto>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetOrganisationByIdHandler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<OrganisationDetailsDto> Handle(GetOrganisationByIdCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Organisations
            .Include(x => x.Services)
            .ThenInclude(x => x.Taxonomies)

            .Include(x => x.Services)
            .ThenInclude(x => x.Locations)
            .ThenInclude(x => x.Contacts)

            .Include(x => x.Services)
            .ThenInclude(x => x.Locations)
            .ThenInclude(x => x.Schedules)

            .AsSplitQuery()
            .AsNoTracking()

            .ProjectTo<OrganisationDetailsDto>(_mapper.ConfigurationProvider)

            .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        if (entity == null)
            throw new NotFoundException(nameof(Organisation), request.Id.ToString());

        return entity;
    }
}


