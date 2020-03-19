// Author: Aleksander Kovač

using com.github.akovac35.Logging;
using com.github.akovac35.Logging.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shared.Services;
using System;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        public WeatherForecastController(WeatherForecastService forecastService, ILoggerFactory loggerFactory = null)
        {
            if (loggerFactory == null)
            {
                _logger = LoggerFactoryProvider.LoggerFactory.CreateLogger<WeatherForecastController>();
            }
            else
            {
                _logger = loggerFactory.CreateLogger<WeatherForecastController>();
            }

            _logger.Here(l => l.Entering(forecastService));

            if (forecastService == null) throw new ArgumentNullException(nameof(forecastService));
            _forecastService = forecastService;

            _logger.Here(l => l.Exiting());
        }

        private ILogger _logger;

        protected WeatherForecastService _forecastService;

        protected IHttpContextAccessor _contextAccessor = new HttpContextAccessor();

        [HttpGet]
        public async Task<WeatherForecast[]> Get()
        {
            _logger.Here(l => l.Entering());

            var forecasts = await _forecastService.GetForecastAsync(DateTime.Now);
            _logger.Here(l => l.LogInformation("CorrelationId for a request instance can be obtained with HttpContextAccessor: {@correlationId}", _contextAccessor.GetCorrelationId()));

            _logger.Here(l => l.Exiting(forecasts));
            return forecasts;
        }
    }
}
