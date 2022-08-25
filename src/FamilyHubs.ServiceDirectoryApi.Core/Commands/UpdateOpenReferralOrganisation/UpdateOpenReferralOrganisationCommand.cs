using Ardalis.GuardClauses;
using FamilyHubs.ServiceDirectory.Shared.Entities;
using fh_service_directory_api.core.Events;
using fh_service_directory_api.core.Interfaces.Entities;
using fh_service_directory_api.core.Interfaces.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace fh_service_directory_api.core.Commands.UpdateOpenReferralOrganisation;


public class UpdateOpenReferralOrganisationCommand : IRequest<string>
{
    public UpdateOpenReferralOrganisationCommand(string id, IOpenReferralOrganisation openReferralOrganisation)
    {
        Id = id;
        OpenReferralOrganisation = openReferralOrganisation;
    }

    public IOpenReferralOrganisation OpenReferralOrganisation { get; init; }

    public string Id { get; set; }
}

public class UpdateOpenReferralOrganisationCommandHandler : IRequestHandler<UpdateOpenReferralOrganisationCommand, string>
{
    private readonly IApplicationDbContext _context;

    public UpdateOpenReferralOrganisationCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<string> Handle(UpdateOpenReferralOrganisationCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));

        var entity = (OpenReferralOrganisation) await _context.OpenReferralOrganisations
          .Include(x => x.Services)
          .SingleOrDefaultAsync(p => p.Id == request.Id, cancellationToken: cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(IOpenReferralOrganisation), request.Id);
        }

        try
        {
            entity.Update(request.OpenReferralOrganisation);

            if (entity.Services != null && request.OpenReferralOrganisation.Services != null)
            {
                // Delete children (does this need to be a soft delete)
                foreach (var existingChild in entity.Services)
                {
                    if (!request.OpenReferralOrganisation.Services.Any(c => c.Id == existingChild.Id))
                    {
                        // Replace with soft delete
                        //_context.OpenReferralServices.Remove(existingChild);
                    }
                }

                // Update and Insert children
                foreach (var childModel in request.OpenReferralOrganisation.Services)
                {
                    var existingChild = entity.Services
                        .Where(c => c.Id == childModel.Id && c.Id != default(string))
                        .SingleOrDefault();

                    if (existingChild != null)
                        existingChild.Update(childModel);
                    else
                    {
                        childModel.OpenReferralOrganisationId = request.Id;

                        if (childModel != null && childModel.Service_taxonomys != null)
                        {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                            for (int i = 0; i < childModel.Service_taxonomys.Count; i++)
                            {
                                if (childModel.Service_taxonomys.ElementAt(i) != null && childModel.Service_taxonomys.ElementAt(i).Taxonomy != null)
                                {
                                    string id = childModel?.Service_taxonomys?.ElementAt(i)?.Taxonomy?.Id ?? string.Empty;
                                    var tx = _context.OpenReferralTaxonomies.FirstOrDefault(x => x.Id == id);
                                    if (childModel != null)
                                        childModel.Service_taxonomys.ElementAt(i).Taxonomy = tx;
                                }
                            }
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                        }

                        if (childModel != null)
                        {
                            entity.RegisterDomainEvent(new OpenReferralServiceCreatedEvent(childModel));
                            _context.OpenReferralServices.Add(childModel as OpenReferralService);
                        }


                    }
                }
            }

            if (entity.Reviews != null && request.OpenReferralOrganisation.Reviews != null)
            {
                // Delete children (does this need to be a soft delete)
                foreach (var existingChild in entity.Reviews)
                {
                    if (!request.OpenReferralOrganisation.Reviews.Any(c => c.Id == existingChild.Id))
                        _context.OpenReferralReviews.Remove(existingChild as OpenReferralReview);
                }

                foreach (var childModel in request.OpenReferralOrganisation.Reviews)
                {
                    var existingChild = entity.Reviews
                        .Where(c => c.Id == childModel.Id && c.Id != default(string))
                        .SingleOrDefault();

                    if (existingChild != null)
                        existingChild.Update(childModel);
                    else
                    {
                        entity.RegisterDomainEvent(new OpenReferralReviewCreatedEvent(childModel));

                        _context.OpenReferralReviews.Add(childModel as OpenReferralReview);

                    }
                }
            }

            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }

        return entity.Id;
    }
}


