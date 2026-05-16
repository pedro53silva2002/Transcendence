using DotNetEnv;
using Serilog;
using Trippie.Common.Services.GlobalExceptionHandler.DependencyInjection;
using Trippie.Common.Services.GlobalExceptionHandler.Logging;

Env.TraversePath().Load();

// Bootstrap logger — captures failures during builder configuration itself.
Log.Logger = SerilogBootstrap.CreateBootstrapLogger();

try
{
    Log.Information("Starting Trippie backend");

    var port = Environment.GetEnvironmentVariable("BACKEND_PORT") ?? "5024";
    var builder = WebApplication.CreateBuilder(args);
    builder.WebHost.UseUrls($"http://+:{port}");

    // Replace MS logging with Serilog (reads "Serilog" + "ErrorHandling" sections).
    builder.Host.UseAppSerilog();

    builder.Services.AddControllers();
    builder.Services.AddOpenApi();
    builder.Services.AddHealthChecks();

    // Exception handling subsystem.
    builder.Services.AddExceptionHandling(builder.Configuration);

    var app = builder.Build();

    // ── Middleware order matters ────────────────────────────────────────────────
    // 1. Correlation ID + global exception handler — must be FIRST so they wrap
    //    every subsequent middleware (auth, static files, endpoints).
    app.UseExceptionHandling();

    // 2. Serilog's request logger writes one line per request with timing.
    app.UseSerilogRequestLogging();

    // 3. Standard pipeline.
    app.MapOpenApi();
    app.MapHealthChecks("/health");
    app.UseAuthorization();
    app.MapControllers();

    app.Run();
}
catch (System.Exception ex)
{
    Log.Fatal(ex, "Trippie backend terminated unexpectedly");
    return 1;
}
finally
{
    Log.CloseAndFlush();
}

return 0;