using CleanTemplate.Domain;
using FluentValidation.Results;

namespace CleanTemplate.Application
{
    public class UseCasePersistenceAssociation<TRequest, TDomainModel> where TDomainModel : IDomainModel
    {
        protected readonly IModelValidator<TRequest> _validator;
        public TRequest Request { get; set; }
        public TDomainModel DomainModel { get; set; }
        public ValidationResult validationResult => _validator.Validate(Request);

        public UseCasePersistenceAssociation(
            TRequest request, 
            TDomainModel domainModel, 
            IModelValidator<TRequest> validator) 
        {
            Request = request;
            DomainModel = domainModel;
            _validator = validator;
        }
    }
}
