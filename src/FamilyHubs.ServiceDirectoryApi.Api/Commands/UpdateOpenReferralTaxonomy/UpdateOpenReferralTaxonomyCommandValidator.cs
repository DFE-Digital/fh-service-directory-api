using FluentValidation;

namespace fh_service_directory_api.api.Commands.UpdateOpenReferralTaxonomy;

public class UpdateOpenReferralTaxonomyCommandValidator : AbstractValidator<UpdateOpenReferralTaxonomyCommand>
{
    public UpdateOpenReferralTaxonomyCommandValidator()
    {
        RuleFor(v => v.OpenReferralTaxonomy)
            .NotNull();

        RuleFor(v => v.Id)
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

