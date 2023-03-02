using FamilyHubs.SharedKernel;
using FluentValidation;

namespace FamilyHubs.ServiceDirectory.Api.Helper
{
    public static class ValidationExtensions
    {
        public static bool ValidateRequest<TDto>(this AbstractValidator<TDto> validator, TDto request, out Result<TDto> result)
        {
            var validationResult = validator.Validate(request);

            if (validationResult is null)
            {
                result = Result<TDto>.Failure("500", new List<string> { "An error occurred during validation" });
                return false;
            }

            if (!validationResult.IsValid)
            {
                result = Result<TDto>.Failure("400", validationResult.Errors.Select(x => x.ErrorMessage));
                return false;
            }

            result = Result<TDto>.Success(request);
            return true;
        }
    }
}
