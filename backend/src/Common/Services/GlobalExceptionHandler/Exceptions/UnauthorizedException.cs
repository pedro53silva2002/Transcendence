using System.Net;

namespace Trippie.Common.Services.GlobalExceptionHandler.Exceptions;

public sealed class UnauthorizedException(string message = "Authentication is required")
    : AppException(HttpStatusCode.Unauthorized, "auth.unauthorized", message)
{ }
