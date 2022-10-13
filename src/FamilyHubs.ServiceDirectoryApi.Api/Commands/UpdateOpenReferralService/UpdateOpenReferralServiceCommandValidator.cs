using FluentValidation;

namespace fh_service_directory_api.api.Commands.UpdateOpenReferralService;

public class UpdateOpenReferralServiceCommandValidator : AbstractValidator<UpdateOpenReferralServiceCommand>
{
    public UpdateOpenReferralServiceCommandValidator()
    {
        RuleFor(v => v.Id)
            .MinimumLength(1)
            .MaximumLength(50)
            .NotNull()
            .NotEmpty();

        RuleFor(v => v.OpenReferralService)
            .NotNull();

        RuleFor(v => v.OpenReferralService.Id)
            .MinimumLength(1)
            .MaximumLength(50)
            .NotNull()
            .NotEmpty();

        RuleFor(v => v.OpenReferralService.Name)
            .MinimumLength(1)
            .MaximumLength(50)
            .NotNull()
            .NotEmpty();
    }
}
