using FluentValidation;

namespace fh_service_directory_api.api.Queries.GetOpenReferralServicesByOrganisation;

public class GetOpenReferralServicesByOrganisationIdCommandValidator : AbstractValidator<GetOpenReferralServicesByOrganisationIdCommand>
{
    public GetOpenReferralServicesByOrganisationIdCommandValidator()
    {
        RuleFor(v => v.Id)
            .NotNull()
            .NotEmpty();
    }
}
