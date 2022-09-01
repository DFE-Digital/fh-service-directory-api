using FluentValidation;

namespace fh_service_directory_api.api.Queries.GetUICacheById;

public class GetUICacheByIdCommandValidator : AbstractValidator<GetUICacheByIdCommand>
{
    public GetUICacheByIdCommandValidator()
    {
        RuleFor(v => v.Id)
            .NotNull()
            .NotEmpty();
    }
}
