using AutoMapper;
using CleanTemplate.UnitOfWork;
using FluentValidation.Results;
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


        protected UseCaseResult<T> CreateResult<T>()
        {
            return Activator.CreateInstance<UseCaseResult<T>>();
        }

        protected List<UseCaseResult<T>> CreateResultList<T>()
        {
            return Activator.CreateInstance<List<UseCaseResult<T>>>();
        }

        protected List<UseCaseResult<T>> CreateResultList<T>(IEnumerable<ValidationResult> validationsResult)
        {
            var resultList = Activator.CreateInstance<List<UseCaseResult<T>>>();
            foreach (var validationResult in validationsResult)
            {
                resultList.Add(
                    (UseCaseResult<T>)Activator.CreateInstance(typeof(UseCaseResult<T>), validationResult)
                );
            }
            return resultList;
        }
    }
}
