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
    //todo: no need to pass the id in the request, it's already in the OrganisationDetailsDto
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
                .Include(o => o.Location)
                .AsSplitQuery()
                .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        if (entity is null)
            throw new NotFoundException(nameof(Organisation), request.Id.ToString());

        try
        {
            entity = _mapper.Map(request.Organisation, entity);

            // note that if locations with the same id are passed to this command (at the organisation and service level)
            // then they should all be the same object. if they are not, then one of the locations will be used and different properties on the other ignored/lost
            //todo: we could catch and throw?
            var distinctExistingLocations = entity.Location
                .Concat(entity.Services.SelectMany(s => s.Locations))
                .Where(l => l.Id != 0)
                .GroupBy(l => l.Id)
                .Select(g => g.First()) // todo: throw if more than one and they aren't the same
                .ToArray();

            foreach (var location in distinctExistingLocations)
            {
                location.AttachExisting(_context, _mapper);
            }

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