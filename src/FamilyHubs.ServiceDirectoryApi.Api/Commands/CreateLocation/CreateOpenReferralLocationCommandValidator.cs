using fh_service_directory_api.api.Commands.CreateModelLink;
using FluentValidation;

namespace fh_service_directory_api.api.Commands.CreateLocation
{
    public class CreateOpenReferralLocationCommandValidator : AbstractValidator<CreateOpenReferralLocationCommand>
    {
        public CreateOpenReferralLocationCommandValidator()
        {
            RuleFor(v => v.OpenReferralLocationDto)
                .NotNull();

            RuleFor(v => v.OpenReferralLocationDto.Id)
                .MinimumLength(1)
                .MaximumLength(50)
                .NotNull()
                .NotEmpty();

            RuleFor(v => v.OpenReferralTaxonomyId)
                .MinimumLength(1)
                .MaximumLength(50)
                .NotNull()
                .NotEmpty();

            RuleFor(v => v.OpenReferralOrganisationId)
                .MinimumLength(1)
                .MaximumLength(50)
                .NotNull()
                .NotEmpty();
        }
    }
}
