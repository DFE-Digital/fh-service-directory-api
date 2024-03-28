using AutoMapper;
using FamilyHubs.ServiceDirectory.Core.Helper;
using FamilyHubs.ServiceDirectory.Data.Entities;
using FamilyHubs.ServiceDirectory.Data.Repository;
using FamilyHubs.ServiceDirectory.Shared.CreateUpdateDto;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FamilyHubs.ServiceDirectory.Core.Commands.Services.CreateService;

public class CreateServiceCommand : IRequest<long>
{
    public CreateServiceCommand(ServiceChangeDto service)
    {
        Service = service;
    }

    public ServiceChangeDto Service { get; }
}

public class CreateServiceCommandHandler : IRequestHandler<CreateServiceCommand, long>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateServiceCommandHandler> _logger;

    public CreateServiceCommandHandler(
        ApplicationDbContext context,
        IMapper mapper,
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
            var service = _mapper.Map<Service>(request.Service);

            service.Taxonomies = await request.Service.TaxonomyIds.GetEntities(_context.Taxonomies);

            _context.Services.Add(service);

            await _context.SaveChangesAsync(cancellationToken);

            return service.Id;
        }
        catch (Exception ex)
        {
            //todo: do we need this exception handler? as long as there's a global exception handler it's not adding much
            _logger.LogError(ex, "An error occurred creating service");
            throw;
        }
    }
}
