using FluentValidation;

namespace FamilyHubs.ServiceDirectory.Api.Commands.UpdateUICache;

public class UpdateUICacheCommandValidator : AbstractValidator<UpdateUICacheCommand>
{
    public UpdateUICacheCommandValidator()
    {
        RuleFor(v => v.Id)
            .MinimumLength(1)
            .MaximumLength(50)
            .NotNull()
            .NotEmpty();

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
