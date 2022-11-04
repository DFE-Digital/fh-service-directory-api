using AutoMapper;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.ModelLink;
using fh_service_directory_api.core.Entities;
using fh_service_directory_api.core.Events;
using fh_service_directory_api.infrastructure.Persistence.Repository;
using MediatR;

namespace fh_service_directory_api.api.Commands.CreateModelLink;

public class CreateModelLinkCommand : IRequest<string>
{
    public CreateModelLinkCommand(ModelLinkDto modelLinkDto)
    {
        ModelLinkDto = modelLinkDto;
    }

    public ModelLinkDto ModelLinkDto { get; init; }
}

public class CreateModelLinkCommandHandler : IRequestHandler<CreateModelLinkCommand, string>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateModelLinkCommandHandler> _logger;
    public CreateModelLinkCommandHandler(ApplicationDbContext context, IMapper mapper, ILogger<CreateModelLinkCommandHandler> logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<string> Handle(CreateModelLinkCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var entity = _mapper.Map<ModelLink>(request.ModelLinkDto);
            ArgumentNullException.ThrowIfNull(entity, nameof(entity));

            entity.RegisterDomainEvent(new ModelLinkCreateEvent(entity));

            _context.ModelLinks.Add(entity);

            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred creating UICache. {exceptionMessage}", ex.Message);
            throw new Exception(ex.Message, ex);
        }

        if (request is not null && request.ModelLinkDto is not null)
            return request.ModelLinkDto.Id;
        else
            return string.Empty;
    }
}
