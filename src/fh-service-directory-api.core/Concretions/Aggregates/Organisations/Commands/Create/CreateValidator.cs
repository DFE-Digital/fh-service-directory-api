using FluentValidation;

namespace fh_service_directory_api.core.Concretions.Features.Organisations.Commands.Create;


public class CreateValidator : AbstractValidator<Create>
{
    public CreateValidator()
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
