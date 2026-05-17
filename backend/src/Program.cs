using DotNetEnv;
using Serilog;
using Trippie.Common.Services.Authentication.DependencyInjection;
using Trippie.Common.Services.GlobalExceptionHandler.DependencyInjection;
using Trippie.Common.Services.GlobalExceptionHandler.Exceptions;
using Trippie.Common.Services.GlobalExceptionHandler.Logging;

Env.TraversePath().Load();

// Bootstrap logger — captures failures during builder configuration itself.
Log.Logger = SerilogBootstrap.CreateBootstrapLogger();

try
{
    Log.Information("Starting Trippie backend");

    var port = Environment.GetEnvironmentVariable("BACKEND_PORT") ?? "5024";
    var environmentName = Environment.GetEnvironmentVariable("ENVIRONMENT") ?? Environments.Production;

    var builder = WebApplication.CreateBuilder(new WebApplicationOptions
    {
        Args = args,
        EnvironmentName = environmentName
    });
    builder.WebHost.UseUrls($"http://+:{port}");

    // Replace MS logging with Serilog (reads "Serilog" + "ErrorHandling" sections).
    builder.Host.UseAppSerilog();

    builder.Services.AddControllers();
    builder.Services.AddOpenApi();
    builder.Services.AddHealthChecks();

    // Exception handling subsystem.
    builder.Services.AddExceptionHandling(builder.Configuration);

    builder.Services.AddTrippieAuthentication(builder.Configuration);

    var app = builder.Build();

    // ── Middleware order matters ────────────────────────────────────────────────
    // 1. Serilog's request logger must be OUTERMOST so it observes the final status
    //    code after GlobalExceptionMiddleware has mapped the exception to e.g. 404.
    //    If it ran inside the exception handler, every error would be logged as 500.
    app.UseSerilogRequestLogging(opts =>
        opts.MessageTemplate =
            "HTTP {RequestMethod:l} {RequestPath:l} responded {StatusCode} in {Elapsed:0.0000} ms");

    // 2. Correlation ID + global exception handler wrap the rest of the pipeline
    //    (auth, static files, endpoints).
    app.UseExceptionHandling();

    // 3. Standard pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/openapi/v1.json", "Trippie v1");
        });
    }
    app.MapHealthChecks("/health");
    app.UseAuthentication();
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