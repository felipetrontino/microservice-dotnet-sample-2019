using Framework.Core.Config;
using Serilog;
using Serilog.Core;
using Serilog.Debugging;
using Serilog.Events;
using System;

namespace Framework.Core.Logging.Serilog
{
    public class SerilogLogger : ISimpleLogger
    {
        private ILogger _logger = null;

        public SerilogLogger()
        {
            Configure();
        }

        public void Info(string message)
        {
            message = FormatMessage(message);
            _logger.Information(message);
        }

        public void Warn(string message)
        {
            message = FormatMessage(message);
            _logger.Warning(message);
        }

        public void Error(string message, Exception ex = null)
        {
            message = FormatMessage(message);
            _logger.Error(ex, message);
        }

        public void Debug(string message)
        {
            message = FormatMessage(message);
            _logger.Debug(message);
        }

        public void Log(LoggingType type, string message)
        {
            switch (type)
            {
                case LoggingType.Error:
                    Error(message);
                    break;

                case LoggingType.Warning:
                    Warn(message);
                    break;

                case LoggingType.Info:
                    Info(message);
                    break;

                case LoggingType.Debug:
                    Debug(message);
                    break;

                default:
                    break;
            }
        }

        private static string FormatMessage(string message)
        {
            return $"[{Configuration.AppName.Get()}] - {message}";
        }

        private void Configure()
        {
            _logger = GetLogger();
        }

        private static ILogger GetLogger()
        {
            SelfLog.Enable(Console.Out);

            var configuration = new LoggerConfiguration();

            if (Configuration.Debugging.Get())
                configuration.MinimumLevel.Debug();
            else
                configuration.MinimumLevel.Information();

            configuration.WriteTo.Console(LogEventLevel.Verbose, "[{Timestamp:dd/MM/yyyy HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}");

            configuration.Enrich.FromLogContext();

            configuration.MinimumLevel.Override("Microsoft", new LoggingLevelSwitch(LogEventLevel.Warning))
                         .MinimumLevel.Override("System", new LoggingLevelSwitch(LogEventLevel.Warning));

            return configuration.CreateLogger();
        }
    }
}