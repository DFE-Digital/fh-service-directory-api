﻿using AutoMapper;
using FamilyHubs.ServiceDirectory.Core.Exceptions;
using FamilyHubs.ServiceDirectory.Core.Helper;
using FamilyHubs.ServiceDirectory.Data.Entities;
using FamilyHubs.ServiceDirectory.Data.Repository;
using FamilyHubs.ServiceDirectory.Shared.Dto;
using FamilyHubs.SharedKernel.Identity;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NotFoundException = Ardalis.GuardClauses.NotFoundException;

namespace FamilyHubs.ServiceDirectory.Core.Commands.Organisations.UpdateOrganisation;

public class UpdateOrganisationCommand : IRequest<long>
{
    public UpdateOrganisationCommand(long id, OrganisationDetailsDto organisation)
    {
        Id = id;
        Organisation = organisation;
    }

    public OrganisationDetailsDto Organisation { get; }

    public long Id { get; }
}

public class UpdateOrganisationCommandHandler : IRequestHandler<UpdateOrganisationCommand, long>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ApplicationDbContext _context;
    private readonly ILogger<UpdateOrganisationCommandHandler> _logger;
    private readonly IMapper _mapper;

    public UpdateOrganisationCommandHandler(
        IHttpContextAccessor httpContextAccessor,
        ApplicationDbContext context, 
        IMapper mapper,
        ILogger<UpdateOrganisationCommandHandler> logger)
    {
        _httpContextAccessor = httpContextAccessor;
        _context = context;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<long> Handle(UpdateOrganisationCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        ThrowIfForbidden(request);

        var entity = await _context.Organisations
                .Include(o => o.Services)
                .ThenInclude(s => s.Taxonomies)
                .Include(o => o.Services)
                .ThenInclude(s => s.Locations)
                .AsSplitQuery()
                .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        if (entity is null)
            throw new NotFoundException(nameof(Organisation), request.Id.ToString());

        try
        {
            entity = _mapper.Map(request.Organisation, entity);

            List<Location> newOrgLocs = new();
            foreach (var orgLocation in entity.Locations)
            {
                Location newLoc;
                //or IsKeySet
                if (orgLocation.Id != 0)
                {
                    var existingLocation = await _context.Locations.FindAsync(orgLocation.Id);
                    _mapper.Map(orgLocation, existingLocation);
                    newLoc = existingLocation;
                    //newOrgLocs.Add(existingLocation);
                }
                else
                {
                    //newOrgLocs.Add(location);
                    newLoc = orgLocation;
                }

                newOrgLocs.Add(newLoc);
            }
            //    List<Contact> newContacts = new();
            //    foreach (var contact in newLoc.Contacts)
            //    {
            //        Contact newContact;
            //        //or IsKeySet
            //        if (contact.Id != 0)
            //        {
            //            var existingContact = _context.Contacts.Find(contact.Id);
            //            _mapper.Map(contact, existingContact);
            //            newContact = existingContact;
            //        }
            //        else
            //        {
            //            newContact = contact;
            //        }
            //        newContacts.Add(newContact);
            //    }
            //    newLoc.Contacts = newContacts;
            //}

            entity.Locations = newOrgLocs;

            //var isTracked = _context.Contacts.Local.Any(e => e.Id == newOrgLocs.First().Contacts.First().Id);


            //todo: what happens if e.g. existing location has new contacts?
            foreach (var service in entity.Services)
            {
                List<Location> newLocs = new();
                foreach (var location in service.Locations)
                {
                    //or IsKeySet
                    if (location.Id != 0)
                    {
                        var existingLocation = await _context.Locations.FindAsync(location.Id);
                        _mapper.Map(location, existingLocation);
                        newLocs.Add(existingLocation);
                    }
                    else
                    {
                        newLocs.Add(location);
                    }
                }

                service.Locations = newLocs;
            }
            //    List<Contact> newContacts = new();
            //    foreach (var contact in service.Contacts)
            //    {
            //        //or IsKeySet
            //        if (contact.Id != 0)
            //        {
            //            var existingContact = _context.Contacts.Find(contact.Id);
            //            _mapper.Map(contact, existingContact);
            //            newContacts.Add(existingContact);
            //        }
            //        else
            //        {
            //            newContacts.Add(contact);
            //        }
            //    }
            //    service.Contacts = newContacts;
            //}

            foreach (var service in entity.Services)
            {
                service.AttachExistingManyToMany(_context, _mapper);
            }

            _context.Organisations.Update(entity);

            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Organisation {Name} saved to DB", request.Organisation.Name);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred updating organisation with Name:{name}.", request.Organisation.Name);
            throw;
        }

        return entity.Id;
    }

    private void ThrowIfForbidden(UpdateOrganisationCommand request)
    {
        var user = _httpContextAccessor?.HttpContext?.GetFamilyHubsUser();
        if(user == null)
        {
            _logger.LogError("No user retrieved from HttpContext");
            throw new ForbiddenException("No user detected"); // This should be impossible as Authorization is applied to the endpoint
        }

        if(user.Role == RoleTypes.DfeAdmin || user.Role == RoleTypes.ServiceAccount) 
        {
            return;
        }

        var userOrganisationId = long.Parse(user.OrganisationId);
        if(userOrganisationId == request.Organisation.Id || userOrganisationId == request.Organisation.AssociatedOrganisationId)
        {
            return;
        }

        throw new ForbiddenException("This user cannot update this organisation");
    }
}