using FluentValidation;

namespace FamilyHubs.ServiceDirectory.Core.Commands.Organisations.DeleteOrganisation;

public class DeleteOrganisationCommandValidator : AbstractValidator<DeleteOrganisationCommand>
{
    public DeleteOrganisationCommandValidator()
    {        
        RuleFor(v => v.Id)
            .NotEqual(0);        
    }
}
