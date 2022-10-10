using Ardalis.GuardClauses;
using Ardalis.Specification;
using fh_service_directory_api.core.Entities;
using fh_service_directory_api.core.Events;
using fh_service_directory_api.infrastructure.Persistence.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace fh_service_directory_api.api.Commands.UpdateOpenReferralOrganisation;


public class UpdateOpenReferralOrganisationCommand : IRequest<string>
{
    public UpdateOpenReferralOrganisationCommand(string id, OpenReferralOrganisation openReferralOrganisation)
    {
        Id = id;
        OpenReferralOrganisation = openReferralOrganisation;
    }

    public OpenReferralOrganisation OpenReferralOrganisation { get; init; }

    public string Id { get; set; }
}

public class UpdateOpenReferralOrganisationCommandHandler : IRequestHandler<UpdateOpenReferralOrganisationCommand, string>
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<UpdateOpenReferralOrganisationCommandHandler> _logger;

    public UpdateOpenReferralOrganisationCommandHandler(ApplicationDbContext context, ILogger<UpdateOpenReferralOrganisationCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<string> Handle(UpdateOpenReferralOrganisationCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));

        var entity = await _context.OpenReferralOrganisations
          .Include(x => x.Services!)
            .ThenInclude(sd => sd.ServiceDelivery)
          .Include(x => x.Services!)
            .ThenInclude(x => x.Contacts).ThenInclude(x => x.Phones)
          .Include(x => x.Services!)
            .ThenInclude(x => x.Languages)
          .Include(x => x.Services!)
            .ThenInclude(x => x.Service_taxonomys)
            .ThenInclude(x => x.Taxonomy)
          .SingleOrDefaultAsync(p => p.Id == request.Id, cancellationToken: cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(OpenReferralOrganisation), request.Id);
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
                        .Where(c => c.Id == childModel.Id && c.Id != default)
                        .SingleOrDefault();

                    if (existingChild != null)
                    {
                        existingChild.Update(childModel);
                        UpdateServiceDeliveryTypes(existingChild, childModel);
                        UpdateContacts(existingChild, childModel);
                        UpdateLanguages(existingChild, childModel);
                        UpdateTaxonomies(existingChild, childModel);
                        UpdateCostOptions(existingChild, childModel);
                    }

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

            if (entity.Reviews != null && request.OpenReferralOrganisation.Reviews != null) //TODO - also check for count=0 s if count==0, dont enter if block
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
                        .Where(c => c.Id == childModel.Id && c.Id != default)
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
            _logger.LogError(ex, "An error occurred updating organisation. {exceptionMessage}", ex.Message);
            throw new Exception(ex.Message, ex);
        }

        return entity.Id;
    }

    private void UpdateCostOptions(OpenReferralService existingService, OpenReferralService updatedService)
    {
        //DEBUG
        var changedEntities = TrackDbContextChanges();

        _context.OpenReferralCost_Options.RemoveRange(existingService.Cost_options);

        changedEntities = TrackDbContextChanges();


        existingService.Cost_options = updatedService.Cost_options;

        changedEntities = TrackDbContextChanges();
    }

    private void UpdateTaxonomies(OpenReferralService existingService, OpenReferralService updatedService)
    {
        //DEBUG
        var changedEntities = TrackDbContextChanges();

        existingService.Service_taxonomys = null;
        
        //_context.OpenReferralTaxonomies.RemoveRange(existingService.Service_taxonomys);

        if (updatedService != null && updatedService.Service_taxonomys != null)
        {
            existingService.Service_taxonomys = updatedService.Service_taxonomys;
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            for (int i = 0; i < updatedService.Service_taxonomys.Count; i++)
            {
                if (updatedService.Service_taxonomys.ElementAt(i) != null && updatedService.Service_taxonomys.ElementAt(i).Taxonomy != null)
                {
                    string id = updatedService?.Service_taxonomys?.ElementAt(i)?.Taxonomy?.Id ?? string.Empty;
                    var tx = _context.OpenReferralTaxonomies.FirstOrDefault(x => x.Id == id);
                    if (existingService != null)
                        existingService.Service_taxonomys.ElementAt(i).Taxonomy = tx;
                }
            }
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        }
    }

    private void UpdateLanguages(OpenReferralService existingService, OpenReferralService updatedService)
    {
        //DEBUG
        var changedEntities = TrackDbContextChanges();

        _context.OpenReferralLanguages.RemoveRange(existingService.Languages);

        changedEntities = TrackDbContextChanges();


        existingService.Languages = updatedService.Languages;

        changedEntities = TrackDbContextChanges();
    }

    private void UpdateContacts(OpenReferralService existingService, OpenReferralService updatedService)
    {
        //DEBUG
        var changedEntities = TrackDbContextChanges();

        _context.OpenReferralContacts.RemoveRange(existingService.Contacts);

        changedEntities = TrackDbContextChanges();


        existingService.Contacts = updatedService.Contacts;

        changedEntities = TrackDbContextChanges();
    }

    private void UpdateServiceDeliveryTypes(OpenReferralService existingService, OpenReferralService updatedService)
    {
        //DEBUG
        var changedEntities = TrackDbContextChanges();

        _context.OpenReferralServiceDeliveries.RemoveRange(existingService.ServiceDelivery);

        changedEntities = TrackDbContextChanges();


        existingService.ServiceDelivery = updatedService.ServiceDelivery;

        changedEntities = TrackDbContextChanges();

        //foreach (var serviceDelivery in existingService.ServiceDelivery)
        //{
        //    var existingChild = entity.Services
        //        .Where(c => c.Id == childModel.Id && c.Id != default)
        //        .SingleOrDefault();

        //    if (existingChild != null)
        //    {
        //        existingChild.Update(childModel);
        //        UpdateServiceDeliveryTypes(childModel);
        //    }
        //}
    }

    private List<EntityEntry> TrackDbContextChanges()
    {
        _context.ChangeTracker.DetectChanges();

        if (!_context.ChangeTracker.HasChanges())
        {
            return null;
        }

        //TEMP
        return _context.ChangeTracker.Entries().Where(e => e.State != EntityState.Unchanged && e.State != EntityState.Detached).ToList();

        //return _dbContext.ChangeTracker.DebugView.LongView;

    }
}


