// License:
// Apache License Version 2.0, January 2004

// Authors:
//   Aleksander Kovač

using com.github.akovac35.Logging;
using com.github.akovac35.Logging.Correlation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Shared.Services;
using System;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        public WeatherForecastController(WeatherForecastService forecastService, ILogger<WeatherForecastController> logger = null)
        {
            if (logger != null) _logger = logger;
            _forecastService = forecastService ?? throw new ArgumentNullException(nameof(forecastService));
        }

        private ILogger _logger = NullLogger.Instance;

        protected WeatherForecastService _forecastService;

        [HttpGet]
        public async Task<WeatherForecast[]> Get()
        {
            _logger.Here(l => l.Entering());

            var forecasts = await _forecastService.GetForecastAsync(DateTime.Now);
            _logger.Here(l => l.LogInformation("CorrelationId can be obtained with CorrelationProvider: {@0}", CorrelationProvider.CurrentCorrelationProvider?.GetCorrelationId()));

            _logger.Here(l => l.Exiting(forecasts));
            return forecasts;
        }
    }
}
