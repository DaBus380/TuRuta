using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace TuRuta.Common.Logger
{
    public static class TraceLoggerProviderExtensions
    {
        public static ILoggingBuilder AddAllTraceLoggers(this ILoggingBuilder builder)
            => builder.AddDebugLogger()
                .AddErrorLogger()
                .AddInfoLogger()
                .AddTraceLogger()
                .AddCriticalLogger()
                .AddWarningLogger();

        public static ILoggingBuilder AddDebugLogger(this ILoggingBuilder builder)
            => builder.AddProvider(new TraceLoggerProvider((_, level) => level == LogLevel.Debug));

        public static ILoggingBuilder AddInfoLogger(this ILoggingBuilder builder)
            => builder.AddProvider(new TraceLoggerProvider((_, level) => level == LogLevel.Information));

        public static ILoggingBuilder AddWarningLogger(this ILoggingBuilder builder)
            => builder.AddProvider(new TraceLoggerProvider((_, level) => level == LogLevel.Warning));

        public static ILoggingBuilder AddErrorLogger(this ILoggingBuilder builder)
            => builder.AddProvider(new TraceLoggerProvider((_, level) => level == LogLevel.Error));

        public static ILoggingBuilder AddCriticalLogger(this ILoggingBuilder builder)
            => builder.AddProvider(new TraceLoggerProvider((_, level) => level == LogLevel.Critical));

        public static ILoggingBuilder AddTraceLogger(this ILoggingBuilder builder)
            => builder.AddProvider(new TraceLoggerProvider((_, level) => level == LogLevel.Trace));
    }
}
