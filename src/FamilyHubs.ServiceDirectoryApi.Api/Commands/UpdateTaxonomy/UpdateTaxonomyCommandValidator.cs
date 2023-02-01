﻿using FluentValidation;

namespace FamilyHubs.ServiceDirectory.Api.Commands.UpdateTaxonomy;

public class UpdateTaxonomyCommandValidator : AbstractValidator<UpdateTaxonomyCommand>
{
    public UpdateTaxonomyCommandValidator()
    {
        RuleFor(v => v.Taxonomy)
            .NotNull();

        RuleFor(v => v.Id)
            .MinimumLength(1)
            .MaximumLength(50)
            .NotNull()
            .NotEmpty();

        RuleFor(v => v.Taxonomy.Id)
            .MinimumLength(1)
            .MaximumLength(50)
            .NotNull()
            .NotEmpty();

        RuleFor(v => v.Taxonomy.Name)
            .MinimumLength(1)
            .MaximumLength(50)
            .NotNull()
            .NotEmpty();

        RuleFor(v => v.Taxonomy.Vocabulary)
            .MinimumLength(1)
            .NotNull()
            .NotEmpty();
    }
}
