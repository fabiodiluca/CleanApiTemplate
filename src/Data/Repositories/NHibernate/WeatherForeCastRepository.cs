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

        /// <summary>
        /// throws EntityNotFoundException
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public WeatherForeCast Select(int id)
        {
            var model = _session.QueryOver<WeatherForeCastDataModel>().Where(x => x.Id == id).List().FirstOrDefault();
            if (model == null)
                throw new EntityNotFoundException();

            return _mapper.Map<WeatherForeCast>(model);
        }

        public List<WeatherForeCast> Select(List<int> ids)
        {
            return _mapper.Map<List<WeatherForeCast>>(
                _session.QueryOver<WeatherForeCastDataModel>()
                .AndRestrictionOn(x => x.Id).IsIn(ids)
                .List()
            );
        }

        public WeatherForeCast Insert(WeatherForeCast domainModel)
        {
            var dataModel = _mapper.Map<WeatherForeCastDataModel>(domainModel);
            _session.Save(dataModel);
            return _mapper.Map<WeatherForeCast>(dataModel);
        }

        public WeatherForeCast Update(WeatherForeCast model)
        {
            var dataModel = _mapper.Map<WeatherForeCastDataModel>(model);
            var mergedModel = _session.Merge(dataModel);
            _session.Update(mergedModel);
            return _mapper.Map<WeatherForeCast>(mergedModel);
        }

        public WeatherForeCast InsertOrUpdate(WeatherForeCast model)
        {
            if (model.Id > 0)
                return Update(model);
            else
                return Insert(model);
        }

        public List<WeatherForeCast> Insert(List<WeatherForeCast> models)
        {
            var returnList = new List<WeatherForeCast>();
            foreach (var model in models)
                returnList.Add(Insert(model));
            return returnList;
        }

        public List<WeatherForeCast> Update(List<WeatherForeCast> models)
        {
            var returnList = new List<WeatherForeCast>();
            foreach (var model in models)
                returnList.Add(Update(model));
            return returnList;
        }


        public List<WeatherForeCast> InsertOrUpdate(List<WeatherForeCast> models)
        {
            var returnList = new List<WeatherForeCast>();
            foreach (var model in models)
                returnList.Add(InsertOrUpdate(model));
            return returnList;
        }

        public void Delete(WeatherForeCast domainModel)
        {
            var dataModel = _mapper.Map<WeatherForeCastDataModel>(domainModel);
            _session.Delete(dataModel);
        }

        public void Delete(List<WeatherForeCast> models)
        {
            foreach (var model in models)
                Delete(model);
        }

        /// <summary>
        /// throws EntityNotFoundException
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            var model = _session.Query<WeatherForeCastDataModel>().Where(x => x.Id == id).FirstOrDefault();
            if (model == null)
                throw new EntityNotFoundException();
            _session.Delete(model);
        }

        /// <summary>
        /// throws EntityNotFoundException if any id does not exist
        /// </summary>
        /// <param name="ids"></param>
        public void Delete(List<int> ids)
        {
            foreach(var id in ids)
                Delete(id);
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
