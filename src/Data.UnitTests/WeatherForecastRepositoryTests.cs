using AutoMapper;
using CleanTemplate.Application.Repositories;
using CleanTemplate.Data.Model;
using CleanTemplate.Data.Repositories.NHibernate;
using CleanTemplate.Domain;
using CleanTemplate.IoC;
using CleanTemplate.UnitOfWork;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NHibernate;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Data.UnitTests
{
    public class WeatherForecastRepositoryTests
    {
        IServiceProvider serviceProvider;
        IMapper mapper;
        Mock<ISession> sessionMock;
        Mock<IUnitOfWork> unitOfWorkMock;
        Mock<IQueryOver<WeatherForeCastDataModel, WeatherForeCastDataModel>> queryOverMock;

        [SetUp]
        public void Setup()
        {
            var services = new ServiceCollection();
            services.AddAutoMapperAndMaps();
            serviceProvider = services.BuildServiceProvider(false);
            using (var scope = serviceProvider.CreateScope())
            {
                mapper = scope.ServiceProvider.GetService<IMapper>();
            }
            sessionMock = new Mock<ISession>();
            unitOfWorkMock = new Mock<IUnitOfWork>();
            queryOverMock = new Mock<IQueryOver<WeatherForeCastDataModel, WeatherForeCastDataModel>>();
        }

        [Test]
        public void Select()
        {
            sessionMock.Setup(x => x.QueryOver<WeatherForeCastDataModel>())
                .Returns(queryOverMock.Object);
            queryOverMock.Setup(x => x.List())
                .Returns(new List<WeatherForeCastDataModel>() { new WeatherForeCastDataModel() { Id = 3 }, new WeatherForeCastDataModel() { Id = 7 } });
            unitOfWorkMock.Setup(x => x.Session)
                .Returns(sessionMock.Object);

            IWeatherForeCastRepository repository = new WeatherForeCastRepository(unitOfWorkMock.Object, mapper);
            var list = repository.Select();
            Assert.AreEqual(list[0].Id, 3);
            Assert.AreEqual(list[1].Id, 7);
        }
    }
}