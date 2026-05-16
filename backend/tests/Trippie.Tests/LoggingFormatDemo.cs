using System.Net.Http.Json;
using System.Text;
using FluentAssertions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Trippie.Common.Services.GlobalExceptionHandler.DependencyInjection;
using Trippie.Common.Services.GlobalExceptionHandler.Exceptions;
using Trippie.Common.Services.GlobalExceptionHandler.Logging;
using Xunit;
using Xunit.Abstractions;

namespace Trippie.Tests;

/// <summary>
/// Boots the real pipeline (Serilog + global exception middleware) against TestServer,
/// fires a representative request per exception family, then dumps:
///   - the HTTP response body the client sees,
///   - the captured Console output (themed Serilog console sink),
///   - the rolling text log file,
///   - the JSON error log file.
/// Run with: dotnet test --logger "console;verbosity=detailed"
/// </summary>
public sealed class LoggingFormatDemo
{
    private sealed class UserStub { }

    private readonly ITestOutputHelper _out;

    public LoggingFormatDemo(ITestOutputHelper output) => _out = output;

    [Fact]
    public async Task PrintConsoleAndFileLogFormats()
    {
        var logsDir = Path.Combine(Path.GetTempPath(), "trippie-log-demo-" + Guid.NewGuid().ToString("N")[..8]);
        Directory.CreateDirectory(logsDir);

        var consoleCapture = new StringBuilder();
        var originalOut = Console.Out;
        Console.SetOut(new StringWriter(consoleCapture));

        try
        {
            var builder = WebApplication.CreateBuilder();
            builder.Configuration.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["LogsPath"] = logsDir,
                ["ErrorHandling:ExposeDetails"] = "true",
                ["Serilog:MinimumLevel:Default"] = "Information",
                ["Serilog:MinimumLevel:Override:Microsoft"] = "Warning",
            });
            builder.WebHost.UseTestServer();
            builder.Host.UseAppSerilog();
            builder.Services.AddExceptionHandling(builder.Configuration);

            var app = builder.Build();
            app.UseSerilogRequestLogging(opts =>
                opts.MessageTemplate =
                    "HTTP {RequestMethod:l} {RequestPath:l} responded {StatusCode} in {Elapsed:0.0000} ms");
            app.UseExceptionHandling();

            app.MapGet("/throw/notfound", () =>
            {
                throw NotFoundException.For<UserStub>(42);
            });

            app.MapGet("/throw/validation", () =>
            {
                var errs = new Dictionary<string, IReadOnlyList<string>>
                {
                    ["email"] = new[] { "Email is required.", "Email must be a valid address." },
                    ["password"] = new[] { "Password must be at least 8 characters." }
                };
                throw new ValidationException(errs);
            });

            app.MapGet("/throw/conflict", () =>
            {
                throw new ConflictException("Email 'a@b.com' is already registered.", "user.email_taken");
            });

            app.MapGet("/throw/internal", () =>
            {
                throw new InvalidOperationException("Simulated DB connection failure");
            });

            await app.StartAsync();
            var client = app.GetTestClient();

            string[] endpoints =
            {
                "/throw/notfound",
                "/throw/validation",
                "/throw/conflict",
                "/throw/internal"
            };

            foreach (var ep in endpoints)
            {
                using var req = new HttpRequestMessage(HttpMethod.Get, ep);
                req.Headers.Add("X-Correlation-Id", $"demo-{ep.Split('/').Last()}");

                var resp = await client.SendAsync(req);
                var body = await resp.Content.ReadAsStringAsync();

                _out.WriteLine($"════════════════════════════════════════════════════════════════════════");
                _out.WriteLine($"  GET {ep}  →  HTTP {(int)resp.StatusCode} {resp.StatusCode}");
                _out.WriteLine($"  Content-Type:     {resp.Content.Headers.ContentType}");
                if (resp.Headers.TryGetValues("X-Correlation-Id", out var corr))
                    _out.WriteLine($"  X-Correlation-Id: {string.Join(",", corr)}");
                _out.WriteLine("  ── response body ──");
                _out.WriteLine(PrettyJson(body));
                _out.WriteLine("");
            }

            await app.StopAsync();
            Log.CloseAndFlush();
            await Task.Delay(200);

            Console.SetOut(originalOut);

            _out.WriteLine("");
            _out.WriteLine("╔══════════════════════════════════════════════════════════════════════╗");
            _out.WriteLine("║  CAPTURED CONSOLE OUTPUT  (Serilog themed console sink)             ║");
            _out.WriteLine("╚══════════════════════════════════════════════════════════════════════╝");
            _out.WriteLine(consoleCapture.ToString());

            _out.WriteLine("");
            _out.WriteLine("╔══════════════════════════════════════════════════════════════════════╗");
            _out.WriteLine("║  ROLLING TEXT FILE  (logs/all-YYYYMMDD.log)                         ║");
            _out.WriteLine("╚══════════════════════════════════════════════════════════════════════╝");
            foreach (var f in Directory.GetFiles(logsDir, "all-*.log"))
            {
                _out.WriteLine($"── {Path.GetFileName(f)} ──");
                _out.WriteLine(await ReadFileSafelyAsync(f));
            }

            _out.WriteLine("");
            _out.WriteLine("╔══════════════════════════════════════════════════════════════════════╗");
            _out.WriteLine("║  JSON ERROR FILE  (logs/error-YYYYMMDD.json)                        ║");
            _out.WriteLine("╚══════════════════════════════════════════════════════════════════════╝");
            foreach (var f in Directory.GetFiles(logsDir, "error-*.json"))
            {
                _out.WriteLine($"── {Path.GetFileName(f)} ──");
                _out.WriteLine(await ReadFileSafelyAsync(f));
            }

            // Sanity assertion — make sure we actually produced output.
            Directory.GetFiles(logsDir, "all-*.log").Should().NotBeEmpty();
            Directory.GetFiles(logsDir, "error-*.json").Should().NotBeEmpty();
        }
        finally
        {
            Console.SetOut(originalOut);
            try { Directory.Delete(logsDir, recursive: true); } catch { /* logs may still be locked */ }
        }
    }

    private static async Task<string> ReadFileSafelyAsync(string path)
    {
        await using var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        using var sr = new StreamReader(fs);
        return await sr.ReadToEndAsync();
    }

    private static string PrettyJson(string raw)
    {
        try
        {
            using var doc = System.Text.Json.JsonDocument.Parse(raw);
            return System.Text.Json.JsonSerializer.Serialize(doc.RootElement,
                new System.Text.Json.JsonSerializerOptions { WriteIndented = true });
        }
        catch { return raw; }
    }
}
