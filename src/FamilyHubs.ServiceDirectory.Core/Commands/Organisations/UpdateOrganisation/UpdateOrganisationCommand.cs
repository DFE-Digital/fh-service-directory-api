using AutoMapper;
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

        var organisation = await _context.Organisations
                .Include(o => o.Services)
                .ThenInclude(s => s.Taxonomies)
                .Include(o => o.Services)
                .ThenInclude(s => s.Locations)
                .AsSplitQuery()
                .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        if (organisation is null)
            throw new NotFoundException(nameof(Organisation), request.Id.ToString());

        try
        {
            organisation = _mapper.Map(request.Organisation, organisation);

            //todo: we need to have an OrganisationChangeDto with LocationChangeDtos and Service ids
            // then all this can go and most of the unit tests too
            organisation.Locations = await organisation.Locations.LinkExistingEntities(_context.Locations, _mapper);

            foreach (var service in organisation.Services)
            {
                service.Locations = await service.Locations.LinkExistingEntities(_context.Locations, _mapper);
                service.AttachExistingManyToMany(_context, _mapper);
            }

            _context.Organisations.Update(organisation);

            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Organisation {Name} saved to DB", request.Organisation.Name);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred updating organisation with Name:{name}.", request.Organisation.Name);
            throw;
        }

        return organisation.Id;
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