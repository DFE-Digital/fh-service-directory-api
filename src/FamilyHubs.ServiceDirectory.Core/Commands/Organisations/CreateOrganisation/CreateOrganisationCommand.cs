using AutoMapper;
using Azure.Core;
using FamilyHubs.ServiceDirectory.Core.Exceptions;
using FamilyHubs.ServiceDirectory.Core.Helper;
using FamilyHubs.ServiceDirectory.Data.Entities;
using FamilyHubs.ServiceDirectory.Data.Repository;
using FamilyHubs.ServiceDirectory.Shared.Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Threading;

namespace FamilyHubs.ServiceDirectory.Core.Commands.Organisations.CreateOrganisation;

public class CreateOrganisationCommand : IRequest<long>
{
    public CreateOrganisationCommand(OrganisationWithServicesDto organisation)
    {
        Organisation = organisation;
    }

    public OrganisationWithServicesDto Organisation { get; }
}

public class CreateOrganisationCommandHandler : IRequestHandler<CreateOrganisationCommand, long>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ISender _sender;
    private readonly ILogger<CreateOrganisationCommandHandler> _logger;

    public CreateOrganisationCommandHandler(ApplicationDbContext context, IMapper mapper, ISender sender,
        ILogger<CreateOrganisationCommandHandler> logger)
    {
        _context = context;
        _mapper = mapper;
        _sender = sender;
        _logger = logger;
    }

    public async Task<long> Handle(CreateOrganisationCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await ThrowIfOrganisationIdExists(request, cancellationToken);
            await ThrowIfOrganisationNameExists(request, cancellationToken);

            if (request.Organisation.AssociatedOrganisationId is not null)
            {
                var associatedOrganisation =
                    _context.Organisations.SingleOrDefault(o => o.Id == request.Organisation.AssociatedOrganisationId);
                if (associatedOrganisation is null)
                    throw new InvalidOperationException("Invalid Associated Organisation ID");
                request.Organisation.AdminAreaCode = associatedOrganisation.AdminAreaCode;
            }

            var organisation = _mapper.Map<Organisation>(request.Organisation);

            foreach (var service in organisation.Services)
            {
                service.AttachExistingManyToMany(_context, _mapper);
            }

            _context.Organisations.Add(organisation);

            await _context.SaveChangesAsync(cancellationToken);

            request.Organisation.Id = organisation.Id;

            _logger.LogInformation("Organisation {Name} saved to DB", request.Organisation.Name);

            await SendNotifications(organisation.Id, cancellationToken);

            return organisation.Id;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred creating organisation with Name:{name}.", request.Organisation.Name);
            throw;
        }
    }

    private async Task SendNotifications(long organisationId, CancellationToken cancellationToken)
    {

        var organisation = _context.Organisations
                .Include(x => x.Services)
                .SingleOrDefault(x => x.Id == organisationId);

        if (organisation is null)
            return;

        if (organisation.Services != null && organisation.Services.Any())
        {
            foreach (var service in organisation.Services)
            {
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
                SendEventGridMessageCommand serviceSendEventGridMessageCommand = new(eventData);
                _ = await _sender.Send(serviceSendEventGridMessageCommand, cancellationToken);
                _logger.LogInformation("Service {Name} completed the event grid message", service.Name);
            }

            return;
        }    

        _logger.LogInformation("Organisation {Name} sending an event grid message", organisation.Name);

        var organisationEventData = new[]
        {
                new
                {
                    Id = Guid.NewGuid(),
                    EventType = "OrganisationDto",
                    Subject = "Organisation",
                    EventTime = DateTime.UtcNow,
                    Data = organisation
                }
            };
        SendEventGridMessageCommand sendEventGridMessageCommand = new(organisationEventData);
        _ = await _sender.Send(sendEventGridMessageCommand, cancellationToken);
        _logger.LogInformation("Organisation {Name} completed the event grid message", organisation.Name);
    }

    private async Task ThrowIfOrganisationIdExists(CreateOrganisationCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Organisations
        .IgnoreAutoIncludes()
        .FirstOrDefaultAsync(x => x.Id == request.Organisation.Id, cancellationToken);

        if (entity is not null)
            throw new AlreadyExistsException("Duplicate Id");
    }

    private async Task ThrowIfOrganisationNameExists(CreateOrganisationCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Organisations
                .IgnoreAutoIncludes()
                .FirstOrDefaultAsync(x => x.Name == request.Organisation.Name && x.AssociatedOrganisationId == request.Organisation.AssociatedOrganisationId, cancellationToken);

        if (entity is not null)
            throw new AlreadyExistsException("Cannot create an organisation with a name that matches an existing organisation"); 
    }
}