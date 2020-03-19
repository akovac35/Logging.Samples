// Author: Aleksander Kovač

using com.github.akovac35.Logging;
using com.github.akovac35.Logging.AspNetCore;
using com.github.akovac35.Logging.Correlation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Shared;
using Shared.Blogs;
using Shared.Services;
using static com.github.akovac35.Logging.LoggerHelper<WebApp.Startup>;

namespace WebApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddScoped<ICorrelationProvider, CorrelationProvider>();
            services.AddScoped<WeatherForecastService>(fact =>
            {
                Here(l => l.Entering());

                ICorrelationProvider correlationProvider = (new HttpContextAccessor()).GetCorrelationProvider();
                var tmp = new WeatherForecastService(correlationProvider);

                Here(l => l.Exiting(tmp));
                return tmp;
            });
            // Blazor "scoped" is for the duration of a user circuit (similar to session). For this sample we will use a connection per application view,
            // so transient scope is required
            services.AddTransient<SqliteConnection>(fact =>
            {
                Here(l => l.Entering());

                var tmp = new SqliteConnection("DataSource=:memory:");

                Here(l => l.ExitingSimpleFormat(tmp));
                return tmp;
            });
            // Same argument as for SqliteConnection
            services.AddTransient<BlogContext>(fact =>
            {
                Here(l => l.Entering());

                // This connection is explicitly provided so we have to manage it explicitly by
                // opening, closing and disposing it
                var connection = fact.GetService<SqliteConnection>();

                // Provided by logger frameworks as a singleton
                var loggerFactory = fact.GetService<ILoggerFactory>();
                var options = new DbContextOptionsBuilder<BlogContext>()
                .UseSqlite(connection)
                .UseLoggerFactory(loggerFactory)
                .Options;
                var context = new BlogContext(options);

                Here(l => l.ExitingSimpleFormat(context));
                return context;
            });
            // Same argument as for SqliteConnection
            services.AddTransient<BlogService>(fact =>
            {
                Here(l => l.Entering());

                var context = fact.GetService<BlogContext>();
                var blogService = new BlogService(context);

                // In-memory database exists only for the duration of an open connection
                context.Database.OpenConnection();
                context.Database.EnsureCreated();

                Here(l => l.ExitingSimpleFormat(blogService));
                return blogService;
            });
            services.AddRazorPages();
            services.AddServerSideBlazor();
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

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
