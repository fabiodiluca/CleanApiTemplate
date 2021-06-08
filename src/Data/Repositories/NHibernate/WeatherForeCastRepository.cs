using AutoMapper;
using CleanTemplate.Application.Repositories;
using CleanTemplate.Application.Repositories.Exceptions;
using CleanTemplate.Data.Model;
using CleanTemplate.Domain;
using CleanTemplate.UnitOfWork;
using NHibernate;
using System.Collections.Generic;
using System.Linq;

namespace CleanTemplate.Data.Repositories.NHibernate
{
    public class WeatherForeCastRepository : RepositoryBase<WeatherForeCast, WeatherForeCastDataModel>, IWeatherForeCastRepository
    {
        public WeatherForeCastRepository(IUnitOfWork unitOfWork, IMapper mapper):
            base(unitOfWork, mapper)
        {

        }
    }
}
