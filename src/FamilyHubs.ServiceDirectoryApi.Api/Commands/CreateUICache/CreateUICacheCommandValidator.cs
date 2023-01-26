using FluentValidation;

namespace FamilyHubs.ServiceDirectory.Api.Commands.CreateUICache;

public class CreateUICacheCommandValidator : AbstractValidator<CreateUICacheCommand>
{
    public CreateUICacheCommandValidator()
    {
        RuleFor(v => v.UICacheDto)
            .NotNull();

        RuleFor(v => v.UICacheDto.Id)
            .MinimumLength(1)
            .MaximumLength(50)
            .NotNull()
            .NotEmpty();

        RuleFor(v => v.UICacheDto.Value)
            .MinimumLength(1)
            .NotNull()
            .NotEmpty();
    }
}


