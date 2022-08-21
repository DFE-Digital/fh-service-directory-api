using fh_service_directory_api.core.Interfaces.Commands;
using FluentValidation;

namespace fh_service_directory_api.core.Commands.CreateOpenReferralOrganisation;

public class CreateOpenReferralOrganisationCommandValidator : AbstractValidator<ICreateOpenReferralOrganisationCommand>
{
    public CreateOpenReferralOrganisationCommandValidator()
    {
        RuleFor(v => v.OpenReferralOrganisation)
            .NotNull();

        RuleFor(v => v.OpenReferralOrganisation.Id)
            .MinimumLength(1)
            .MaximumLength(50)
            .NotNull()
            .NotEmpty();

        RuleFor(v => v.OpenReferralOrganisation.Name)
            .MinimumLength(1)
            .MaximumLength(50)
            .NotNull()
            .NotEmpty();
    }
}
