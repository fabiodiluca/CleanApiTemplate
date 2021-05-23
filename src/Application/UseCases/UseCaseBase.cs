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
            Func<PersistenceAssociation<TRequest, TDomainIn, TDomainOut>, TResponse> itemPersistenceFunction
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
                    var resultData = itemPersistenceFunction(persistenceAssociation);
                    result = new UseCaseResult<TResponse>(resultData);
                }
                else
                    result = new UseCaseResult<TResponse>(persistenceAssociation.validationResult);
                results.Add(result);
            }
            return results;
        }
    }
}
