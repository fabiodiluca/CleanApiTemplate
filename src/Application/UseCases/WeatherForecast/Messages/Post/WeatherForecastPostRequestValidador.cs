using FluentValidation;

namespace CleanTemplate.Application.UseCases.WeatherForecast.Messages.Post
{
    public class WeatherForecastPostRequestValidador: ModelValidator<WeatherForecastPostRequest>
    {
        public WeatherForecastPostRequestValidador()
        {
            Date();
            TemperatureC();
            Summary();
        }

        protected void Date()
        {
            RuleFor(x => x.Date).NotNull();
        }

        protected void TemperatureC()
        {
            RuleFor(x => x.TemperatureC).NotNull();
        }

        protected void Summary()
        {
            RuleFor(x => x.Summary).NotNull();
        }
    }
}
