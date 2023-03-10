using FluentValidation;

namespace FamilyHubs.ServiceDirectory.Api.Commands.CreateTaxonomy;

public class CreateTaxonomyCommandValidator : AbstractValidator<CreateTaxonomyCommand>
{
    public CreateTaxonomyCommandValidator()
    {
        RuleFor(v => v.Taxonomy)
            .NotNull();

        RuleFor(v => v.Taxonomy.Name)
            .MinimumLength(1)
            .MaximumLength(50)
            .NotNull()
            .NotEmpty();

        RuleFor(v => v.Taxonomy.TaxonomyType)
            .IsInEnum();
    }
}

