using Microsoft.Extensions.Logging;
using System;

namespace TuRuta.Common.Logger
{
    public class TraceLoggerProvider : ILoggerProvider
    {
        private readonly Func<string, LogLevel, bool> _filter;

        public TraceLoggerProvider(Func<string, LogLevel, bool> filter)
        {
            _filter = filter;
        }

        public ILogger CreateLogger(string categoryName) => new TraceLogger(categoryName, _filter);

        public void Dispose() { }
    }
}
