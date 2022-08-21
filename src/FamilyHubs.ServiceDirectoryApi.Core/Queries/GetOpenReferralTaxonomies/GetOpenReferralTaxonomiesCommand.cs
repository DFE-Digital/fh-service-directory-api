using FamilyHubs.SharedKernel;
using fh_service_directory_api.core.Interfaces.Infrastructure;
using fh_service_directory_api.core.RecordEntities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace fh_service_directory_api.core.Commands.GetOpenReferralTaxonomies;

public class GetOpenReferralTaxonomiesCommand : IRequest<PaginatedList<OpenReferralTaxonomyRecord>>
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

public class GetOpenReferralTaxonomiesCommandHandler : IRequestHandler<GetOpenReferralTaxonomiesCommand, PaginatedList<OpenReferralTaxonomyRecord>>
{
    private readonly IApplicationDbContext _context;

    public GetOpenReferralTaxonomiesCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<OpenReferralTaxonomyRecord>> Handle(GetOpenReferralTaxonomiesCommand request, CancellationToken cancellationToken)
    {
        var entities = await _context.OpenReferralTaxonomies.ToListAsync();

        if (request.Text != null)
        {
            entities = entities.Where(x => x.Name.Contains(request.Text)).ToList();
        }

        var filteredTaxonomies = entities.Select(x => new OpenReferralTaxonomyRecord(
            x.Id,
            x.Name,
            x.Vocabulary,
            x.Parent
            )).ToList();

        if (request != null)
        {
            var pagelist = filteredTaxonomies.Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize).ToList();
            var result = new PaginatedList<OpenReferralTaxonomyRecord>(filteredTaxonomies, pagelist.Count, request.PageNumber, request.PageSize);
            return result;
        }

        return new PaginatedList<OpenReferralTaxonomyRecord>(filteredTaxonomies, filteredTaxonomies.Count, 1, 10);
    }
}