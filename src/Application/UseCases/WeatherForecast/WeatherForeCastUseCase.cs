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
        private readonly IWeatherForeCastRepository _repository;
        private readonly IPersistenceContext<WeatherForecastPostRequest, WeatherForeCast, WeatherForeCast> _persistenceContext;

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

        public UseCaseResult<WeatherForecastGetResponse>[] Get(int[] ids)
        {
            var results = CreateResultList<WeatherForecastGetResponse>();

            List<WeatherForeCast> list;
            if (ids.Length > 0)
                list = _repository.Select(ids.ToList());
            else
                list = _repository.Select();

            foreach (var id in ids)
            {
                if (list.Where(x => x.Id == id).Any())
                {
                    var weatherForeCast = list.Where(x => x.Id == id).First();
                    _logger.LogDebug("Adicionando resultado para id {id}", weatherForeCast.Id);
                    var response = _mapper.Map<WeatherForecastGetResponse>(weatherForeCast);
                    results.Add(new UseCaseResult<WeatherForecastGetResponse>(response));
                }
                else
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

        public UseCaseResult<int>[] Delete(int[] ids)
        {
            //TODO repository must have a repository to return only ids for better performance
            var existingIds = _repository.Select(ids.ToList()).Select(x => x.Id).ToList();
            var results = CreateResultList<int>();

            _unitOfWork.BeginTransaction();
            foreach(int id in ids)
            {
                if (!existingIds.Contains(id))
                {
                    results.AddSpecifiedIdDoesNotExist();
                } 
                else
                {
                    _repository.Delete(id);
                    results.Add(new UseCaseResult<int>(id));
                }
            }
            _unitOfWork.Commit();
            return results.ToArray();
        }
    }
}
