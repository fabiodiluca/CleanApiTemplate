using CleanTemplate.Application.Repositories;
using CleanTemplate.Data.Repositories.NHibernate;
using Microsoft.Extensions.DependencyInjection;

namespace CleanTemplate.IoC
{
    public static class AddNHibernateRepositoriesExtension
    {
        public static void AddNHibernateRepositories(this IServiceCollection services)
        {
            services.AddScoped<IWeatherForeCastRepository, WeatherForeCastRepository>();
        }
    }
}
