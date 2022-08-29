using FluentValidation;

namespace FamilyHubs.ServiceDirectoryApi.Api.Queries.GetOpenReferralService;

public class GetOpenReferralServiceByIdQueryValidator : AbstractValidator<GetOpenReferralServiceByIdQuery>
{
    public GetOpenReferralServiceByIdQueryValidator()
    {
        RuleFor(v => v.Id)
            .NotNull()
            .NotEmpty();
    }
}
