using AutoMapper;
using CleanTemplate.Domain;
using System.Collections.Generic;

namespace CleanTemplate.Application
{
    public class UseCasePersistenceContext<TRequest, TDomainModel> : 
        IUseCasePersistenceContext<TRequest, TDomainModel> where TDomainModel : IDomainModel
    {
        protected IMapper _mapper;
        protected IModelValidator<TRequest> _requestValidator;
        public IEnumerable<UseCasePersistenceAssociation<TRequest, TDomainModel>> PersistenceAssociations { get; internal set; }


        public UseCasePersistenceContext(IModelValidator<TRequest> validator, IMapper mapper)
        {
            _mapper = mapper;
            _requestValidator = validator;
        }

        public void Set(IEnumerable<TRequest> requestModels)
        {
            var persistenceAssociationsList = new List<UseCasePersistenceAssociation<TRequest, TDomainModel>>();
            foreach (var request in requestModels)
            {
                var domailModel = _mapper.Map<TDomainModel>(request);
                persistenceAssociationsList.Add(
                    new UseCasePersistenceAssociation<TRequest, TDomainModel>(request, domailModel, _requestValidator)
                );
            }
            PersistenceAssociations = persistenceAssociationsList;
        }
    }
}
