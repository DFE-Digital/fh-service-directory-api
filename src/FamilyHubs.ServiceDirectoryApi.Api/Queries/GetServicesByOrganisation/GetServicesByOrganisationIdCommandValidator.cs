using FluentValidation;

namespace FamilyHubs.ServiceDirectory.Api.Queries.GetServicesByOrganisation;

public class GetServicesByOrganisationIdCommandValidator : AbstractValidator<GetServicesByOrganisationIdCommand>
{
    public GetServicesByOrganisationIdCommandValidator()
    {
        RuleFor(v => v.Id)
            .NotNull()
            .NotEmpty();
    }
}
