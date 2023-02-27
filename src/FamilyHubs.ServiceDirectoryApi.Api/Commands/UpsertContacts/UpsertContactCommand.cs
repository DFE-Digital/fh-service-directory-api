using AutoMapper;
using FamilyHubs.ServiceDirectory.Core.Entities;
using FamilyHubs.ServiceDirectory.Infrastructure.Services;
using FamilyHubs.ServiceDirectory.Shared.Dto;
using FamilyHubs.SharedKernel;
using MediatR;

namespace FamilyHubs.ServiceDirectory.Api.Commands.UpsertContacts
{
    public class UpsertContactCommand : IRequest<Result<ContactDto>>
    {
        public UpsertContactCommand(ContactDto contactDto)
        {
            ContactDto = contactDto;
        }

        public ContactDto ContactDto { get; }
    }

    public class UpsertContactCommandHandler : IRequestHandler<UpsertContactCommand, Result<ContactDto>>
    {
        private readonly IContactService _contactService;
        private readonly IMapper _mapper;
        private readonly ILogger<UpsertContactCommandHandler> _logger;
        private readonly UpsertContactCommandValidator _validator;

        public UpsertContactCommandHandler(IContactService contactService, IMapper mapper, ILogger<UpsertContactCommandHandler> logger)
        {
            _contactService = contactService;
            _mapper = mapper;
            _logger = logger;
            _validator = new UpsertContactCommandValidator();
        }

        public async Task<Result<ContactDto>> Handle(UpsertContactCommand request, CancellationToken cancellationToken)
        {
            _logger.LogDebug("UpsertContactCommandHandler.Handle - Begin");

            if (!ValidateRequest(request, out var validationResult))
            {
                _logger.LogInformation("UpsertContactCommandHandler.Handle - Validation failed {errors}", string.Join(", ", validationResult.Errors));
                return validationResult;
            }

            var contact = _mapper.Map<Contact>(request.ContactDto);
            var result = await _contactService.UpsertContact(contact);
            var contactDto = _mapper.Map<ContactDto>(result);

            _logger.LogDebug("UpsertContactCommandHandler.Handle - Return successfully");
            return Result<ContactDto>.Success(contactDto);
        }

        private bool ValidateRequest(UpsertContactCommand request, out Result<ContactDto> result)
        {
            var validationResult = _validator.Validate(request);

            if (validationResult is null)
            {
                result = Result<ContactDto>.Failure("500", new List<string> { "An error occurred during validation" });
                return false;
            }

            if(!validationResult.IsValid)
            {
                result = Result<ContactDto>.Failure("400", validationResult.Errors.Select(x=>x.ErrorMessage));
                return false;
            }

            result = Result<ContactDto>.Success(request.ContactDto);
            return true;
        }
    }

}
