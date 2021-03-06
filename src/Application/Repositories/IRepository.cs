using CleanTemplate.Domain;
using System.Collections.Generic;

namespace CleanTemplate.Application.Repositories
{
    public interface IRepository<T>
        where T : IDomainModel
    {
        List<T> Select();
        /// <summary>
        /// throws EntityNotFoundException
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        T Select(int id);
        List<T> Select(List<int> ids);
        bool Exists(int id);
        bool ExistAll(List<int> ids);
        T Insert(T model);
        T InsertOrUpdate(T model);
        List<T> Insert(List<T> model);
        List<T> InsertOrUpdate(List<T> model);
        T Update(T model);
        List<T> Update(List<T> model);
        void Delete(T model);
        void Delete(List<T> model);
        /// <summary>
        /// throws EntityNotFoundException
        /// </summary>
        /// <param name="id"></param>
        void Delete(int id);
        /// <summary>
        /// throws EntityNotFoundException
        /// </summary>
        /// <param name="id"></param>
        void Delete(List<int> id);
    }
}
