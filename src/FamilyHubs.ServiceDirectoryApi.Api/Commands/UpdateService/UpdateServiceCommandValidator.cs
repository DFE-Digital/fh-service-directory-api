using FluentValidation;
using System.Text.RegularExpressions;

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
                var hasInvalidUrl = list.Any(x => x != null && x.Contact != null && x.Contact.Url != null && !IsValidURL(x.Contact.Url));
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
                        var hasInvalidUrl = item.Any(x => x != null && x.Contact != null && x.Contact.Url != null && !IsValidURL(x.Contact.Url));
                        if (hasInvalidUrl)
                        {
                            context.AddFailure("Contact Url must be valid");
                        }
                    }
                }
            }
        });
    }

    private static bool IsValidURL(string URL)
    {
        if (string.IsNullOrEmpty(URL))
            return true;
        string Pattern = @"^(?:http(s)?:\/\/)?[\w.-]+(?:\.[\w\.-]+)+[\w\-\._~:/?#[\]@!\$&'\(\)\*\+,;=.]+$";
        Regex Rgx = new(Pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
        return Rgx.IsMatch(URL);
    }
}
