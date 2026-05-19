using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Serilog;
using Trippie.Common.Database;
using Trippie.Common.Services.Authentication.DependencyInjection;
using Trippie.Common.Services.GlobalExceptionHandler.DependencyInjection;
using Trippie.Common.Services.GlobalExceptionHandler.Exceptions;
using Trippie.Common.Services.GlobalExceptionHandler.Logging;
using Trippie.Modules.Auth.Model;
using Trippie.Modules.Auth.Service;

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

    var connectionString = new NpgsqlConnectionStringBuilder
    {
        Host = Environment.GetEnvironmentVariable("POSTGRES_HOST") ?? "localhost",
        Port = int.Parse(Environment.GetEnvironmentVariable("POSTGRES_PORT") ?? "5432"),
        Database = Environment.GetEnvironmentVariable("POSTGRES_DB") ?? throw new InvalidOperationException("POSTGRES_DB not set"),
        Username = Environment.GetEnvironmentVariable("POSTGRES_USER") ?? throw new InvalidOperationException("POSTGRES_USER not set"),
        Password = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD") ?? throw new InvalidOperationException("POSTGRES_PASSWORD not set"),
    }.ConnectionString;

    builder.Services.AddDbContext<AppDbContext>(opts => opts.UseNpgsql(connectionString));


    // Replace MS logging with Serilog (reads "Serilog" + "ErrorHandling" sections).
    builder.Host.UseAppSerilog();

    builder.Services.AddCors(options =>
    {
        options.AddDefaultPolicy(policy =>
        {
            var allowedOrigins = builder.Configuration
                .GetSection("AllowedOrigins")
                .Get<string[]>() ?? ["http://localhost:4200"];

            policy.WithOrigins(allowedOrigins)
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();
        });
    });

    builder.Services.AddControllers();
    builder.Services.AddOpenApi();
    builder.Services.AddHealthChecks();

    // Exception handling subsystem.
    builder.Services.AddExceptionHandling(builder.Configuration);

    builder.Services.AddTrippieAuthentication(builder.Configuration);


    //Add dependency injection for model and service
    builder.Services.AddScoped<UserModel>();
    builder.Services.AddScoped<UserService>();
    builder.Services.AddScoped<AuthService>();

    var app = builder.Build();

    // ── Handle --migrate argument to run database migrations ────────────────────
    if (args.Contains("--migrate"))
    {
        Log.Information("Running database migrations");
        using var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        // Load and execute SQL files from Migrations folder
        // AppContext.BaseDirectory is bin/Debug/net9.0, so go up 3 levels to reach src/
        var sourceDir = Path.Combine(AppContext.BaseDirectory, "..", "..", "..");
        var migrationsPath = Path.Combine(sourceDir, "Common", "Database", "Migrations");
        var migrationsPathResolved = Path.GetFullPath(migrationsPath);

        if (!Directory.Exists(migrationsPathResolved))
        {
            Log.Error("Migrations directory not found at {Path}", migrationsPathResolved);
            return 1;
        }

        var sqlFiles = Directory.GetFiles(migrationsPathResolved, "*.sql").OrderBy(f => f).ToList();
        Log.Information("Found {Count} SQL files to execute", sqlFiles.Count);

        foreach (var file in sqlFiles)
        {
            try
            {
                Log.Information("Executing migration: {File}", Path.GetFileName(file));
                var sql = await File.ReadAllTextAsync(file);
                Log.Debug("SQL content: {Length} bytes", sql.Length);
                await db.Database.ExecuteSqlRawAsync(sql);
                Log.Information("✓ Successfully executed: {File}", Path.GetFileName(file));
            }
            catch (Exception ex)
            {
                Log.Error(ex, "✗ Failed to execute migration: {File}", Path.GetFileName(file));
                return 1;
            }
        }

        Log.Information("Database migrations completed successfully");
        return 0;
    }

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
    app.UseCors();
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