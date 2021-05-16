using CleanTemplate.Domain;
using System.Collections.Generic;

namespace CleanTemplate.Application.Repositories
{
    public interface IRepository<T>
        where T : IDomainModel
    {
        List<T> Select();
        T Select(int id);
        List<T> Select(List<int> ids);
        bool Exists(int id);
        bool ExistAll(List<int> ids);
        T Insert(T model);
        List<T> Insert(List<T> model);
        T Update(T model);
        List<T> Update(List<T> model);
        void Delete(T model);
        void Delete(List<T> model);
        void Delete(int id);
        void Delete(List<int> id);
    }
}
