// License:
// Apache License Version 2.0, January 2004

// Authors:
//   Aleksander Kovač

using com.github.akovac35.Logging;
using com.github.akovac35.Logging.Correlation;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Shared.Services
{
    public class WeatherForecastService
    {
        public WeatherForecastService(CorrelationProviderAccessor correlationProviderAccessor, ILogger<WeatherForecastService> logger = null)
        {
            if (logger != null) _logger = logger;
            CorrelationProviderAccessorInstance = correlationProviderAccessor ?? throw new ArgumentNullException(nameof(correlationProviderAccessor));
        }

        private ILogger _logger = NullLogger.Instance;

        protected CorrelationProviderAccessor CorrelationProviderAccessorInstance { get; set; }

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        public Task<WeatherForecast[]> GetForecastAsync(DateTime startDate)
        {
            _logger.Here(l => l.Entering(startDate));

            _logger.Here(l => l.LogInformation("CorrelationId is useful for correlating log contents with service or web page requests: {@0}", CorrelationProviderAccessorInstance.Current?.GetCorrelationId()));

            var rng = new Random();
            var tmp = Task.FromResult(Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = startDate.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            }).ToArray());

            _logger.Here(l => l.Exiting(tmp));
            return tmp;
        }
    }
}
