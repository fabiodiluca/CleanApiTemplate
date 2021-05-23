using CleanTemplate.Domain;
using System.Collections.Generic;

namespace CleanTemplate.Application.UseCases
{
    public interface IPersistenceContext<TRequest, TDomainModelIn, TDomainModelOut> 
        where TDomainModelIn : IDomainModel
        where TDomainModelOut : IDomainModel
    {
        IEnumerable<PersistenceAssociation<TRequest, TDomainModelIn, TDomainModelOut>> PersistenceAssociations { get; }

        void Set(IEnumerable<TRequest> requestModels);
    }
}