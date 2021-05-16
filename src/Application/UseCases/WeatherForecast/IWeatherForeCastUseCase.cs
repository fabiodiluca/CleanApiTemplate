using CleanTemplate.Application.UseCases.WeatherForecast.Messages.Get;
using CleanTemplate.Application.UseCases.WeatherForecast.Messages.Post;

namespace CleanTemplate.Application.UseCases.WeatherForecast
{
    public interface IWeatherForeCastUseCase: IUseCaseBase
    {
        UseCaseResponseMessage<WeatherForecastGetResponse[]> Get();
        UseCaseResponseMessage<WeatherForecastGetResponse[]> Post(WeatherForecastPostRequest request);
    }
}