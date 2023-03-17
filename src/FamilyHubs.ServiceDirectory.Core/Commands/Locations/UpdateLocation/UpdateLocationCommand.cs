using Ardalis.GuardClauses;
using AutoMapper;
using FamilyHubs.ServiceDirectory.Data.Entities;
using FamilyHubs.ServiceDirectory.Data.Repository;
using FamilyHubs.ServiceDirectory.Shared.Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FamilyHubs.ServiceDirectory.Core.Commands.Locations.UpdateLocation;

public class UpdateLocationCommand : IRequest<long>
{
    public UpdateLocationCommand(long id, LocationDto location)
    {
        Id = id;
        Location = location;
    }

    public LocationDto Location { get; }

    public long Id { get; }
}

public class UpdateLocationCommandHandler : IRequestHandler<UpdateLocationCommand, long>
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<UpdateLocationCommandHandler> _logger;
    private readonly IMapper _mapper;

    public UpdateLocationCommandHandler(ApplicationDbContext context, IMapper mapper, ILogger<UpdateLocationCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<long> Handle(UpdateLocationCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var entity = await _context.Locations
            .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        if (entity is null)
            throw new NotFoundException(nameof(Location), request.Id.ToString());

        try
        {
            entity = _mapper.Map(request.Location, entity);

            _context.Locations.Update(entity);

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