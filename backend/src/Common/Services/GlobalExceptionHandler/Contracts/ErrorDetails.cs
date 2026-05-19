using System.Text.Json.Serialization;

namespace Trippie.Common.Services.GlobalExceptionHandler.Contracts;

public sealed record ErrorDetails
{
    [JsonPropertyName("exceptionType")]
    public required string ExceptionType { get; init; }

    [JsonPropertyName("file")]
    public string? File { get; init; }

    [JsonPropertyName("line")]
    public int? Line { get; init; }

    [JsonPropertyName("stackTrace")]
    public string? StackTrace { get; init; }

    [JsonPropertyName("inner")]
    public string? Inner { get; init; }
}
