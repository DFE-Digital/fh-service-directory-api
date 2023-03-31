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

            entity.AttachExistingManyToMany(_context, _mapper);

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
}