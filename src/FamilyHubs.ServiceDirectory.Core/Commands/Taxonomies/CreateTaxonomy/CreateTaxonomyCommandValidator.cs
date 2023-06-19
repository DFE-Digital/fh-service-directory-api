using FluentValidation;

namespace FamilyHubs.ServiceDirectory.Core.Commands.Taxonomies.CreateTaxonomy;

public class CreateTaxonomyCommandValidator : AbstractValidator<CreateTaxonomyCommand>
{
    public CreateTaxonomyCommandValidator()
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

