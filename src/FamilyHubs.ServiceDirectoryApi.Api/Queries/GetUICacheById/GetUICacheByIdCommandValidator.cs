using FluentValidation;

namespace FamilyHubs.ServiceDirectory.Api.Queries.GetUiCacheById;

public class GetUiCacheByIdCommandValidator : AbstractValidator<GetUiCacheByIdCommand>
{
    public GetUiCacheByIdCommandValidator()
    {
        RuleFor(v => v.Id)
            .MinimumLength(1)
            .NotNull()
            .NotEmpty();
    }
}
