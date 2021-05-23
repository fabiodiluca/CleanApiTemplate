using AutoMapper;
using CleanTemplate.Application.Repositories;
using CleanTemplate.Application.UseCases.WeatherForecast.Messages.Get;
using CleanTemplate.Application.UseCases.WeatherForecast.Messages.Post;
using CleanTemplate.Domain;
using CleanTemplate.UnitOfWork;
using System;
using System.Collections.Generic;

namespace CleanTemplate.Application.UseCases.WeatherForecast
{
    public class WeatherForeCastUseCase : UseCaseBase, IWeatherForeCastUseCase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        protected readonly IWeatherForeCastRepository _repository;
        protected readonly IPersistenceContext<WeatherForecastPostRequest, Domain.WeatherForeCast, Domain.WeatherForeCast> _persistenceContext;

        public WeatherForeCastUseCase(
            IUnitOfWork unitOfWork, 
            IMapper mapper, 
            IWeatherForeCastRepository repository,
            IPersistenceContext<WeatherForecastPostRequest, Domain.WeatherForeCast, Domain.WeatherForeCast> persistenceContext)
            :base(unitOfWork, mapper)
        {
            _repository = repository;
            _persistenceContext = persistenceContext;
        }

        public UseCaseResult<WeatherForecastGetResponse[]> Get()
        {
            var list = _repository.Select();
            var data = _mapper.Map<WeatherForecastGetResponse[]>(list);

            return new UseCaseResult<WeatherForecastGetResponse[]>(data);
        }

        public UseCaseResult<WeatherForecastPostResponse>[] Post(WeatherForecastPostRequest[] models)
        {
            _persistenceContext.Set(models);

            _unitOfWork.BeginTransaction();

            var results = PersistAndCreateUseCaseResult(_persistenceContext, 
                (persistenceAssociation) => {
                persistenceAssociation.DomainModelOut = _repository.Insert(persistenceAssociation.DomainModelIn);
                return _mapper.Map<WeatherForecastPostResponse>(persistenceAssociation.DomainModelOut);
            });

            _unitOfWork.Commit();

            return results.ToArray();
        }

    }
}
