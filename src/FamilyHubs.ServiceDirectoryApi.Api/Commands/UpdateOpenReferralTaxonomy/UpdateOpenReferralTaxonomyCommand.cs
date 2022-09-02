using Ardalis.GuardClauses;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralTaxonomys;
using fh_service_directory_api.core.Entities;
using fh_service_directory_api.infrastructure.Persistence.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace fh_service_directory_api.api.Commands.UpdateOpenReferralTaxonomy;

public class UpdateOpenReferralTaxonomyCommand : IRequest<string>
{
    public UpdateOpenReferralTaxonomyCommand(string id, OpenReferralTaxonomyDto openReferralTaxonomy)
    {
        Id = id;
        OpenReferralTaxonomy = openReferralTaxonomy;
    }

    public OpenReferralTaxonomyDto OpenReferralTaxonomy { get; init; }

    public string Id { get; set; }
}

public class UpdateOpenReferralTaxonomyCommandHandler : IRequestHandler<UpdateOpenReferralTaxonomyCommand, string>
{
    private readonly ApplicationDbContext _context;

    public UpdateOpenReferralTaxonomyCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<string> Handle(UpdateOpenReferralTaxonomyCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));

        var entity = await _context.OpenReferralTaxonomies
          .SingleOrDefaultAsync(p => p.Id == request.Id, cancellationToken: cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(OpenReferralTaxonomy), request.Id);
        }

        try
        {
            entity.Name = request.OpenReferralTaxonomy.Name;
            entity.Vocabulary = request.OpenReferralTaxonomy.Vocabulary;
            entity.Parent = request.OpenReferralTaxonomy.Parent;

            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }

        return entity.Id;
    }
}


