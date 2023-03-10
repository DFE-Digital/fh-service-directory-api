using FluentValidation;

namespace FamilyHubs.ServiceDirectory.Api.Commands.UpdateOrganisation;

public class UpdateOrganisationCommandValidator : AbstractValidator<UpdateOrganisationCommand>
{
    public UpdateOrganisationCommandValidator()
    {
        RuleFor(v => v.Organisation)
            .NotNull();

        RuleFor(v => v.Organisation.Name)
            .MinimumLength(1)
            .MaximumLength(50)
            .NotNull()
            .NotEmpty();
    }
}
