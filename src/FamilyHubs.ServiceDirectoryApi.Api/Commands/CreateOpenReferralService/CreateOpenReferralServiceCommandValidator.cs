using FluentValidation;

namespace fh_service_directory_api.api.Commands.CreateOpenReferralService;

public class CreateOpenReferralServiceCommandValidator : AbstractValidator<CreateOpenReferralServiceCommand>
{
    public CreateOpenReferralServiceCommandValidator()
    {
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
