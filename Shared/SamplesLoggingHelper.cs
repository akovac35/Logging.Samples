// License:
// Apache License Version 2.0, January 2004

// Authors:
//   Aleksander Kovač

using System;

namespace Shared
{
    public static class SamplesLoggingHelper
    {
        public enum Loggers
        {
            NLog,
            Serilog,
            None
        }

        public static Loggers SelectedLogger { get; set; } = Loggers.None;

        public static void LoggerConfig(Action configActionNLog = null, Action configActionSerilog = null)
        {
            if (configActionNLog != null && SelectedLogger == Loggers.NLog) configActionNLog();
            if (configActionSerilog != null && SelectedLogger == Loggers.Serilog) configActionSerilog();
        }

        public static void LoggerInit(string[] args, Action configActionNLog = null, Action configActionSerilog = null)
        {
            if (args == null || args.Length == 0 || string.IsNullOrWhiteSpace(args[0]))
            {
                throw new ArgumentException("Please specify logger name - i.e. dotnet run nlog or dotnet run serilog", nameof(args));
            }

            SelectedLogger = (Loggers)Enum.Parse(typeof(Loggers), args[0], true);

            LoggerConfig(configActionNLog, configActionSerilog);
        }
    }
}
