using Ardalis.GuardClauses;
using AutoMapper;
using FamilyHubs.ServiceDirectory.Data.Entities;
using FamilyHubs.ServiceDirectory.Data.Entities.Base;
using FamilyHubs.ServiceDirectory.Data.Repository;
using FamilyHubs.ServiceDirectory.Shared.Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FamilyHubs.ServiceDirectory.Core.Commands.Organisations.UpdateOrganisation;

public class UpdateOrganisationCommand : IRequest<long>
{
    public UpdateOrganisationCommand(long id, OrganisationWithServicesDto organisation)
    {
        Id = id;
        Organisation = organisation;
    }

    public OrganisationWithServicesDto Organisation { get; }

    public long Id { get; }
}

public class UpdateOrganisationCommandHandler : IRequestHandler<UpdateOrganisationCommand, long>
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<UpdateOrganisationCommandHandler> _logger;
    private readonly IMapper _mapper;

    public UpdateOrganisationCommandHandler(ApplicationDbContext context, IMapper mapper, ILogger<UpdateOrganisationCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<long> Handle(UpdateOrganisationCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var entity = await _context.Organisations
            .Include(o => o.Services)
            .ThenInclude(s => s.Taxonomies)

            .Include(o => o.Services)
            .ThenInclude(s => s.Locations)
            .ThenInclude(l => l.Contacts)

            .Include(o => o.Services)
            .ThenInclude(s => s.Locations)
            .ThenInclude(l => l.HolidaySchedules)

            .Include(o => o.Services)
            .ThenInclude(s => s.Locations)
            .ThenInclude(l => l.RegularSchedules)

            .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        if (entity is null)
            throw new NotFoundException(nameof(Organisation), request.Id.ToString());

        try
        {
            entity = _mapper.Map(request.Organisation, entity);
            foreach (var service in entity.Services)
            {
                service.Locations = AddOrAttachExisting(service.Locations);
                service.Taxonomies = AddOrAttachExisting(service.Taxonomies);
            }

            _context.Organisations.Update(entity);

            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred updating organisation. {exceptionMessage}", ex.Message);
            throw;
        }

        return entity.Id;
    }

    public IList<TEntity> AddOrAttachExisting<TEntity>(IList<TEntity> unSavedEntities) where TEntity : EntityBase<long>
    {
        var returnList = new List<TEntity>();

        if (!unSavedEntities.Any())
            return returnList;

        var existingIds = unSavedEntities.Where(s => s.Id != 0).Select(s => s.Id);
        var existing = _context.Set<TEntity>().Where(x => existingIds.Contains(x.Id)).ToList();

        foreach (var unSavedItem in unSavedEntities)
        {
            var savedItem = existing.SingleOrDefault(x => x.Id == unSavedItem.Id);

            if (savedItem is null)
            {
                returnList.Add(unSavedItem);
            }
            else
            {
                savedItem = _mapper.Map(unSavedItem, savedItem);
                returnList.Add(savedItem);
            }
        }

        return returnList;
    }
}