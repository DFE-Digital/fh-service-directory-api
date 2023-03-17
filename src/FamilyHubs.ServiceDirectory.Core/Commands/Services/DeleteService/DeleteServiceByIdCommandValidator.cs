using FluentValidation;

namespace FamilyHubs.ServiceDirectory.Core.Commands.Services.DeleteService;

public class DeleteServiceByIdCommandValidator : AbstractValidator<DeleteServiceByIdCommand>
{
    public DeleteServiceByIdCommandValidator()
    {
        RuleFor(v => v.Id)
            .NotNull()
            .NotEmpty();
    }
}