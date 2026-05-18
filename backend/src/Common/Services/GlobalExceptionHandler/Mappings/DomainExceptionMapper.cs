using Trippie.Common.Services.GlobalExceptionHandler.Exceptions;

namespace Trippie.Common.Services.GlobalExceptionHandler.Mappings;

public sealed class DomainExceptionMapper : IExceptionMapper
{
    public ErrorMappingResult? TryMap(System.Exception exception)
    {
        if (exception is not AppException app) return null;

        var level = (int)app.StatusCode >= 500 ? LogLevel.Error : LogLevel.Warning;

        var errors = (app as ValidationException)?.Errors;

        return new ErrorMappingResult(
            app.StatusCode,
            app.ErrorCode,
            app.Message,
            Metadata: app.Metadata.Count == 0 ? null : app.Metadata,
            Errors: errors,
            LogLevel: level
        );
    }
}
