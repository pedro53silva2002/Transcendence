using System.Diagnostics;

namespace Trippie.Common.Services.GlobalExceptionHandler.Middleware;

public sealed class CorrelationIdMiddleware(RequestDelegate next)
{
    private const string HeaderName = "X-Correlation-Id";
public async Task InvokeAsync(HttpContext context)
{
    var inbound = context.Request.Headers[HeaderName].ToString();
    var correlationId = !string.IsNullOrWhiteSpace(inbound) ? inbound
        : Activity.Current?.TraceId.ToString() ?? context.TraceIdentifier;

    context.TraceIdentifier = correlationId;
    context.Response.OnStarting(() =>
    {
        context.Response.Headers[HeaderName] = correlationId;
        return Task.CompletedTask;
    });

    await next(context);
}
}
