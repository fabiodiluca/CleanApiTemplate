using CleanTemplate.Api.Settings;
using CleanTemplate.Api.Settings.Persistence;
using FluentMigrator.Runner;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace CleanTemplate.Migrations.Runner
{
    class Program
    {
        //static void Main(string[] args)
        //{
        //    Console.WriteLine("Build this project then execute bin\\Debug\\netcoreapp5.0\\Migrate.ps1");
        //}

        private static IConfigurationRoot _configuration;
        private static PersistenceSettings _persistenceSettings;

        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Environment.CurrentDirectory)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            _configuration = builder.Build();
            var appSettings = new AppSettings();
            _configuration.Bind(appSettings);

            _persistenceSettings = appSettings.Persistence;
            var serviceProvider = CreateServices();

            // Put the database update into a scope to ensure
            // that all resources will be disposed.
            using (var scope = serviceProvider.CreateScope())
            {
                UpdateDatabase(scope.ServiceProvider);
            }
        }

        /// <summary>
        /// Configure the dependency injection services
        /// </summary>
        private static IServiceProvider CreateServices()
        {
            return new ServiceCollection()
                // Add common FluentMigrator services
                .AddFluentMigratorCore()
                .ConfigureRunner(rb => _ConfigureRunner(rb, _persistenceSettings))
                // Enable logging to console in the FluentMigrator way
                .AddLogging(lb => lb.AddFluentMigratorConsole())
                // Build the service provider
                .BuildServiceProvider(false);
        }

        private static void _ConfigureRunner(IMigrationRunnerBuilder migrationRunnerBuilder, PersistenceSettings persistenceSettings)
        {
            switch (persistenceSettings.Database)
            {
                case "SQLite3":
                    migrationRunnerBuilder.ConfigureRunnerSQLite3(persistenceSettings);
                    break;
                ////case "MsSql2012":
                ////    services.AddPersistenceMsSql2012(persistenceSettings);
                ////    break;
                default:
                    throw new NotImplementedException($"Persistence '{persistenceSettings.Database}' not implemented yet.");
            }
        }


        /// <summary>
        /// Update the database
        /// </summary>
        private static void UpdateDatabase(IServiceProvider serviceProvider)
        {
            // Instantiate the runner
            var runner = serviceProvider.GetRequiredService<IMigrationRunner>();

            // Execute the migrations
            runner.MigrateUp();
        }
    }
}

