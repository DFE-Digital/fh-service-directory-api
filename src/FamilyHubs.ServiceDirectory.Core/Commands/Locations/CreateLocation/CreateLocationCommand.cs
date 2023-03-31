using AutoMapper;
using FamilyHubs.ServiceDirectory.Data.Entities;
using FamilyHubs.ServiceDirectory.Data.Repository;
using FamilyHubs.ServiceDirectory.Shared.Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FamilyHubs.ServiceDirectory.Core.Commands.Locations.CreateLocation;

public class CreateLocationCommand : IRequest<long>
{
    public CreateLocationCommand(LocationDto location)
    {
        Location = location;
    }

    public LocationDto Location { get; }
}

public class CreateLocationCommandHandler : IRequestHandler<CreateLocationCommand, long>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateLocationCommandHandler> _logger;

    public CreateLocationCommandHandler(ApplicationDbContext context, IMapper mapper, ILogger<CreateLocationCommandHandler> logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<long> Handle(CreateLocationCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var entity = await _context.Locations
                .IgnoreAutoIncludes()
                .FirstOrDefaultAsync(x => x.Name+x.PostCode == request.Location.Name+request.Location.PostCode, cancellationToken);

            if (entity is not null)
                throw new ArgumentException($"Duplicate Location with Id:{request.Location.Id} Name:{request.Location.Name} and PostCode:{request.Location.PostCode} Already Exists, Please use Update Command");

            var location = _mapper.Map<Location>(request.Location);

            _context.Locations.Add(location);

            await _context.SaveChangesAsync(cancellationToken);

            return location.Id;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred creating organisation. {exceptionMessage}", ex.Message);
            throw;
        }
    }
}