using Ardalis.GuardClauses;
using Ardalis.Specification;
using AutoMapper;
using Azure.Core;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralContacts;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralCostOptions;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralEligibilitys;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralHolidaySchedule;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralLanguages;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralLinkContacts;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralRegularSchedule;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralServiceAreas;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralServiceAtLocations;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralServiceDeliverysEx;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralServices;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralServiceTaxonomys;
using fh_service_directory_api.api.Helper;
using fh_service_directory_api.core.Entities;
using fh_service_directory_api.core.Events;
using fh_service_directory_api.infrastructure.Persistence.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Runtime.CompilerServices;

namespace fh_service_directory_api.api.Commands.UpdateOpenReferralService;

public class UpdateOpenReferralServiceCommand : IRequest<string>
{
    public UpdateOpenReferralServiceCommand(string id, OpenReferralServiceDto openReferralService)
    {
        Id = id;
        OpenReferralService = openReferralService;
    }

    public OpenReferralServiceDto OpenReferralService { get; init; }

    public string Id { get; set; }
}

public class UpdateOpenReferralServiceCommandHandler : IRequestHandler<UpdateOpenReferralServiceCommand, string>
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<UpdateOpenReferralServiceCommandHandler> _logger;
    private readonly IMapper _mapper;

    public UpdateOpenReferralServiceCommandHandler(
        ApplicationDbContext context,
        IMapper mapper,
        ILogger<UpdateOpenReferralServiceCommandHandler> logger
    )
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(mapper);
        ArgumentNullException.ThrowIfNull(logger);

        _context = context;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<string> Handle(UpdateOpenReferralServiceCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));
        OpenReferralService? efOpenReferralService = await GetDbOpenReferralService(request, cancellationToken);

        try
        {
            await UpdateDbOpenReferralService(request, efOpenReferralService, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred updating Service. {exceptionMessage}", ex.Message);
            throw new Exception(ex.Message, ex);
        }

        return efOpenReferralService.Id;
    }

    private async Task UpdateDbOpenReferralService(UpdateOpenReferralServiceCommand request, OpenReferralService dbServiceEntity, CancellationToken cancellationToken)
    {
        var serviceEntity = _mapper.Map<OpenReferralService>(request.OpenReferralService);
        ArgumentNullException.ThrowIfNull(serviceEntity, nameof(serviceEntity));

        MapServiceEntityToDbServiceEntity(request, dbServiceEntity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    private void MapServiceEntityToDbServiceEntity(UpdateOpenReferralServiceCommand request, OpenReferralService dbServiceEntity)
    {
        if (request != null && dbServiceEntity != null)
        {
            UpdateServiceType(request, dbServiceEntity);
            UpdateService(request, dbServiceEntity);
            UpdateLanguages(request, dbServiceEntity);
            UpdateServiceTaxonomies(request, dbServiceEntity);

            UpdateCostOptions(request, dbServiceEntity);
            UpdateServiceArea(request, dbServiceEntity);
            UpdateEligibility(request, dbServiceEntity);
            UpdateServiceDelivery(request, dbServiceEntity);

            UpdateRegularSchedule(request, dbServiceEntity, null);
            UpdateHolidaySchedule(request, dbServiceEntity, null);
            UpdateServiceAtLocation(request, dbServiceEntity);
        }
    }

    private async Task<OpenReferralService> GetDbOpenReferralService(UpdateOpenReferralServiceCommand request, CancellationToken cancellationToken)
    {
        var dbServiceEntity = await _context.OpenReferralServices
           .Include(x => x.ServiceType)
           .Include(x => x.ServiceDelivery)
           .Include(x => x.Eligibilities)
           .Include(x => x.Link_Contacts)
           .Include(x => x.Cost_options)
           .Include(x => x.Languages)
           .Include(x => x.Service_areas)

           .Include(x => x.Service_at_locations)
           .ThenInclude(x => x.Location)
           .ThenInclude(x => x.Physical_addresses)

           .Include(x => x.Service_at_locations)
           .ThenInclude(x => x.Location)
           .ThenInclude(x => x.LinkTaxonomies!)
           .ThenInclude(x => x.Taxonomy)

           .Include(x => x.Service_at_locations)
           .ThenInclude(x => x.Regular_schedule)

           .Include(x => x.Service_at_locations)
           .ThenInclude(x => x.HolidayScheduleCollection)

           .Include(x => x.Regular_schedules)
           .Include(x => x.Holiday_schedules)

           .Include(x => x.Service_taxonomys)
           .ThenInclude(x => x.Taxonomy)
           .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        if (dbServiceEntity == null)
        {
            throw new NotFoundException(nameof(OpenReferralService), request.Id);
        }

        return dbServiceEntity;
    }

    private void UpdateServiceType(UpdateOpenReferralServiceCommand request, OpenReferralService dbServiceEntity)
    {
        if (request.OpenReferralService != null && request.OpenReferralService.ServiceType != null)
        {
            var serviceType = _context.ServiceTypes.FirstOrDefault(x => x.Id == request.OpenReferralService.ServiceType.Id);
            if (serviceType != null)
                dbServiceEntity.ServiceType = serviceType;
        }
    }

    private static void UpdateService(UpdateOpenReferralServiceCommand request, OpenReferralService dbServiceEntity)
    {
        if (request.OpenReferralService != null)
        {
            dbServiceEntity.Name = request.OpenReferralService.Name;
            dbServiceEntity.Description = request.OpenReferralService.Description;
            dbServiceEntity.Accreditations = request.OpenReferralService.Accreditations;
            dbServiceEntity.Assured_date = request.OpenReferralService.Assured_date;
            dbServiceEntity.Attending_access = request.OpenReferralService.Attending_access;
            dbServiceEntity.Attending_type = request.OpenReferralService.Attending_type;
            dbServiceEntity.Deliverable_type = request.OpenReferralService.Deliverable_type;
            dbServiceEntity.Status = request.OpenReferralService.Status;
            dbServiceEntity.Url = request.OpenReferralService.Url;
            dbServiceEntity.Email = request.OpenReferralService.Email;
            dbServiceEntity.Fees = request.OpenReferralService.Fees;
        }
    }

    private void UpdateLanguages(UpdateOpenReferralServiceCommand request, OpenReferralService dbServiceEntity)
    {
        if (dbServiceEntity.Languages.Serialize() != request?.OpenReferralService?.Languages?.Serialize())
        {
            var updatedLanguages = request?.OpenReferralService?.Languages ?? new Collection<OpenReferralLanguageDto>();
            var existingLanguages = dbServiceEntity.Languages;
            List<string> currentIds = new();

            foreach (var updatedLanguage in updatedLanguages)
            {
                var currentLanguage = existingLanguages.FirstOrDefault(x => x.Language == updatedLanguage.Language && x.OpenReferralServiceId == request.OpenReferralService.Id);
                if (currentLanguage == null)
                {
                    var entity = _mapper.Map<OpenReferralLanguage>(updatedLanguage);
                    entity.OpenReferralServiceId = request.OpenReferralService.Id;
                    entity.RegisterDomainEvent(new OpenReferralLanguageCreatedEvent(entity));
                    _context.OpenReferralLanguages.Add(entity);
                    currentIds.Add(entity.Id);
                }
                else
                {
                    currentLanguage.Language = updatedLanguage?.Language;
                    currentIds.Add(currentLanguage.Id);
                }
            }
            var dataToDelete = existingLanguages.Where(a => !currentIds.Contains(a.Id)).ToList();
            if (dataToDelete != null && dataToDelete.Any())
                _context.OpenReferralLanguages.RemoveRange(dataToDelete);
        }
    }

    private void UpdateServiceTaxonomies(UpdateOpenReferralServiceCommand request, OpenReferralService dbServiceEntity)
    {
        if (dbServiceEntity.Service_taxonomys?.Serialize() != request?.OpenReferralService?.Service_taxonomys?.Serialize())
        {
            var updatedServiceTaxonomies = request?.OpenReferralService.Service_taxonomys ?? new Collection<OpenReferralServiceTaxonomyDto>();
            var existingServiceTaxonomies = dbServiceEntity?.Service_taxonomys;

            List<string> currentIds = new();
            foreach (var updatedServiceTaxonomy in updatedServiceTaxonomies)
            {
                var currentServiceTaxonomy = existingServiceTaxonomies.FirstOrDefault(x => x.Id == updatedServiceTaxonomy.Id);
                if (currentServiceTaxonomy == null)
                {
                    var entity = _mapper.Map<OpenReferralService_Taxonomy>(updatedServiceTaxonomy);
                    if (updatedServiceTaxonomy.Taxonomy != null)
                        entity.Taxonomy = _context.OpenReferralTaxonomies.FirstOrDefault(x => x.Id == updatedServiceTaxonomy.Taxonomy.Id);
                    entity.OpenReferralServiceId = request.OpenReferralService.Id;
                    _context.OpenReferralService_Taxonomies.Add(entity);
                    currentIds.Add(entity.Id);
                    entity.RegisterDomainEvent(new OpenReferralServiceTaxonomyCreatedEvent(entity));
                }
                else
                {
                    if (currentServiceTaxonomy != null && updatedServiceTaxonomy.Taxonomy != null)
                    {
                        currentServiceTaxonomy.Name = updatedServiceTaxonomy.Taxonomy.Name;
                        currentServiceTaxonomy.Vocabulary = updatedServiceTaxonomy.Taxonomy.Vocabulary;
                        currentServiceTaxonomy.Parent = updatedServiceTaxonomy.Taxonomy.Parent;
                    }

                    //currentIds.Add(currentServiceTaxonomy.Id);
                    var entity = _mapper.Map<OpenReferralService_Taxonomy>(updatedServiceTaxonomy);
                    if (updatedServiceTaxonomy.Taxonomy != null)
                        entity.Taxonomy = _context.OpenReferralTaxonomies.FirstOrDefault(x => x.Id == updatedServiceTaxonomy.Taxonomy.Id);
                    entity.OpenReferralServiceId = request.OpenReferralService.Id;
                    _context.OpenReferralService_Taxonomies.Add(entity);
                    currentIds.Add(entity.Id);
                    entity.RegisterDomainEvent(new OpenReferralServiceTaxonomyCreatedEvent(entity));
                }
            }

            var dataToDelete = existingServiceTaxonomies?.Where(a => !currentIds.Contains(a.Id)).ToList();
            if(dataToDelete != null)
            {
                if (dataToDelete.Any())
                    _context.OpenReferralService_Taxonomies.RemoveRange(dataToDelete);
            }
        }
    }

    private void UpdateCostOptions(UpdateOpenReferralServiceCommand request, OpenReferralService dbServiceEntity)
    {
        if (dbServiceEntity.Cost_options?.Serialize() != request?.OpenReferralService?.Cost_options?.Serialize())
        {
            var updatedCostOptions = request?.OpenReferralService?.Cost_options ?? new Collection<OpenReferralCostOptionDto>();
            var existingCostOptions = dbServiceEntity.Cost_options;

            List<string> currentIds = new();
            foreach (var updatedCostOption in updatedCostOptions)
            {
                var currentCostOption = existingCostOptions?.FirstOrDefault(x => x.Id == updatedCostOption?.Id);
                if (currentCostOption == null)
                {
                    var entity = _mapper.Map<OpenReferralCost_Option>(updatedCostOption);
                    entity.OpenReferralServiceId = request.OpenReferralService.Id;
                    _context.OpenReferralCost_Options.Add(entity);
                    currentIds?.Add(entity.Id);
                    entity.RegisterDomainEvent(new OpenReferralCostOptionCreatedEvent(entity));
                }
                else
                {
                    currentCostOption.LinkId = updatedCostOption.LinkId;
                    currentCostOption.Amount = updatedCostOption.Amount;
                    currentCostOption.Amount_description = updatedCostOption.Amount_description;
                    currentCostOption.Option = updatedCostOption.Option;
                    currentCostOption.Valid_from = updatedCostOption.Valid_from;
                    currentCostOption.Valid_to = updatedCostOption.Valid_to;
                    currentIds?.Add(currentCostOption.Id);
                }
            }

            var dataToDelete = existingCostOptions?.Where(a => !currentIds.Contains(a.Id)).ToList();
            if (dataToDelete.Any())
                _context.OpenReferralCost_Options.RemoveRange(dataToDelete);
        }
    }

    private void UpdateServiceArea(UpdateOpenReferralServiceCommand request, OpenReferralService dbServiceEntity)
    {
        if (dbServiceEntity.Service_areas.Serialize() != request?.OpenReferralService.Service_areas?.Serialize())
        {
            var updatedServiceAreas = request?.OpenReferralService.Service_areas ?? new Collection<OpenReferralServiceAreaDto>();
            var existingServiceAreas = dbServiceEntity.Service_areas;
            List<string> currentIds = new();

            foreach (var updatedServiceArea in updatedServiceAreas)
            {
                var currentupdatedServiceArea = existingServiceAreas.FirstOrDefault(x => x.Id == updatedServiceArea.Id);
                if (currentupdatedServiceArea == null)
                {
                    var entity = _mapper.Map<OpenReferralService_Area>(updatedServiceArea);
                    entity.OpenReferralServiceId = request.OpenReferralService.Id;
                    _context.OpenReferralService_Areas.Add(entity);
                    currentIds.Add(entity.Id);
                    entity.RegisterDomainEvent(new OpenReferralServiceAreaCreatedEvent(entity));
                }
                else
                {
                    currentupdatedServiceArea.Service_area = updatedServiceArea.Service_area;
                    currentupdatedServiceArea.Extent = updatedServiceArea.Extent;
                    currentupdatedServiceArea.Uri = updatedServiceArea.Uri;
                }
            }

            var dataToDelete = existingServiceAreas.Where(a => !currentIds.Contains(a.Id)).ToList();
            if (dataToDelete.Any())
                _context.OpenReferralService_Areas.RemoveRange(dataToDelete);
        }
    }

    private void UpdateEligibility(UpdateOpenReferralServiceCommand request, OpenReferralService dbServiceEntity)
    {
        if (dbServiceEntity.Eligibilities.Serialize() != request?.OpenReferralService?.Eligibilities?.Serialize())
        {
            var updatedEligibilities = request?.OpenReferralService?.Eligibilities ?? new Collection<OpenReferralEligibilityDto>();
            var existingEligibilities = dbServiceEntity.Eligibilities;

            List<string> currentIds = new();
            foreach (var updatedEligibility in updatedEligibilities)
            {
                var currentEligibility = existingEligibilities.FirstOrDefault(x => x.Id == updatedEligibility.Id);
                if (currentEligibility == null)
                {
                    var entity = _mapper.Map<OpenReferralEligibility>(updatedEligibility);
                    entity.OpenReferralServiceId = request.OpenReferralService.Id;
                    entity.RegisterDomainEvent(new OpenReferralEligibilityCreatedEvent(entity));
                    _context.OpenReferralEligibilities.Add(entity);
                    currentIds.Add(entity.Id);
                }
                else
                {
                    currentIds.Add(updatedEligibility.Id);
                    currentEligibility.Eligibility = updatedEligibility.Eligibility;
                    currentEligibility.Maximum_age = updatedEligibility.Maximum_age;
                    currentEligibility.Minimum_age = updatedEligibility.Minimum_age;
                }
            }

            var dataToDelete = existingEligibilities.Where(a => !currentIds.Contains(a.Id)).ToList();
            if (dataToDelete.Any())
                _context.OpenReferralEligibilities.RemoveRange(dataToDelete);
        }
    }

    private void UpdateServiceDelivery(UpdateOpenReferralServiceCommand request, OpenReferralService dbServiceEntity)
    {
        if (dbServiceEntity.ServiceDelivery.Serialize() != request?.OpenReferralService.ServiceDelivery?.Serialize())
        {
            var updatedServiceDeliveries = request?.OpenReferralService.ServiceDelivery ?? new Collection<OpenReferralServiceDeliveryExDto>();
            var existingServiceDeliveries = dbServiceEntity.ServiceDelivery;
            List<string> currentIds = new();

            foreach (var updatedServiceDelivery in updatedServiceDeliveries)
            {
                var currentServiceDelivery = existingServiceDeliveries.FirstOrDefault(x => x.Id == updatedServiceDelivery.Id);
                if (currentServiceDelivery == null)
                {
                    var entity = _mapper.Map<OpenReferralServiceDelivery>(updatedServiceDelivery);
                    entity.OpenReferralServiceId = request.OpenReferralService.Id;
                    _context.OpenReferralServiceDeliveries.Add(entity);
                    currentIds.Add(entity.Id);
                    entity.RegisterDomainEvent(new OpenReferralServiceDeliveryCreatedEvent(entity));
                }
                else
                {
                    currentServiceDelivery.ServiceDelivery = updatedServiceDelivery.ServiceDelivery;
                    currentIds.Add(currentServiceDelivery.Id);
                }
            }

            var dataToDelete = existingServiceDeliveries.Where(a => !currentIds.Contains(a.Id)).ToList();
            if (dataToDelete.Any())
                _context.OpenReferralServiceDeliveries.RemoveRange(dataToDelete);
        }
    }

    //private void UpdateContacts(UpdateOpenReferralServiceCommand request, OpenReferralService dbServiceEntity)
    //{
    //    if (dbServiceEntity.ServiceDelivery.Serialize() != request?.OpenReferralService.ServiceDelivery?.Serialize())
    //    {
    //        var updatedContacts = request?.OpenReferralService.Contacts ?? new Collection<OpenReferralContactDto>();
    //        var existingContacts = dbServiceEntity.Contacts;
    //        List<string> contactIds = new();
    //        List<string> phoneIds = new();

    //        foreach (var updatedContact in updatedContacts)
    //        {
    //            var currentContact = existingContacts.FirstOrDefault(x => x.Id == updatedContact.Id);
    //            if (currentContact == null)
    //            {
    //                List<OpenReferralPhone> listPhones = new();
    //                if (updatedContact.Phones != null && updatedContact.Phones.Any())
    //                {
    //                    foreach (var phone in updatedContact.Phones)
    //                    {
    //                        var existingphone = _context.OpenReferralPhones.FirstOrDefault(x => x.Id == phone.Id);
    //                        if (existingphone == null)
    //                        {
    //                            var phoneentity = _mapper.Map<OpenReferralPhone>(phone);
    //                            phoneentity.OpenReferralContactId = updatedContact.Id;
    //                            phoneentity.RegisterDomainEvent(new OpenReferralPhoneCreatedEvent(phoneentity));
    //                            _context.OpenReferralPhones.Add(phoneentity);
    //                            listPhones.Add(phoneentity);
    //                            phoneIds.Add(phoneentity.Id);
    //                        }
    //                        else
    //                        {
    //                            existingphone.Number = phone.Number;
    //                            phoneIds.Add(existingphone.Id);
    //                        }

    //                    }
    //                }

    //                var entity = _mapper.Map<OpenReferralContact>(updatedContact);
    //                entity.OpenReferralServiceId = request.OpenReferralService.Id;
    //                entity.Phones = listPhones;
    //                _context.OpenReferralContacts.Add(entity);
    //                contactIds.Add(entity.Id);
    //                entity.RegisterDomainEvent(new OpenReferralContactCreatedEvent(entity));
    //            }
    //            else
    //            {
    //                currentContact.Title = updatedContact.Title;
    //                currentContact.Name = updatedContact.Name;
    //                if (updatedContact.Phones != null)
    //                {
    //                    foreach (var phone in updatedContact.Phones)
    //                    {
    //                        var existingphone = _context.OpenReferralPhones.FirstOrDefault(x => x.Id == phone.Id);
    //                        if (existingphone == null)
    //                        {
    //                            var entity = _mapper.Map<OpenReferralPhone>(phone);
    //                            entity.OpenReferralContactId = currentContact.Id;
    //                            entity.RegisterDomainEvent(new OpenReferralPhoneCreatedEvent(entity));
    //                            _context.OpenReferralPhones.Add(entity);
    //                            phoneIds.Add(entity.Id);
    //                        }
    //                        else
    //                        {
    //                            existingphone.Number = phone.Number;
    //                            phoneIds.Add(existingphone.Id);
    //                        }
    //                    }
    //                }
    //                contactIds.Add(currentContact.Id);
    //            }
    //        }

    //        foreach (var contact in existingContacts)
    //        {
    //            if (contact != null && contact.Phones != null)
    //            {
    //                foreach (var phone in contact.Phones)
    //                {
    //                    if (!phoneIds.Contains(phone.Id))
    //                    {
    //                        _context.OpenReferralPhones.Remove(phone);
    //                    }
    //                }
    //            }
    //        }

    //        var contactToDelete = existingContacts.Where(a => !existingContacts.Select(x => x.Id).Contains(a.Id)).ToList();
    //        if (contactToDelete != null && contactToDelete.Any())
    //        {
    //            _context.OpenReferralContacts.RemoveRange(contactToDelete);
    //        }
    //    }
    //}

    private void UpdateRegularSchedule(UpdateOpenReferralServiceCommand request, OpenReferralService dbServiceEntity, OpenReferralServiceAtLocation? serviceAtlocation)
    {
        if (dbServiceEntity.Regular_schedules?.Serialize() != request?.OpenReferralService?.RegularSchedules?.Serialize())
        {
            var updatedRegularSchedules = request?.OpenReferralService?.RegularSchedules ?? new Collection<OpenReferralRegularScheduleDto>();
            var existingRegularSchedules = dbServiceEntity.Regular_schedules;
            List<string> currentIds = new();

            foreach (var updatedSchedule in updatedRegularSchedules)
            {
                var currentSchedule = existingRegularSchedules?.FirstOrDefault(x => x.Id == updatedSchedule.Id);
                if (currentSchedule == null)
                {
                    var entity = _mapper.Map<OpenReferralRegular_Schedule>(updatedSchedule);
                    if (serviceAtlocation != null)
                        entity.OpenReferralServiceAtLocationId = serviceAtlocation.Id;
                    entity.RegisterDomainEvent(new OpenReferralRegularScheduleCreatedEvent(entity));
                    _context.OpenReferralRegular_Schedules.Add(entity);
                    currentIds.Add(entity.Id);
                }
                else
                {
                    currentSchedule.Description = updatedSchedule.Description;
                    currentSchedule.Opens_at = updatedSchedule.Opens_at;
                    currentSchedule.Closes_at = updatedSchedule.Closes_at;
                    currentSchedule.Byday = updatedSchedule.Byday;
                    currentSchedule.Bymonthday = updatedSchedule.Bymonthday;
                    currentSchedule.Dtstart = updatedSchedule.Dtstart;
                    currentSchedule.Freq = updatedSchedule.Freq;
                    currentSchedule.Interval = updatedSchedule.Interval;
                    currentSchedule.Valid_from = updatedSchedule.Valid_from;
                    currentSchedule.Valid_to = updatedSchedule.Valid_to;
                }
            }

            var dataToDelete = existingRegularSchedules?.Where(a => !currentIds.Contains(a.Id)).ToList();
            if (dataToDelete.Any())
                _context.OpenReferralRegular_Schedules.RemoveRange(dataToDelete);
        }
    }

    private void UpdateHolidaySchedule(UpdateOpenReferralServiceCommand request, OpenReferralService dbServiceEntity, OpenReferralServiceAtLocation? serviceAtlocation)
    {
        if (dbServiceEntity.Holiday_schedules?.Serialize() != request?.OpenReferralService?.HolidaySchedules?.Serialize())
        {
            var updatedHolidaySchedules = request?.OpenReferralService?.HolidaySchedules ?? new Collection<OpenReferralHolidayScheduleDto>();
            var existingHolidaySchedules = dbServiceEntity.Holiday_schedules;
            List<string> currentIds = new();

            foreach (var updatedSchedule in updatedHolidaySchedules)
            {
                var currentSchedule = existingHolidaySchedules?.FirstOrDefault(x => x.Id == updatedSchedule.Id);
                if (currentSchedule == null)
                {
                    var entity = _mapper.Map<OpenReferralHoliday_Schedule>(updatedSchedule);
                    if (serviceAtlocation != null)
                        entity.OpenReferralServiceAtLocationId = serviceAtlocation.Id;
                    _context.OpenReferralHoliday_Schedules.Add(entity);
                    currentIds.Add(entity.Id);
                    entity.RegisterDomainEvent(new OpenReferralHolidayScheduleCreatedEvent(entity));
                }
                else
                {
                    currentSchedule.Closed = updatedSchedule.Closed;
                    currentSchedule.Closes_at = updatedSchedule.Closes_at;
                    currentSchedule.Start_date = updatedSchedule.Start_date;
                    currentSchedule.End_date = updatedSchedule.End_date;
                    currentSchedule.Opens_at = updatedSchedule.Opens_at;
                }
            }

            var dataToDelete = existingHolidaySchedules.Where(a => !currentIds.Contains(a.Id)).ToList();
            if (dataToDelete.Any())
                _context.OpenReferralHoliday_Schedules.RemoveRange(dataToDelete);
        }
    }

    private void UpdateServiceAtLocation(UpdateOpenReferralServiceCommand request, OpenReferralService dbServiceEntity)
    {
        if (request != null && dbServiceEntity != null)
        {
            var requestServiceAtLocationDtos = request.OpenReferralService?.Service_at_locations ?? new Collection<OpenReferralServiceAtLocationDto>();
            var dbServiceAtLocationEntities = dbServiceEntity.Service_at_locations ?? new Collection<OpenReferralServiceAtLocation>();

            // Check if there's any differences between the request ServiceAtLocations and the db ServiceAtLocations
            if (requestServiceAtLocationDtos.Any() && requestServiceAtLocationDtos.Serialize() != dbServiceAtLocationEntities.Serialize())
            {
                List<string> newServiceAtLocationsIdlist = new();
                List<string> contactDtoList = new();
                List<string> list = new();
                List<string> listAddress = new();

                // Process the request ServiceAtLocationDtos (Add or Update in the db as appropriate)
                foreach (var requestServiceAtLocationDto in requestServiceAtLocationDtos)
                {
                    // Check if this requestServiceAtLocationDto exists in the db (dbServiceAtLocationEntities)
                    var existingDbServiceAtLocationEntity = dbServiceAtLocationEntities.FirstOrDefault(x => x.Id == requestServiceAtLocationDto.Id);
                    if (existingDbServiceAtLocationEntity == null)
                    {
                        // This requestServiceAtLocationDto doesn't exist in the database so create a new ServiceAtLocationEntity from the requestServiceAtLocationDto
                        var newDbServiceAtLocationEntity = _mapper.Map<OpenReferralServiceAtLocation>(requestServiceAtLocationDto);
                        newDbServiceAtLocationEntity.OpenReferralServiceId = request.OpenReferralService?.Id;

                        // Add the new ServiceAtLocationEntity to the db context
                        _context.OpenReferralServiceAtLocations.Add(newDbServiceAtLocationEntity);
                        newDbServiceAtLocationEntity.RegisterDomainEvent(new OpenReferralServiceAtLocationCreatedEvent(newDbServiceAtLocationEntity));

                        // Keep track of the id's of the newly added ServiceAtLocationEntity
                        newServiceAtLocationsIdlist.Add(newDbServiceAtLocationEntity.Id);

                        // Check if there are any LinkContacts associated with the requestServiceAtLocationDto
                        if (requestServiceAtLocationDto.LinkContacts != null)
                        {
                            // Process the requestLinkedContacts
                            foreach (var requestLinkContactDto in requestServiceAtLocationDto.LinkContacts)
                            {
                                // Create new DbLinkContactEntities
                                var newDbLinkContactEntity = _mapper.Map<OpenReferralLinkContact>(requestLinkContactDto);
                                newDbLinkContactEntity.Id = requestLinkContactDto.Id;

                                // Check if there are any Contacts associated with the requestLinkContactDto
                                // Add the new newDbLinkContactEntity to the db context
                                _context.OpenReferralLinkContact.Add(newDbLinkContactEntity);
                                newDbServiceAtLocationEntity.RegisterDomainEvent(new OpenReferralServiceAtLocationCreatedEvent(newDbServiceAtLocationEntity));

                                // Keep track of the id's of the newly added ServiceAtLocationEntity
                                newServiceAtLocationsIdlist.Add(newDbServiceAtLocationEntity.Id);
                            }
                        }
                    }
                }
            }
        }
    }
}


 
    //private void UpdateServiceAtLocation_Locations(UpdateOpenReferralServiceCommand request, ICollection<OpenReferralServiceAtLocation> existingServiceAtLocations, List<string> list, List<string> listAddress, OpenReferralServiceAtLocationDto updatedServiceAtLocation)
    //{
    //    var currentServiceAtLocation = existingServiceAtLocations.FirstOrDefault(x => x.Id == updatedServiceAtLocation.Id);
    //    if (currentServiceAtLocation == null)
    //    {
    //        var entity = _mapper.Map<OpenReferralServiceAtLocation>(updatedServiceAtLocation);
    //        entity.OpenReferralServiceId = request.OpenReferralService.Id;

    //        _context.OpenReferralServiceAtLocations.Add(entity);
    //        entity.RegisterDomainEvent(new OpenReferralServiceAtLocationCreatedEvent(entity));

    //        list.Add(entity.Id);
    //        if (entity.Location.Physical_addresses != null)
    //        {
    //            foreach (var address in entity.Location.Physical_addresses)
    //            {
    //                listAddress.Add(address.Id);
    //            }
    //        }

    //    }
    //    else
    //    {
    //        currentServiceAtLocation.Location.Name = updatedServiceAtLocation.Location.Name;
    //        currentServiceAtLocation.Location.Description = updatedServiceAtLocation.Location.Description;
    //        currentServiceAtLocation.Location.Latitude = updatedServiceAtLocation.Location.Latitude;
    //        currentServiceAtLocation.Location.Longitude = updatedServiceAtLocation.Location.Longitude;

    //        if (updatedServiceAtLocation.Location.Physical_addresses != null)
    //        {
    //            foreach (var address in updatedServiceAtLocation.Location.Physical_addresses)
    //            {
    //                var currentAddress = _context.OpenReferralPhysical_Addresses.FirstOrDefault(x => x.Id == address.Id);
    //                if (currentAddress == null)
    //                {
    //                    var entity = _mapper.Map<OpenReferralPhysical_Address>(address);
    //                    entity.RegisterDomainEvent(new OpenReferralPhysicalAddressCreatedEvent(entity));
    //                    _context.OpenReferralPhysical_Addresses.Add(entity);
    //                    listAddress.Add(entity.Id);
    //                }
    //                else
    //                {
    //                    currentAddress.Address_1 = address.Address_1;
    //                    currentAddress.City = address.City;
    //                    currentAddress.Postal_code = address.Postal_code;
    //                    currentAddress.Country = address.Country;
    //                    currentAddress.State_province = address.State_province;
    //                    listAddress.Add(currentAddress.Id);
    //                }
    //            }
    //            //if (newDbServiceAtLocationEntity.Regular_schedule != null && existingDbServiceAtLocationEntity.Regular_schedule != null && existingDbServiceAtLocationEntity.Regular_schedule.Serialize() != newDbServiceAtLocationEntity.Regular_schedule.Serialize())
    //            //{
    //            //    UpdateRegularSchedule(request, existingDbServiceAtLocationEntity.Regular_schedule ?? new Collection<OpenReferralRegular_Schedule>(), newDbServiceAtLocationEntity.Regular_schedule ?? new Collection<OpenReferralRegularScheduleDto>(), existingDbServiceAtLocationEntity);
    //            //}
    //            //if (existingDbServiceAtLocationEntity.HolidayScheduleCollection != null && newDbServiceAtLocationEntity.HolidayScheduleCollection != null && existingDbServiceAtLocationEntity.HolidayScheduleCollection.Serialize() != newDbServiceAtLocationEntity.HolidayScheduleCollection.Serialize())
    //            //{
    //            //    UpdateHolidaySchedule(existingDbServiceAtLocationEntity.HolidayScheduleCollection ?? new Collection<OpenReferralHoliday_Schedule>(), newDbServiceAtLocationEntity.HolidayScheduleCollection ?? new Collection<OpenReferralHolidayScheduleDto>(), existingDbServiceAtLocationEntity);
    //            //}
    //        }
    //        if (updatedServiceAtLocation != null && updatedServiceAtLocation.Location.LinkTaxonomies.Any())
    //        {
    //            foreach (var linkTaxonomyDto in updatedServiceAtLocation.Location.LinkTaxonomies)
    //            {
    //                var linkTaxonomy = _context.OpenReferralLinkTaxonomies.SingleOrDefault(p => p.Id == linkTaxonomyDto.Id);
    //                if (linkTaxonomy != null)
    //                {
    //                    linkTaxonomy.LinkType = linkTaxonomyDto.LinkType;
    //                    linkTaxonomy.LinkId = linkTaxonomyDto.LinkId;

    //                    if (linkTaxonomyDto.Taxonomy != null)
    //                    {
    //                        var taxonomy = _context.OpenReferralTaxonomies.FirstOrDefault(x => linkTaxonomy.Taxonomy != null && x.Id == linkTaxonomy.Taxonomy.Id);
    //                        if (taxonomy != null)
    //                        {
    //                            linkTaxonomy.Taxonomy = taxonomy;
    //                        }
    //                    }
    //                }
    //                else
    //                {
    //                    var linkTaxonomyEntity = _mapper.Map<OpenReferralLinkTaxonomy>(linkTaxonomyDto);

    //                    if (linkTaxonomyEntity.Taxonomy != null)
    //                    {
    //                        var taxonomy = _context.OpenReferralTaxonomies.FirstOrDefault(x => x.Id == linkTaxonomyEntity.Taxonomy.Id);
    //                        if (taxonomy != null)
    //                        {
    //                            linkTaxonomyEntity.Taxonomy = taxonomy;
    //                        }
    //                    }

    //                    ArgumentNullException.ThrowIfNull(linkTaxonomyEntity, nameof(linkTaxonomyEntity));

    //                    linkTaxonomyEntity.RegisterDomainEvent(new OpenReferralLinkTaxonomyCreatedEvent(linkTaxonomyEntity));

    //                    _context.OpenReferralLinkTaxonomies.Add(linkTaxonomyEntity);
    //                }
    //            }
    //        }

    //        list.Add(currentServiceAtLocation.Id);
    //    }
    //}

    //private void UpdateLinkContacts(UpdateOpenReferralServiceCommand request, OpenReferralService dbServiceEntity)
    //{
    //    if (dbServiceEntity.Link_Contacts.Serialize() != request?.OpenReferralService.LinkContacts?.Serialize())
    //    {
    //        var updatedLinkContacts = request?.OpenReferralService.LinkContacts ?? new Collection<OpenReferralLinkContactDto>();
    //        var existingLinkContacts = dbServiceEntity.Link_Contacts;
    //        List<string> contactIds = new();
    //        List<string> phoneIds = new();

    //        foreach (var updatedLinkContact in updatedLinkContacts)
    //        {
    //            var currentLinkContact = existingLinkContacts.FirstOrDefault(x => x.Id == updatedLinkContact.Id);
    //            if (currentLinkContact == null)
    //            {
    //                // Not using the Phone entity
    //                //List<OpenReferralPhone> listPhones = new();
    //                //if (updatedContact.Phones != null && updatedContact.Phones.Any())
    //                //{
    //                //    foreach (var phone in updatedContact.Phones)
    //                //    {
    //                //        var existingphone = _context.OpenReferralPhones.FirstOrDefault(x => x.Id == phone.Id);
    //                //        if (existingphone == null)
    //                //        {
    //                //            var phoneentity = _mapper.Map<OpenReferralPhone>(phone);
    //                //            phoneentity.OpenReferralContactId = updatedContact.Id;
    //                //            phoneentity.RegisterDomainEvent(new OpenReferralPhoneCreatedEvent(phoneentity));
    //                //            _context.OpenReferralPhones.Add(phoneentity);
    //                //            listPhones.Add(phoneentity);
    //                //            phoneIds.Add(phoneentity.Id);
    //                //        }
    //                //        else
    //                //        {
    //                //            existingphone.Number = phone.Number;
    //                //            phoneIds.Add(existingphone.Id);
    //                //        }

    //                //    }
    //                //}

    //                //var entity = _mapper.Map<OpenReferralContact>(updatedContact);
    //                //entity.OpenReferralServiceId = request.OpenReferralService.Id;
    //                //entity.Phones = listPhones;
    //                //_context.OpenReferralContacts.Add(entity);
    //                //contactIds.Add(entity.Id);
    //                //entity.RegisterDomainEvent(new OpenReferralContactCreatedEvent(entity));
    //            }
    //            else
    //            {
    //                current.Title = updatedContact.Title;
    //                current.Name = updatedContact.Name;
    //                if (updatedContact.Phones != null)
    //                {
    //                    foreach (var phone in updatedContact.Phones)
    //                    {
    //                        var existingphone = _context.OpenReferralPhones.FirstOrDefault(x => x.Id == phone.Id);
    //                        if (existingphone == null)
    //                        {
    //                            var entity = _mapper.Map<OpenReferralPhone>(phone);
    //                            entity.OpenReferralContactId = current.Id;
    //                            entity.RegisterDomainEvent(new OpenReferralPhoneCreatedEvent(entity));
    //                            _context.OpenReferralPhones.Add(entity);
    //                            phoneIds.Add(entity.Id);
    //                        }
    //                        else
    //                        {
    //                            existingphone.Number = phone.Number;
    //                            phoneIds.Add(existingphone.Id);
    //                        }
    //                    }
    //                }
    //                contactIds.Add(current.Id);
    //            }
    //        }

    //        foreach (var contact in existingContacts)
    //        {
    //            if (contact != null && contact.Phones != null)
    //            {
    //                foreach (var phone in contact.Phones)
    //                {
    //                    if (!phoneIds.Contains(phone.Id))
    //                    {
    //                        _context.OpenReferralPhones.Remove(phone);
    //                    }
    //                }
    //            }
    //        }

    //        var contactToDelete = existingContacts.Where(a => !existingContacts.Select(x => x.Id).Contains(a.Id)).ToList();
    //        if (contactToDelete != null && contactToDelete.Any())
    //        {
    //            _context.OpenReferralContacts.RemoveRange(contactToDelete);
    //        }
    //    }
    //}
//}


