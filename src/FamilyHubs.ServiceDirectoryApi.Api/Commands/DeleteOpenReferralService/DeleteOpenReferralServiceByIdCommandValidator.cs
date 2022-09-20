using FluentValidation;

namespace fh_service_directory_api.api.Commands.DeleteOpenReferralService;

public class DeleteOpenReferralServiceByIdCommandValidator : AbstractValidator<DeleteOpenReferralServiceByIdCommand>
{
    public DeleteOpenReferralServiceByIdCommandValidator()
    {
        RuleFor(v => v.Id)
            .NotNull()
            .NotEmpty();
    }
}