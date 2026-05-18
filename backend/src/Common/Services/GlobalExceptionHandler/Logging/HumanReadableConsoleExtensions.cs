using Serilog;
using Serilog.Configuration;
using Serilog.Events;

namespace Trippie.Common.Services.GlobalExceptionHandler.Logging;

public static class HumanReadableConsoleExtensions
{
    public static LoggerConfiguration HumanReadableConsole(
        this LoggerSinkConfiguration sinkConfiguration,
        LogEventLevel restrictedToMinimumLevel = LevelAlias.Minimum,
        IFormatProvider? formatProvider = null,
        bool? useColors = null)
    {
        ArgumentNullException.ThrowIfNull(sinkConfiguration);
        return sinkConfiguration.Sink(
            new HumanReadableConsoleSink(formatProvider, useColors),
            restrictedToMinimumLevel);
    }
}
