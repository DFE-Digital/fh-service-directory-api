using FamilyHubs.ServiceDirectory.Shared.Dto;
using FluentValidation;
using System.Text.RegularExpressions;

namespace FamilyHubs.ServiceDirectory.Core.Validators
{
    public class ContactDtoValidator : AbstractValidator<ContactDto>
    {
        public ContactDtoValidator()
        {

            RuleFor(v => v.Title)
                .MaximumLength(50);

            RuleFor(v => v.Name)
                .MaximumLength(50);

            RuleFor(v => v.Telephone)
                .MaximumLength(50);

            RuleFor(v => v.TextPhone)
                .MaximumLength(50);

            RuleFor(v => v.Url)
                .Must(x => ValidateUrl(x))
                .WithMessage("Provided URL not valid");

            RuleFor(v => v.Email)
                .EmailAddress();
        }

        public static bool ValidateUrl(string? url)
        {
            // just so the validation passes if the uri is not required / nullable
            if (string.IsNullOrEmpty(url))
            {
                return true;
            }

            string Pattern = @"[\][a-z.]{2,3}$+([./?%&=]*)?";
            Regex Rgx = new Regex(Pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            return Rgx.IsMatch(url);
        }
    }
}
