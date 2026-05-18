using System.Text.Json.Serialization;

namespace Trippie.Common.Services.GlobalExceptionHandler.Contracts;

public record ErrorResponse
{
    [JsonPropertyName("statusCode")]
    public required int StatusCode { get; init; }

    [JsonPropertyName("errorCode")]
    public required string ErrorCode { get; init; }

    [JsonPropertyName("message")]
    public required string Message { get; init; }

    [JsonPropertyName("traceId")]
    public required string TraceId { get; init; }

    [JsonPropertyName("timestamp")]
    public required DateTimeOffset Timestamp { get; init; }

    [JsonPropertyName("metadata")]
    public IReadOnlyDictionary<string, object?>? Metadata { get; init; }

    [JsonPropertyName("errors")]
    public IReadOnlyDictionary<string, IReadOnlyList<string>>? Errors { get; init; }

    [JsonPropertyName("details")]
    public ErrorDetails? Details { get; init; }
}
