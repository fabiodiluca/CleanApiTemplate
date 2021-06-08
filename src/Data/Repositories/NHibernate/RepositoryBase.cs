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
    public abstract class RepositoryBase<TDomain, TData> : IRepository<TDomain> 
        where TDomain : IDomainModel
        where TData: DataModel
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected ISession _session => _unitOfWork.Session as ISession;
        protected readonly IMapper _mapper;

        public RepositoryBase(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public List<TDomain> Select()
        {
            var a = _session.QueryOver<TData>();

            return _mapper.Map<List<TDomain>>(
                _session.QueryOver<TData>().List()
            );
        }

        /// <summary>
        /// throws EntityNotFoundException
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public TDomain Select(int id)
        {
            var model = _session.QueryOver<TData>().Where(x => x.Id == id).List().FirstOrDefault();
            if (model == null)
                throw new EntityNotFoundException();

            return _mapper.Map<TDomain>(model);
        }

        public List<TDomain> Select(List<int> ids)
        {
            return _mapper.Map<List<TDomain>>(
                _session.QueryOver<TData>()
                .AndRestrictionOn(x => x.Id).IsIn(ids)
                .List()
            );
        }

        public TDomain Insert(TDomain domainModel)
        {
            var dataModel = _mapper.Map<TData>(domainModel);
            _session.Save(dataModel);
            return _mapper.Map<TDomain>(dataModel);
        }

        public TDomain Update(TDomain model)
        {
            var dataModel = _mapper.Map<TData>(model);
            var mergedModel = _session.Merge(dataModel);
            _session.Update(mergedModel);
            return _mapper.Map<TDomain>(mergedModel);
        }

        public TDomain InsertOrUpdate(TDomain model)
        {
            if (model.Id > 0)
                return Update(model);
            else
                return Insert(model);
        }

        public List<TDomain> Insert(List<TDomain> models)
        {
            var returnList = new List<TDomain>();
            foreach (var model in models)
                returnList.Add(Insert(model));
            return returnList;
        }

        public List<TDomain> Update(List<TDomain> models)
        {
            var returnList = new List<TDomain>();
            foreach (var model in models)
                returnList.Add(Update(model));
            return returnList;
        }


        public List<TDomain> InsertOrUpdate(List<TDomain> models)
        {
            var returnList = new List<TDomain>();
            foreach (var model in models)
                returnList.Add(InsertOrUpdate(model));
            return returnList;
        }

        public void Delete(TDomain domainModel)
        {
            var dataModel = _mapper.Map<TData>(domainModel);
            _session.Delete(dataModel);
        }

        public void Delete(List<TDomain> models)
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
            var model = _session.Query<TData>().Where(x => x.Id == id).FirstOrDefault();
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
            foreach (var id in ids)
                Delete(id);
        }

        public bool Exists(int id)
        {
            return _session.QueryOver<TData>().Where(x => x.Id == id).RowCount() == 1;
        }

        public bool ExistAll(List<int> ids)
        {
            var idsExistentes = _session.QueryOver<TData>()
                .Where(x => ids.Contains(x.Id))
                .Select(x => x.Id);

            return ids.Distinct().Count() == ids.Distinct().Count();
        }
    }
}
