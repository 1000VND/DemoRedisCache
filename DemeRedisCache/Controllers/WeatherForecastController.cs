using DemeRedisCache.Attributes;
using DemeRedisCache.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemeRedisCache.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IResponseCacheService _responseCacheService;

        public WeatherForecastController(
            ILogger<WeatherForecastController> logger,
            IResponseCacheService responseCacheService
            )
        {
            _logger = logger;
            _responseCacheService = responseCacheService;
        }

        [HttpGet("GetAll")]
        [Cache(100)]
        public async Task<IActionResult> Get(string keyword, int pageIndex, int pageSize)
        {
            var rng = new Random();
            var forecasts = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();

            return Ok(forecasts);
        }

        [HttpGet("Create")]
        public async Task<IActionResult> Craete()
        {
            await _responseCacheService.RemoveCacheResponseAsync("/weatherforecast/getall");
            return Ok();
        }
    } 
}
