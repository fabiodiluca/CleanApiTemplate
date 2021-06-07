using CleanTemplate.Api.Settings.Persistence;
using CleanTemplate.Application.UseCases;
using CleanTemplate.Application.UseCases.WeatherForecast.Messages.Post;
using CleanTemplate.Attributes;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace CleanTemplate.IoC
{
    public static class AddPersistenceExtensions
    {
        public static void AddPersistence(this IServiceCollection services, PersistenceSettings persistenceSettings)
        {
            DatabaseSource databaseSource;
            var parsed = Enum.TryParse(persistenceSettings.Database, out databaseSource);
            if (!parsed)
                throw new NotImplementedException($"Persistence '{persistenceSettings.Database}' not implemented yet.");

            switch (databaseSource)
            {
                case  DatabaseSource.SQLite3:
                    services.AddPersistenceSQLite3(persistenceSettings);
                    break;
                case DatabaseSource.MsSql2012:
                    services.AddPersistenceMsSql2012(persistenceSettings);
                    break;
                default:
                    throw new NotImplementedException($"Persistence '{persistenceSettings.Database}' not implemented yet.");
            }

             services.AddScoped<
                 IPersistenceContext<WeatherForecastPostRequest, Domain.WeatherForeCast, Domain.WeatherForeCast>, 
                 PersistenceContext<WeatherForecastPostRequest, Domain.WeatherForeCast, Domain.WeatherForeCast>>
            ();
        }
    }
}
