using System.Net;

namespace Trippie.Common.Services.GlobalExceptionHandler.Mappings;

public sealed record ErrorMappingResult(
    HttpStatusCode StatusCode,
    string ErrorCode,
    string ClientMessage,
    IReadOnlyDictionary<string, object?>? Metadata = null,
    IReadOnlyDictionary<string, IReadOnlyList<string>>? Errors = null,
    LogLevel LogLevel = LogLevel.Error
);
