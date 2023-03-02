using FamilyHubs.ServiceDirectory.Shared.Dto;
using FluentValidation;

namespace FamilyHubs.ServiceDirectory.Core.Validators
{
    public class LocationDtoValidator : AbstractValidator<LocationDto>
    {
        public LocationDtoValidator()
        {

            RuleFor(v => v.Name)
                .MaximumLength(255);

            RuleFor(v => v.Description)
                .MaximumLength(500);

            RuleFor(v => v.PhysicalAddresses)
                .NotEmpty();

            RuleForEach(x => x.PhysicalAddresses).SetValidator(new PhysicalAddressDtoValidator());
        }

    }

    public class PhysicalAddressDtoValidator : AbstractValidator<PhysicalAddressDto>
    {
        public PhysicalAddressDtoValidator()
        {

            RuleFor(v => v.Address1)
                .NotEmpty()
                .MaximumLength(1024);

            RuleFor(v => v.City)
                .MaximumLength(50);

            RuleFor(v => v.PostCode)
                .NotEmpty()
                .MaximumLength(15);
        }

    }
}
