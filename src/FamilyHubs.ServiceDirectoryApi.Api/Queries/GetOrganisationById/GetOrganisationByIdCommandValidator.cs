using FluentValidation;

namespace FamilyHubs.ServiceDirectory.Api.Queries.GetOrganisationById;
public class GetOrganisationByIdCommandValidator : AbstractValidator<GetOrganisationByIdCommand>
{
    public GetOrganisationByIdCommandValidator()
    {
        RuleFor(v => v.Id)
            .NotNull()
            .NotEmpty();
    }
}