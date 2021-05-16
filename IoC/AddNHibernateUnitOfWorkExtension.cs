using CleanTemplate.UnitOfWork;
using CleanTemplate.UnitOfWork.NHibernateImplementation;
using Microsoft.Extensions.DependencyInjection;

namespace CleanTemplate.IoC
{
    public static class AddNHibernateUnitOfWorkExtension
    {
        public static void AddNHibernateUnitOfWork(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, NhibernateUnitOfWork>();
        }
    }
}
