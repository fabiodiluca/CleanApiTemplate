using CleanTemplate.Attributes;
using CleanTemplate.Data.Model;
using FluentNHibernate.Mapping;

namespace CleanTemplate.Data.Maps
{

    [DatabaseSource(
        DatabaseSource.MsSql2012, 
        DatabaseSource.Oracle10,
        DatabaseSource.Oracle9,
        DatabaseSource.PostgreSQL82,
        DatabaseSource.SQLite3)
    ]
    public class WeatherForeCastMap: ClassMap<WeatherForeCastDataModel> 
    {
        public WeatherForeCastMap()
        {
            Table("WeatherForeCast");

            Id(x => x.Id, "Id").GeneratedBy.Identity();
            Map(x => x.Date, "Date");
            Map(x => x.TemperatureC, "TemperatureC");
            Map(x => x.Summary, "Summary");
        }
    }
}
