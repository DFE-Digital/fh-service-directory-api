using FluentValidation;

namespace fh_service_directory_api.core.Queries.GetOpenReferralOrganisationById;
public class GetOpenReferralOrganisationByIdCommandValidator : AbstractValidator<GetOpenReferralOrganisationByIdCommand>
{
    public GetOpenReferralOrganisationByIdCommandValidator()
    {
        RuleFor(v => v.Id)
            .NotNull()
            .NotEmpty();
    }
}