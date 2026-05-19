using System.Net;

namespace Trippie.Common.Services.GlobalExceptionHandler.Exceptions;

public abstract class AppException(
    HttpStatusCode statusCode,
    string errorCode,
    string message,
    System.Exception? innerException = null,
    IReadOnlyDictionary<string, object?>? metadata = null
    ) : System.Exception(message, innerException)
{
    public HttpStatusCode StatusCode { get; } = statusCode;
public string ErrorCode { get; } = errorCode;
public IReadOnlyDictionary<string, object?> Metadata { get; } = metadata ?? new Dictionary<string, object?>();
}
