using CleanTemplate.Api.Settings;
using CleanTemplate.Application.UseCases.WeatherForecast;
using CleanTemplate.Application.UseCases.WeatherForecast.Messages.Post;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace CleanTemplate.IoC
{
    public static class AddApplicationServicesExtensions
    {
        public static void AddApplicationServices(this IServiceCollection services, AppSettings settings)
        {
            services.AddPersistence(settings.Persistence);

            AddUseCases(services);

            AddValidators(services);

            services.AddAutoMapperAndMaps();
        }

        private static void AddUseCases(IServiceCollection services)
        {
            services.AddScoped<IWeatherForeCastUseCase, WeatherForeCastUseCase>();
        }

        private static void AddValidators(IServiceCollection services)
        {
            services.AddScoped<IValidator<WeatherForecastPostRequest>, WeatherForecastPostRequestValidador>();
        }
    }
}
