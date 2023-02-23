using FluentValidation;

namespace FamilyHubs.ServiceDirectory.Api.Commands.CreateOrganisation;

public class CreateOrganisationCommandValidator : AbstractValidator<CreateOrganisationCommand>
{
    public CreateOrganisationCommandValidator()
    {
        RuleFor(v => v.Organisation)
            .NotNull();

        RuleFor(v => v.Organisation.Id)
            .MinimumLength(1)
            .MaximumLength(50)
            .NotNull()
            .NotEmpty();

        RuleFor(v => v.Organisation.Name)
            .MinimumLength(1)
            .MaximumLength(50)
            .NotNull()
            .NotEmpty();
    }
}
