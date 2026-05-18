using System.Net;

namespace Trippie.Common.Services.GlobalExceptionHandler.Exceptions;

public sealed class ForbiddenException(string message = "You are not allowed to perform this action.")
    : AppException(HttpStatusCode.Forbidden, "auth.forbidden", message)
{ }
