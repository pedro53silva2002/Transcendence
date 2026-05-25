using System.Net;

namespace Trippie.Common.Services.GlobalExceptionHandler.Exceptions;

public sealed class NotFoundException(string resource, object identifier) : AppException(
    HttpStatusCode.NotFound,
    $"{resource.ToLowerInvariant()}.not_found",
    $"{resource} '{identifier}' was not found.",
    metadata: new Dictionary<string, object?>
    {
        ["resource"] = resource,
        ["id"] = identifier
    }
    )
{
    public static NotFoundException For<TResource>(object identifier) => new(typeof(TResource).Name, identifier);
}

