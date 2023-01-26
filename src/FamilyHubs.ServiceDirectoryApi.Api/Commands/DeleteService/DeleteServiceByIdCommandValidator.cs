using FluentValidation;

namespace FamilyHubs.ServiceDirectory.Api.Commands.DeleteService;

public class DeleteServiceByIdCommandValidator : AbstractValidator<DeleteServiceByIdCommand>
{
    public DeleteServiceByIdCommandValidator()
    {
        RuleFor(v => v.Id)
            .NotNull()
            .NotEmpty();
    }
}