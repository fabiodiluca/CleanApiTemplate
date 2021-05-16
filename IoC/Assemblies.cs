using CleanTemplate.Application.UseCases;
using CleanTemplate.Data.Maps;
using CleanTemplate.Data.Repositories.NHibernate;
using CleanTemplate.Migrations;
using System.Reflection;

namespace CleanTemplate.IoC
{
    public static class Assemblies
    {
        public static Assembly Application => typeof(UseCaseBase).Assembly;
        public static Assembly Data => typeof(WeatherForeCastRepository).Assembly;
        public static Assembly DataMaps => typeof(WeatherForeCastMap).Assembly;
        public static Assembly Migrations => typeof(Migration_0000_0000_0001).Assembly;
    }
}
