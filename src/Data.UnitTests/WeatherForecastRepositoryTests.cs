using AutoMapper;
using CleanTemplate.Application.Repositories;
using CleanTemplate.Data.Model;
using CleanTemplate.Data.Repositories.NHibernate;
using CleanTemplate.IoC;
using CleanTemplate.UnitOfWork;
using FluentAssertions;
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
        Mock<ISession> session;
        Mock<IUnitOfWork> unitOfWork;
        Mock<IQueryOver<WeatherForeCastDataModel, WeatherForeCastDataModel>> queryOver;
        IWeatherForeCastRepository repository;

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

            session = new Mock<ISession>();
            unitOfWork = new Mock<IUnitOfWork>();
            queryOver = new Mock<IQueryOver<WeatherForeCastDataModel, WeatherForeCastDataModel>>();

            session
                .Setup(x => x.QueryOver<WeatherForeCastDataModel>())
                .Returns(queryOver.Object);

            repository = new WeatherForeCastRepository(unitOfWork.Object, mapper);
        }

        [Test]
        public void Select()
        {
            queryOver
                .Setup(x => x.List())
                .Returns(new List<WeatherForeCastDataModel>() 
                { 
                    new WeatherForeCastDataModel() { Id = 3 }, 
                    new WeatherForeCastDataModel() { Id = 7 } 
                });

            unitOfWork
                .Setup(x => x.Session)
                .Returns(session.Object);

            var list = repository.Select();
            list[0].Id.Should().Be(3);
            list[1].Id.Should().Be(7);
        }
    }
}