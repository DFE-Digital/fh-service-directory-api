using FluentValidation;

namespace FamilyHubs.ServiceDirectory.Api.Commands.UpdateUiCache;

public class UpdateUiCacheCommandValidator : AbstractValidator<UpdateUiCacheCommand>
{
    public UpdateUiCacheCommandValidator()
    {
        RuleFor(v => v.Id)
            .MinimumLength(1)
            .MaximumLength(50)
            .NotNull()
            .NotEmpty();

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
