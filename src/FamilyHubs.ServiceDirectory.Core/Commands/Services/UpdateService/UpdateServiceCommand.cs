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
    private readonly ISender _sender;

    public UpdateServiceCommandHandler(ApplicationDbContext context, IMapper mapper, ISender sender,
        ILogger<UpdateServiceCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
        _mapper = mapper;
        _sender = sender;
    }

    public async Task<long> Handle(UpdateServiceCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        //Many to Many needs to be included otherwise EF core does not know how to perform merge on navigation tables
        var entity = await _context.Services
            .Include(s => s.Taxonomies)
            .Include(s => s.Locations)
            .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        if (entity is null)
            throw new NotFoundException(nameof(Service), request.Id.ToString());

        try
        {
            entity = _mapper.Map(request.Service, entity);

            entity.AttachExistingManyToMany(_context, _mapper);

            _context.Services.Update(entity);

            await _context.SaveChangesAsync(cancellationToken);

            request.Service.Id = entity.Id;
            await SendEventGridMessage(entity, cancellationToken);


        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred updating Service with Id:{ServiceRef}.", entity.ServiceOwnerReferenceId);
            throw;
        }

        return entity.Id;
    }

    private async Task SendEventGridMessage(Service service, CancellationToken cancellationToken)
    {
        Organisation? organisation = _context.Organisations.FirstOrDefault(x => x.Services.Contains(service));

        ArgumentNullException.ThrowIfNull(organisation);

        var eventData = new[]
        {
            new
            {
                Id = Guid.NewGuid(),
                EventType = "ReferralServiceDto",
                Subject = "Service",
                EventTime = DateTime.UtcNow,
                Data = new
                {
                    service.Id,
                    service.Name,
                    service.Description,
                    OrganisationDto = new
                    {
                        organisation.Id,
                        ReferralServiceId = service.Id,
                        organisation.Name,
                        organisation.Description
                    }
                }
            }
        };
        _logger.LogInformation("Sending Service {Name} to the event grid command", service.Name);
        SendEventGridMessageCommand sendEventGridMessageCommand = new(eventData);
        _ = await _sender.Send(sendEventGridMessageCommand, cancellationToken);
        _logger.LogInformation("Service {Name} completed the event grid message", service.Name);
    }
}