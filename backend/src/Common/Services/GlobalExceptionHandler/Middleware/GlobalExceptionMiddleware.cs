using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.Json;
using Microsoft.Extensions.Options;
using Trippie.Common.Services.GlobalExceptionHandler.Config;
using Trippie.Common.Services.GlobalExceptionHandler.Contracts;
using Trippie.Common.Services.GlobalExceptionHandler.Mappings;

namespace Trippie.Common.Services.GlobalExceptionHandler.Middleware;

public sealed class GlobalExceptionMiddleware(
    RequestDelegate next,
    ExceptionMapperPipeline pipeline,
    ILogger<GlobalExceptionMiddleware> logger,
    IHostEnvironment env,
    IOptions<ErrorHandlingOptions> options
)
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
    {
        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
    };

private readonly ErrorHandlingOptions _options = options.Value;

public async Task InvokeAsync(HttpContext context)
{
    try
    {
        await next(context);
    }
    catch (System.Exception ex)
    {
        await HandleAsync(context, ex);
    }
}

private async Task HandleAsync(HttpContext context, System.Exception exception)
{
    var result = pipeline.Map(exception);

    // Prefer context.TraceIdentifier so the body matches the X-Correlation-Id header
    // that CorrelationIdMiddleware echoes (which itself honours an inbound header).
    var traceId = !string.IsNullOrWhiteSpace(context.TraceIdentifier)
        ? context.TraceIdentifier
        : Activity.Current?.TraceId.ToString() ?? string.Empty;

    var (file, line) = ExtractFileLine(exception);

    var details = env.IsDevelopment() || _options.ExposeDetails
        ? new ErrorDetails
        {
            ExceptionType = exception.GetType().FullName ?? exception.GetType().Name,
            File = file,
            Line = line,
            StackTrace = exception.StackTrace,
            Inner = exception.InnerException?.Message
        } : null;

    var response = new ErrorResponse
    {
        StatusCode = (int)result.StatusCode,
        ErrorCode = result.ErrorCode,
        Message = result.ClientMessage,
        TraceId = traceId,
        Timestamp = DateTimeOffset.UtcNow,
        Metadata = result.Metadata,
        Errors = result.Errors,
        Details = details
    };

    logger.Log(
        result.LogLevel,
        exception,
        "Request {Method} {Path} failed with {ExceptionType}: {ErrorCode} ({StatusCode}) at {SourceFile}:{SourceLine} [trace={TraceId}]",
        context.Request.Method,
        context.Request.Path.Value,
        exception.GetType().Name,
        result.ErrorCode,
        (int)result.StatusCode,
        file is null ? "?" : Path.GetFileName(file),
        line ?? 0,
        traceId
    );

    if (context.Response.HasStarted)
    {
        logger.LogWarning("Response already started - cannot write error body for trace {TraceId}.", traceId);
        return;
    }

    context.Response.Clear();
    context.Response.StatusCode = response.StatusCode;
    context.Response.ContentType = "application/problem+json; charset=utf-8";
    context.Response.Headers["X-Correlation-Id"] = traceId;
    context.Response.Headers.CacheControl = "no-store";

    await JsonSerializer.SerializeAsync(context.Response.Body, response, JsonOptions);
}

private static (string? File, int? Line) ExtractFileLine(System.Exception exception)
{
    var trace = new StackTrace(exception, fNeedFileInfo: true);
    foreach (var frame in trace.GetFrames())
    {
        var f = frame.GetFileName();
        if (!string.IsNullOrEmpty(f))
            return (f, frame.GetFileLineNumber());
    }
    return (null, null);
}
}
