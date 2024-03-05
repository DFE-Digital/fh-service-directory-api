using FamilyHubs.ServiceDirectory.Core.Helper;
using FluentValidation;

namespace FamilyHubs.ServiceDirectory.Core.Commands.Locations.UpdateLocation;

public class UpdateLocationCommandValidator : AbstractValidator<UpdateLocationCommand>
{
    public UpdateLocationCommandValidator()
    {
        RuleFor(v => v.Location)
            .NotNull();

        RuleFor(v => v.Location.Id)
            .NotEqual(0);

        RuleFor(v => v.Location.Name)
            .MaximumLength(255);

        RuleFor(v => v.Location.PostCode)
            .MinimumLength(5)
            .MaximumLength(15)
            .NotNull()
            .NotEmpty();

        RuleFor(v => v.Location.Contacts).Custom((list, context) =>
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
