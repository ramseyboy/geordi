using Meziantou.Extensions.Logging.Xunit;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace Geordi.Fixtures;

public sealed class XUnitLoggerFactory(ITestOutputHelper testOutputHelper) : ILoggerFactory
{
    public void AddProvider(ILoggerProvider provider)
    {
    }

    public ILogger CreateLogger(string categoryName)
    {
        return new XUnitLogger(testOutputHelper, new LoggerExternalScopeProvider(), categoryName);
    }

    public ILogger<T> CreateLogger<T>()
    {
        return XUnitLogger.CreateLogger<T>(testOutputHelper);
    }

    public void Dispose()
    {
        testOutputHelper = null!;
    }

    private class XUnitLoggerWrapper<T>: ILogger<T>
    {
        private readonly ILogger<T> _delegate;

        public XUnitLoggerWrapper(XUnitLoggerFactory factory)
        {
            _delegate = factory.CreateLogger<T>();
        }

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull
        {
            return _delegate.BeginScope(state);
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return _delegate.IsEnabled(logLevel);
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            _delegate.Log(logLevel, eventId, state, exception, formatter);
        }
    }
}
