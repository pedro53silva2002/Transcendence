using System.Net;

namespace Trippie.Common.Services.GlobalExceptionHandler.Exceptions;

public sealed class InternalServerException(string message, System.Exception? inner = null)
    : AppException(HttpStatusCode.InternalServerError, "common.internal_error", message, inner)
{ }
