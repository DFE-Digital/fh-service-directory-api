using Ardalis.GuardClauses;
using Ardalis.Specification;
using fh_service_directory_api.api.Helper;
using fh_service_directory_api.core.Entities;
using fh_service_directory_api.core.Events;
using fh_service_directory_api.infrastructure.Persistence.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;

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

                        var one = childModel.ServiceDelivery.Serialize();
                        var two = existingChild.ServiceDelivery.Serialize();

                        if (childModel.ServiceDelivery.Serialize() != existingChild.ServiceDelivery.Serialize())
                            UpdateServiceDelivery(existingChild.ServiceDelivery, childModel.ServiceDelivery);
                        if (childModel.Contacts.Serialize() != existingChild.Contacts.Serialize())
                            UpdateContacts(existingChild.Contacts, childModel.Contacts);
                        if (childModel.Languages.Serialize() != existingChild.Languages.Serialize())
                            UpdateLanguages(existingChild.Languages, childModel.Languages);
                        if (childModel?.Service_taxonomys?.Serialize() != existingChild.Service_taxonomys.Serialize())
                            UpdateTaxonomies(existingChild.Service_taxonomys, childModel?.Service_taxonomys ?? new Collection<OpenReferralService_Taxonomy>());
                        if (childModel?.Cost_options?.Serialize() != existingChild.Cost_options.Serialize())
                            UpdateCostOptions(existingChild.Cost_options, childModel?.Cost_options ?? new Collection<OpenReferralCost_Option>());
                    }

                    else
                    {
                        childModel.OpenReferralOrganisationId = request.Id;

                        if (childModel != null && childModel.Service_taxonomys != null)
                        {
                            for (int i = 0; i < childModel?.Service_taxonomys.Count; i++)
                            {
                                if (childModel.Service_taxonomys.ElementAt(i) != null && childModel.Service_taxonomys.ElementAt(i).Taxonomy != null)
                                {
                                    string id = childModel?.Service_taxonomys?.ElementAt(i)?.Taxonomy?.Id ?? string.Empty;
                                    var tx = _context.OpenReferralTaxonomies.FirstOrDefault(x => x.Id == id);
                                    if (childModel != null)
                                        childModel.Service_taxonomys.ElementAt(i).Taxonomy = tx;
                                }
                            }
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

    private void UpdateCostOptions(ICollection<OpenReferralCost_Option> existing, ICollection<OpenReferralCost_Option> updated)
    {
        foreach (var updatedCostOption in updated)
        {
            var current = existing.FirstOrDefault(x => x.Id == updatedCostOption.Id);
            if (current == null)
            {
                _context.OpenReferralCost_Options.Add(updatedCostOption);
            }
            else
            {
                current.LinkId = updatedCostOption.LinkId;
                current.Amount = updatedCostOption.Amount;
                current.Amount_description = updatedCostOption.Amount_description;
                current.Option = updatedCostOption.Option;
                current.Valid_from = updatedCostOption.Valid_from;
                current.Valid_to = updatedCostOption.Valid_to;
            }
        }
    }

    private void UpdateTaxonomies(ICollection<OpenReferralService_Taxonomy> existing, ICollection<OpenReferralService_Taxonomy> updated)
    {
        foreach (var updatedServiceTaxonomy in updated)
        {
            var current = existing.FirstOrDefault(x => x.Id == updatedServiceTaxonomy.Id);
            if (current == null)
            {
                _context.OpenReferralService_Taxonomies.Add(updatedServiceTaxonomy);
            }
            else
            {
                current.LinkId = updatedServiceTaxonomy.LinkId;
                current.Taxonomy = updatedServiceTaxonomy.Taxonomy;
            }
        }
    }

    private void UpdateLanguages(ICollection<OpenReferralLanguage> existing, ICollection<OpenReferralLanguage> updated)
    {
        foreach (var updatedLanguage in updated)
        {
            var current = existing.FirstOrDefault(x => x.Id == updatedLanguage.Id);
            if (current == null)
            {
                _context.OpenReferralLanguages.Add(updatedLanguage);
            }
            else
            {
                current.Language = updatedLanguage.Language;
            }
        }
    }

    private void UpdateContacts(ICollection<OpenReferralContact> existing, ICollection<OpenReferralContact> updated)
    {
        foreach (var updatedContact in updated)
        {
            var current = existing.FirstOrDefault(x => x.Id == updatedContact.Id);
            if (current == null)
            {
                _context.OpenReferralContacts.Add(updatedContact);
            }
            else
            {
                current.Title = updatedContact.Title;
                current.Name = updatedContact.Name;
                current.Phones = updatedContact.Phones;
            }
        }
    }

    private void UpdateServiceDelivery(ICollection<OpenReferralServiceDelivery> existing, ICollection<OpenReferralServiceDelivery> updated)
    {
        foreach(var updatedServiceDelivery in updated)
        {
            var current = existing.FirstOrDefault(x => x.Id == updatedServiceDelivery.Id);
            if (current == null)
            {
                _context.OpenReferralServiceDeliveries.Add(updatedServiceDelivery);
            }
            else
            {
                current.ServiceDelivery = updatedServiceDelivery.ServiceDelivery;
            }
        }
    }
}


