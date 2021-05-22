﻿using CleanTemplate.Api.Settings.Persistence;
using CleanTemplate.Application;
using CleanTemplate.Application.UseCases.WeatherForecast.Messages.Post;
using Microsoft.Extensions.DependencyInjection;

namespace CleanTemplate.IoC
{
    public static class AddPersistenceExtensions
    {
        public static void AddPersistence(this IServiceCollection services, PersistenceSettings persistenceSettings)
        {
            switch(persistenceSettings.Database)
            {
                case "SQLite3":
                    services.AddPersistenceSQLite3(persistenceSettings);
                    break;
                case "MsSql2012":
                    services.AddPersistenceMsSql2012(persistenceSettings);
                    break;
                default:
                    throw new System.NotImplementedException($"Persistence '{persistenceSettings.Database}' not implemented yet.");
            }

             services.AddScoped<
                 IUseCasePersistenceContext<WeatherForecastPostRequest, Domain.WeatherForeCast>, 
                 UseCasePersistenceContext<WeatherForecastPostRequest, Domain.WeatherForeCast>>
            ();
        }
    }
}
