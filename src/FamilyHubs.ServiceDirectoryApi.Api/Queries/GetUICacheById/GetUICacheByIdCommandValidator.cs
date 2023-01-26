using FluentValidation;

namespace FamilyHubs.ServiceDirectory.Api.Queries.GetUICacheById;

public class GetUICacheByIdCommandValidator : AbstractValidator<GetUICacheByIdCommand>
{
    public GetUICacheByIdCommandValidator()
    {
        RuleFor(v => v.Id)
            .MinimumLength(1)
            .NotNull()
            .NotEmpty();
    }
}
