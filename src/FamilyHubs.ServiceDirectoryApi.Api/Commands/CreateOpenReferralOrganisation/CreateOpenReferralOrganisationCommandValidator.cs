using FamilyHubs.ServiceDirectoryApi.Core.Api.Commands;
using FluentValidation;

namespace FamilyHubs.ServiceDirectoryApi.Api.Commands.CreateOpenReferralOrganisation;

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
