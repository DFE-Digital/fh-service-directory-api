using AutoMapper;
using FamilyHubs.ServiceDirectory.Api.Helper;
using FamilyHubs.ServiceDirectory.Core.Entities;
using FamilyHubs.ServiceDirectory.Core.Validators;
using FamilyHubs.ServiceDirectory.Infrastructure.Services;
using FamilyHubs.ServiceDirectory.Shared.Dto;
using FamilyHubs.SharedKernel;
using MediatR;

namespace FamilyHubs.ServiceDirectory.Api.Commands.UpsertContact
{
    public class UpsertContactCommand : IRequest<Result<ContactDto>>
    {
        public UpsertContactCommand(ContactDto dto)
        {
            Dto = dto;
        }

        public ContactDto Dto { get; }
    }

    public class UpsertContactCommandHandler : IRequestHandler<UpsertContactCommand, Result<ContactDto>>
    {
        private readonly IContactService _contactService;
        private readonly IMapper _mapper;
        private readonly ILogger<UpsertContactCommandHandler> _logger;
        private readonly ContactDtoValidator _validator;

        public UpsertContactCommandHandler(IContactService contactService, IMapper mapper, ILogger<UpsertContactCommandHandler> logger)
        {
            _contactService = contactService;
            _mapper = mapper;
            _logger = logger;
            _validator = new ContactDtoValidator();
        }

        public async Task<Result<ContactDto>> Handle(UpsertContactCommand request, CancellationToken cancellationToken)
        {
            _logger.LogDebug("UpsertContactCommandHandler.Handle - Begin");

            if (!_validator.ValidateRequest(request.Dto, out var validationResult))
            {
                _logger.LogInformation("UpsertContactCommandHandler.Handle - Validation failed {errors}", string.Join(", ", validationResult.Errors));
                return validationResult;
            }

            var contact = _mapper.Map<Contact>(request.Dto);
            var result = await _contactService.Upsert(contact);
            var contactDto = _mapper.Map<ContactDto>(result);

            _logger.LogDebug("UpsertContactCommandHandler.Handle - Return successfully");
            return Result<ContactDto>.Success(contactDto);
        }
    }

}
