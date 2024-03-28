using FamilyHubs.ServiceDirectory.Core.Helper;
using FluentValidation;

namespace FamilyHubs.ServiceDirectory.Core.Commands.Services.CreateService;

public class CreateServiceCommandValidator : AbstractValidator<CreateServiceCommand>
{
    public CreateServiceCommandValidator()
    {
        RuleFor(v => v.Service)
            .NotNull();

        RuleFor(v => v.Service.Id)
            .Equal(0);

        RuleFor(v => v.Service.Name)
            .MinimumLength(1)
            .MaximumLength(255)
            .NotNull()
            .NotEmpty();

        RuleFor(v => v.Service.Contacts).Custom((list, context) =>
        {
            if (list is null) return;

            var hasInvalidUrl = list.Any(x => x.Url is not null && !HelperUtility.IsValidUrl(x.Url));
            if (hasInvalidUrl)
            {
                context.AddFailure("Contact Url must be valid");
            }
        });
    }
}
