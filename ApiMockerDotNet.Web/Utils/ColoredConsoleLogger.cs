using Microsoft.Extensions.Logging;
using System;

namespace ApiMockerDotNet.Web.Utils
{
    public class ColoredConsoleLoggerProvider : ILoggerProvider
    {
        private readonly LogLevel logLevel;
        private ConsoleColor consoleColor;

        public ColoredConsoleLoggerProvider(LogLevel logLevel, ConsoleColor consoleColor)
        {
            this.logLevel = logLevel;
            this.consoleColor = consoleColor;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new ColoredConsoleLogger(this.logLevel, this.consoleColor);
        }

        public void Dispose()
        {
            //throw new NotImplementedException();
        }
    }

    public class ColoredConsoleLogger : ILogger
    {
        private readonly LogLevel logLevel;
        private readonly ConsoleColor consoleColor;

        public ColoredConsoleLogger(LogLevel logLevel, ConsoleColor consoleColor)
        {
            this.logLevel = logLevel;
            this.consoleColor = consoleColor;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return this.logLevel == logLevel;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }
            
            var defaultColor = Console.ForegroundColor;
            Console.ForegroundColor = this.consoleColor;
            Console.WriteLine($"{formatter(state, exception)}");
            Console.ForegroundColor = defaultColor;
        }
    }
}
