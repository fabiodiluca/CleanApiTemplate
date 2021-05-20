using FluentValidation;
using FluentValidation.Results;
using System.Collections.Generic;

namespace CleanTemplate.Application
{
    public class ModelValidator<T> : AbstractValidator<T>, IModelValidator<T>
    {
        public IEnumerable<ValidationResult> ValidateAll(T[] instances)
        {
            var results = new List<ValidationResult>();
            foreach (var instance in instances)
                results.Add(Validate(instance));
            return results;
        }

        public IEnumerable<ValidationResult> ValidateAll(IEnumerable<T> instances)
        {
            var results = new List<ValidationResult>();
            foreach (var instance in instances)
                results.Add(Validate(instance));
            return results;
        }
    }
}
