using CleanTemplate.Api.Settings.Persistence;
using FluentMigrator.Runner;

namespace CleanTemplate.Migrations.Runner
{
    public static class MigrationRunnerBuilderExtensions
    {
        public static void ConfigureRunnerSQLite3(this IMigrationRunnerBuilder builder, PersistenceSettings persistenceSettings)
        {
            builder
                // Add SQLite support to FluentMigrator
                .AddSQLite()
                // Set the connection string
                .WithGlobalConnectionString(persistenceSettings.ConnectionString)
                // Define the assembly containing the migrations    
                .ScanIn(typeof(Migration_0000_0000_0001).Assembly).For.Migrations();
        }
    }
}
