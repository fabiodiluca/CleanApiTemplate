using CleanTemplate.Application.UseCases.WeatherForecast.Messages.Get;
using CleanTemplate.Application.UseCases.WeatherForecast.Messages.Post;
using System.Collections.Generic;

namespace CleanTemplate.Application.UseCases.WeatherForecast
{
    public interface IWeatherForeCastUseCase: IUseCaseBase
    {
        UseCaseResult<WeatherForecastGetResponse>[] Get();
        UseCaseResult<WeatherForecastGetResponse>[] Get(int id);
        UseCaseResult<WeatherForecastPostResponse>[] Post(WeatherForecastPostRequest[] models);
        UseCaseResult<int>[] Delete(int id);
        UseCaseResult<int>[] Delete(List<int> ids);
    }
}