using AutoMapper;
using CleanTemplate.Domain;
using CleanTemplate.UnitOfWork;
using System;
using System.Collections.Generic;

namespace CleanTemplate.Application.UseCases
{
    public class UseCaseBase : IUseCaseBase
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IMapper _mapper;

        public UseCaseBase(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public void BeginTransaction()
        {
            _unitOfWork.BeginTransaction();
        }

        public void Commit()
        {
            _unitOfWork.Commit();
        }

        protected List<UseCaseResult<T>> CreateResultList<T>()
        {
            return Activator.CreateInstance<List<UseCaseResult<T>>>();
        }

        protected List<UseCaseResult<TResponse>> PersistAndCreateUseCaseResult<TRequest, TDomainIn, TDomainOut, TResponse>(
            IPersistenceContext<TRequest, TDomainIn, TDomainOut> persistenceContext,
            Func<PersistenceAssociation<TRequest, TDomainIn, TDomainOut>, TResponse> persistenceFunction,
            Func<PersistenceAssociation<TRequest, TDomainIn, TDomainOut>, bool> mustAlreadyBePersistedDomainInFunction,
            Func<PersistenceAssociation<TRequest, TDomainIn, TDomainOut>, bool> IsAlreadyPersistedDomainInFunction
            )
            where TDomainIn : IDomainModel
            where TDomainOut : IDomainModel
        {
            var results = CreateResultList<TResponse>();
            foreach (var persistenceAssociation in persistenceContext.PersistenceAssociations)
            {
                UseCaseResult<TResponse> result;
                if (persistenceAssociation.validationResult.IsValid)
                {
                    if (mustAlreadyBePersistedDomainInFunction(persistenceAssociation) &&
                        !IsAlreadyPersistedDomainInFunction(persistenceAssociation))
                    {
                        results.AddSpecifiedIdDoesNotExist();
                        continue;
                    }
                    var resultData = persistenceFunction(persistenceAssociation);
                    result = new UseCaseResult<TResponse>(resultData);
                    results.Add(result);
                    continue;
                }
                else
                {
                    result = new UseCaseResult<TResponse>(persistenceAssociation.validationResult);
                    results.Add(result);
                    continue;
                }
            }
            return results;
        }
    }
}
