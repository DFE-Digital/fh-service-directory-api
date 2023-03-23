using FluentValidation;

namespace FamilyHubs.ServiceDirectory.Core.Commands.Organisations.CreateOrganisation;

public class CreateOrganisationCommandValidator : AbstractValidator<CreateOrganisationCommand>
{
    public CreateOrganisationCommandValidator()
    {
        RuleFor(v => v.Organisation)
            .NotNull();

        RuleFor(v => v.Organisation.Name)
            .MinimumLength(1)
            .MaximumLength(255)
            .NotNull()
            .NotEmpty();
    }
}
