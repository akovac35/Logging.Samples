// License:
// Apache License Version 2.0, January 2004

// Authors:
//   Aleksander Kovač

using com.github.akovac35.Logging;
using com.github.akovac35.Logging.NLog;
using com.github.akovac35.Logging.Serilog;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;
using Serilog;
using Shared;
using System;
using static com.github.akovac35.Logging.LoggerHelper<WebApp.Program>;

namespace WebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            SamplesLoggingHelper.LoggerInit(args, configActionNLog: () =>
            {
                NLogHelper.CreateLogger("NLog.config");
                LoggerFactoryProvider.LoggerFactory = NLogHelper.CreateLoggerFactory();
            }, configActionSerilog: () =>
            {
                SerilogHelper.CreateLogger(configure => configure.AddJsonFile("serilog.json", optional: false, reloadOnChange: true));
                LoggerFactoryProvider.LoggerFactory = SerilogHelper.CreateLoggerFactory();
            });

            Here(l => l.Entering(args));

            try
            {
                CreateHostBuilder(args).Build().Run();

                // Previous logger factory (created in host builder) is now disposed ...
                SamplesLoggingHelper.LoggerConfig(configActionNLog: () =>
                {
                    LoggerFactoryProvider.LoggerFactory = NLogHelper.CreateLoggerFactory();
                }, configActionSerilog: () =>
                {
                    LoggerFactoryProvider.LoggerFactory = SerilogHelper.CreateLoggerFactory();
                });

                Here(l => l.Exiting());
            }
            catch (Exception ex)
            {
                Here(l => l.LogError(ex, ex.Message));
                throw ex;
            }
            finally
            {
                SamplesLoggingHelper.LoggerConfig(configActionNLog: () =>
                {
                    NLogHelper.CloseAndFlushLogger();
                }, configActionSerilog: () =>
                {
                    SerilogHelper.CloseAndFlushLogger();
                });
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();

                    SamplesLoggingHelper.LoggerConfig(configActionNLog: () =>
                    {
                        webBuilder.ConfigureLogging(logging =>
                        {
                            // Needed to remove duplicate log entries
                            logging.ClearProviders();
                        }).UseNLog();
                    }, configActionSerilog: () =>
                    {
                        webBuilder.UseSerilog();
                    });
                });
    }
}
