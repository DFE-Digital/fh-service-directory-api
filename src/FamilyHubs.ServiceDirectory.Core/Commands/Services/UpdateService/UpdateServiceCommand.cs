using Ardalis.GuardClauses;
using AutoMapper;
using FamilyHubs.ServiceDirectory.Core.Helper;
using FamilyHubs.ServiceDirectory.Data.Entities;
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

    public UpdateServiceCommandHandler(ApplicationDbContext context, IMapper mapper,
        ILogger<UpdateServiceCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<long> Handle(UpdateServiceCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        //Many to Many needs to be included otherwise EF core does not know how to perform merge on navigation tables
        var service = await _context.Services
            .Include(s => s.Taxonomies)
            .Include(s => s.Locations)
            .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        if (service is null)
            throw new NotFoundException(nameof(Service), request.Id.ToString());

        try
        {
            service = _mapper.Map(request.Service, service);

            service.Locations = await service.Locations.LinkExistingEntities(_context.Locations, _mapper);
            service.AttachExistingManyToMany(_context, _mapper);

            _context.Services.Update(service);

            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred updating Service with Id:{ServiceRef}.", service.ServiceOwnerReferenceId);
            throw;
        }

        return service.Id;
    }
}