using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Text;

namespace TuRuta.Common.Logger
{
    public class TraceLogger : ILogger
    {
        private StringBuilder logBuilder = new StringBuilder();
        private string _categoryName;
        private Func<string, LogLevel, bool> _filter;

        public TraceLogger(string categoryName, Func<string, LogLevel, bool> filter)
        {
            _filter = filter;
            _categoryName = categoryName;
        }

        public IDisposable BeginScope<TState>(TState state) => throw new NotImplementedException();

        public bool IsEnabled(LogLevel logLevel)
            => (_filter == null || _filter(_categoryName, logLevel));

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {

            if (!IsEnabled(logLevel))
            {
                return;
            }

            if (formatter == null)
            {
                throw new ArgumentNullException(nameof(formatter));
            }

            logBuilder.Append($"{logLevel}: ");
            logBuilder.Append(formatter(state, exception));

            if (exception != null)
            {
                logBuilder.AppendLine(exception.ToString());
            }

            Trace.WriteLine(logBuilder.ToString());

            logBuilder.Clear();
        }
    }
}
