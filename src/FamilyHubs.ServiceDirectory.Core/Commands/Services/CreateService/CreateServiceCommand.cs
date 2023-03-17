using AutoMapper;
using FamilyHubs.ServiceDirectory.Data.Entities;
using FamilyHubs.ServiceDirectory.Data.Entities.Base;
using FamilyHubs.ServiceDirectory.Data.Repository;
using FamilyHubs.ServiceDirectory.Shared.Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FamilyHubs.ServiceDirectory.Core.Commands.Services.CreateService;

public class CreateServiceCommand : IRequest<long>
{
    public CreateServiceCommand(ServiceDto service)
    {
        Service = service;
    }

    public ServiceDto Service { get; }
}

public class CreateServiceCommandHandler : IRequestHandler<CreateServiceCommand, long>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateServiceCommandHandler> _logger;

    public CreateServiceCommandHandler(ApplicationDbContext context, IMapper mapper, ILogger<CreateServiceCommandHandler> logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<long> Handle(CreateServiceCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var entity = await _context.Services
                .IgnoreAutoIncludes()
                .FirstOrDefaultAsync(x => x.Id == request.Service.Id, cancellationToken);

            if (entity is not null)
                throw new ArgumentException("Duplicate Id");

            var service = _mapper.Map<Service>(request.Service);

            service.Locations = AddOrAttachExisting(service.Locations);
            service.Taxonomies = AddOrAttachExisting(service.Taxonomies);

            _context.Services.Add(service);

            await _context.SaveChangesAsync(cancellationToken);

            return service.Id;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred creating organisation. {exceptionMessage}", ex.Message);
            throw;
        }
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