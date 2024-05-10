using System;
using System.Collections.Generic;

namespace DemeRedisCache
{
    public class WeatherForecast
    {
        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string Summary { get; set; }
    }

    public class WeatherForecastResult
    {
        public IEnumerable<WeatherForecast> Forecasts { get; set; }
    }
}
