using CleanTemplate.Api.Settings.Persistence;
using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;

namespace CleanTemplate.IoC
{
    public static class AddPersistenceMsSql2012Extensions
    {
        public static void AddPersistenceMsSql2012(this IServiceCollection services, PersistenceSettings persistenceSettings)
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
            .ConfigureRunner(rb => rb
                // Add SQLite support to FluentMigrator
                .AddSqlServer2012()
                // Set the connection string
                .WithGlobalConnectionString(persistenceSettings.ConnectionString)
                // Define the assembly containing the migrations
                .ScanIn(Assemblies.Migrations).For.Migrations())
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
