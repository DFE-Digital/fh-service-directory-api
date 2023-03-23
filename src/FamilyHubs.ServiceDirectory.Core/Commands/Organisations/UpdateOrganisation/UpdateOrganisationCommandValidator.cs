using FluentValidation;

namespace FamilyHubs.ServiceDirectory.Core.Commands.Organisations.UpdateOrganisation;

public class UpdateOrganisationCommandValidator : AbstractValidator<UpdateOrganisationCommand>
{
    public UpdateOrganisationCommandValidator()
    {
        RuleFor(v => v.Organisation)
            .NotNull();

        RuleFor(v => v.Organisation.Id)
            .NotEqual(0);

        RuleFor(v => v.Organisation.Name)
            .MinimumLength(1)
            .MaximumLength(255)
            .NotNull()
            .NotEmpty();
    }
}
