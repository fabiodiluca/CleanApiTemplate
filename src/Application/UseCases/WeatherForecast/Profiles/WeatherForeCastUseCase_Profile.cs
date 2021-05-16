using AutoMapper;
using CleanTemplate.Application.UseCases.WeatherForecast.Messages.Get;
using CleanTemplate.Application.UseCases.WeatherForecast.Messages.Post;
using CleanTemplate.Domain;

namespace CleanTemplate.Application.UseCases.WeatherForecast.Profiles
{
    public class WeatherForeCastUseCase_Profile : Profile
    {
        public WeatherForeCastUseCase_Profile()
        {
            CreateMap<WeatherForeCast, WeatherForecastGetResponse>();
            CreateMap<WeatherForecastPostRequest, WeatherForeCast>();

        }
    }
}
