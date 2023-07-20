using System;
using Serilog;

namespace Shield.Client.Helpers
{
    public static class LogHelper
    {
        public static void InitializeLogger(string logFileName = "logs.txt")
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.File($"logs/{logFileName}", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            LogDebug("--");
        }

        public static void LogDebug(string message) => Log.Debug(message);

        public static void LogInformation(string message) => Log.Information(message);

        public static void LogWarning(string message) => Log.Warning(message);

        public static void LogError(string message) => Log.Error(message);

        public static void LogException(Exception ex, string message = "An exception occurred") =>
            Log.Error(ex, message);

        public static void Dispose() => Log.CloseAndFlush();
    }
}