using System.Net;
using System.Text.Json;

namespace Trippie.Common.Services.GlobalExceptionHandler.Mappings;

public sealed class FrameworkExceptionMapper : IExceptionMapper
{
    public ErrorMappingResult? TryMap(System.Exception exception) => exception switch
    {
        UnauthorizedAccessException =>
            new ErrorMappingResult(HttpStatusCode.Unauthorized, "auth.unauthorized",
                "Authentication is required.", LogLevel: LogLevel.Warning),

        BadHttpRequestException bhr =>
            new ErrorMappingResult((HttpStatusCode)bhr.StatusCode, "request.bad",
                "The request could not be processed.", LogLevel: LogLevel.Warning),

        JsonException =>
            new ErrorMappingResult(HttpStatusCode.BadRequest, "request.invalid_json",
                "Request body is not valid JSON.", LogLevel: LogLevel.Warning),

        OperationCanceledException =>
            new ErrorMappingResult((HttpStatusCode)499, "request.cancelled",
                "The request was cancelled.", LogLevel: LogLevel.Information),

        TimeoutException =>
            new ErrorMappingResult(HttpStatusCode.GatewayTimeout, "common.timeout",
                "The operation timed out.", LogLevel: LogLevel.Warning),

        NotImplementedException =>
            new ErrorMappingResult(HttpStatusCode.NotImplemented, "common.not_implemented",
                "This feature is not implemented yet.", LogLevel: LogLevel.Error),

        _ => null

    };
}
