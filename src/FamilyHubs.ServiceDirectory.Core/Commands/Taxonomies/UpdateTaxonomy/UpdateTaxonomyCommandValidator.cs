using FluentValidation;

namespace FamilyHubs.ServiceDirectory.Core.Commands.Taxonomies.UpdateTaxonomy;

public class UpdateTaxonomyCommandValidator : AbstractValidator<UpdateTaxonomyCommand>
{
    public UpdateTaxonomyCommandValidator()
    {
        RuleFor(v => v.Taxonomy)
            .NotNull();

        RuleFor(v => v.Taxonomy.Name)
            .MinimumLength(1)
            .MaximumLength(255)
            .NotNull()
            .NotEmpty();

        RuleFor(v => v.Taxonomy.TaxonomyType)
            .IsInEnum();
    }
}

