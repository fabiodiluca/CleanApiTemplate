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
        protected readonly IModelValidator<WeatherForecastPostRequest> _weatherForecastPostRequestValidator;

        public WeatherForeCastUseCase(
            IUnitOfWork unitOfWork, 
            IMapper mapper, 
            IWeatherForeCastRepository repository,
            IModelValidator<WeatherForecastPostRequest> weatherForecastPostRequestValidator) : 
            base(unitOfWork, mapper)
        {
            _repository = repository;
            _weatherForecastPostRequestValidator = weatherForecastPostRequestValidator;
        }

        public UseCaseResult<WeatherForecastGetResponse[]> Get()
        {
            var list = _repository.Select();
            var data = _mapper.Map<WeatherForecastGetResponse[]>(list);

            return new UseCaseResult<WeatherForecastGetResponse[]>(data);
        }

        public UseCaseResult<WeatherForecastGetResponse>[] Post(WeatherForecastPostRequest[] models)
        {
            var validationResults = new List<ValidationResult>();
            foreach (var model in models)
            {
                var validationResult = _weatherForecastPostRequestValidator.Validate(model);
                validationResults.Add(validationResult);
            }

            var result = CreateResultList<WeatherForecastGetResponse>(
                _weatherForecastPostRequestValidator.ValidateAll(models)
            );

            _unitOfWork.BeginTransaction();

            var weatherForeCast = _repository.Insert(
                _mapper.Map<List<Domain.WeatherForeCast>>(models)
            );

            var savedWeatherForeCast = _mapper.Map<List<WeatherForecastGetResponse>>(weatherForeCast);

            //var data = new WeatherForecastGetResponse[] { savedWeatherForeCast };

            _unitOfWork.Commit();

            return result.ToArray();
        }
    }
}
