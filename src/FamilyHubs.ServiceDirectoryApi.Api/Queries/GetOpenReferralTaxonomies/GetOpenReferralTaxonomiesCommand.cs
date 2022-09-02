using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralTaxonomys;
using FamilyHubs.SharedKernel;
using fh_service_directory_api.core.Interfaces.Infrastructure;
using fh_service_directory_api.infrastructure.Persistence.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace fh_service_directory_api.api.Queries.GetOpenReferralTaxonomies;

public class GetOpenReferralTaxonomiesCommand : IRequest<PaginatedList<OpenReferralTaxonomyDto>>
{
    public GetOpenReferralTaxonomiesCommand(int? pageNumber, int? pageSize, string? text)
    {
        PageNumber = pageNumber != null ? pageNumber.Value : 1;
        PageSize = pageSize != null ? pageSize.Value : 1;
        Text = text;
    }

    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? Text { get; set; }
}

public class GetOpenReferralTaxonomiesCommandHandler : IRequestHandler<GetOpenReferralTaxonomiesCommand, PaginatedList<OpenReferralTaxonomyDto>>
{
    private readonly ApplicationDbContext _context;

    public GetOpenReferralTaxonomiesCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<OpenReferralTaxonomyDto>> Handle(GetOpenReferralTaxonomiesCommand request, CancellationToken cancellationToken)
    {
        var entities = await _context.OpenReferralTaxonomies.ToListAsync();

        if (request.Text != null)
        {
            entities = entities.Where(x => x.Name.Contains(request.Text)).ToList();
        }

        var filteredTaxonomies = entities.Select(x => new OpenReferralTaxonomyDto(
            x.Id,
            x.Name,
            x.Vocabulary,
            x.Parent
            )).ToList();

        if (request != null)
        {
            var pagelist = filteredTaxonomies.Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize).ToList();
            var result = new PaginatedList<OpenReferralTaxonomyDto>(filteredTaxonomies, pagelist.Count, request.PageNumber, request.PageSize);
            return result;
        }

        return new PaginatedList<OpenReferralTaxonomyDto>(filteredTaxonomies, filteredTaxonomies.Count, 1, 10);
    }
}