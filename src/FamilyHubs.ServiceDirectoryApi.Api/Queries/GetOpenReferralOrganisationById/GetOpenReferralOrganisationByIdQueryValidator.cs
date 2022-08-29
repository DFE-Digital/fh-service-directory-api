using FluentValidation;

namespace FamilyHubs.ServiceDirectoryApi.Api.Queries.GetOpenReferralOrganisationById;
public class GetOpenReferralOrganisationByIdQueryValidator : AbstractValidator<GetOpenReferralOrganisationByIdQuery>
{
    public GetOpenReferralOrganisationByIdQueryValidator()
    {
        RuleFor(v => v.Id)
            .NotNull()
            .NotEmpty();
    }
}