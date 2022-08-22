using FluentValidation;

namespace fh_service_directory_api.core.Queries.GetOpenReferralService;

public class GetOpenReferralServiceByIdCommandValidator : AbstractValidator<GetOpenReferralServiceByIdCommand>
{
    public GetOpenReferralServiceByIdCommandValidator()
    {
        RuleFor(v => v.Id)
            .NotNull()
            .NotEmpty();
    }
}
