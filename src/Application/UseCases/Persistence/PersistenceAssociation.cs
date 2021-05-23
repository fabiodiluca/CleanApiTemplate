using CleanTemplate.Domain;
using FluentValidation.Results;

namespace CleanTemplate.Application.UseCases
{
    public class PersistenceAssociation<TRequest, TDomainModelIn, TDomainModelOut> 
        where TDomainModelIn : IDomainModel
        where TDomainModelOut : IDomainModel
    {
        protected readonly IModelValidator<TRequest> _validator;
        public TRequest Request { get; set; }
        public TDomainModelIn DomainModelIn { get; set; }
        public TDomainModelOut DomainModelOut { get; set; }
        public ValidationResult validationResult => _validator.Validate(Request);

        public PersistenceAssociation(
            TRequest request,
            TDomainModelIn domainModel, 
            IModelValidator<TRequest> validator) 
        {
            Request = request;
            DomainModelIn = domainModel;
            _validator = validator;
        }
    }
}
