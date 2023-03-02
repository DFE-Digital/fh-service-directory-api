using FluentValidation;

namespace FamilyHubs.ServiceDirectory.Api.Commands
{
    public abstract class BaseUpsertCommandHandler<TCommand> 
    {
        private readonly AbstractValidator<TCommand> _validator;

        public BaseUpsertCommandHandler(AbstractValidator<TCommand> validator)
        {
            _validator = validator;
        }

        protected bool ValidateRequest(TCommand request, out List<string> failureReasons)
        {
            var validationResult = _validator.Validate(request);
            failureReasons = new List<string>();

            if (validationResult is null)
            {
                failureReasons.Add("An error occurred during validation");
                return false;
            }

            if (!validationResult.IsValid)
            {
                failureReasons = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
                return false;
            }

            return true;
        }
    }
}
