using FluentValidation;
using System;
using System.Text.RegularExpressions;

namespace FamilyHubs.ServiceDirectory.Api.Commands.UpsertContacts
{
    public class UpsertContactCommandValidator : AbstractValidator<UpsertContactCommand>
    {
        public UpsertContactCommandValidator()
        {
            RuleFor(v => v.ContactDto)
                .NotNull();

            RuleFor(v => v.ContactDto.Title)
                .MaximumLength(50);

            RuleFor(v => v.ContactDto.Name)
                .MaximumLength(50);

            RuleFor(v => v.ContactDto.Telephone)
                .MaximumLength(50);

            RuleFor(v => v.ContactDto.Telephone)
                .MaximumLength(50);

            RuleFor(v => v.ContactDto.Url)
                .Must(x=> ValidateUrl(x))
                .WithMessage("Provided URL not valid");

            RuleFor(v => v.ContactDto.Email)
                .EmailAddress();
        }

        public static bool ValidateUrl(string url)
        {
            // just so the validation passes if the uri is not required / nullable
            if (string.IsNullOrEmpty(url))
            {
                return true;
            }

            string Pattern = @"(http(s)?://)?([\w-]+\.)+[\w-]+[\w-]+[\.]+[\][a-z.]{2,3}$+([./?%&=]*)?";
            Regex Rgx = new Regex(Pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            return Rgx.IsMatch(url);
        }
    }
}
