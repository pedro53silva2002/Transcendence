using System.Net;

namespace Trippie.Common.Services.GlobalExceptionHandler.Exceptions;

public sealed class ValidationException(IReadOnlyDictionary<string, IReadOnlyList<string>> errors) : AppException(
    HttpStatusCode.BadRequest,
    "validation.failed",
    "One or more validation errors occurred.",
    metadata: null
    )
{
    public IReadOnlyDictionary<string, IReadOnlyList<string>> Errors { get; } = errors;

    public ValidationException(string field, string message, string? clientMessage = null)
        : this(new Dictionary<string, IReadOnlyList<string>> { [field] = [message] }) { }
}
