using Ardalis.GuardClauses;
using AutoMapper;
using FamilyHubs.ServiceDirectory.Data.Entities;
using FamilyHubs.ServiceDirectory.Data.Entities.Base;
using FamilyHubs.ServiceDirectory.Data.Repository;
using FamilyHubs.ServiceDirectory.Shared.Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FamilyHubs.ServiceDirectory.Core.Commands.Services.UpdateService;

public class UpdateServiceCommand : IRequest<long>
{
    public UpdateServiceCommand(long id, ServiceDto service)
    {
        Id = id;
        Service = service;
    }

    public ServiceDto Service { get; }

    public long Id { get; }
}

public class UpdateServiceCommandHandler : IRequestHandler<UpdateServiceCommand, long>
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<UpdateServiceCommandHandler> _logger;
    private readonly IMapper _mapper;

    public UpdateServiceCommandHandler(ApplicationDbContext context, IMapper mapper, ILogger<UpdateServiceCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<long> Handle(UpdateServiceCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        //Many to Many needs to be included otherwise EF core does not know how to perform merge on navigation tables
        var entity = await _context.Services
            .Include(s => s.Taxonomies)

            .Include(s => s.Locations)
            .ThenInclude(l => l.Contacts)

            .Include(s => s.Locations)
            .ThenInclude(l => l.HolidaySchedules)

            .Include(s => s.Locations)
            .ThenInclude(l => l.RegularSchedules)

            .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        if (entity is null)
            throw new NotFoundException(nameof(Service), request.Id.ToString());

        try
        {
            entity = _mapper.Map(request.Service, entity);

            entity.Locations = AddOrAttachExisting(entity.Locations);
            entity.Taxonomies = AddOrAttachExisting(entity.Taxonomies);

            _context.Services.Update(entity);

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

        var existingIds = unSavedEntities.Where(s => s.Id != 0).Select(s => s.Id).ToList();
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