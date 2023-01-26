using FluentValidation;

namespace FamilyHubs.ServiceDirectory.Api.Queries.GetService;

public class GetServiceByIdCommandValidator : AbstractValidator<GetServiceByIdCommand>
{
    public GetServiceByIdCommandValidator()
    {
        RuleFor(v => v.Id)
            .NotNull()
            .NotEmpty();
    }
}
