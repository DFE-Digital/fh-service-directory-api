using FluentValidation;

namespace fh_service_directory_api.api.Commands.CreateOpenReferralTaxonomy;

public class CreateOpenReferralTaxonomyCommandValidator : AbstractValidator<CreateOpenReferralTaxonomyCommand>
{
    public CreateOpenReferralTaxonomyCommandValidator()
    {
        RuleFor(v => v.OpenReferralTaxonomy)
            .NotNull();

        RuleFor(v => v.OpenReferralTaxonomy.Id)
            .MinimumLength(1)
            .MaximumLength(50)
            .NotNull()
            .NotEmpty();

        RuleFor(v => v.OpenReferralTaxonomy.Name)
            .MinimumLength(1)
            .MaximumLength(50)
            .NotNull()
            .NotEmpty();

        RuleFor(v => v.OpenReferralTaxonomy.Vocabulary)
            .MinimumLength(1)
            .NotNull()
            .NotEmpty();
    }
}

