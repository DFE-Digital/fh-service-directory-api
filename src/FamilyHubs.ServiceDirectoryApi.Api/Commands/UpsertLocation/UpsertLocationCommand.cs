using AutoMapper;
using FamilyHubs.ServiceDirectory.Api.Helper;
using FamilyHubs.ServiceDirectory.Core.Entities;
using FamilyHubs.ServiceDirectory.Core.Validators;
using FamilyHubs.ServiceDirectory.Infrastructure.Domain;
using FamilyHubs.ServiceDirectory.Shared.Dto;
using FamilyHubs.SharedKernel;
using MediatR;

namespace FamilyHubs.ServiceDirectory.Api.Commands.UpsertLocation
{
    public class UpsertLocationCommand : IRequest<Result<LocationDto>>
    {
        public UpsertLocationCommand(LocationDto dto)
        {
            Dto = dto;
        }

        public LocationDto Dto { get; }
    }

    public class UpsertLocationCommandHandler : IRequestHandler<UpsertLocationCommand, Result<LocationDto>>
    {
        private readonly ILocationRootAggregate _locationRootAggregate;
        private readonly ILogger<UpsertLocationCommandHandler> _logger;
        private readonly LocationDtoValidator _validator;
        private readonly IMapper _mapper;

        public UpsertLocationCommandHandler(
            ILocationRootAggregate locationRootAggregate, 
            ILogger<UpsertLocationCommandHandler> logger, 
            IMapper mapper)
        {
            _locationRootAggregate = locationRootAggregate;
            _logger = logger;
            _validator = new LocationDtoValidator();
            _mapper = mapper;
        }

        public async Task<Result<LocationDto>> Handle(UpsertLocationCommand request, CancellationToken cancellationToken)
        {
            _logger.LogDebug("UpsertLocationCommandHandler.Handle - Begin");

            if (!_validator.ValidateRequest(request.Dto, out var validationResult))
            {
                _logger.LogInformation("UpsertLocationCommandHandler.Handle - Validation failed {errors}", string.Join(", ", validationResult.Errors));
                return validationResult;
            }

            var locationWithNewValues = _mapper.Map<Location>(request.Dto);
            var locationRecord = await _locationRootAggregate.Upsert(locationWithNewValues);
            var locationDto = _mapper.Map<LocationDto>(locationRecord);

            _logger.LogDebug("UpsertLocationCommandHandler.Handle - Return successfully");
            return Result<LocationDto>.Success(locationDto);
        }

    }
}
