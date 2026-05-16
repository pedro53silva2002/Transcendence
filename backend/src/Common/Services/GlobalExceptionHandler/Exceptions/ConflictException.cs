using System.Net;

namespace Trippie.Common.Services.GlobalExceptionHandler.Exceptions;

public sealed class ConflictException(string message, string errorCode = "common.conflict")
    : AppException(HttpStatusCode.Conflict, errorCode, message)
{ }
