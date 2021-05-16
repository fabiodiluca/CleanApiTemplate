using CleanTemplate.Api.Settings.Persistence;
using CleanTemplate.Data.Maps;
using CleanTemplate.Data.NHibernate;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace CleanTemplate.IoC
{
    public static class AddNHibernateSessionFactoryExtensions
    {
        public static void AddNHibernateSessionFactory(this IServiceCollection services, PersistenceSettings persistenceSettings)
        {
            var sessionFactory = new SessionFactory().ConfigureSessionFactory(a =>
            {
                a.AddFluentMapping(typeof(WeatherForeCastMap));
                a.ConnectionString = persistenceSettings.ConnectionString;
                a.ShowSql = persistenceSettings.ShowSql;
                a.DatabaseType = (DatabaseType)Enum.Parse(typeof(DatabaseType), persistenceSettings.Database);
                a.Configuration(c =>
                {
                    //Set driver here
                    //c.SetProperty("connection.driver_class", "Microsoft.Data.Sqlite.Core");
                    //c.SetProperty("connection.driver_class", "NHibernate.Driver.SQLiteDriver");
                    //c.SetProperty("connection.driver_class", "System.Data.SQLite");
                });
            });

            services.AddSingleton(sessionFactory);
        }
    }
}
