using CleanTemplate.Application.UseCases;
using CleanTemplate.Application.UseCases.WeatherForecast;
using CleanTemplate.Application.UseCases.WeatherForecast.Messages.Get;
using CleanTemplate.Application.UseCases.WeatherForecast.Messages.Post;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Net;

namespace CleanTemplate.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IPresenter _presenter;
        private readonly IWeatherForeCastUseCase _weatherForeCastUseCase;

        public WeatherForecastController(
              ILogger<WeatherForecastController> logger
            , IPresenter presenter
            , IWeatherForeCastUseCase weatherForeCastUseCase
            )
        {
            _logger = logger;
            _presenter = presenter;
            _weatherForeCastUseCase = weatherForeCastUseCase;
        }

        [HttpGet]
        [ProducesResponseType(typeof(UseCaseResult<WeatherForecastGetResponse[]>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(UseCaseResultMessageBase), (int)HttpStatusCode.InternalServerError)]
        public IActionResult Get()
        {
            _logger.LogInformation("Getting WeatherForecast");
            _presenter.Handler(_weatherForeCastUseCase.Get());
            return _presenter.ActionResult;
        }

        [HttpPost]
        [ProducesResponseType(typeof(UseCaseResult<WeatherForecastGetResponse[]>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(UseCaseResultMessageBase), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(UseCaseResultMessageBase), (int)HttpStatusCode.InternalServerError)]
        public IActionResult Post([FromBody] WeatherForecastPostRequest[] models)
        {
            _logger.LogInformation("Posted WeatherForecast");
            _presenter.Handler(_weatherForeCastUseCase.Post(models));
            return _presenter.ActionResult;
        }
    }
}
