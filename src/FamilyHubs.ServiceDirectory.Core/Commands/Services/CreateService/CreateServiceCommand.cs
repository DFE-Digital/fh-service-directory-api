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
    private readonly ILogger<CreateServiceCommandHandler> _logger;

    public CreateServiceCommandHandler(ApplicationDbContext context, IMapper mapper,
        ILogger<CreateServiceCommandHandler> logger)
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

            //todo: need to do the same here as per update organisation, i.e. attach all existing entities
            // can probably share common code

            service.AttachExistingManyToMany(_context, _mapper);

            _context.Services.Add(service);

            await _context.SaveChangesAsync(cancellationToken);

            return service.Id;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred creating Service with Id:{ServiceRef}.", request.Service.ServiceOwnerReferenceId);
            throw;
        }
    }
}