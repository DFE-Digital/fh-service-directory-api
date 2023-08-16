using AutoMapper;
using FamilyHubs.ServiceDirectory.Core.Helper;
using FamilyHubs.ServiceDirectory.Data.Entities;
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
    private readonly ISender _sender;
    private readonly ILogger<CreateServiceCommandHandler> _logger;

    public CreateServiceCommandHandler(ApplicationDbContext context, IMapper mapper, ISender sender,
        ILogger<CreateServiceCommandHandler> logger)
    {
        _context = context;
        _mapper = mapper;
        _sender = sender;
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

            service.AttachExistingManyToMany(_context, _mapper);

            _context.Services.Add(service);

            await _context.SaveChangesAsync(cancellationToken);

            request.Service.Id = service.Id;
            await SendEventGridMessage(service, cancellationToken);

            return service.Id;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred creating Service with Id:{ServiceRef}.", request.Service.ServiceOwnerReferenceId);
            throw;
        }
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