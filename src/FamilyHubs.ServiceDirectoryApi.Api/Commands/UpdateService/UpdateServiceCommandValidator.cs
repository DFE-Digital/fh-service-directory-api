using FamilyHubs.ServiceDirectory.Api.Helper;
using FluentValidation;

namespace FamilyHubs.ServiceDirectory.Api.Commands.UpdateService;

public class UpdateServiceCommandValidator : AbstractValidator<UpdateServiceCommand>
{
    public UpdateServiceCommandValidator()
    {
        RuleFor(v => v.Id)
            .MinimumLength(1)
            .MaximumLength(50)
            .NotNull()
            .NotEmpty();

        RuleFor(v => v.Service)
            .NotNull();

        RuleFor(v => v.Service.Id)
            .MinimumLength(1)
            .MaximumLength(50)
            .NotNull()
            .NotEmpty();

        RuleFor(v => v.Service.Name)
            .MinimumLength(1)
            .MaximumLength(50)
            .NotNull()
            .NotEmpty();

        RuleFor(v => v.Service.LinkContacts).Custom((list, context) =>
        {
            if (list != null)
            {
                var hasInvalidUrl = list.Any(x => x != null && x.Contact != null && x.Contact.Url != null && !UtilHelper.IsValidURL(x.Contact.Url));
                if (hasInvalidUrl)
                {
                    context.AddFailure("Contact Url must be valid");
                }
            }
        });

        RuleFor(v => v.Service.ServiceAtLocations).Custom((list, context) =>
        {
            if (list != null)
            {
                foreach (var item in list.Select(x => x.LinkContacts))
                {
                    if (item != null)
                    {
                        var hasInvalidUrl = item.Any(x => x != null && x.Contact != null && x.Contact.Url != null && !UtilHelper.IsValidURL(x.Contact.Url));
                        if (hasInvalidUrl)
                        {
                            context.AddFailure("Contact Url must be valid");
                        }
                    }
                }
            }
        });
    }

}
