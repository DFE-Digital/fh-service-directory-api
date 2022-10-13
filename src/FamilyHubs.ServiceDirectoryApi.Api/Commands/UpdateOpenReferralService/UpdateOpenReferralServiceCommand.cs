using Ardalis.GuardClauses;
using Ardalis.Specification;
using fh_service_directory_api.api.Helper;
using fh_service_directory_api.core.Entities;
using fh_service_directory_api.core.Events;
using fh_service_directory_api.infrastructure.Persistence.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;

namespace fh_service_directory_api.api.Commands.UpdateOpenReferralService;

public class UpdateOpenReferralServiceCommand : IRequest<string>
{
    public UpdateOpenReferralServiceCommand(string id, OpenReferralService openReferralService)
    {
        Id = id;
        OpenReferralService = openReferralService;
    }

    public OpenReferralService OpenReferralService { get; init; }

    public string Id { get; set; }
}

public class UpdateOpenReferralServiceCommandHandler : IRequestHandler<UpdateOpenReferralServiceCommand, string>
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<UpdateOpenReferralServiceCommandHandler> _logger;

    public UpdateOpenReferralServiceCommandHandler(ApplicationDbContext context, ILogger<UpdateOpenReferralServiceCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<string> Handle(UpdateOpenReferralServiceCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));

        var entity = await _context.OpenReferralServices
           .Include(x => x.ServiceDelivery)
           .Include(x => x.Eligibilities)
           .Include(x => x.Contacts)
           .ThenInclude(x => x.Phones)
           .Include(x => x.Cost_options)
           .Include(x => x.Languages)
           .Include(x => x.Service_areas)
           .Include(x => x.Service_at_locations)
           .ThenInclude(x => x.Location)
           .ThenInclude(x => x.Physical_addresses)
           .Include(x => x.Service_taxonomys)
           .ThenInclude(x => x.Taxonomy)
           .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken: cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(OpenReferralService), request.Id);
        }

        try
        {
            entity.Update(request.OpenReferralService);

            if (entity.Eligibilities.Serialize() != request.OpenReferralService.Eligibilities.Serialize())
                UpdateEligibility(entity.Eligibilities, request.OpenReferralService.Eligibilities);
            if (entity.Service_areas.Serialize() != request.OpenReferralService.Service_areas.Serialize())
                UpdateServiceArea(entity.Service_areas, request.OpenReferralService.Service_areas);
            if (entity.ServiceDelivery.Serialize() != request.OpenReferralService.ServiceDelivery.Serialize())
                UpdateServiceDelivery(entity.ServiceDelivery, request.OpenReferralService.ServiceDelivery);
            if (entity.Contacts.Serialize() != request.OpenReferralService.Contacts.Serialize())
                UpdateContacts(entity.Contacts, request.OpenReferralService.Contacts);
            if (entity.Languages.Serialize() != request.OpenReferralService.Languages.Serialize())
                UpdateLanguages(entity.Languages, request.OpenReferralService.Languages);
            if (entity.Service_at_locations.Serialize() != request.OpenReferralService.Service_at_locations.Serialize())
                UpdateServiceAtLocation(entity.Service_at_locations, request.OpenReferralService.Service_at_locations);
            if (entity.Service_taxonomys?.Serialize() != request.OpenReferralService.Service_taxonomys.Serialize())
                UpdateTaxonomies(entity.Service_taxonomys ?? new Collection<OpenReferralService_Taxonomy>(), request.OpenReferralService?.Service_taxonomys ?? new Collection<OpenReferralService_Taxonomy>());
            if (entity.Cost_options?.Serialize() != request?.OpenReferralService?.Cost_options.Serialize())
                UpdateCostOptions(entity.Cost_options ?? new Collection<OpenReferralCost_Option>(), request?.OpenReferralService?.Cost_options ?? new Collection<OpenReferralCost_Option>());

            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred updating organisation. {exceptionMessage}", ex.Message);
            throw new Exception(ex.Message, ex);
        }

        return entity.Id;
    }

    private void UpdateEligibility(ICollection<OpenReferralEligibility> existing, ICollection<OpenReferralEligibility> updated)
    {
        foreach (var updatedEligibility in updated)
        {
            var current = existing.FirstOrDefault(x => x.Id == updatedEligibility.Id);
            if (current == null)
            {
                updatedEligibility.RegisterDomainEvent(new OpenReferralEligibilityEvent(updatedEligibility));
                _context.OpenReferralEligibilities.Add(updatedEligibility);
            }
            else
            {
                current.LinkId = updatedEligibility.LinkId;
                current.Maximum_age = updatedEligibility.Maximum_age;
                current.Minimum_age = updatedEligibility.Minimum_age;
                current.Taxonomys = updatedEligibility.Taxonomys;
            }
        }
    }

    private void UpdateServiceAtLocation(ICollection<OpenReferralServiceAtLocation> existing, ICollection<OpenReferralServiceAtLocation> updated)
    {
        foreach (var updatedServiceLoc in updated)
        {
            var current = existing.FirstOrDefault(x => x.Id == updatedServiceLoc.Id);
            if (current == null)
            {
                updatedServiceLoc.RegisterDomainEvent(new OpenReferralServiceAtLocationEvent(updatedServiceLoc));
                _context.OpenReferralServiceAtLocations.Add(updatedServiceLoc);
            }
            else
            {
                current.Location = updatedServiceLoc.Location;
                if (current?.HolidayScheduleCollection?.Serialize() != updatedServiceLoc?.HolidayScheduleCollection?.Serialize())
                {
                    UpdateHolidaySchedule(current?.HolidayScheduleCollection ?? new Collection<OpenReferralHoliday_Schedule>(), updatedServiceLoc?.HolidayScheduleCollection ?? new Collection<OpenReferralHoliday_Schedule>());
                }
                if (current?.Regular_schedule?.Serialize() != updatedServiceLoc?.Regular_schedule?.Serialize())
                {
                    UpdateRegularSchedule(current?.Regular_schedule ?? new Collection<OpenReferralRegular_Schedule>(), updatedServiceLoc?.Regular_schedule ?? new Collection<OpenReferralRegular_Schedule>());
                }
            }
        }
    }

    private void UpdateHolidaySchedule(ICollection<OpenReferralHoliday_Schedule> existing, ICollection<OpenReferralHoliday_Schedule> updated)
    {
        foreach (var updatedSchedule in updated)
        {
            var current = existing.FirstOrDefault(x => x.Id == updatedSchedule.Id);
            if (current == null)
            {
                updatedSchedule.RegisterDomainEvent(new OpenReferralHolidayScheduleEvent(updatedSchedule));
                _context.OpenReferralHoliday_Schedules.Add(updatedSchedule);
            }
            else
            {
                current.Closed = updatedSchedule.Closed;
                current.Closes_at = updatedSchedule.Closes_at;
                current.Start_date = updatedSchedule.Start_date;
                current.End_date = updatedSchedule.End_date;
                current.Opens_at = updatedSchedule.Opens_at;
            }
        }
    }

    private void UpdateRegularSchedule(ICollection<OpenReferralRegular_Schedule> existing, ICollection<OpenReferralRegular_Schedule> updated)
    {
        foreach (var updatedSchedule in updated)
        {
            var current = existing.FirstOrDefault(x => x.Id == updatedSchedule.Id);
            if (current == null)
            {
                updatedSchedule.RegisterDomainEvent(new OpenReferralRegularScheduleEvent(updatedSchedule));
                _context.OpenReferralRegular_Schedules.Add(updatedSchedule);
            }
            else
            {
                current.Description = updatedSchedule.Description;
                current.Opens_at = updatedSchedule.Opens_at;
                current.Closes_at = updatedSchedule.Closes_at;
                current.Byday = updatedSchedule.Byday;
                current.Bymonthday = updatedSchedule.Bymonthday;
                current.Dtstart = updatedSchedule.Dtstart;
                current.Freq = updatedSchedule.Freq;
                current.Interval = updatedSchedule.Interval;
                current.Valid_from = updatedSchedule.Valid_from;
                current.Valid_to = updatedSchedule.Valid_to;
            }
        }
    }

    private void UpdateServiceArea(ICollection<OpenReferralService_Area> existing, ICollection<OpenReferralService_Area> updated)
    {
        foreach (var updatedServiceArea in updated)
        {
            var current = existing.FirstOrDefault(x => x.Id == updatedServiceArea.Id);
            if (current == null)
            {
                updatedServiceArea.RegisterDomainEvent(new OpenReferralServiceAreaEvent(updatedServiceArea));
                _context.OpenReferralService_Areas.Add(updatedServiceArea);
            }
            else
            {
                current.LinkId = updatedServiceArea.LinkId;
                current.Service_area = updatedServiceArea.Service_area;
                current.Extent = updatedServiceArea.Extent;
                current.Uri = updatedServiceArea.Uri;
            }
        }
    }

    private void UpdateCostOptions(ICollection<OpenReferralCost_Option> existing, ICollection<OpenReferralCost_Option> updated)
    {
        foreach (var updatedCostOption in updated)
        {
            var current = existing.FirstOrDefault(x => x.Id == updatedCostOption.Id);
            if (current == null)
            {
                updatedCostOption.RegisterDomainEvent(new OpenReferralCostOptionEvent(updatedCostOption));
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
                updatedServiceTaxonomy.RegisterDomainEvent(new OpenReferralServiceTaxonomyEvent(updatedServiceTaxonomy));
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
                updatedLanguage.RegisterDomainEvent(new OpenReferralLanguageEvent(updatedLanguage));
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
                updatedContact.RegisterDomainEvent(new OpenReferralContactEvent(updatedContact));
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
        foreach (var updatedServiceDelivery in updated)
        {
            var current = existing.FirstOrDefault(x => x.Id == updatedServiceDelivery.Id);
            if (current == null)
            {
                updatedServiceDelivery.RegisterDomainEvent(new OpenReferralServiceDeliveryEvent(updatedServiceDelivery));
                _context.OpenReferralServiceDeliveries.Add(updatedServiceDelivery);
            }
            else
            {
                current.ServiceDelivery = updatedServiceDelivery.ServiceDelivery;
            }
        }
    }
}


