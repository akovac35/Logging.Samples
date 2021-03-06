﻿// License:
// Apache License Version 2.0, January 2004

// Authors:
//   Aleksander Kovač

using com.github.akovac35.Logging;
using com.github.akovac35.Logging.NLog;
using com.github.akovac35.Logging.Serilog;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Shared;
using Shared.Mocks;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static com.github.akovac35.Logging.LoggerHelper<ConsoleApp.Program>;

namespace ConsoleApp
{
    public class Program
    {
        public static async Task Main(string[] args)
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
                // Set correlationId for the current activity, it is preserved for the current thread and
                // across async/await
                using (Logger.BeginScope(new[] { new KeyValuePair<string, object>(Constants.CorrelationId, 12345678) }))
                {
                    List<Task<int>> tasks = new List<Task<int>>();
                    for (int i = 0; i < 10; i++)
                    {
                        tasks.Add(BusinessLogicMock<object>.GetTaskInstance(LoggerFactoryProvider.LoggerFactory));
                    }

                    // Business logic call sample
                    await Task.WhenAll(tasks);
                }

                Here(l => l.Exiting());
            }
            catch (Exception ex)
            {
                Here(l => l.LogError(ex, ex.Message));
                throw;
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
    }
}
