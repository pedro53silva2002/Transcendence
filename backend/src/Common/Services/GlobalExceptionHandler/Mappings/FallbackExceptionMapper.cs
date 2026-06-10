using System.Net;

namespace Trippie.Common.Services.GlobalExceptionHandler.Mappings;

public sealed class FallbackExceptionMapper : IExceptionMapper
{
    public ErrorMappingResult? TryMap(System.Exception _) =>
        new(HttpStatusCode.InternalServerError,
        "common.unhandled",
        "An unexpected error occurred. Please try again later.",
        LogLevel: LogLevel.Error
        );
}
