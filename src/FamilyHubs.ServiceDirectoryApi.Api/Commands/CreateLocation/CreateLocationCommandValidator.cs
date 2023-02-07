using FluentValidation;

namespace FamilyHubs.ServiceDirectory.Api.Commands.CreateLocation
{
    public class CreateLocationCommandValidator : AbstractValidator<CreateLocationCommand>
    {
        public CreateLocationCommandValidator()
        {
            RuleFor(v => v.LocationDto)
                .NotNull();

            RuleFor(v => v.LocationDto.Id)
                .MinimumLength(1)
                .MaximumLength(50)
                .NotNull()
                .NotEmpty();
        }
    }
}
