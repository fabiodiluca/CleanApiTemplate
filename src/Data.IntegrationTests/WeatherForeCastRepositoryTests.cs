using CleanTemplate.Api.Settings.Persistence;
using CleanTemplate.Application.Repositories;
using CleanTemplate.Attributes;
using CleanTemplate.IoC;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;

namespace Data.UnitTests
{
    public class WeatherForeCastRepositoryTests
    {
        IServiceProvider serviceProvider;

        [SetUp]
        public void Setup()
        {
            var persistenceSettings = CreatePersistenceSettings();

            var services = new ServiceCollection();
            services.AddAutoMapperAndMaps();
            services.AddPersistence(persistenceSettings);

            serviceProvider = services.BuildServiceProvider(false);
            Insert(); //Insert at least one for the select test
        }

        private PersistenceSettings CreatePersistenceSettings()
        {
            var persistenceSettings = new PersistenceSettings();
            persistenceSettings.ConnectionString = "Data Source=CleanTemplate.db";
            persistenceSettings.Database = DatabaseSource.SQLite3.ToString();
            persistenceSettings.UpdateDatabaseOnStartup = true;
            persistenceSettings.ShowSql = true;
            return persistenceSettings;
        }

        [Test]
        public void Select()
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var repository = scope.ServiceProvider.GetRequiredService<IWeatherForeCastRepository>();
                var list = repository.Select();
                Assert.Greater(list.Count, 0);
            }
        }

        [Test]
        public void SelectById()
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var repository = scope.ServiceProvider.GetRequiredService<IWeatherForeCastRepository>();
                var list = repository.Select();
                var weatherForecast = repository.Select(list[0].Id);
                Assert.IsNotNull(weatherForecast);
                Assert.Greater(weatherForecast.Id, 0);
            }
        }

        [Test]
        public void Insert()
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var repository = scope.ServiceProvider.GetRequiredService<IWeatherForeCastRepository>();
                var inserted = repository.Insert(new CleanTemplate.Domain.WeatherForeCast()
                {
                      Date = DateTime.Now
                    , Id = 0
                    , Summary = "test"
                    , TemperatureC = 23
                });
                Assert.Greater(inserted.Id, 0);
            }
        }

        [Test]
        public void InsertOrUpdate()
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var repository = scope.ServiceProvider.GetRequiredService<IWeatherForeCastRepository>();
                var inserted = repository.InsertOrUpdate(new CleanTemplate.Domain.WeatherForeCast()
                {
                      Date = DateTime.Now
                    , Id = 0
                    , Summary = "test"
                    , TemperatureC = 23
                });

                var list = repository.Select();
                list[0].Summary = "updateTest";
                var updated = repository.InsertOrUpdate(list[0]);
                var selectUpdated = repository.Select(updated.Id);

                Assert.Greater(inserted.Id, 0);
                Assert.Greater(updated.Id, 0);
                Assert.AreEqual(list[0].Summary, selectUpdated.Summary);
            }
        }

    }
}