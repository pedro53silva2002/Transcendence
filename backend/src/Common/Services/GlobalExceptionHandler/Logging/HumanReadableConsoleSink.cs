using System.Globalization;
using Serilog.Core;
using Serilog.Events;

namespace Trippie.Common.Services.GlobalExceptionHandler.Logging;

/// <summary>
/// Console sink that renders one line per field for error events emitted by the
/// global exception middleware, using a fixed colour scheme: timestamp green,
/// label red, value white. All other events fall back to a single
/// <c>[time] LVL message</c> line.
/// </summary>
public sealed class HumanReadableConsoleSink : ILogEventSink
{
    private const string Reset = "[0m";
    private const string Green = "[32m";
    private const string Red = "[31m";
    private const string White = "[97m";
    private const string Yellow = "[33m";
    private const string DarkGrey = "[90m";

    // Fields to render for an error event (label -> property name on the log event).
    // Order is the order they appear on screen.
    private static readonly (string Label, string Property)[] ErrorFields =
    {
        ("Method",      "Method"),
        ("Path",        "Path"),
        ("Status Code", "StatusCode"),
        ("Error Code",  "ErrorCode"),
        ("Exception",   "ExceptionType"),
        ("File",        "SourceFile"),
        ("Line",        "SourceLine"),
        ("Trace",       "TraceId"),
    };

    private static readonly Lock _lock = new();

    private readonly IFormatProvider? _format;
    private readonly bool _useColors;

    public HumanReadableConsoleSink(IFormatProvider? formatProvider = null, bool? useColors = null)
    {
        _format = formatProvider;
        _useColors = useColors ?? !Console.IsOutputRedirected;
    }

    public void Emit(LogEvent ev)
    {
        var time = ev.Timestamp.ToString("HH:mm:ss", CultureInfo.InvariantCulture);
        var prefix = Color($"[{time}]", Green);

        lock (_lock)
        {
            if (ev.Properties.ContainsKey("ErrorCode"))
                WriteErrorBlock(prefix, ev);
            else
                WriteOneLiner(prefix, ev);
        }
    }

    private void WriteOneLiner(string prefix, LogEvent ev)
    {
        var (lvlText, lvlColor) = LevelStyle(ev.Level);
        var msg = ev.RenderMessage(_format);
        Console.WriteLine($"{prefix} {Color(lvlText, lvlColor)} {Color(msg, White)}");
        if (ev.Exception is not null)
            Console.WriteLine(Color(ev.Exception.ToString(), DarkGrey));
    }

    private void WriteErrorBlock(string prefix, LogEvent ev)
    {
        var msg = ev.Exception?.Message ?? ev.RenderMessage(_format);

        Console.WriteLine($"{prefix} {Color("Message:", Red)}     {Color(msg, White)}");

        foreach (var (label, prop) in ErrorFields)
        {
            if (!ev.Properties.TryGetValue(prop, out var val)) continue;
            var rendered = RenderScalar(val);
            var padded = (label + ":").PadRight(12);
            Console.WriteLine($"{prefix} {Color(padded, Red)} {Color(rendered, White)}");
        }

        if (ev.Exception is not null)
        {
            Console.WriteLine($"{prefix} {Color("Stack:".PadRight(12), Red)}");
            foreach (var line in (ev.Exception.StackTrace ?? string.Empty)
                .Split('\n', StringSplitOptions.RemoveEmptyEntries))
            {
                Console.WriteLine($"{prefix}   {Color(line.TrimEnd(), DarkGrey)}");
            }
        }
    }

    private static string RenderScalar(LogEventPropertyValue v) =>
        v is ScalarValue { Value: { } x } ? x.ToString() ?? string.Empty : v.ToString();

    private static (string text, string color) LevelStyle(LogEventLevel l) => l switch
    {
        LogEventLevel.Verbose => ("VRB", DarkGrey),
        LogEventLevel.Debug => ("DBG", DarkGrey),
        LogEventLevel.Information => ("INF", Green),
        LogEventLevel.Warning => ("WRN", Yellow),
        LogEventLevel.Error => ("ERR", Red),
        LogEventLevel.Fatal => ("FTL", Red),
        _ => ("???", White)
    };

    private string Color(string text, string code) =>
        _useColors ? $"{code}{text}{Reset}" : text;
}
