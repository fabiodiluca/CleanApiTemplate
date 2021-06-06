using CleanTemplate.Application.UseCases.WeatherForecast.Messages.Get;
using CleanTemplate.Application.UseCases.WeatherForecast.Messages.Post;
using System.Collections.Generic;

namespace CleanTemplate.Application.UseCases.WeatherForecast
{
    public interface IWeatherForeCastUseCase: IUseCaseBase
    {
        UseCaseResult<WeatherForecastGetResponse>[] Get(int[] ids);
        UseCaseResult<WeatherForecastPostResponse>[] Post(WeatherForecastPostRequest[] models);
        UseCaseResult<int>[] Delete(int[] ids);
    }
}