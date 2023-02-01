using FluentValidation;

namespace FamilyHubs.ServiceDirectory.Api.Commands.CreateUiCache;

public class CreateUiCacheCommandValidator : AbstractValidator<CreateUiCacheCommand>
{
    public CreateUiCacheCommandValidator()
    {
        RuleFor(v => v.UiCacheDto)
            .NotNull();

        RuleFor(v => v.UiCacheDto.Id)
            .MinimumLength(1)
            .MaximumLength(50)
            .NotNull()
            .NotEmpty();

        RuleFor(v => v.UiCacheDto.Value)
            .MinimumLength(1)
            .NotNull()
            .NotEmpty();
    }
}


