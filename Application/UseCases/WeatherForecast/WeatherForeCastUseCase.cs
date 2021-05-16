using AutoMapper;
using CleanTemplate.Application.Repositories;
using CleanTemplate.Application.UseCases.WeatherForecast.Messages.Get;
using CleanTemplate.Application.UseCases.WeatherForecast.Messages.Post;
using CleanTemplate.UnitOfWork;
using FluentValidation;

namespace CleanTemplate.Application.UseCases.WeatherForecast
{
    public class WeatherForeCastUseCase : UseCaseBase, IWeatherForeCastUseCase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        protected readonly IWeatherForeCastRepository _repository;
        protected readonly IValidator<WeatherForecastPostRequest> _weatherForecastPostRequestValidator;

        public WeatherForeCastUseCase(
            IUnitOfWork unitOfWork, 
            IMapper mapper, 
            IWeatherForeCastRepository repository,
            IValidator<WeatherForecastPostRequest> weatherForecastPostRequestValidator) : 
            base(unitOfWork, mapper)
        {
            _repository = repository;
            _weatherForecastPostRequestValidator = weatherForecastPostRequestValidator;
        }

        public UseCaseResponseMessage<WeatherForecastGetResponse[]> Get()
        {
            var list = _repository.Select();
            var data = _mapper.Map<WeatherForecastGetResponse[]>(list);

            return new UseCaseResponseMessage<WeatherForecastGetResponse[]>(data);
        }

        public UseCaseResponseMessage<WeatherForecastGetResponse[]> Post(WeatherForecastPostRequest request)
        {
            var validationResult = _weatherForecastPostRequestValidator.Validate(request);
            if (!validationResult.IsValid)
            {
                return new UseCaseResponseMessage<WeatherForecastGetResponse[]>(validationResult);
            }

            _unitOfWork.BeginTransaction();

            var weatherForeCast = _repository.Insert(
                _mapper.Map<Domain.WeatherForeCast>(request)
            );

            var savedweatherForeCast = _mapper.Map<WeatherForecastGetResponse>(weatherForeCast);

            var data = new WeatherForecastGetResponse[] { savedweatherForeCast };

            _unitOfWork.Commit();

            return new UseCaseResponseMessage<WeatherForecastGetResponse[]>(data);
        }
    }
}
