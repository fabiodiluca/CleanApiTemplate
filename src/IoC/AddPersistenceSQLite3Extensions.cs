using CleanTemplate.Api.Settings.Persistence;
using CleanTemplate.Migrations.Runner;
using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;

namespace CleanTemplate.IoC
{
    public static class AddPersistenceSqlite3Extensions
    {
        public static void AddPersistenceSQLite3(this IServiceCollection services, PersistenceSettings persistenceSettings)
        {
            if (persistenceSettings.UpdateDatabaseOnStartup)
            {
                _ConfigureToUpdateDatabase(services, persistenceSettings);
                _UpdateDatabase(services);
            }

            services.AddNHibernateSessionFactory(persistenceSettings);
            services.AddNHibernateUnitOfWork();
            services.AddNHibernateRepositories();
        }

        private static void _ConfigureToUpdateDatabase(this IServiceCollection services, PersistenceSettings persistenceSettings)
        {
            services
            // Add common FluentMigrator services
            .AddFluentMigratorCore()
            .ConfigureRunner(rb => rb.ConfigureRunnerSQLite3(persistenceSettings))
            // Enable logging to console in the FluentMigrator way
            .AddLogging(lb => lb.AddFluentMigratorConsole());
        }

        /// <summary>
        /// Update the database
        /// </summary>
        private static void _UpdateDatabase(this IServiceCollection services)
        {
            // Put the database update into a scope to ensure
            // that all resources will be disposed.
            using (var scope = services.BuildServiceProvider().CreateScope())
            {
                // Instantiate the runner
                var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();

                // Execute the migrations
                runner.MigrateUp();
            }
        }
    }
}
