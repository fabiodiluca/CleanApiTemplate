using AutoMapper;
using CleanTemplate.Data.Model;
using CleanTemplate.Application.Repositories;
using CleanTemplate.Domain;
using NHibernate;
using System.Collections.Generic;
using CleanTemplate.UnitOfWork;
using System.Linq;

namespace CleanTemplate.Data.Repositories.NHibernate
{
    public class WeatherForeCastRepository : IWeatherForeCastRepository
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected ISession _session => _unitOfWork.Session as ISession;
        protected readonly IMapper _mapper;

        public WeatherForeCastRepository(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public List<WeatherForeCast> Select()
        {

            return _mapper.Map<List<WeatherForeCast>>(
                _session.QueryOver<WeatherForeCastDataModel>().List()
            );
        }

        public WeatherForeCast Select(int id)
        {

            return _mapper.Map<WeatherForeCast>(
                _session.QueryOver<WeatherForeCastDataModel>().Where(x => x.Id == id).List().First()
            );
        }

        public List<WeatherForeCast> Select(List<int> ids)
        {
            return _mapper.Map<List<WeatherForeCast>>(
                _session.QueryOver<WeatherForeCastDataModel>().Where(x => ids.Contains(x.Id)).List()
            );
        }

        public WeatherForeCast Insert(WeatherForeCast domainModel)
        {
            var dataModel = _mapper.Map<WeatherForeCastDataModel>(domainModel);
            _session.Save(dataModel);
            return _mapper.Map<WeatherForeCast>(dataModel);
        }

        public void Delete(WeatherForeCast domainModel)
        {
            var dataModel = _mapper.Map<WeatherForeCastDataModel>(domainModel);
            _session.Delete(dataModel);
        }

        public WeatherForeCast Update(WeatherForeCast model)
        {
            var dataModel = _mapper.Map<WeatherForeCastDataModel>(model);
            _session.Update(dataModel);
            return _mapper.Map<WeatherForeCast>(dataModel);
        }

        public List<WeatherForeCast> Insert(List<WeatherForeCast> models)
        {
            var returnList = new List<WeatherForeCast>();
            foreach (var model in models)
                returnList.Add(Insert(model));
            return returnList;
        }

        public List<WeatherForeCast> Update(List<WeatherForeCast> model)
        {
            throw new System.NotImplementedException();
        }

        public void Delete(List<WeatherForeCast> model)
        {
            throw new System.NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new System.NotImplementedException();
        }

        public void Delete(List<int> id)
        {
            throw new System.NotImplementedException();
        }

        public bool Exists(int id)
        {
            return _session.QueryOver<WeatherForeCastDataModel>().Where(x => x.Id == id).RowCount() == 1;
        }

        public bool ExistAll(List<int> ids)
        {
            var idsExistentes = _session.QueryOver<WeatherForeCastDataModel>()
                .Where(x => ids.Contains(x.Id))
                .Select(x => x.Id);

            return ids.Distinct().Count() == ids.Distinct().Count();
        }
    }
}
