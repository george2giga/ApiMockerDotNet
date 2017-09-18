using Microsoft.Extensions.Logging;
using System;

namespace ApiMockerDotNet.Utils
{
    public class ColoredConsoleLoggerProvider : ILoggerProvider
    {
        private readonly LogLevel _logLevel;
        private readonly ConsoleColor _consoleColor;

        public ColoredConsoleLoggerProvider(LogLevel logLevel, ConsoleColor consoleColor)
        {
            this._logLevel = logLevel;
            this._consoleColor = consoleColor;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new ColoredConsoleLogger(_logLevel, _consoleColor);
        }

        public void Dispose()
        {
            //throw new NotImplementedException();
        }
    }

    public class ColoredConsoleLogger : ILogger
    {
        private readonly LogLevel _logLevel;
        private readonly ConsoleColor _consoleColor;

        public ColoredConsoleLogger(LogLevel logLevel, ConsoleColor consoleColor)
        {
            _logLevel = logLevel;
            _consoleColor = consoleColor;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return _logLevel == logLevel;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }
            
            var defaultColor = Console.ForegroundColor;
            Console.ForegroundColor = _consoleColor;
            Console.WriteLine($"{formatter(state, exception)}");
            Console.ForegroundColor = defaultColor;
        }
    }
}
