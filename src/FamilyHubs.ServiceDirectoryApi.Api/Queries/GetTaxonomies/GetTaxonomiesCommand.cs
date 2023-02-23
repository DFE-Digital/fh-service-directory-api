using FamilyHubs.ServiceDirectory.Infrastructure.Persistence.Repository;
using FamilyHubs.ServiceDirectory.Shared.Dto;
using FamilyHubs.ServiceDirectory.Shared.Enums;
using FamilyHubs.SharedKernel;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FamilyHubs.ServiceDirectory.Api.Queries.GetTaxonomies;

public class GetTaxonomiesCommand : IRequest<PaginatedList<TaxonomyDto>>
{
    public GetTaxonomiesCommand(TaxonomyType taxonomyType, int? pageNumber, int? pageSize, string? text)
    {
        PageNumber = pageNumber ?? 1;
        PageSize = pageSize ?? 10;
        TaxonomyType = taxonomyType;
        Text = text;
    }

    public int PageNumber { get; }
    public int PageSize { get; }
    public TaxonomyType TaxonomyType { get; }
    public string? Text { get; }
}

public class GetTaxonomiesCommandHandler : IRequestHandler<GetTaxonomiesCommand, PaginatedList<TaxonomyDto>>
{
    private readonly ApplicationDbContext _context;

    public GetTaxonomiesCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<TaxonomyDto>> Handle(GetTaxonomiesCommand request, CancellationToken cancellationToken)
    {
        var filteredTaxonomies = await _context.Taxonomies
            .Where(t => request.Text == null || t.Name.Contains(request.Text))
            .Where(t => request.TaxonomyType == TaxonomyType.NotSet || t.TaxonomyType == request.TaxonomyType)
            .Select(x => new TaxonomyDto(
                x.Id,
                x.Name,
                x.TaxonomyType,
                x.Parent
            ))
            .ToListAsync(cancellationToken: cancellationToken);

        var pagedList = filteredTaxonomies.Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize).ToList();
        
        var result = new PaginatedList<TaxonomyDto>(pagedList, filteredTaxonomies.Count, request.PageNumber, request.PageSize);
        
        return result;
    }
}