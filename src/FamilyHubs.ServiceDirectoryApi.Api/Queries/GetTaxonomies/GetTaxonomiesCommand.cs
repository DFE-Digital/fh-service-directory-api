using FamilyHubs.ServiceDirectory.Infrastructure.Persistence.Repository;
using FamilyHubs.ServiceDirectory.Shared.Dto;
using FamilyHubs.SharedKernel;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FamilyHubs.ServiceDirectory.Api.Queries.GetTaxonomies;

public class GetTaxonomiesCommand : IRequest<PaginatedList<TaxonomyDto>>
{
    public GetTaxonomiesCommand(int? pageNumber, int? pageSize, string? text)
    {
        PageNumber = pageNumber != null ? pageNumber.Value : 1;
        PageSize = pageSize != null ? pageSize.Value : 1;
        Text = text;
    }

    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? Text { get; set; }
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
        var entities = await _context.Taxonomies.ToListAsync();

        if (request != null && request.Text != null)
        {
            entities = entities.Where(x => x.Name.Contains(request.Text)).ToList();
        }

        var filteredTaxonomies = entities.Select(x => new TaxonomyDto(
            x.Id,
            x.Name,
            x.Vocabulary,
            x.Parent
            )).ToList();

        if (request != null)
        {
            var pagelist = filteredTaxonomies.Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize).ToList();
            var result = new PaginatedList<TaxonomyDto>(pagelist, filteredTaxonomies.Count, request.PageNumber, request.PageSize);
            return result;
        }

        return new PaginatedList<TaxonomyDto>(filteredTaxonomies, filteredTaxonomies.Count, 1, 10);
    }
}