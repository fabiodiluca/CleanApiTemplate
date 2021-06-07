using CleanTemplate.Attributes;
using FluentMigrator;

namespace CleanTemplate.Migrations
{
    [Migration(000000000001, "Weather Forecast Table")]
    public class Migration_0000_0000_0001 : Migration
    {
        public override void Up()
        {
            Create.Table("WeatherForeCast")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("Date").AsDate().NotNullable()
                .WithColumn("TemperatureC").AsDecimal(8, 5).NotNullable()
                .WithColumn("Summary").AsString(200).Nullable();
    }

        public override void Down()
        {
            throw new System.NotImplementedException();
        }
    }
}
