using AutoMapper;
using CleanTemplate.Application.Repositories;
using CleanTemplate.Application.UseCases.WeatherForecast.Messages.Get;
using CleanTemplate.Application.UseCases.WeatherForecast.Messages.Post;
using CleanTemplate.UnitOfWork;
using FluentValidation;
using FluentValidation.Results;
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
        protected readonly IUseCasePersistenceContext<WeatherForecastPostRequest, Domain.WeatherForeCast> _persistenceContext;

        public WeatherForeCastUseCase(
            IUnitOfWork unitOfWork, 
            IMapper mapper, 
            IWeatherForeCastRepository repository,
            IUseCasePersistenceContext<WeatherForecastPostRequest, Domain.WeatherForeCast> persistenceContext)
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

        public UseCaseResult<WeatherForecastGetResponse>[] Post(WeatherForecastPostRequest[] models)
        {
            _persistenceContext.Set(models);

            _unitOfWork.BeginTransaction();

            var results = new List<UseCaseResult<WeatherForecastGetResponse>>();
            foreach(var persistenceAssociation in _persistenceContext.PersistenceAssociations)
            {
                UseCaseResult<WeatherForecastGetResponse> result;
                if (persistenceAssociation.validationResult.IsValid)
                {
                    persistenceAssociation.DomainModel = _repository.Insert(persistenceAssociation.DomainModel);
                    var resultData = _mapper.Map<WeatherForecastGetResponse>(persistenceAssociation.DomainModel);
                    result = new UseCaseResult<WeatherForecastGetResponse>(resultData);
                }
                else
                    result = new UseCaseResult<WeatherForecastGetResponse>(persistenceAssociation.validationResult);
                results.Add(result);
            }

            _unitOfWork.Commit();

            return results.ToArray();
        }
    }
}
