using Serilog;
using Serilog.Enrichers.Span;
using Serilog.Events;
using Serilog.Formatting.Compact;
using Serilog.Sinks.SystemConsole.Themes;

namespace Trippie.Common.Services.GlobalExceptionHandler.Logging;

public static class SerilogBootstrap
{
    private const string ConsoleTemplate = "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}";

    private const string FileTemplate =
        "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] " +
        "{TraceId} {SourceContext} :: {Message:lj}{NewLine}{Exception}";

    public static Serilog.ILogger CreateBootstrapLogger() =>
        new LoggerConfiguration()
            .MinimumLevel.Information()
            .WriteTo.Console(theme: AnsiConsoleTheme.Literate, outputTemplate: ConsoleTemplate)
            .CreateBootstrapLogger();

    public static IHostBuilder UseAppSerilog(this IHostBuilder host) =>
        host.UseSerilog((context, services, configuration) =>
        {
            var logsRoot = context.Configuration["LogsPath"] ?? "logs";

            configuration
                .ReadFrom.Configuration(context.Configuration)
                .ReadFrom.Services(services)
                .Enrich.FromLogContext()
                .Enrich.WithEnvironmentName()
                .Enrich.WithMachineName()
                .Enrich.WithThreadId()
                .Enrich.WithSpan()
                .Enrich.WithProperty("Application", "Trippie")
                .WriteTo.Console(
                    theme: AnsiConsoleTheme.Literate,
                    outputTemplate: ConsoleTemplate
                )
                .WriteTo.File(
                    path: Path.Combine(logsRoot, "all-.log"),
                    outputTemplate: FileTemplate,
                    rollingInterval: RollingInterval.Day,
                    fileSizeLimitBytes: 100 * 1024 * 1024,
                    rollOnFileSizeLimit: true,
                    retainedFileCountLimit: 30,
                    shared: true
                )
                .WriteTo.File(
                    formatter: new CompactJsonFormatter(),
                    path: Path.Combine(logsRoot, "error-.json"),
                    restrictedToMinimumLevel: LogEventLevel.Error,
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: 90,
                    shared: true
                );
        });
}
