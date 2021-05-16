using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace CleanTemplate.Data.NHibernate
{
    public class SessionFactory
    {
        public ISessionFactory ConfigureSessionFactory(Action<SessionFactoryConfiguration> configurationAction)
        {
            var config = new SessionFactoryConfiguration();
            configurationAction(config);

            return _InitializeSessionFactory(config);
        }

        private ISessionFactory _InitializeSessionFactory(SessionFactoryConfiguration configuration)
        {
            var persistenceConfigurer = _InitializePersistenceConfigurer(
                configuration.DatabaseType, 
                configuration.ConnectionString, 
                configuration.ShowSql
            );
            var fluentConfiguration = _InitializeFluentConfiguration(
                persistenceConfigurer, 
                configuration.Mappings,
                configuration.ActionConfiguration
            );
            return fluentConfiguration.BuildSessionFactory();
        }

        private static IPersistenceConfigurer _InitializePersistenceConfigurer(
            DatabaseType dbType, 
            string connString, 
            bool showSql)
        {
            IPersistenceConfigurer persistenceConfigurer;

            switch (dbType)
            {
                case DatabaseType.MsSql2012:
                    if (showSql)
                        persistenceConfigurer = MsSqlConfiguration.MsSql2012.ConnectionString(connString).ShowSql();
                    else
                        persistenceConfigurer = MsSqlConfiguration.MsSql2012.ConnectionString(connString);
                    break;
                case DatabaseType.Oracle10:
                    if (showSql)
                        persistenceConfigurer = OracleDataClientConfiguration.Oracle10.ConnectionString(connString).ShowSql();
                    else
                        persistenceConfigurer = OracleDataClientConfiguration.Oracle10.ConnectionString(connString);
                    break;
                case DatabaseType.Oracle9:
                    if (showSql)
                        persistenceConfigurer = OracleDataClientConfiguration.Oracle9.ConnectionString(connString).ShowSql();
                    else
                        persistenceConfigurer = OracleDataClientConfiguration.Oracle9.ConnectionString(connString);
                    break;
                case DatabaseType.SQLite3:
                    if (showSql)
                        persistenceConfigurer = SQLiteConfiguration.Standard.ConnectionString(connString).ShowSql();
                    else
                        persistenceConfigurer = SQLiteConfiguration.Standard.ConnectionString(connString);
                    break;
                default:
                    if (showSql)
                        persistenceConfigurer = PostgreSQLConfiguration.PostgreSQL82.ConnectionString(connString).ShowSql();
                    else
                        persistenceConfigurer = PostgreSQLConfiguration.PostgreSQL82.ConnectionString(connString);
                    break;
            }

            return persistenceConfigurer;
        }

        private static FluentConfiguration _InitializeFluentConfiguration(
            IPersistenceConfigurer persistenceConfigurer, 
            IList<Type> mappings, 
            Action<Configuration> configuration)
        {
            FluentConfiguration fluentConfiguration;

            fluentConfiguration = Fluently.Configure();

            fluentConfiguration.Mappings(m =>
            {
                foreach (var map in mappings)
                {
                    m.FluentMappings.Add(map);
                }
            });

            fluentConfiguration.Database(persistenceConfigurer);
            fluentConfiguration.CurrentSessionContext("web");

            if (configuration != null)
                fluentConfiguration.ExposeConfiguration(configuration);

            _AddFluentMappingsFromHibernatePersistenceAttribute(ref fluentConfiguration);
            return fluentConfiguration;
        }

        private static void _AddFluentMappingsFromHibernatePersistenceAttribute(ref FluentConfiguration configuration)
        {
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var attribute in assembly.GetCustomAttributes(true))
                {
                    if (attribute is HibernatePersistenceAttribute)
                    {
                        Assembly bgmAssembly = assembly;
                        configuration.Mappings(m => m.FluentMappings.AddFromAssembly(bgmAssembly));
                    }
                }
            }
        }
    }
}
