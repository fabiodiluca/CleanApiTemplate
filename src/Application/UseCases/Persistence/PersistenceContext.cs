using AutoMapper;
using CleanTemplate.Domain;
using System.Collections.Generic;

namespace CleanTemplate.Application.UseCases
{
    public class PersistenceContext<TRequest, TDomainModelIn, TDomainModelOut> : 
        IPersistenceContext<TRequest, TDomainModelIn, TDomainModelOut> 
        where TDomainModelIn : IDomainModel
        where TDomainModelOut : IDomainModel
    {
        protected IMapper _mapper;
        protected IModelValidator<TRequest> _requestValidator;
        public IEnumerable<PersistenceAssociation<TRequest, TDomainModelIn, TDomainModelOut>> PersistenceAssociations { get; internal set; }


        public PersistenceContext(IModelValidator<TRequest> validator, IMapper mapper)
        {
            _mapper = mapper;
            _requestValidator = validator;
        }

        public void Set(IEnumerable<TRequest> requestModels)
        {
            var persistenceAssociationsList = new List<PersistenceAssociation<TRequest, TDomainModelIn, TDomainModelOut>>();
            foreach (var request in requestModels)
            {
                var domailModel = _mapper.Map<TDomainModelIn>(request);
                persistenceAssociationsList.Add(
                    new PersistenceAssociation<TRequest, TDomainModelIn, TDomainModelOut>(request, domailModel, _requestValidator)
                );
            }
            PersistenceAssociations = persistenceAssociationsList;
        }
    }
}
