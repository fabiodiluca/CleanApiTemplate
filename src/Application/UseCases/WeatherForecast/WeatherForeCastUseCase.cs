using AutoMapper;
using CleanTemplate.Application.Repositories;
using CleanTemplate.Application.UseCases.WeatherForecast.Messages.Get;
using CleanTemplate.Application.UseCases.WeatherForecast.Messages.Post;
using CleanTemplate.Domain;
using CleanTemplate.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;

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

        public UseCaseResult<WeatherForecastGetResponse[]> Get(int? id)
        {
            List<WeatherForeCast> list;
            if (id.HasValue) 
                list = new List<WeatherForeCast> { _repository.Select(id.Value) };
            else
                list = _repository.Select();
            var data = _mapper.Map<WeatherForecastGetResponse[]>(list);

            return new UseCaseResult<WeatherForecastGetResponse[]>(data);
        }

        public UseCaseResult<WeatherForecastPostResponse>[] Post(WeatherForecastPostRequest[] models)
        {
            _persistenceContext.Set(models);

            _unitOfWork.BeginTransaction();

            var existingWeatherForecastIds = ExistingWeatherForecastIds(models);

            var results = PersistAndCreateUseCaseResult(_persistenceContext, 
                (persistenceAssociation) => {
                    persistenceAssociation.DomainModelOut = _repository.Insert(persistenceAssociation.DomainModelIn);
                    return _mapper.Map<WeatherForecastPostResponse>(persistenceAssociation.DomainModelOut);
                },
                (persistenceAssociation) => {
                    return persistenceAssociation.DomainModelIn.Id > 0;
                },
                (persistenceAssociation) => {
                    return existingWeatherForecastIds.Contains(persistenceAssociation.DomainModelIn.Id);
                });

            _unitOfWork.Commit();

            return results.ToArray();
        }

        protected List<int> ExistingWeatherForecastIds(WeatherForecastPostRequest[] models)
        {
            return _repository.Select(
                 models
                     .Where(x => x.Id.HasValue && x.Id > 0)
                     .Select(y => y.Id.Value)
                     .ToList()
             ).Select(x => x.Id).ToList();
        }
    }
}
