﻿using System;

namespace CleanTemplate.Application.UseCases.WeatherForecast.Messages.Get
{
    public class WeatherForecastGetResponse
    {
        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string Summary { get; set; }
    }
}