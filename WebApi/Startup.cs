// License:
// Apache License Version 2.0, January 2004

// Authors:
//   Aleksander Kovač

using com.github.akovac35.Logging.AspNetCore;
using com.github.akovac35.Logging.AspNetCore.Correlation;
using com.github.akovac35.Logging.Correlation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Shared;
using Shared.Services;

namespace WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddScoped<ICorrelationProvider, CorrelationProvider>();
            services.AddScoped<WeatherForecastService>(fact =>
            {
                ICorrelationProvider correlationProvider = (new HttpContextAccessor()).GetCorrelationProvider();
                return new WeatherForecastService(correlationProvider);
            });
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            SamplesLoggingHelper.LoggerConfig(configActionNLog: () =>
            {
                // Update the LoggerFactoryProvider once logging config is fully complete
                com.github.akovac35.Logging.LoggerFactoryProvider.LoggerFactory = loggerFactory;
            }, configActionSerilog: () =>
            {
                app.UseSerilogRequestLogging();
                // Update the LoggerFactoryProvider once logging config is fully complete
                com.github.akovac35.Logging.LoggerFactoryProvider.LoggerFactory = loggerFactory;
            });

            app.UseMiddleware<CorrelationIdMiddleware>();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
