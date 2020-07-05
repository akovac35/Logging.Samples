// License:
// Apache License Version 2.0, January 2004

// Authors:
//   Aleksander Kovač

using com.github.akovac35.Logging.AspNetCore.Correlation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
            SamplesLoggingHelper.LoggerConfig(configActionNLog: () =>
            {
                services.AddLoggingCorrelation(obtainCorrelationIdFromRequestHeaders: true);
            }, configActionSerilog: () =>
            {
                services.AddLoggingCorrelation(obtainCorrelationIdFromRequestHeaders: true);
            });
            services.AddScoped<WeatherForecastService>();

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            SamplesLoggingHelper.LoggerConfig(configActionNLog: () =>
            {
                app.UseLoggingCorrelation();
            }, configActionSerilog: () =>
            {
                app.UseLoggingCorrelation();
                app.UseSerilogRequestLogging();
            });

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
