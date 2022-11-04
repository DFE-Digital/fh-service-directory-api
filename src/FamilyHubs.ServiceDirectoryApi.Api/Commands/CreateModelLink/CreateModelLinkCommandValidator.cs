using fh_service_directory_api.api.Commands.CreateOpenReferralOrganisation;
using FluentValidation;

namespace fh_service_directory_api.api.Commands.CreateModelLink;

public class CreateModelLinkCommandValidator : AbstractValidator<CreateModelLinkCommand>
{
    public CreateModelLinkCommandValidator()
    {
        RuleFor(v => v.ModelLinkDto)
            .NotNull();

        RuleFor(v => v.ModelLinkDto.Id)
            .MinimumLength(1)
            .MaximumLength(50)
            .NotNull()
            .NotEmpty();

        RuleFor(v => v.ModelLinkDto.LinkType)
            .MinimumLength(1)
            .NotNull()
            .NotEmpty();

        RuleFor(v => v.ModelLinkDto.ModelOneId)
            .MinimumLength(1)
            .NotNull()
            .NotEmpty();

        RuleFor(v => v.ModelLinkDto.ModelTwoId)
            .MinimumLength(1)
            .NotNull()
            .NotEmpty();
    }
}
