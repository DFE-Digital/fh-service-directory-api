using FluentValidation;

namespace fh_service_directory_api.core.OrganisationAggregate.Commands.Create;


public class CreateOrganisationValidator : AbstractValidator<Create>
{
    public CreateOrganisationValidator()
    {
        RuleFor(v => v.Organisation.Name)
            .MinimumLength(1)
            .MaximumLength(50)
            .NotNull()
            .NotEmpty();

        RuleFor(v => v.Organisation.Description)
            .MaximumLength(500);
    }
}
