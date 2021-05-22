using CleanTemplate.Domain;
using System.Collections.Generic;

namespace CleanTemplate.Application
{
    public interface IUseCasePersistenceContext<TRequest, TDomainModel> where TDomainModel : IDomainModel
    {
        IEnumerable<UseCasePersistenceAssociation<TRequest, TDomainModel>> PersistenceAssociations { get; }

        void Set(IEnumerable<TRequest> requestModels);
    }
}