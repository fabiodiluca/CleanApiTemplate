using FluentValidation;
using FluentValidation.Results;
using System.Collections;
using System.Collections.Generic;

namespace CleanTemplate.Application
{
    public interface IModelValidator<TModel>: IValidator<TModel>, IValidator, IEnumerable<IValidationRule>, IEnumerable
    {
        IEnumerable<ValidationResult> ValidateAll(TModel[] instances);
        IEnumerable<ValidationResult> ValidateAll(IEnumerable<TModel> instances);
    }
}