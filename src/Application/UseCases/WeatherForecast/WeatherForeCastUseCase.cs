using AutoMapper;
using CleanTemplate.Application.Repositories;
using CleanTemplate.Application.Repositories.Exceptions;
using CleanTemplate.Application.UseCases.WeatherForecast.Messages.Get;
using CleanTemplate.Application.UseCases.WeatherForecast.Messages.Post;
using CleanTemplate.Domain;
using CleanTemplate.UnitOfWork;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace CleanTemplate.Application.UseCases.WeatherForecast
{
    public class WeatherForeCastUseCase : UseCaseBase, IWeatherForeCastUseCase
    {
        private readonly ILogger<WeatherForeCastUseCase> _logger;
        protected readonly IWeatherForeCastRepository _repository;
        protected readonly IPersistenceContext<WeatherForecastPostRequest, WeatherForeCast, WeatherForeCast> _persistenceContext;

        public WeatherForeCastUseCase(
            ILogger<WeatherForeCastUseCase> logger,
            IUnitOfWork unitOfWork, 
            IMapper mapper, 
            IWeatherForeCastRepository repository,
            IPersistenceContext<WeatherForecastPostRequest, WeatherForeCast, WeatherForeCast> persistenceContext)
            :base(unitOfWork, mapper)
        {
            _logger = logger;
            _repository = repository;
            _persistenceContext = persistenceContext;
        }

        public UseCaseResult<WeatherForecastGetResponse>[] Get()
        {
            var results = CreateResultList<WeatherForecastGetResponse>();

            var list = _repository.Select();
            foreach (var weatherForeCast in list)
            {
                _logger.LogDebug("Adicionando resultado para id {id}", weatherForeCast.Id);
                var response = _mapper.Map<WeatherForecastGetResponse>(weatherForeCast);
                results.Add(new UseCaseResult<WeatherForecastGetResponse>(response));
            }
            return results.ToArray();
        }

        public UseCaseResult<WeatherForecastGetResponse>[] Get(int id)
        {
            var results = CreateResultList<WeatherForecastGetResponse>();
            try
            {
                var weatherForeCast = _repository.Select(id);
                var response = _mapper.Map<WeatherForecastGetResponse>(weatherForeCast);
                _logger.LogDebug("Adicionando resultado para id {id}", weatherForeCast.Id);
                results.Add(new UseCaseResult<WeatherForecastGetResponse>(response));
            }
            catch (EntityNotFoundException)
            {
                _logger.LogDebug("Adicionando resultado EntityNotFoundException para id {id}", id);
                results.AddSpecifiedIdDoesNotExist();
            }
            return results.ToArray();
        }

        public UseCaseResult<WeatherForecastPostResponse>[] Post(WeatherForecastPostRequest[] models)
        {
            _persistenceContext.Set(models);

            var existingWeatherForecastIds = ExistingWeatherForecastIds(models);

            _unitOfWork.BeginTransaction();

            var results = PersistAndCreateUseCaseResult(_persistenceContext, 
                (persistenceAssociation) => {
                    persistenceAssociation.DomainModelOut = _repository.InsertOrUpdate(persistenceAssociation.DomainModelIn);
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

        private List<int> ExistingWeatherForecastIds(WeatherForecastPostRequest[] models)
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
