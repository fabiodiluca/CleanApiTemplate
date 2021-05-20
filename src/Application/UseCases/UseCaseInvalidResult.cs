using FluentValidation.Results;

namespace CleanTemplate.Application.UseCases
{
    public class UseCaseInvalidResult : UseCaseResultMessageBase
    {
        public UseCaseInvalidResult(ValidationResult validationResult) : base(validationResult) { }
    }
}
